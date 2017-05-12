using System.ComponentModel.DataAnnotations;

namespace HelloWebAPI.Models
{
    public class Author
    {
        public int AuthorId { get; set; }
        [Required]
        public string Name { get; set; }

    }
}