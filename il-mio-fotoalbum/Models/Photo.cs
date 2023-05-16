using il_mio_fotoalbum.Utilities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace il_mio_fotoalbum.Models
{

    [Index(nameof(PhotoId), IsUnique = true)]
    public class Photo
    {

        [Key]
        public long PhotoId { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "A photo title is required")]
        public string Title { get; set; }

        [StringLength(100)]
        [LongDescriptionValidation]
        public string? Description { get; set; }

        [Required(ErrorMessage = "An image URL is required")]
        public string Location { get; set; }

        public bool Private { get; set; } = true;

        public long? AlbumId { get; set; }
        public Album? Album { get; set; }

        public List<Category>? Categories { get; set; } = new();

        public Photo()
        { }

        public Photo(string title, string? description, string location, bool @private, long? albumId)
        {
            Title = title;
            Description = description;
            Location = location;
            Private = @private;
            AlbumId = albumId;
        }
    }
}
