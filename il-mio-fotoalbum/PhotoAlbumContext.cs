using il_mio_fotoalbum.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace il_mio_fotoalbum
{
    public class PhotoAlbumContext : IdentityDbContext<IdentityUser>
    {

        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=fotoalbum;Integrated Security=True;Trust Server Certificate=True");
        }

    }
}
