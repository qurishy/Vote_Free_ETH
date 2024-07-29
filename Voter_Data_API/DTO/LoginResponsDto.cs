namespace Voter_Data_API.DTO
{
    public class LoginResponsDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public string Userid { get; set; } = string.Empty;

        public string email { get; set; } = string.Empty;
        public string message { get; set; } = string.Empty;

        public bool IsSuccess { get; set; }
    }
}
