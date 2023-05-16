using il_mio_fotoalbum.Models;
using Microsoft.EntityFrameworkCore;

namespace il_mio_fotoalbum.Seeders
{
    public abstract class Seeder<TRawData, TModel> : ISeeder where TModel : class
    {

        //Database connection
        static protected PizzeriaContext context = new PizzeriaContext();

        //List of raw data for elements creation
        static private List<TRawData> RawList { get; set; }

        public Seeder(List<TRawData> rawList)
        {
            RawList = rawList;
        }

        //Public method for running the database seeding
        public void Seed()
        {
            foreach (TRawData rawData in RawList)
            {
                TModel newItem = GenerateElementFromRawData(rawData);
                GetDbSet(context).Add(newItem);
            }
            context.SaveChanges();
        }

        //ABSTRACTS
        //Retrieve the DbSet to store elements into
        abstract public DbSet<TModel> GetDbSet(PizzeriaContext context);
        //Logic to translate raw data into new elements
        abstract public TModel GenerateElementFromRawData(TRawData rawData);
    }

    //Utility implementation for translationsless seeders
    public abstract class Seeder<TRawData> : Seeder<TRawData, TRawData> where TRawData : class
    {
        protected Seeder(List<TRawData> rawList) : base(rawList)
        { }

        public override TRawData GenerateElementFromRawData(TRawData data)
        {
            return data;
        }
    }
}
