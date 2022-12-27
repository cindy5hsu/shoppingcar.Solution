using BookStore.Site.Models.DTOs;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BookStore.Site.Models.ViewModels
{
	public class EditPasswordVM
	{
		[Display(Name = "原始密碼")]
		[Required]
		[StringLength(50)]
		[DataType(DataType.Password)]
		public string OriginalPassword { get; set; }

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

	public static class EditPasswordVMExts
	{
		public static ChangePasswordRequest ToRequest(this EditPasswordVM source, string userAccount)
		{
			return new ChangePasswordRequest
			{
				CurrentUserAccount = userAccount,
				OriginalPassword = source.OriginalPassword,
				NewPassword = source.Password
			};
		}
	}
}