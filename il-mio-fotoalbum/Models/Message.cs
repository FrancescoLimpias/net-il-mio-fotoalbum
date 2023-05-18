using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace il_mio_fotoalbum.Models
{
    public class Message
    {

        [Key]
        public long MessageId { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "A email address must be provided")]
        public string Email { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "A message must be written")]
        public string Content { get; set; }

        public Message()
        {
        }
    }
}
