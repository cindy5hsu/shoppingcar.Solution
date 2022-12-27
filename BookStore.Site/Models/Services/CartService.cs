using BookStore.Site.Models.DTOs;
using BookStore.Site.Models.Services.Interfaces;
using System.Collections.Generic;
using System;
using System.Linq;

namespace BookStore.Site.Models.Services
{
	public class CartService
	{
		private readonly ICartRepository _repository;
		private readonly IProductRepository _productRepo;
		private readonly ICustomerRepository _customerRepo;

		public CartService(ICartRepository repository,
							IProductRepository productRepo,
							ICustomerRepository customerRepo)
		{
			_repository = repository;
			_productRepo = productRepo;
			_customerRepo = customerRepo;
		}

		/// <summary>
		/// 結帳時,觸發本事件
		/// </summary>
		public event Action<CartService, string> RequestCheckout;

		private ShippingInfo shippingInfo;
		/// <summary>
		/// 結帳, 完成後, 若想得知是哪一筆訂單, 就取最新的一筆
		/// </summary>
		/// <param name="customerAccount"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public void Checkout(string customerAccount, ShippingInfo shippingInfo)
		{
			if (string.IsNullOrEmpty(customerAccount)) throw new ArgumentNullException(nameof(customerAccount));
			if (shippingInfo == null) throw new Exception("ShippingInfo not allow null");

			this.shippingInfo = shippingInfo; // 先暫存,ToCreateOrderRequest會用到

			OnRequestCheckout(customerAccount); // 觸發 RequestCheckout 事件
		}

		protected virtual void OnRequestCheckout(string customerAccount)
		{
			if (RequestCheckout != null)
			{
				RequestCheckout(this, customerAccount);
			}
		}

		/// <summary>
		/// 取得目前使用者的購物車
		/// </summary>
		/// <param name="customerAccount"></param>
		/// <returns></returns>
		public CartEntity Current(string customerAccount)
		{
			if (_repository.IsExists(customerAccount))
			{
				return _repository.Load(customerAccount);
			}
			else
			{
				return _repository.CreateNewCart(customerAccount);
			}
		}

		/// <summary>
		/// 在購物車加入一項商品
		/// </summary>
		/// <param name="customerAccount"></param>
		/// <param name="productId"></param>
		/// <param name="qty"></param>
		public void AddItem(string customerAccount, int productId, int qty = 1)
		{
			var cart = Current(customerAccount);

			var product = _productRepo.Load(productId, true);
			var cartProd = new CartProductEntity { Id = productId, Name = product.Name, Price = product.Price };

			cart.AddItem(cartProd, qty);

			_repository.Save(cart);
		}

		/// <summary>
		/// 更新購物車中,單一商品的購買數量
		/// </summary>
		/// <param name="customerAccount"></param>
		/// <param name="productId"></param>
		/// <param name="newQty"></param>
		public void UpdateItem(string customerAccount, int productId, int newQty)
		{
			var cart = Current(customerAccount);

			cart.UpdateQty(productId, newQty);

			_repository.Save(cart);
		}

		/// <summary>
		/// 從購物車中刪除一項商品
		/// </summary>
		/// <param name="customerAccount"></param>
		/// <param name="productId"></param>
		public void RemoveItem(string customerAccount, int productId)
		{
			var cart = Current(customerAccount);

			cart.RemoveItem(productId);

			_repository.Save(cart);
		}

		/// <summary>
		/// 清空購物車
		/// </summary>
		/// <param name="customerAccount"></param>
		public void EmptyCart(string customerAccount)
		{
			_repository.EmptyCart(customerAccount);
		}

		/// <summary>
		/// 收集購物車資訊, 建立一個'請求建立一筆訂單的物件'
		/// </summary>
		/// <param name="cart"></param>
		/// <returns></returns>
		public CreateOrderRequest ToCreateOrderRequest(CartEntity cart)
		{
			List<CreateOrderItem> items = cart.GetItems()
				.Select(x =>
					new CreateOrderItem
					{
						ProductId = x.Product.Id,
						ProductName = x.Product.Name,
						Price = x.Product.Price,
						Qty = x.Qty
					})
				.ToList();

			return new CreateOrderRequest
			{
				CustomerId = _customerRepo.GetCustomerId(cart.CustomerAccount),
				Items = items,
				ShippingInfo = this.shippingInfo
			};
		}

		/// <summary>
		/// 從購物車裡,收集需要扣除庫庫的商品資訊
		/// </summary>
		/// <param name="cart"></param>
		/// <returns></returns>
		public DeductStockInfo[] GetDeductStockInfo(CartEntity cart)
		{
			return cart.GetItems()
				.Select(x =>
					new DeductStockInfo { ProductId = x.Product.Id, Qty = x.Qty })
				.ToArray();
		}
	}
}