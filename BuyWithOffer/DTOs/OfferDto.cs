namespace BuyWithOffer
{
    public class OfferDto : CommonEntityDto
    {
        public int OfferId { get; set; }
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public bool isActive { get; set; }
        public bool isConfirmed { get; set; }
    }

    public class CreateOfferDto
    {
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public int Percentage { get; set; }
    }

    public class UpdateOfferDto : CreateOfferDto
    {
        public int OfferId { get; set; }
    }

}
