namespace BuyWithOffer
{
    public class ProductDto : CommonEntityDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Explanation { get; set; }
        public string Category { get; set; }
        public string Color { get; set; }
        public string Brand { get; set; }
        public string UsageStatus { get; set; }
        public byte[] Image { get; set; }
        public int Price { get; set; }
        public bool isOfferable { get; set; }
        public bool isSold { get; set; }
    }

    public class CreateProductDto
    {
        public string ProductName { get; set; }
        public string Explanation { get; set; }
        public string Category { get; set; }
        public string Color { get; set; }
        public string Brand { get; set; }
        public string UsageStatus { get; set; }
        public int Price { get; set; }
    }
    public class UpdateProductDto : CreateProductDto
    {
        public int ProductId { get; set; }
    }

}
