using System.ComponentModel.DataAnnotations;

namespace Potato.Models
{
    public class CookieUser
    {
        [Key]
        public Guid CookieID { get; init; }
        public Guid UserID { get; set; }
        public DateTime DateTime { get; init; } = DateTime.UtcNow.AddYears(1);

        public CookieUser(Guid userID)
        {
            UserID = userID;
        }

    }
}
