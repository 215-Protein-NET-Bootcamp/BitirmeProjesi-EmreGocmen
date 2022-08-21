namespace BuyWithOffer
{
    public class Sale : CommonEntity
    {
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public int Amount { get; set; }
    }
}
