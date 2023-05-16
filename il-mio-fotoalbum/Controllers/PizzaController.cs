using il_mio_fotoalbum.Models;
using il_mio_fotoalbum.Seeders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace il_mio_fotoalbum.Controllers;
public class PizzaController : Controller
{

    PizzeriaContext context = new PizzeriaContext();

    // GET: PizzaController
    public ActionResult Index()
    {
        return View(
            context.Pizzas
            .Include(pizza => pizza.Category)
            .Include(pizza => pizza.Ingredients)
            .ToList()
            );
    }

    // GET: PizzaController/Details/5
    public ActionResult Details(long id)
    {
        return View(
            context.Pizzas
            .Where(pizza => pizza.PizzaId == id)
            .Include(pizza => pizza.Category)
            .Include(pizza => pizza.Ingredients)
            .FirstOrDefault());
    }

    // GET: PizzaController/Create
    [Authorize(Roles = "ADMIN")]
    public ActionResult Create()
    {
        return View(new PizzaPayload()
        {
            Pizza = new(),
            Categories = context.Categories.ToList(),
            Ingredients = RetrieveIngredientsSelectList(),
        });
    }

    // POST: PizzaController/Create
    [Authorize(Roles = "ADMIN")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(Pizza data, [FromForm] List<string> SelectedIngredients)
    {

        if (!ModelState.IsValid)
        {
            return View("Create", new PizzaPayload()
            {
                Pizza = data,
                Categories = context.Categories.ToList(),
                Ingredients = RetrieveIngredientsSelectList(),
            });
        }

        try
        {
            // attach ingredients
            foreach (var selectedIngredientStringId in SelectedIngredients)
            {
                int selectedIngredientId = int.Parse(selectedIngredientStringId);
                Ingredient selectedIngredient = context.Ingredients.Where(ingredient => ingredient.IngredientId == selectedIngredientId).First();
                data.Ingredients.Add(selectedIngredient);
            }

            // attempt to save the pizza
            context.Pizzas.Add(data);
            context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            return RedirectToAction(nameof(Create));
        }
    }

    // GET: PizzaController/Edit/5
    [Authorize(Roles = "ADMIN")]
    public ActionResult Edit(long id)
    {
        Pizza? searchedPizza =
            context.Pizzas
            .Include(pizza => pizza.Ingredients)
            .Where(pizza => pizza.PizzaId == id)
            .FirstOrDefault();

        if (searchedPizza == null)
            return NotFound();

        return View(new PizzaPayload()
        {
            Pizza = searchedPizza,
            Categories = context.Categories.ToList(),
            Ingredients = RetrieveIngredientsSelectList(searchedPizza),
        });
    }

    // POST: PizzaController/Edit/5
    [Authorize(Roles = "ADMIN")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(long id, Pizza data, [FromForm] List<string> SelectedIngredients)
    {
        Pizza? searchedPizza =
        context.Pizzas
        .Include(pizza => pizza.Ingredients)
        .Where(pizza => pizza.PizzaId == id)
        .FirstOrDefault();

        if (searchedPizza == null)
            return NotFound();

        if (!ModelState.IsValid)
        {

            return View("Edit", new PizzaPayload()
            {
                Pizza = data,
                Categories = context.Categories.ToList(),
                Ingredients = RetrieveIngredientsSelectList(searchedPizza),
            });
        }

        try
        {

            // update pizza's fields
            searchedPizza.Name = data.Name;
            searchedPizza.Description = data.Description;
            searchedPizza.Price = data.Price;
            searchedPizza.CategoryId = data.CategoryId;

            // sync ingredients
            searchedPizza.Ingredients.Clear();
            foreach (var selectedIngredientStringId in SelectedIngredients)
            {
                int selectedIngredientId = int.Parse(selectedIngredientStringId);
                Ingredient selectedIngredient = context.Ingredients.Where(ingredient => ingredient.IngredientId == selectedIngredientId).First();
                searchedPizza.Ingredients.Add(selectedIngredient);
            }

            // save
            context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return NotFound();
        }
    }

    // GET: PizzaController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "ADMIN")]
    public ActionResult Delete(long id)
    {
        Pizza? pizzaToDelete = context.Pizzas.Find(id);

        // nullity check
        if (pizzaToDelete == null)
            return NotFound();

        context.Pizzas.Remove(pizzaToDelete);
        context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    // Retrieve Ingredients Select list
    private List<SelectListItem> RetrieveIngredientsSelectList(Pizza? pizzaToEdit = null)
    {
        List<SelectListItem> selectListItems = new();

        var ingredients = context.Ingredients;
        foreach (var ingredient in ingredients)
        {
            selectListItems.Add(new()
            {
                Text = $"{ingredient.Name} {ingredient.Symbol}",
                Value = ingredient.IngredientId.ToString(),
                Selected = pizzaToEdit != null
                           && pizzaToEdit.Ingredients
                            .Any(pizzaIngredient => ingredient.IngredientId == pizzaIngredient.IngredientId),
            });
        }

        return selectListItems;
    }
}
