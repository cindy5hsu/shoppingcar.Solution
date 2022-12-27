namespace BookStore.Site.Models.DTOs
{
	/// <summary>
	/// 訂單取消時,要增加庫存量的資訊
	/// </summary>
	public class ReviseStockInfo
	{
		public int ProductId { get; set; }

		/// <summary>
		/// 傳入正數
		/// </summary>
		public int Qty { get; set; }
	}
}