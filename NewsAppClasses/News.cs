using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using NewsAppClasses.Validators;

namespace NewsAppClasses
{
    public class News
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "You must enter a title for the news")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "You must enter an article for the news")]
        public string NewsArticle { get; set; } = string.Empty;

        public byte[]? Image { get; set; }

        [DataType(DataType.Date), PubDateValidator(ErrorMessage = "Publication date must be not older than a week from current date")]
        public DateTime PublicationDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; } = DateTime.Now;

        public Author Author { get; set; } = new Author();
    }
}
