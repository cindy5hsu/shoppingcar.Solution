using System;
using BookStore.Site.Models.DTOs;
using BookStore.Site.Models.Services.Interfaces;

namespace BookStore.Site.Models.Services
{
	public class OrderService
	{
		private readonly IOrderRepository _repository;
		public event Action<OrderService, int> OrderCreated;
		public event Action<OrderService, int> RefundRequestByCustomer;

		public OrderService(IOrderRepository repository)
		{
			_repository = repository;
		}

		public void PlaceOrder(CreateOrderRequest request)
		{
			// 修改IOrderRepository.Crate, 傳回值由 void 改為 int
			int orderId = _repository.Create(request);

			// 觸發事件
			OnOrderCreated(orderId);
		}

		protected virtual void OnOrderCreated(int orderId)
		{
			if (OrderCreated != null)
			{
				OrderCreated(this, orderId);
			}
		}

		public void Refund(string customerAccount, int orderId)
		{
			OrderEntity orderEntity = _repository.Load(orderId);
			if (orderEntity == null) throw new Exception("找不到訂單記錄");

			if (string.Compare(customerAccount, orderEntity.CustomerAccount,
				    StringComparison.CurrentCultureIgnoreCase) != 0)
			{
				throw new Exception("您無法針對他人訂單進行退訂");
			}

			if (orderEntity.AllowRefund == false) throw new Exception("此訂單目前無法進行退訂");

			_repository.RefundByCustomer(orderId);

			// raise event 
			OnRefundRequestByCustomer(orderId);
		}

		protected virtual void OnRefundRequestByCustomer(int orderId)
		{
			if (RefundRequestByCustomer != null)
			{
				RefundRequestByCustomer(this, orderId);
			}
		}
	}
}