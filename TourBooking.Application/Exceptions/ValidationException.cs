using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourBooking.Application.Expactions
{
   /// <summary>
    /// Bir veya daha fazla doğrulama hatası oluştuğunda fırlatılır.
    /// HTTP 400 Bad Request durum koduna karşılık gelir.
    /// </summary>
    public class ValidationException : ApplicationException
    {
        public List<string> Errors { get; }

        public ValidationException(List<string> errors) 
            : base("One or more validation failures have occurred.") // Genel, sabit bir mesaj
        {
            Errors = errors;
        }

        // FluentValidation gibi kütüphanelerden gelen hatalarla uyumlu hale getirmek için
        // bu constructor da eklenebilir.
        public ValidationException(IEnumerable<FluentValidation.Results.ValidationFailure> failures)
            : this(new List<string>())
        {
            foreach (var failure in failures)
            {
                Errors.Add(failure.ErrorMessage);
            }
        }
    }
}