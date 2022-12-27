using BookStore.Site.Models.EFModels;

namespace BookStore.Site.Models.DTOs
{
	public class CustomerDto
	{
		public int Id { get; set; }
		public string CustomerAccount { get; set; }
	}

	public static class CustomerExts
	{
		public static CustomerDto ToCustomerEntity(this Member source)
			=> new CustomerDto { Id = source.Id, CustomerAccount = source.Account };
	}
}