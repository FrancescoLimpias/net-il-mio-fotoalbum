using il_mio_fotoalbum.Models;
using il_mio_fotoalbum.Seeders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace il_mio_fotoalbum.Controllers;
public class PhotoController : Controller
{

    PhotoAlbumContext context = new PhotoAlbumContext();

    // GET: PhotoController
    public ActionResult Index()
    {
        return View(
            context.Photos
            .Include(pizza => pizza.Album)
            .Include(pizza => pizza.Categories)
            .ToList()
            );
    }

    // GET: PhotoController/Details/5
    public ActionResult Details(long id)
    {
        return View(
            context.Photos
            .Where(pizza => pizza.PhotoId == id)
            .Include(pizza => pizza.Album)
            .Include(pizza => pizza.Categories)
            .FirstOrDefault());
    }

    // GET: PhotoController/Create
    [Authorize(Roles = "ADMIN")]
    public ActionResult Create()
    {
        return View(new PhotoPayload()
        {
            Photo = new(),
            Albums = context.Albums.ToList(),
            Categories = RetrieveIngredientsSelectList(),
        });
    }

    // POST: PhotoController/Create
    [Authorize(Roles = "ADMIN")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(Photo data, [FromForm] List<string> SelectedIngredients)
    {

        if (!ModelState.IsValid)
        {
            return View("Create", new PhotoPayload()
            {
                Photo = data,
                Albums = context.Albums.ToList(),
                Categories = RetrieveIngredientsSelectList(),
            });
        }

        try
        {
            // attach ingredients
            foreach (var selectedIngredientStringId in SelectedIngredients)
            {
                int selectedIngredientId = int.Parse(selectedIngredientStringId);
                Category selectedIngredient = context.Categories.Where(ingredient => ingredient.CategoryId == selectedIngredientId).First();
                data.Categories.Add(selectedIngredient);
            }

            // attempt to save the pizza
            context.Photos.Add(data);
            context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            return RedirectToAction(nameof(Create));
        }
    }

    // GET: PhotoController/Edit/5
    [Authorize(Roles = "ADMIN")]
    public ActionResult Edit(long id)
    {
        Photo? searchedPizza =
            context.Photos
            .Include(pizza => pizza.Categories)
            .Where(pizza => pizza.PhotoId == id)
            .FirstOrDefault();

        if (searchedPizza == null)
            return NotFound();

        return View(new PhotoPayload()
        {
            Photo = searchedPizza,
            Albums = context.Albums.ToList(),
            Categories = RetrieveIngredientsSelectList(searchedPizza),
        });
    }

    // POST: PhotoController/Edit/5
    [Authorize(Roles = "ADMIN")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(long id, Photo data, [FromForm] List<string> SelectedIngredients)
    {
        Photo? searchedPizza =
        context.Photos
        .Include(pizza => pizza.Categories)
        .Where(pizza => pizza.PhotoId == id)
        .FirstOrDefault();

        if (searchedPizza == null)
            return NotFound();

        if (!ModelState.IsValid)
        {

            return View("Edit", new PhotoPayload()
            {
                Photo = data,
                Albums = context.Albums.ToList(),
                Categories = RetrieveIngredientsSelectList(searchedPizza),
            });
        }

        try
        {

            // update pizza's fields
            searchedPizza.Title = data.Title;
            searchedPizza.Description = data.Description;
            searchedPizza.AlbumId = data.AlbumId;

            // sync ingredients
            searchedPizza.Categories.Clear();
            foreach (var selectedIngredientStringId in SelectedIngredients)
            {
                int selectedIngredientId = int.Parse(selectedIngredientStringId);
                Category selectedIngredient = context.Categories.Where(ingredient => ingredient.CategoryId == selectedIngredientId).First();
                searchedPizza.Categories.Add(selectedIngredient);
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

    // GET: PhotoController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "ADMIN")]
    public ActionResult Delete(long id)
    {
        Photo? pizzaToDelete = context.Photos.Find(id);

        // nullity check
        if (pizzaToDelete == null)
            return NotFound();

        context.Photos.Remove(pizzaToDelete);
        context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    // Retrieve Albums Select list
    private List<SelectListItem> RetrieveIngredientsSelectList(Photo? pizzaToEdit = null)
    {
        List<SelectListItem> selectListItems = new();

        var ingredients = context.Categories;
        foreach (var ingredient in ingredients)
        {
            selectListItems.Add(new()
            {
                Text = $"{ingredient.Name} {ingredient.Symbol}",
                Value = ingredient.CategoryId.ToString(),
                Selected = pizzaToEdit != null
                           && pizzaToEdit.Categories
                            .Any(pizzaIngredient => ingredient.CategoryId == pizzaIngredient.CategoryId),
            });
        }

        return selectListItems;
    }
}
