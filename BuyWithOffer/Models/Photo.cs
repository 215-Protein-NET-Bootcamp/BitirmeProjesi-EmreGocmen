using System.ComponentModel.DataAnnotations;

namespace BuyWithOffer
{
    public class Photo : CommonEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public byte[] Image { get; set; }
    }
}
