using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace il_mio_fotoalbum.Models
{
    public class Album
    {

        [Key]
        public long AlbumId { get; set; }

        [Required(ErrorMessage = "Define the album's title")]
        public string Title { get; set; }

        public bool Private { get; set; } = true;

        [JsonIgnore]
        public List<Photo> Photos { get; set; }

        public Album()
        { }

    }
}
