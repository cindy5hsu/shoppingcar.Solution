using BookStore.Site.Models.EFModels;

namespace BookStore.Site.Models.DTOs
{
	public class CreateOrderItem
	{
		public int ProductId { get; set; }
		public string ProductName { get; set; }
		public int Price { get; set; }

		public int Qty { get; set; }
		public int SubTotal => Price * Qty;
	}

	public static partial class CreateOrderItemExts
	{
		public static OrderItem ToEF(this CreateOrderItem source)
		{
			return new OrderItem
			{
				ProductId = source.ProductId,
				ProductName = source.ProductName,
				Price = source.Price,
				Qty = source.Qty,
				SubTotal = source.SubTotal
			};
		}
	}
}