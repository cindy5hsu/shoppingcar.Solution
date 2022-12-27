using BookStore.Site.Models.EFModels;

namespace BookStore.Site.Models.DTOs
{
	public class ProductDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public CategoryDto Category { get; set; }
		public string Description { get; set; }
		public int Price { get; set; }
		public bool Status { get; set; }
		public string ProductImage { get; set; }
		public int Stock { get; set; }
	}

	public static partial class ProductExts
	{
		public static ProductDto ToEntity(this Product source)
			=> new ProductDto
			{
				Id = source.Id,
				Name = source.Name,
				Category = source.Category.ToEntity(),
				Description = source.Description,
				Price = source.Price,
				Stock = source.Stock,
				Status = source.Status,
				ProductImage = source.ProductImage
			};
	}
}