using System.ComponentModel.DataAnnotations;

namespace Potato.Models
{
    public class User
    {

        public Guid Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "This field doesn't empty")]
        public string Username { get; init; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "This field doesn't empty")]
        public string Email { get; init; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "This field doesn't empty")]
        public string Password { get; init; }

        public User(string username, string email, string password)
        {
            Username = username;
            Email = email;
            Password = Crypto.GetCrypto(password);
        }
    }
}
