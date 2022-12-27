using BookStore.Site.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookStore.Site.Models.DTOs
{
	/// <summary>
	/// 這是 Aggregate root
	/// </summary>
	public class CartEntity
	{
		public CartEntity(int id, string customerAccount)
		{
			this.Id = id;
			this.CustomerAccount = customerAccount;
			Items = new List<CartItemEntity>();
		}

		public CartEntity(int id, string customerAccount, List<CartItemEntity> items)
		{
			this.Id = id;
			this.CustomerAccount = customerAccount;
			Items = items;
		}

		public int Id { get; set; }

		private string _CustomerAccount;

		public string CustomerAccount
		{
			get => _CustomerAccount;
			set => _CustomerAccount = string.IsNullOrEmpty(value) == false
				? value
				: throw new Exception("CustomerAccount 不能是空值");
		}

		private List<CartItemEntity> Items;

		public int Total => Items == null || Items.Count == 0 ? 0 : Items.Sum(x => x.SubTotal);

		public bool AllowCheckout => Items != null && Items.Count > 0;

		public ShippingInfo ShippingInfo { get; set; }

		/// <summary>
		/// 若購物車已有相同商品,則增加數量
		/// </summary>
		/// <param name="product"></param>
		/// <param name="qty"></param>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public void AddItem(CartProductEntity product, int qty)
		{
			if (product == null) throw new ArgumentNullException(nameof(product));
			if (qty <= 0) throw new ArgumentOutOfRangeException(nameof(qty));

			var item = Items.SingleOrDefault(x => x.Product.Id == product.Id);
			if (item == null)
			{
				var cartItem = new CartItemEntity(product, qty);
				this.Items.Add(cartItem);
			}
			else
			{
				item.Qty += qty;
			}
		}

		public void RemoveItem(int productId)
		{
			var item = Items.SingleOrDefault(x => x.Product.Id == productId);
			if (item == null) return;

			Items.Remove(item);
		}

		public void UpdateQty(int productId, int newQty)
		{
			if (newQty <= 0)
			{
				RemoveItem(productId);
				return;
				// throw new ArgumentOutOfRangeException(nameof(newQty));
			}

			var item = Items.SingleOrDefault(x => x.Product.Id == productId);
			if (item == null) return;

			item.Qty = newQty;
		}

		public IEnumerable<CartItemEntity> GetItems()
			=> this.Items;
	}

	public static class CartExts
	{
		public static CartEntity ToEntity(this Cart source)
		{
			var items = source.CartItems.Select(x => x.ToEntity()).ToList();
			return new CartEntity(source.Id, source.MemberAccount, items);
		}

		public static Cart ToEF(this CartEntity source)
		{
			var items = source.GetItems().Select(x => x.ToEF(source.Id)).ToList();

			return new Cart { Id = source.Id, MemberAccount = source.CustomerAccount, CartItems = items };
		}
	}
}