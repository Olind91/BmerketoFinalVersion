namespace OskarLAspNet.Models.ViewModels
{
    public class ProductDetailViewModel
    {
        public string ArticleNumber { get; set; } = null!;
        public string Name { get; set; } = null!;
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
    }
}
