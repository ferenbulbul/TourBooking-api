using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourBooking.Application.DTOs.Mobile
{
    public class MobileSearchGuidesDto
    {
        public Guid GuideId { get; set; }
        public decimal Price { get; set; }
        public List<string>? Languages { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Image { get; set; }
    }
}