using il_mio_fotoalbum.Utilities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace il_mio_fotoalbum.Models
{

    [Index(nameof(PizzaId), IsUnique = true)]
    public class Pizza
    {

        [Key]
        public long PizzaId { get; set; }

        [Required(ErrorMessage = "Il campo è obbligatorio")]
        public string Name { get; set; }

        [StringLength(100)]
        [LongDescriptionValidation]
        public string? Description { get; set; }

        private bool ThereIsAnImage { get; set; }

        public string? ImagePath
        {
            get
            {
                if (ThereIsAnImage)
                    return "../../img/pizza" + Name.ToLower() + ".jpg";
                else
                    return null;
            }
        }

        [Required(ErrorMessage = "Il campo è obbligatorio")]
        public double Price { get; set; }

        public long? CategoryId { get; set; }
        public Category? Category { get; set; }
        public List<Ingredient>? Ingredients { get; set; } = new();

        public Pizza()
        {
            ThereIsAnImage = false;
        }

        public Pizza(string name, string description, double price, bool thereIsAnImage = false)
        {
            this.Name = name;
            this.Description = description;
            this.Price = price;
            ThereIsAnImage = thereIsAnImage;
        }
    }
}
