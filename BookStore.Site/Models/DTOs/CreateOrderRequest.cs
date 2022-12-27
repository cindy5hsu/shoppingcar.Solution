using System.Collections.Generic;
using System.Linq;

namespace BookStore.Site.Models.DTOs
{
	public class CreateOrderRequest
	{
		public int CustomerId { get; set; }
		public List<CreateOrderItem> Items { get; set; }
		public int Total => Items.Sum(x => x.SubTotal);
		public ShippingInfo ShippingInfo { get; set; }
	}
}