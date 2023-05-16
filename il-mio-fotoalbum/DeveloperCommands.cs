using il_mio_fotoalbum.Models;
using il_mio_fotoalbum.Models.Utility;
using il_mio_fotoalbum.Seeders;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace il_mio_fotoalbum
{
    public static class DeveloperCommands
    {

        private static readonly PizzeriaContext context = new();

        // Developer Commands
        public static List<DeveloperCommandsGroup> Groups = new()
        {
            // General section
            new("General", new()
            {
                    
                // All database populated
                new(
                    "Are all database tables populated?",
                    () =>
                    {
                        return
                            context.Categories.Count() > 0
                            && context.Ingredients.Count() > 0
                            && context.Pizzas.Count() > 0;
                    }),

            }),

            // Seeders sections
            GetCommandGroupForModel<Category, CategorySeeder>(context.Categories),
            GetCommandGroupForModel<Ingredient, IngredientSeeder>(context.Ingredients),
            GetCommandGroupForModel<Pizza, PizzaSeeder>(context.Pizzas),
        };

        private static DeveloperCommandsGroup GetCommandGroupForModel<TModel, TSeeder>(DbSet<TModel> dbSet, string? groupName = null) where TModel : class where TSeeder : ISeeder, new()
        {
            // Determine group name            
            groupName ??= "Model: " + typeof(TModel).Name;

            // Instantiate seeder
            TSeeder seeder = new();

            return new(groupName, new()
                {
                    //population command
                    new("is db table populated?", () =>
                        {
                            return dbSet.Count() > 0;
                        }),

                    //refresh command
                    new("refresh db table", () =>
                        {
                            dbSet.ExecuteDelete();
                            context.SaveChanges();

                            seeder.Seed();
                            return true;
                        }),

                    //clear command
                    new ("clear db table", () =>
                        {
                            dbSet.ExecuteDelete();
                            context.SaveChanges();
                            return true;
                        }),

                    //seed command  
                    new ("seed db table", () =>
                        {
                            seeder.Seed();
                            return true;
                        }),
                }
            );
        }

    }
}