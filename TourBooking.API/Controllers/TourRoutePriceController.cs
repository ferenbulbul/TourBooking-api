using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourBooking.API;
using TourBooking.Application.DTOs;
using TourBooking.Domain.Entities;
using TourBooking.Infrastructure.Context;

[ApiController]
[Route("api/route-price")]
public class TourRoutePriceController : BaseController
{
    private readonly AppDbContext _db;

    public TourRoutePriceController(AppDbContext db) => _db = db;

    // LIST: ?tourPointId=...
    [HttpGet]
    public async Task<ActionResult> List()
    {
        var agencyId = GetUserIdFromToken();
        var items = await _db
            .TourRoutePrices.Where(x => x.AgencyId == agencyId)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new TourRoutePriceDto(
                x.Id,
                x.TourPointId,
                x.CountryId,
                x.RegionId,
                x.CityId,
                x.DistrictId,
                x.VehicleId,
                x.DriverId,
                x.Price,
                x.Currency
            ))
            .ToListAsync();

        return Ok(new { items });
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TourRoutePriceDto>> GetById(Guid id)
    {
        var x = await _db.TourRoutePrices.FirstOrDefaultAsync(p => p.Id == id);
        if (x is null)
            return NotFound();

        return new TourRoutePriceDto(
            x.Id,
            x.TourPointId,
            x.CountryId,
            x.RegionId,
            x.CityId,
            x.DistrictId,
            x.VehicleId,
            x.DriverId,
            x.Price,
            x.Currency
        );
    }

    // UPSERT: aynı kombinasyon varsa GÜNCELLE, yoksa EKLE.
    [Authorize]
    [HttpPost("upsert")]
    public async Task<ActionResult<TourRoutePriceDto>> Upsert(
        [FromBody] UpsertTourRoutePriceRequest req
    )
    {
        var agencyId = GetUserIdFromToken();

        if (req.TourPointId == Guid.Empty)
            return BadRequest("tourPointId zorunlu.");
        if (req.Price <= 0)
            return BadRequest("price > 0 olmalı.");

        var existing = await _db.TourRoutePrices.FirstOrDefaultAsync(x =>
            x.TourPointId == req.TourPointId
            && x.CountryId == req.CountryId
            && x.RegionId == req.RegionId
            && x.CityId == req.CityId
            && x.DistrictId == req.DistrictId
            && x.VehicleId == req.VehicleId
            && x.DriverId == req.DriverId
            && x.AgencyId == agencyId
        );

        if (existing is null)
        {
            var entity = new TourRoutePriceEntity
            {
                Id = Guid.NewGuid(),
                TourPointId = req.TourPointId,
                CountryId = req.CountryId,
                RegionId = req.RegionId,
                CityId = req.CityId,
                DistrictId = req.DistrictId,
                VehicleId = req.VehicleId,
                DriverId = req.DriverId,
                Price = req.Price,
                Currency = string.IsNullOrWhiteSpace(req.Currency) ? "TRY" : req.Currency!,
                AgencyId = agencyId
            };

            _db.TourRoutePrices.Add(entity);
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Paralel isteklerde unique index çakışmasına karşı
                return Conflict("Aynı kombinasyon zaten mevcut.");
            }

            return CreatedAtAction(
                nameof(GetById),
                new { id = entity.Id },
                new TourRoutePriceDto(
                    entity.Id,
                    entity.TourPointId,
                    entity.CountryId,
                    entity.RegionId,
                    entity.CityId,
                    entity.DistrictId,
                    entity.VehicleId,
                    entity.DriverId,
                    entity.Price,
                    entity.Currency
                )
            );
        }
        else
        {
            existing.Price = req.Price;
            if (!string.IsNullOrWhiteSpace(req.Currency))
                existing.Currency = req.Currency!;
            existing.UpdatedAt = DateTime.UtcNow;
            existing.VehicleId = req.VehicleId;
            existing.DriverId = req.DriverId;

            await _db.SaveChangesAsync();

            return Ok(
                new TourRoutePriceDto(
                    existing.Id,
                    existing.TourPointId,
                    existing.CountryId,
                    existing.RegionId,
                    existing.CityId,
                    existing.DistrictId,
                    existing.VehicleId,
                    existing.DriverId,
                    existing.Price,
                    existing.Currency
                )
            );
        }
    }

    // DELETE
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var x = await _db.TourRoutePrices.FirstOrDefaultAsync(p => p.Id == id);
        if (x is null)
            return NotFound();

        _db.TourRoutePrices.Remove(x);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
