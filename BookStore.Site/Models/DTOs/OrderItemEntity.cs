using BookStore.Site.Models.EFModels;
using System;

namespace BookStore.Site.Models.DTOs
{
	public class OrderItemEntity
	{
		public OrderItemEntity(int id, OrderProductEntity product, int qty)
		{
			if (product == null) throw new ArgumentNullException(nameof(product));
			if (qty <= 0) throw new ArgumentOutOfRangeException(nameof(qty));

			Id = id;
			Product = product;
			Qty = qty;
		}
		public int Id { get; set; }
		public OrderProductEntity Product { get; set; }
		public int Qty { get; set; }
		public int SubTotal => Product.Price * Qty;
	}

	public static partial class OrderItemExts
	{
		public static OrderItemEntity ToEntity(this OrderItem source)
		{
			return new OrderItemEntity(source.Id, source.Product.ToOrderProductEntity(), source.Qty);
		}
	}
}