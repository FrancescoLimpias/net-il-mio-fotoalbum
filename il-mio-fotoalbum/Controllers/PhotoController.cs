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

    readonly PhotoAlbumContext context = new();

    // GET: PhotoController
    public ActionResult Index()
    {
        return View(
            context.Photos
            .Include(photo => photo.Album)
            .Include(photo => photo.Categories)
            .ToList()
            );
    }

    // GET: PhotoController/Details/5
    public ActionResult Details(long id)
    {
        return View(
            context.Photos
            .Where(photo => photo.PhotoId == id)
            .Include(photo => photo.Album)
            .Include(photo => photo.Categories)
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
            Categories = RetrieveCategoriesListItems(),
        });
    }

    // POST: PhotoController/Create
    [Authorize(Roles = "ADMIN")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(Photo newPhoto, [FromForm] List<string> SelectedCategories)
    {

        if (!ModelState.IsValid)
        {
            return View("Create", new PhotoPayload()
            {
                Photo = newPhoto,
                Albums = context.Albums.ToList(),
                Categories = RetrieveCategoriesListItems(),
            });
        }

        try
        {
            // attach categories
            foreach (var selectedCategoryStringId in SelectedCategories)
            {
                int selectedCategoryId = int.Parse(selectedCategoryStringId);
                Category selectedCategory = context.Categories.Where(category => category.CategoryId == selectedCategoryId).First();
                newPhoto.Categories.Add(selectedCategory);
            }

            // attempt to save the pizza
            context.Photos.Add(newPhoto);
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
        Photo? searchedPhoto =
            context.Photos
            .Include(photo => photo.Categories)
            .Where(photo => photo.PhotoId == id)
            .FirstOrDefault();

        if (searchedPhoto == null)
            return NotFound();

        return View(new PhotoPayload()
        {
            Photo = searchedPhoto,
            Albums = context.Albums.ToList(),
            Categories = RetrieveCategoriesListItems(searchedPhoto),
        });
    }

    // POST: PhotoController/Edit/5
    [Authorize(Roles = "ADMIN")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(long id, Photo formGeneratedPhoto, [FromForm] List<string> SelectedCategories)
    {
        Photo? photoToEdit =
        context.Photos
        .Include(photo => photo.Categories)
        .Where(photo => photo.PhotoId == id)
        .FirstOrDefault();

        if (photoToEdit == null)
            return NotFound();

        if (!ModelState.IsValid)
            return View("Edit", new PhotoPayload()
            {
                Photo = formGeneratedPhoto,
                Albums = context.Albums.ToList(),
                Categories = RetrieveCategoriesListItems(photoToEdit),
            });

        try
        {

            // update pizza's fields
            photoToEdit.Title = formGeneratedPhoto.Title;
            photoToEdit.Description = formGeneratedPhoto.Description;
            photoToEdit.Location = formGeneratedPhoto.Location;
            photoToEdit.Private = formGeneratedPhoto.Private;
            photoToEdit.AlbumId = formGeneratedPhoto.AlbumId;

            // sync categories
            photoToEdit.Categories.Clear();
            foreach (var selectedCategoryStringId in SelectedCategories)
            {
                int selectedCategoryId = int.Parse(selectedCategoryStringId);
                Category selectedCategory = context.Categories.Where(category => category.CategoryId == selectedCategoryId).First();
                photoToEdit.Categories.Add(selectedCategory);
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
        Photo? photoToDelete = context.Photos.Find(id);

        // nullity check
        if (photoToDelete == null)
            return NotFound();

        context.Photos.Remove(photoToDelete);
        context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    // Retrieve Albums Select list
    private List<SelectListItem> RetrieveCategoriesListItems(Photo? photoToEdit = null)
    {
        List<SelectListItem> selectListItems = new();

        var categories = context.Categories;
        foreach (var category in categories)
        {
            selectListItems.Add(new()
            {
                Text = $"{category.Name} {category.Symbol}",
                Value = category.CategoryId.ToString(),
                Selected = photoToEdit != null
                           && photoToEdit.Categories
                            .Any(photoCategory => category.CategoryId == photoCategory.CategoryId),
            });
        }

        return selectListItems;
    }
}
