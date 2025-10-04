using System;
using Microsoft.EntityFrameworkCore;
using TourBooking.Domain.Entities;
using TourBooking.Domain.Enums;
using TourBooking.Infrastructure.Context;

namespace TourBooking.Infrastructure.Services
{
    public sealed class CalendarService
    {
        private readonly AppDbContext _db;

        public CalendarService(AppDbContext db) => _db = db;

        public async Task AddBlockAsync(
            Guid guideId,
            DateOnly start,
            DateOnly end,
            string? note,
            CancellationToken ct = default
        )
        {
            if (end < start)
                throw new ArgumentException("End must be >= Start");

            // Müşteri rezervasyonlarıyla çakışma yasak
            bool overlapsBooking = await _db.Bookings.AnyAsync(
                b =>
                    b.GuideTourPrice!.GuideId == guideId
                    && b.Status != BookingStatus.Fail
                    && b.StartDate <= end
                    && start <= b.EndDate,
                ct
            );
            if (overlapsBooking)
                throw new InvalidOperationException("Seçilen aralıkta müşteri rezervasyonu var.");

            // Kendi bloklarıyla çakışmayı engelle (istersen birleştirme de yapabilirsin)
            bool overlapsBlock = await _db.GuideBlocks.AnyAsync(
                gb => gb.GuideId == guideId && gb.StartDate <= end && start <= gb.EndDate,
                ct
            );
            if (overlapsBlock)
                throw new InvalidOperationException("Seçilen aralık mevcut bir blokla çakışıyor.");

            _db.GuideBlocks.Add(
                new GuideBlock
                {
                    Id = Guid.NewGuid(),
                    GuideId = guideId,
                    StartDate = start,
                    EndDate = end,
                    Note = note,
                    CreatedDate = DateTime.UtcNow
                }
            );
            await _db.SaveChangesAsync(ct);
        }

        public async Task RemoveBlockAsync(
            Guid guideId,
            Guid blockId,
            CancellationToken ct = default
        )
        {
            var block = await _db.GuideBlocks.FirstOrDefaultAsync(
                x => x.Id == blockId && x.GuideId == guideId,
                ct
            );
            if (block is null)
                throw new KeyNotFoundException();
            _db.GuideBlocks.Remove(block);
            await _db.SaveChangesAsync(ct);
        }
    }
}
