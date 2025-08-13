using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourBooking.Application.DTOs.Mobile
{
    public class MobileTourBookingSummaryDto
    {
        public string TourPointTitle { get; set; }
        public string TourPointCity { get; set; }
        public string TourPointDistrict { get; set; }
        public string CarBrand { get; set; }
        public string DriverName { get; set; }
        public decimal TourPrice { get; set; }
        public string GuidName { get; set; }
        public decimal GuidePrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}