using System.Linq;
using BookStore.Site.Models.DTOs;
using BookStore.Site.Models.EFModels;
using BookStore.Site.Models.Services.Interfaces;

namespace BookStore.Site.Models.Infrastructures.Repositories
{
	public class CustomerRepository : ICustomerRepository
	{
		private readonly AppDbContext _db;

		public CustomerRepository(AppDbContext db)
		{
			_db = db;
		}

		/// <summary>
		/// 有權限在本網站購物的會員才傳回true
		/// </summary>
		/// <param name="customerAccount"></param>
		/// <returns></returns>
		public bool IsExists(string customerAccount)
		{
			var member = _db.Members.SingleOrDefault(x => x.IsConfirmed == true && x.Account == customerAccount);
			return member != null;
		}

		public int GetCustomerId(string customerAccount)
			=> _db.Members.Single(x => x.Account == customerAccount).Id;

		public CustomerDto Load(string customerAccount)
		{
			return _db.Members.Single(x => x.Account == customerAccount).ToCustomerEntity();
		}
	}
}