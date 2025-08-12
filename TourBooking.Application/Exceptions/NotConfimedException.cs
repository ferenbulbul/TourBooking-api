namespace TourBooking.Application.Expactions
{
    public class NotConfimedException : ApplicationException
    {
        /// <summary>
        /// HTTP 404 Not Found durum koduna karşılık gelir.
        /// </summary>
        public NotConfimedException(string message)
            : base(message) { }
    }
}
