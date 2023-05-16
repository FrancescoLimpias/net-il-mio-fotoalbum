using il_mio_fotoalbum.Models;
using Microsoft.EntityFrameworkCore;

namespace il_mio_fotoalbum.Seeders
{
    public class IngredientSeeder : Seeder<ValueTuple<string, string, bool>, Ingredient>
    {

        public IngredientSeeder() : base(new()
            {
                ("Pomodoro", "🍅", false),
                ("Mozzarella", "🧀", true),
                ("Funghi", "🍄", false),
                ("Prosciutto", "🍖", true),
                ("Salame", "🐖", true),
                ("Peperoni", "🌶️", false),
                ("Cipolla", "🧅", false),
                ("Salsiccia", "🌭", true),
                ("Carciofi", "🌿", false),
                ("Basilico", "🌿", false),
                ("Origano", "🍃", false),
                ("Pepperoncino", "🌶", false),
                ("Ananas", "🍍", false),
                ("Gamberetti", "🍤", true),
                ("Tonno", "🐟", true),
                ("Melanzane", "🍆", false),
                ("Zucchine", "🥒", false),
                ("Mais", "🌽", false),
                ("Patate", "🥔", false),
                ("Bacon", "🥓", true),
                ("Patatine", "🍟", false),
            })
        { }

        public override DbSet<Ingredient> GetDbSet(PizzeriaContext context)
        {
            return context.Ingredients;
        }

        public override Ingredient GenerateElementFromRawData(ValueTuple<string, string, bool> rawData)
        {
            return new()
            {
                Name = rawData.Item1,
                Symbol = rawData.Item2,
                Allergen = rawData.Item3,
            };
        }
    }
}
