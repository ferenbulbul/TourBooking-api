using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourBooking.Application.DTOs;
using TourBooking.Application.DTOs.Comman;
using TourBooking.Application.Features;
using TourBooking.Application.Features.Settings.Queries;
using TourBooking.Domain.Entities;
using TourBooking.Infrastructure.Context;

namespace TourBooking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AvailabilityController : BaseController
    {
        private readonly AppDbContext _db;

        public AvailabilityController(AppDbContext db) => _db = db;

        [HttpPost]
        public async Task<IActionResult> CreateOrReplace(
            [FromBody] PostAvailabilityRequest req,
            CancellationToken ct
        )
        {
            if (req.vehicleId == null || req.vehicleId == Guid.Empty)
                return BadRequest("vehicleId zorunludur.");

            var (days, errors) = ExtractBusyDays(req);
            if (errors.Count > 0)
                return BadRequest(new { errors });

            // İsteğe bağlı: geçmiş tarihleri reddet
            // var today = DateOnly.FromDateTime(DateTime.UtcNow);
            // days = days.Where(d => d >= today).ToList();

            // Upsert mantığı: varsa günleri yeni set ile değiştir, yoksa oluştur
            var availability = await _db
                .Availabilities.Include(a => a.BusyDays)
                .SingleOrDefaultAsync(a => a.VehicleId == req.vehicleId, ct);

            using var tx = await _db.Database.BeginTransactionAsync(ct);
            if (availability is null)
            {
                availability = new AvailabilityEntity
                {
                    VehicleId = req.vehicleId,
                    BusyDays = days.Select(d => new BusyDayEntity { Day = d }).ToList()
                };
                _db.Availabilities.Add(availability);
                await _db.SaveChangesAsync(ct);
            }
            else
            {
                // Replace: önce mevcutları sil, sonra yeni seti ekle
                _db.BusyDays.RemoveRange(availability.BusyDays);
                await _db.SaveChangesAsync(ct);

                availability.BusyDays = days.Select(d => new BusyDayEntity { Day = d }).ToList();
                await _db.SaveChangesAsync(ct);
            }

            await tx.CommitAsync(ct);

            var response = new AvailabilityResponse(
                id: availability.Id,
                vehicleId: availability.VehicleId,
                busyDays: availability.BusyDays.Select(b => b.Day.ToString("yyyy-MM-dd")).ToList()
            );

            // Frontend, JSON-server gibi 201 + body bekliyor
            return CreatedAtAction(
                nameof(GetByVehicleId),
                new { vehicleId = availability.VehicleId },
                response
            );
        }

        // Frontend'in tekrar yüklemesinde kullanabileceğin GET
        [HttpGet]
        public async Task<IActionResult> GetByVehicleId(
            [FromQuery] Guid vehicleId,
            CancellationToken ct
        )
        {
            var availability = await _db
                .Availabilities.Include(a => a.BusyDays)
                .SingleOrDefaultAsync(a => a.VehicleId == vehicleId, ct);

            if (availability is null)
                return Ok(new object[] { }); // JSON-server davranışı: [] dönüyordu

            var response = new
            {
                id = availability.Id,
                vehicleId = availability.VehicleId,
                events = availability
                    .BusyDays.Select(d => new
                    {
                        id = (string?)null, // frontend kendi id’sini üretebilir
                        title = "Dolu",
                        start = d.Day.ToString("yyyy-MM-dd"),
                        allDay = true,
                        calendar = "Danger"
                    })
                    .ToList()
            };

            // JSON-server ile uyumlu: array dönelim
            return Ok(new { response });
        }

        [HttpPatch("{id:Guid}")]
        public async Task<IActionResult> Patch(
            Guid id,
            [FromBody] PatchAvailabilityRequest req,
            CancellationToken ct
        )
        {
            var (days, errors) = ExtractBusyDays(
                new PostAvailabilityRequest(Guid.Empty, req.events ?? new())
            );
            if (errors.Count > 0)
                return BadRequest(new { errors });

            var availability = await _db
                .Availabilities.Include(a => a.BusyDays)
                .SingleOrDefaultAsync(a => a.Id == id, ct);
            if (availability is null)
                return NotFound();

            using var tx = await _db.Database.BeginTransactionAsync(ct);

            _db.BusyDays.RemoveRange(availability.BusyDays);
            await _db.SaveChangesAsync(ct);

            availability.BusyDays = days.Select(d => new BusyDayEntity { Day = d }).ToList();
            await _db.SaveChangesAsync(ct);

            await tx.CommitAsync(ct);
            return NoContent();
        }

        static (List<DateOnly> days, List<string> errors) ExtractBusyDays(
            PostAvailabilityRequest req
        )
        {
            var errors = new List<string>();
            var set = new HashSet<DateOnly>();

            foreach (var e in req.events ?? new())
            {
                if (!string.Equals(e.calendar, "Danger", StringComparison.OrdinalIgnoreCase))
                    continue; // Sadece Dolu

                if (!DateOnly.TryParse(e.start, out var day))
                {
                    errors.Add($"Geçersiz tarih: {e.start}");
                    continue;
                }
                set.Add(day);
            }
            return (set.ToList(), errors);
        }
    }
}
