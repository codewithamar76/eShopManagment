namespace eShopFlix.Models
{
    public class CategoryModal
    {
        public int CategoryId { get; set; }

        public string Name { get; set; }

        public virtual ICollection<ProductModel> Products { get; set; } = new List<ProductModel>();
    }
}
