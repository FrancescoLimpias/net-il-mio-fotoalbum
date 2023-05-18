using il_mio_fotoalbum.Models;
using Microsoft.EntityFrameworkCore;

namespace il_mio_fotoalbum.Seeders
{
    public abstract class Seeder<TRawData, TModel> : ISeeder where TModel : class
    {

        //Database connection
        static protected PhotoAlbumContext context = new PhotoAlbumContext();

        //List of raw data for elements creation
        static private List<TRawData> RawList { get; set; }

        public Seeder(List<TRawData> rawList)
        {
            RawList = rawList;
        }

        //Public method for running the database seeding
        public async void Seed()
        {
            foreach (TRawData rawData in RawList)
            {
                TModel newItem = await GenerateElementFromRawData(rawData);
                GetDbSet(context).Add(newItem);
            }
            context.SaveChanges();
        }

        //ABSTRACTS
        //Retrieve the DbSet to store elements into
        abstract public DbSet<TModel> GetDbSet(PhotoAlbumContext context);
        //Logic to translate raw data into new elements
        abstract public Task<TModel> GenerateElementFromRawData(TRawData rawData);
    }

    //Utility implementation for translationsless seeders
    public abstract class Seeder<TRawData> : Seeder<TRawData, TRawData> where TRawData : class
    {
        protected Seeder(List<TRawData> rawList) : base(rawList)
        { }

        public override async Task<TRawData> GenerateElementFromRawData(TRawData data)
        {
            return data;
        }
    }
}
