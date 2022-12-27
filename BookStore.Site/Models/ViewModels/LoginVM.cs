using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BookStore.Site.Models.ViewModels
{
	public class LoginVM
	{
		[Display(Name = "帳號")]
		[Required]
		public string Account { get; set; }

		[Display(Name = "密碼")]
		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}
}