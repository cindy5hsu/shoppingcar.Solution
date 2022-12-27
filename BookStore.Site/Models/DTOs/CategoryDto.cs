using BookStore.Site.Models.EFModels;

namespace BookStore.Site.Models.DTOs
{
	public class CategoryDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int DisplayOrder { get; set; }
	}

	public static class CategoryExts
	{
		public static CategoryDto ToEntity(this Category source)
			=> new CategoryDto
			{
				Id = source.Id,
				Name = source.Name,
				DisplayOrder = source.DisplayOrder
			};
	}
}