namespace BuyWithOffer
{
    public class Offer : CommonEntity
    {
        public int OfferId { get; set; }
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public bool isActive { get; set; }
        public bool isConfirmed { get; set; }
    }
}
