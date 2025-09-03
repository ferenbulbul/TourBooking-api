using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourBooking.Application.DTOs
{
    public record  CustomerUserDto
    (
         Guid Id ,
         string FirstName ,
         string Username ,
         bool EmailConfirmed ,
         string PhoneNumber ,
         bool PhoneNumberConfirmed ,
         DateTime? CreatedAt 
    );
        
    
}