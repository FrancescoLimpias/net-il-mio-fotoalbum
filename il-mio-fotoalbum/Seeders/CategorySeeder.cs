using il_mio_fotoalbum.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace il_mio_fotoalbum.Seeders
{
    public class CategorySeeder : Seeder<ValueTuple<string, string>, Category>
    {
        public CategorySeeder() : base(new()
            {
                ("Classica", "<i class=\"fa-solid fa-pizza-slice\" style=\"color: #d04949;\"></i>"),
                ("Bianca", "<i class=\"fa-solid fa-pizza-slice\" style=\"color: #ecd3a7;\"></i>"),
                ("Vegana", "<i class=\"fa-solid fa-seedling\" style=\"color: #1f5137;\"></i>"),
                ("Marittima", "<i class=\"fa-solid fa-fish\" style=\"color: #2e80b2;\"></i>"),
            })
        { }

        public override DbSet<Category> GetDbSet(PizzeriaContext context)
        {
            return context.Categories;
        }

        public override Category GenerateElementFromRawData(ValueTuple<string, string> rawData)
        {
            return new()
            {
                Name = rawData.Item1,
                Symbol = rawData.Item2,
            };
        }
    }
}
