using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BookStore.Site.Models.ViewModels
{
	public class ResetPasswordVM
	{
		[Display(Name = "新密碼")]
		[Required]
		[StringLength(50)]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Display(Name = "確認密碼")]
		[Required]
		[StringLength(50)]
		[Compare(nameof(Password))]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }
	}
}