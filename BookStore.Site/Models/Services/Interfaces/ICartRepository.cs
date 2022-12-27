using BookStore.Site.Models.DTOs;

namespace BookStore.Site.Models.Services.Interfaces
{
	public interface ICartRepository
	{
		/// <summary>
		/// 傳回此客戶的購物車是否存在
		/// </summary>
		/// <param name="customerAccount"></param>
		/// <returns></returns>
		bool IsExists(string customerAccount);

		/// <summary>
		/// 建立一筆購物車主檔記錄
		/// </summary>
		/// <param name="customerAccount"></param>
		/// <returns></returns>
		CartEntity CreateNewCart(string customerAccount);

		/// <summary>
		/// 若購物車不存在, 傳回異常
		/// </summary>
		/// <param name="customerAccount"></param>
		/// <returns></returns>
		CartEntity Load(string customerAccount);

		/// <summary>
		/// 清空購物車-刪除在 db中的購物車記錄主檔及明細檔
		/// </summary>
		/// <param name="customerAccount"></param>
		void EmptyCart(string customerAccount);

		void Save(CartEntity cart);
	}
}