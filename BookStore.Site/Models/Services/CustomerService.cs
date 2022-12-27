using System;
using BookStore.Site.Models.DTOs;
using BookStore.Site.Models.Services.Interfaces;

namespace BookStore.Site.Models.Services
{
	public class CustomerService
	{
		private readonly ICustomerRepository _repository;

		public CustomerService(ICustomerRepository repository)
		{
			_repository = repository;
		}

		public CustomerDto Load(string customerAccount)
			=> _repository.IsExists(customerAccount)
				? Load(customerAccount)
				: throw new Exception("找不到有權限購物的客戶資訊");

		public int GetCustomerId(string customerAccount)
			=> Load(customerAccount).Id;
	}
}