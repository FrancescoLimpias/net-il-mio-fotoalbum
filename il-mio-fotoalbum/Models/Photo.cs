using il_mio_fotoalbum.Utilities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public string? Description { get; set; }

        public string? Location { get; set; }

        public bool Private { get; set; }

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
