using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourBooking.Application.DTOs
{
    public class TourTypesTranslationDto
    {
        public string Description { get; set; }
        public string Title { get; set; }
        public Guid LanguageId { get; set; }
    }
}