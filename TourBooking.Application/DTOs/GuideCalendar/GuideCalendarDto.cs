using System;

namespace TourBooking.Application.DTOs.GuideCalendar
{
    public record CalendarEventDto2(
        Guid? id, // block id, booking'te null bırakabilir ya da ayrı key
        string title,
        DateOnly start, // ISO "YYYY-MM-DD" döndür
        DateOnly end, // dahil → FullCalendar’da end’i +1 gün yapacağız açıklama aşağıda
        bool editable,
        string color // booking: kırmızı, block: gri
    );

    public record CreateBlockRequest(DateOnly start, DateOnly end, string? note);
}
