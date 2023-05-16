using il_mio_fotoalbum.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace il_mio_fotoalbum.Models
{
    public class PhotoPayload
    {

        public Photo Photo { get; set; }
        public List<Album>? Albums { get; set; }
        public List<SelectListItem>? Categories { get; set; }
        public List<string>? SelectedCategories { get; set; }

    }
}
