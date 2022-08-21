using System;

namespace BuyWithOffer
{
    public class ApplicationResponse<T> : CommonApplicationResult
    {
        public T Result { get; set; }
    }
    public class ApplicationResponse : CommonApplicationResult
    {
    }

    public class CommonApplicationResult
    {
        public DateTime ResponseTime { get; set; } = DateTime.UtcNow;
        public bool Succeeded { get; set; }
        public string ErrorMessage { get; set; }

    }
}
