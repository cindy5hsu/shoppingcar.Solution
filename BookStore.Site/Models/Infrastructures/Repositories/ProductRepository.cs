using BookStore.Site.Models.EFModels;
using BookStore.Site.Models.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using BookStore.Site.Models.DTOs;

namespace BookStore.Site.Models.Infrastructures.Repositories
{
	public class ProductRepository : IProductRepository
	{
		private readonly AppDbContext _db;

		public ProductRepository(AppDbContext db)
		{
			_db = db;
		}

		public IEnumerable<ProductDto> Search(int? categoryId, string productName, bool? status)
		{
			IEnumerable<Product> query = _db.Products;
			if (categoryId.HasValue) query = query.Where(x => x.CategoryId == categoryId);
			if (!string.IsNullOrEmpty(productName)) query = query.Where(x => x.Name.Contains(productName));
			if (status.HasValue) query = query.Where(x => x.Status == status);
			query = query.OrderBy(x => x.Name);

			return query.Select(x => x.ToEntity());
		}

		public ProductDto Load(int productId, bool? status)
		{
			IEnumerable<Product> query = _db.Products.Where(x => x.Id == productId);
			if (status.HasValue) query = query.Where(x => x.Status == status);

			var product = query.FirstOrDefault();

			return product == null ? null : product.ToEntity();
		}
	}
}