using il_mio_fotoalbum.Models;
using Microsoft.EntityFrameworkCore;

namespace il_mio_fotoalbum.Seeders
{
    public class PizzaSeeder : Seeder<Pizza>
    {

        public PizzaSeeder() : base(new()
        {
            new ("Margherita", "La classica margherita", 8, true),
            new ("Quattro Formaggi", "Gorgonzola, bufala, ricotta e cacio", 9.5),
            new ("Prataiola", "Solo verdure e ingredienti bio!", 10.5),
            new ("Spicy Salmon", "Ispirata al famoso sushi roll", 11),
            new ("Capricciosa", "Il nostro cavallo di battaglia", 9.5),
            new ("Diavola", "La mia preferita :p", 9, true),
            new ("Vienna", null, 9, false),
        })
        { }

        readonly Random random = new Random();
        public override Pizza GenerateElementFromRawData(Pizza data)
        {
            // Attach category if available
            var categories = context.Categories.ToList();
            if (categories.Count() > 0)
            {
                int randomCategoryId = random.Next(categories.Count() - 1);
                data.CategoryId = categories[randomCategoryId].CategoryId;
            }

            // Attach ingredients if available
            var ingredients = context.Ingredients.ToList();
            if (ingredients.Count() > 0)
            {
                int numberOfIngredients = random.Next(2, ingredients.Count() / 3);
                for (int i = 0; i < numberOfIngredients; i++)
                {
                    Ingredient randomIngredient;

                    do
                    {
                        int randomIngredientId = random.Next(ingredients.Count() - 1);
                        randomIngredient = ingredients[randomIngredientId];
                    } while (data.Ingredients.Any(ingredient => ingredient == randomIngredient));

                    data.Ingredients.Add(randomIngredient);
                }
            }

            return data;
        }

        public override DbSet<Pizza> GetDbSet(PizzeriaContext context)
        {
            return context.Pizzas;
        }
    }
}
