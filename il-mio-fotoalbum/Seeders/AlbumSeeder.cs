using il_mio_fotoalbum.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace il_mio_fotoalbum.Seeders
{
    public class AlbumSeeder : Seeder<ValueTuple<string, bool>, Album>
    {
        public AlbumSeeder() : base(new()
            {
                ("Venice 2023", true),
                ("Landscapes", false),
                ("Yummy Food", false),
                ("Models", false),
            })
        { }

        public override DbSet<Album> GetDbSet(PhotoAlbumContext context)
        {
            return context.Albums;
        }

        public override Album GenerateElementFromRawData(ValueTuple<string, bool> rawData)
        {
            return new()
            {
                Title = rawData.Item1,
                Private = rawData.Item2,
            };
        }
    }
}
