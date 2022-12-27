using BookStore.Site.Models.DTOs;
using BookStore.Site.Models.Services.Interfaces;
using System.Linq;

namespace BookStore.Site.Models.Services
{
	public class StockService
	{
		private readonly IStockRepository _repository;

		public StockService(IStockRepository repository)
		{
			_repository = repository;
		}

		/// <summary>
		/// 減少庫存量
		/// </summary>
		/// <param name="info"></param>
		public void Deduct(DeductStockInfo[] info)
		{
			var data = info.Select(x => (x.ProductId, x.Qty * -1)).ToArray();
			_repository.Update(data);
		}

		/// <summary>
		/// 增加庫存量
		/// </summary>
		/// <param name="info"></param>
		public void Revise(ReviseStockInfo[] info)
		{
			var data = info.Select(x => (x.ProductId, x.Qty)).ToArray();
			_repository.Update(data);
		}
	}
}