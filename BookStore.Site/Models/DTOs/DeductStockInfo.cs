namespace BookStore.Site.Models.DTOs
{
	/// <summary>
	/// 新增訂單時, 要扣除庫存量的資訊
	/// </summary>
	public class DeductStockInfo
	{
		public int ProductId { get; set; }

		/// <summary>
		/// 傳入正數
		/// </summary>
		public int Qty { get; set; }
	}
}