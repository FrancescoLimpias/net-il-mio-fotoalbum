using ExtensionMethods;
using il_mio_fotoalbum.Models;
using il_mio_fotoalbum.Seeders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Cryptography;

namespace il_mio_fotoalbum.Controllers;

[Authorize(Roles = "CREATOR")]
public class PhotoController : Controller
{

    readonly PhotoAlbumContext context = new();

    // GET: PhotoController
    public ActionResult Index(string? searchedWord)
    {
        return View(
            context.Photos
            .Where(photo =>
                searchedWord == null
                || photo.Title.ToLower().StartsWith(searchedWord.ToLower()))
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
    [HttpGet]
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
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create([Bind(Prefix = "Item2.Photo")] Photo newPhoto, [FromForm] List<string> SelectedCategories, IFormFile image)
    {

        if (!ModelState.IsValid || image.Length == 0)
        {
            return View("Create", new PhotoPayload()
            {
                Photo = newPhoto,
                Albums = context.Albums.ToList(),
                Categories = RetrieveCategoriesListItems(SelectedCategories),
            });
        }

        try
        {

            //Hash the file content
            string newHashedImageFileName = image.GetContentHash() + Path.GetExtension(image.FileName);
            newPhoto.Location = newHashedImageFileName;

            //Save the image file (with updated name)
            image.WriteToUploads(newHashedImageFileName);

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
    [HttpGet]
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
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(long id, [Bind(Prefix = "Item2.Photo")] Photo formGeneratedPhoto, [FromForm] List<string> SelectedCategories, IFormFile image)
    {
        Photo? photoToEdit =
        context.Photos
        .Include(photo => photo.Categories)
        .Where(photo => photo.PhotoId == id)
        .FirstOrDefault();

        if (photoToEdit == null)
            return NotFound();

        if (!ModelState.IsValid || image.Length == 0)
            return View("Edit", new PhotoPayload()
            {
                Photo = formGeneratedPhoto,
                Albums = context.Albums.ToList(),
                Categories = RetrieveCategoriesListItems(SelectedCategories),
            });

        try
        {

            //Hash the file content
            string newHashedImageFileName = image.GetContentHash() + Path.GetExtension(image.FileName);

            if (photoToEdit.Location != newHashedImageFileName)
            {
                DeleteIfNotReferencedByOthers(photoToEdit.Location);

                //Save the image file (with updated name)
                image.WriteToUploads(newHashedImageFileName);
            }

            // update pizza's fields
            photoToEdit.Title = formGeneratedPhoto.Title;
            photoToEdit.Description = formGeneratedPhoto.Description;
            photoToEdit.Location = newHashedImageFileName;
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
    public ActionResult Delete(long id)
    {
        Photo? photoToDelete = context.Photos.Find(id);

        // nullity check
        if (photoToDelete == null)
            return NotFound();

        DeleteIfNotReferencedByOthers(photoToDelete.Location);

        context.Photos.Remove(photoToDelete);
        context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    // Retrieve Albums Select list
    private List<SelectListItem> _RetrieveCategoriesListItems(Func<long, bool>? filter = null)
    {
        List<SelectListItem> selectListItems = new();

        var categories = context.Categories;
        foreach (var category in categories)
        {
            selectListItems.Add(new()
            {
                Text = $"{category.Name} {category.Symbol}",
                Value = category.CategoryId.ToString(),
                Selected = filter != null
                           && filter(category.CategoryId),
            });
        }

        return selectListItems;
    }

    private List<SelectListItem> RetrieveCategoriesListItems(Photo? photoToEdit = null)
    {
        if (photoToEdit == null)
            return _RetrieveCategoriesListItems();

        return _RetrieveCategoriesListItems((comparedId) =>
        {
            return photoToEdit.Categories.Any(photoCategory => photoCategory.CategoryId == comparedId);
        });
    }

    private List<SelectListItem> RetrieveCategoriesListItems(List<string> selectedCategories)
    {
        return _RetrieveCategoriesListItems((comparedId) =>
        {
            return selectedCategories.Any(photoCategory => Convert.ToInt64(photoCategory) == comparedId);
        });
    }

    //Photos
    private void DeleteIfNotReferencedByOthers(string FileName)
    {
        if (context.Photos.Where(photo => photo.Location == FileName).Count() == 1)
        {
            try
            {
                System.IO.File.Delete(Path.Combine("wwwroot/uploads/", FileName));
            }
            catch { }
        }
    }

}
