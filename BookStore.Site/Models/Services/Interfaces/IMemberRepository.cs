using BookStore.Site.Models.DTOs;

namespace BookStore.Site.Models.Services.Interfaces
{
	public interface IMemberRepository
	{
		bool IsExist(string account);
		void Create(RegisterDto dto);

		MemberDto Load(int memberId);
		MemberDto GetByAccount(string account);
		void ActiveRegister(int memberId);

		void Update(MemberDto entity);

		void UpdatePassword(int memberId, string newEncryptedPassword);
	}
}