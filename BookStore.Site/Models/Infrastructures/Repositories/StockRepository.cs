using BookStore.Site.Models.EFModels;
using BookStore.Site.Models.Services.Interfaces;
using System.Linq;

namespace BookStore.Site.Models.Infrastructures.Repositories
{
	public class StockRepository : IStockRepository
	{
		private readonly AppDbContext _db;

		public StockRepository(AppDbContext db)
		{
			_db = db;
		}

		/// <summary>
		/// qty不是新庫存值,而是要異動的差異數量;負數表示扣庫存,正數表示增加庫存
		/// </summary>
		/// <param name="info"></param>
		public void Update((int productId, int qty)[] info)
		{
			if (info == null || info.Length == 0) return;

			int[] productIds = info.Select(x => x.productId).ToArray();
			var products = _db.Products.Where(x => productIds.Contains(x.Id)).ToList();
			foreach (var product in products)
			{
				product.Stock += info.Single(x => x.productId == product.Id).qty;
			}

			_db.SaveChanges();

		}
	}
}