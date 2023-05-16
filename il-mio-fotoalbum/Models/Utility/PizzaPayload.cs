using il_mio_fotoalbum.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace il_mio_fotoalbum.Models
{
    public class PizzaPayload
    {

        public Pizza Pizza { get; set; }
        public List<Category>? Categories { get; set; }
        public List<SelectListItem>? Ingredients { get; set; }
        public List<string>? SelectedIngredients { get; set; }

    }
}
