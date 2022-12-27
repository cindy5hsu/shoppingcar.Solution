using BookStore.Site.Models.Services.Interfaces;
using System.Collections.Generic;
using BookStore.Site.Models.DTOs;

namespace BookStore.Site.Models.Services
{
	public class ProductService
	{
		private readonly IProductRepository _repository;

		public ProductService(IProductRepository repository)
		{
			_repository = repository;
		}

		public IEnumerable<ProductDto> Search(int? categoryId, string productName)
			=> _repository.Search(categoryId, productName, true);

		public ProductDto Load(int productId)
			=> _repository.Load(productId, true);
	}
}