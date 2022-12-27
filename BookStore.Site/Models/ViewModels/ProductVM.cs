using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using BookStore.Site.Models.DTOs;

namespace BookStore.Site.Models.ViewModels
{
	public class ProductVM
	{
		public int Id { get; set; }

		[Display(Name = "名稱")]
		public string Name { get; set; }

		[Display(Name = "分類名稱")]
		public string CategoryName { get; set; }

		[Display(Name = "商品描述")]
		public string Description { get; set; }

		[Display(Name = "售價")]
		public int Price { get; set; }

		public string ProductImage { get; set; }

		[Display(Name = "庫存量")]
		public int Stock { get; set; }
	}


	public static partial class PorductDtoExts
	{
		public static ProductVM ToVM(this ProductDto source)
		{
			return new ProductVM
			{
				Id = source.Id,
				Name = source.Name,
				CategoryName = source.Category.Name,
				Description = source.Description,
				Price = source.Price,
				ProductImage = source.ProductImage,
				Stock = source.Stock
			};
		}
	}
}