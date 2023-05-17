using System.ComponentModel.DataAnnotations;

namespace il_mio_fotoalbum.Models
{
    public class Category
    {

        [Key]
        public long CategoryId { get; set; }

        [Required(ErrorMessage = "A category name is required")]
        public string Name { get; set; }

        public string? Symbol { get; set; }

        public string? Color { get; set; }

        [Required(ErrorMessage = "Set this category as prioritary or not")]
        public bool Prioritary { get; set; } = false;

        public List<Photo> Photos { get; set; }

        public Category()
        {
            Photos = new();
        }

        public Category(string name, bool prioritary)
        {
            Name = name;
            Prioritary = prioritary;
            Photos = new List<Photo>();
        }

        public Category(string name, string symbol, string color, bool prioritary)
        {
            Name = name;
            Symbol = symbol;
            Color = color;
            Prioritary = prioritary;
            Photos = new List<Photo>();
        }
    }
}
