using BookStore.Site.Models.DTOs;
using BookStore.Site.Models.EFModels;

namespace BookStore.Site.Models.Infrastructures.ExtensionMethods
{
	public static class MemberExts
	{
		public static MemberDto ToDto(this Member entity)
		{
			return entity==null
				? null 
				: new MemberDto
			{
				Id = entity.Id,
				Account = entity.Account,
				EncryptedPassword = entity.EncryptedPassword,
				Email = entity.Email,
				Name = entity.Name,
				Mobile = entity.Mobile,
				IsConfirmed = entity.IsConfirmed,
				ConfirmCode = entity.ConfirmCode
			};
		}
	}
}