using System.ComponentModel.DataAnnotations;

namespace il_mio_fotoalbum.Models
{
    public class Category
    {

        [Key]
        public long CategoryId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Symbol { get; set; }

        public List<Pizza> Pizzas { get; set; }

        public Category() { }

    }
}
