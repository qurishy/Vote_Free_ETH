using System.ComponentModel.DataAnnotations;

namespace Voter_Data_API.DTO
{
    public class RegisterRequestDto
    {
        [Required,EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [Required, DataType(DataType.Password),Compare(nameof(Password), ErrorMessage ="Password don't match")]
        public string ConfirmPassword { get; set; } = string.Empty;



    }
}
