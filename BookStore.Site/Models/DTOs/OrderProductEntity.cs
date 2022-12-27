using BookStore.Site.Models.EFModels;
using System;

namespace BookStore.Site.Models.DTOs
{
	public class OrderProductEntity
	{
		public OrderProductEntity(int id, string name, int price)
		{
			if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
			if (price < 0) throw new ArgumentOutOfRangeException(nameof(price));

			Id = id;
			Name = name;
			Price = price;
		}
		public int Id { get; set; }
		/// <summary>
		/// 反正規化, 視需要再增加其他要記錄的商品欄位到訂單記錄裡
		/// </summary>
		public string Name { get; set; }

		public int Price { get; set; }
	}

	public static partial class ProductExts
	{
		public static OrderProductEntity ToOrderProductEntity(this Product source)
			=> new OrderProductEntity(source.Id, source.Name, source.Price);
	}
}