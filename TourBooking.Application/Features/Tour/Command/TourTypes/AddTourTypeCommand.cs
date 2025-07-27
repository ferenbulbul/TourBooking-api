using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using TourBooking.Application.DTOs;

namespace TourBooking.Application.Features
{
    public class AddTourTypeCommand : IRequest
    {
        public Guid? Id { get; set; }
        public string MainImageUrl { get; set; }
        public string ThumbImageUrl { get; set; }
        public  List<TourTypesTranslationDto> translations { get; set; }
    }
}