namespace BookStore.Site.Models.DTOs
{
	public class ChangePasswordRequest
	{
		public string CurrentUserAccount { get; set; }
		public string NewPassword { get; set; }
		public string OriginalPassword { get; set; }
	}
}