using System.ComponentModel.DataAnnotations;

namespace Potato.Models
{
    public class User
    {
        [Key]
        public Guid UserId { get; init; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "This field doesn't empty")]
        public string Username { get; init; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "This field doesn't empty")]
        public string Email { get; init; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "This field doesn't empty")]
        public string Password { get; init; }
        public string Permission { get; init; } = "USER";

        public User(string username, string email, string password)
        {
            Username = username;
            Email = email;
            Password = password;
        }
    }
}
