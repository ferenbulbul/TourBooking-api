using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourBooking.Application.DTOs.Comman
{
    public abstract class BaseResponse
    {
        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public bool HasExceptionError { get; set; } 
        public List<string>? ValidationErrors { get; set; } 
    }
}