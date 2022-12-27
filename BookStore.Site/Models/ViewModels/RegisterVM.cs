using BookStore.Site.Models.DTOs;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BookStore.Site.Models.ViewModels
{
	public class RegisterVM
	{
		public int Id { get; set; }

		[Display(Name = "帳號")]
		[Required]
		[StringLength(30)]
		public string Account { get; set; }

		/// <summary>
		/// 明碼
		/// </summary>
		[Required]
		[StringLength(50)]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required]
		[StringLength(50)]
		[Compare(nameof(Password))]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }

		[Required]
		[StringLength(256)]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[StringLength(30)]
		public string Name { get; set; }

		[StringLength(10)]
		public string Mobile { get; set; }
	}

	public static class RegisterVMExts
	{
		public static RegisterDto ToRequestDto(this RegisterVM source)
		{
			return new RegisterDto
			{
				Account = source.Account,
				Password = source.Password,
				Email = source.Email,
				Name = source.Name,
				Mobile = source.Mobile
			};
		}
	}
}