using BookStore.Site.Models.DTOs;

namespace BookStore.Site.Models.Services
{
	public class CartMediator
	{
		private readonly CartService _cart;
		private readonly OrderService _order;
		private readonly StockService _stock;

		public CartMediator(CartService cart, OrderService order, StockService stock)
		{
			_cart = cart;
			_order = order;
			_stock = stock;

			_cart.RequestCheckout += _cart_RequestCheckout;
		}

		private void _cart_RequestCheckout(CartService sender, string customerAccount)
		{
			CartEntity cart = _cart.Current(customerAccount);

			CreateOrderRequest request = _cart.ToCreateOrderRequest(cart);
			_order.PlaceOrder(request);

			DeductStockInfo[] info = _cart.GetDeductStockInfo(cart);
			_stock.Deduct(info);

			_cart.EmptyCart(customerAccount);

		}
	}
}