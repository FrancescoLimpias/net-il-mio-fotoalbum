using il_mio_fotoalbum.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace il_mio_fotoalbum
{
    public class PhotoAlbumContext : IdentityDbContext<IdentityUser>
    {

        public DbSet<Photo> Photos { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Message> Messages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=fotoalbum;Integrated Security=True;Trust Server Certificate=True");
        }

    }
}
