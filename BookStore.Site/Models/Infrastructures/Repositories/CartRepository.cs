using System.Linq;
using BookStore.Site.Models.DTOs;
using BookStore.Site.Models.EFModels;
using BookStore.Site.Models.Services.Interfaces;

using System.Data.Entity;
namespace BookStore.Site.Models.Infrastructures.Repositories
{
	public class CartRepository : ICartRepository
	{
		private readonly AppDbContext _db;

		public CartRepository(AppDbContext db)
		{
			_db = db;
		}

		public bool IsExists(string customerAccount)
			=> _db.Carts.SingleOrDefault(x => x.MemberAccount == customerAccount) != null;

		/// <summary>
		/// 叫用之前, 必需先用IsExists判斷, 沒有才叫用
		/// </summary>
		/// <param name="customerAccount"></param>
		/// <returns></returns>
		public CartEntity CreateNewCart(string customerAccount)
		{
			var cart = new Cart { MemberAccount = customerAccount };
			_db.Carts.Add(cart);
			_db.SaveChanges();

			return Load(customerAccount);
		}

		/// <summary>
		/// 要加入 include,一起載入CartItems, Products,效能才會好
		/// </summary>
		/// <param name="customerAccount"></param>
		/// <returns></returns>
		public CartEntity Load(string customerAccount)
			=> _db.Carts
				.AsNoTracking()
				// .Include(x=>x.CartItems)
				.Include(x => x.CartItems.Select(x2 => x2.Product)) // 使用這行,必需在最上方加上using System.Data.Entity;
				.Single(x => x.MemberAccount == customerAccount).ToEntity();

		public void EmptyCart(string customerAccount)
		{
			var cart = _db.Carts.SingleOrDefault(x => x.MemberAccount == customerAccount);
			if (cart == null) return;
			_db.Carts.Remove(cart);
			_db.SaveChanges();
		}

		/// <summary>
		/// 更新購物車記錄主檔/明細檔
		/// </summary>
		/// <param name="cart"></param>
		/// <exception cref="System.NotImplementedException"></exception>
		public void Save(CartEntity cart)
		{
			var cartEF = cart.ToEF();

			var efInDb = _db.Carts
				.Include(x => x.CartItems)
				.Single(x => x.Id == cart.Id);

			var efItemsInDb = efInDb.CartItems.ToList();

			// 若 efInDb中的商品不存在於 cartEF, 表示使用者刪除了
			var deletedProducts = efItemsInDb.Select(x => x.ProductId)
				.Except(cartEF.CartItems.Select(x => x.ProductId))
				.ToList();

			foreach (var productId in deletedProducts)
			{
				var delItem = efInDb.CartItems.Single(x => x.ProductId == productId);
				// 不能直接remove item,要標註它已被刪除,才能正常執行
				_db.Entry(delItem).State = EntityState.Deleted;
			}

			// 新增商品或異動數量
			foreach (var item in cartEF.CartItems)
			{
				if (item.Id == 0)
				{
					efInDb.CartItems.Add(item);
				}
				else
				{
					efInDb.CartItems.Single(x => x.Id == item.Id).Qty = item.Qty;
				}
			}

			_db.SaveChanges();
		}
	}
}