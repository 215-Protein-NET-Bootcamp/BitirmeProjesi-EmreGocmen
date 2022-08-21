using System;

namespace BuyWithOffer
{
    public class CommonEntityDto
    {
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedById { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedById { get; set; }
    }
}
