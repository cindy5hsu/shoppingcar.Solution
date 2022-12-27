using BookStore.Site.Models.DTOs;

namespace BookStore.Site.Models.Services.Interfaces
{
	/// <summary>
	/// 對前台購物網站而言, 買家是Customer, 與 Member的概念還是有一點不同, 所以在此額外寫一個CustomerService/CustomerRepository
	/// </summary>
	public interface ICustomerRepository
	{
		/// <summary>
		/// 有權限在本網站購物的會員才傳回true
		/// </summary>
		/// <param name="customerAccount"></param>
		/// <returns></returns>
		bool IsExists(string customerAccount);

		/// <summary>
		/// 有權限在本網站購物的會員才傳回其 id 值
		/// </summary>
		/// <param name="customerAccount"></param>
		/// <returns></returns>
		int GetCustomerId(string customerAccount);

		/// <summary>
		/// 有權限在本網站購物的會員才傳回物件
		/// </summary>
		/// <param name="customerAccount"></param>
		/// <returns></returns>
		CustomerDto Load(string customerAccount);
	}
}