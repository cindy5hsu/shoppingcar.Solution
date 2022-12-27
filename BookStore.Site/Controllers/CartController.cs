using BookStore.Site.Models.DTOs;
using BookStore.Site.Models.EFModels;
using BookStore.Site.Models.Infrastructures.Repositories;
using BookStore.Site.Models.Services;
using BookStore.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Site.Controllers
{
    public class CartController : Controller
    {
		private CartService cartService;
		private readonly OrderService orderService;
		private readonly StockService stockService;

		public CartController()
		{
			var db = new AppDbContext();
			var cartRepo = new CartRepository(db);
			var productRepo = new ProductRepository(db);
			var customerRepo = new CustomerRepository(db);

			this.cartService = new CartService(cartRepo, productRepo, customerRepo);

			var orderRepo = new OrderRepository(db);
			this.orderService = new OrderService(orderRepo);
			this.stockService = new StockService(new StockRepository(db));
		}

		public ActionResult AddItem(int productId)
		{
			var customerAccount = User.Identity.Name;
			cartService.AddItem(customerAccount, productId, 1);

			return new EmptyResult();
		}

		public string CustomerAccount => User.Identity.Name;

		[Authorize]
		public ActionResult Info()
		{
			var cart = cartService.Current(CustomerAccount);
			return View(cart);
		}

		public ActionResult UpdateItem(int productId, int newQty)
		{
			newQty = newQty <= 0 ? 0 : newQty;

			cartService.UpdateItem(CustomerAccount, productId, newQty);

			return new EmptyResult();
		}

		public ActionResult Checkout()
		{
			var cart = cartService.Current(this.CustomerAccount);
			if (cart.AllowCheckout == false)
			{
				ViewBag.ErrorMessage = "購物車是空的, 無法進行結帳";
			}

			return View();
		}

		[HttpPost]
		public ActionResult Checkout(CheckoutVM model, ShippingInfo info)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var cart = cartService.Current(this.CustomerAccount);
			if (cart.AllowCheckout == false)
			{
				ModelState.AddModelError(string.Empty, "購物車是空的, 無法進行結帳");
				return View(model);
			}

			var mediator = new CartMediator(this.cartService, orderService, stockService);

			cartService.Checkout(this.CustomerAccount, info);
			return View("CheckoutConfirm");
		}
	}
}