namespace BookStore.Site.Models.DTOs
{
	public class MemberDto
	{
		public int Id { get; set; }

		public string Account { get; set; }

		public string EncryptedPassword { get; set; }

		public string Email { get; set; }

		public string Name { get; set; }

		public string Mobile { get; set; }

		public bool? IsConfirmed { get; set; }

		public string ConfirmCode { get; set; }
	}
}