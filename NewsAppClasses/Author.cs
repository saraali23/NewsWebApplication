using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace NewsAppClasses
{
    public class Author
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        [JsonIgnore]
        public virtual ICollection<News>? News { get; set; }
    }
}
