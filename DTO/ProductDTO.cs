namespace ProductsAPI.DTO
{
    public class ProductDTO
    {
        //kullanucuya göndermek istenen alanlar
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public int Price { get; set; }
    }
}
