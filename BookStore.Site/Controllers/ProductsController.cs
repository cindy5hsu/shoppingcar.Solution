using BookStore.Site.Models.EFModels;
using BookStore.Site.Models.Infrastructures.Repositories;
using BookStore.Site.Models.Services.Interfaces;
using BookStore.Site.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Site.Models.ViewModels;

namespace BookStore.Site.Controllers
{
    public class ProductsController : Controller
    {
		private ProductService productService;

		public ProductsController()
		{
			var db = new AppDbContext();
			IProductRepository repo = new ProductRepository(db);
			this.productService = new ProductService(repo);
		}

		// GET: Products
		public ActionResult Index()
		{
			var data = productService.Search(null, null)
				.Select(x => x.ToVM());
			
			return View(data);
		}
	}
}