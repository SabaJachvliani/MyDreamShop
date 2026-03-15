namespace Application.Queries.ProductQueries
{
    public class ProductDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;

        public double AverageRating { get; set; }
        public int ReviewsCount { get; set; }

        public List<ProductReviewDto> Reviews { get; set; } = new();
    }
}
