using System.ComponentModel.DataAnnotations;

namespace BookStore.Site.Models.ViewModels
{
	public class ForgetPasswordVM
	{
		[Required]
		[StringLength(30)]
		public string Account { get; set; }

		[Required]
		[StringLength(256)]
		public string Email { get; set; }
	}
}