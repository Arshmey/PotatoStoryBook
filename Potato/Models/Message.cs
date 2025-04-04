using System.ComponentModel.DataAnnotations;

namespace Potato.Models
{
    public class Message
    {

        public Guid Id { get; init; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "This field doesn't empty")]
        public string Title { get; init; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "This field doesn't empty")]
        public string Content { get; init; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "This field doesn't empty")]
        public string Author { get; init; }
        public DateTime DateCriation { get; init; }

        public Message(string title, string content, string author)
        {
            Title = title;
            Content = content;
            Author = author;
            DateCriation = DateTime.UtcNow;
        }

    }
}
