using System.ComponentModel.DataAnnotations;
using BookStore.Site.Models.DTOs;

namespace BookStore.Site.Models.ViewModels
{
	public class EditProfileVM
	{
		public int Id { get; set; }

		[Required]
		[StringLength(256)]
		public string Email { get; set; }

		[Required]
		[StringLength(30)]
		public string Name { get; set; }

		[StringLength(10)]
		public string Mobile { get; set; }
	}

	public static class MemberDtoExts
	{
		public static EditProfileVM ToEditProfileVM(this MemberDto source)
		{
			return new EditProfileVM
			{
				Id = source.Id,
				// Account = source.Account,
				Email = source.Email,
				Name = source.Name,
				Mobile = source.Mobile
			};
		}

		public static UpdateProfileDto ToDto(this EditProfileVM source, string currentUserAccount)
		{
			return new UpdateProfileDto
			{
				//CurrentUserAccount = currentUserAccount,
				Account = currentUserAccount,
				Email = source.Email,
				Mobile = source.Mobile,
				Name = source.Name
			};
		}
	}
}