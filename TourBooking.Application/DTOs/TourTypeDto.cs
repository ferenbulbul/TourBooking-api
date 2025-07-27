using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourBooking.Application.DTOs
{
    public class TourTypeDto
    {
        public Guid Id { get; set; }
        public List<TourTypesTranslationDto> Translations { get; set; }
        public string MainImageUrl { get; set; }
        public string ThumbImageUrl { get; set; }
    }
}