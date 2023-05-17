using il_mio_fotoalbum.Models;
using il_mio_fotoalbum.Models.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace il_mio_fotoalbum.Controllers
{

    [Authorize(Roles = "CREATOR")]
    public class CategoryController : Controller
    {

        readonly PhotoAlbumContext context = new();

        // GET: PhotoController
        public ActionResult Index(string? searchedWord)
        {
            return View(new CategoryPayload()
            {
                NewCategory = new Category(),
                Categories = context.Categories
                .Where(category =>
                    searchedWord == null
                    || category.Name.ToLower().StartsWith(searchedWord.ToLower()))
                .ToList(),
            });
        }

        // POST: PhotoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Prefix = "Item2")] Category newCategory)
        {

            if (!ModelState.IsValid)
            {
                return View("Index", new CategoryPayload()
                {
                    NewCategory = newCategory,
                    Categories = context.Categories.ToList(),
                });
            }

            try
            {

                // attempt to save the pizza
                context.Categories.Add(newCategory);
                context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: PhotoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(long id, [Bind(Prefix = "Item2")] Category formGenerateCategory)
        {
            Category? categoryToEdit =
            context.Categories
            .Where(category => category.CategoryId == id)
            .FirstOrDefault();

            if (categoryToEdit == null)
                return NotFound();

            if (!ModelState.IsValid)
                return View("Index", new CategoryPayload()
                {
                    NewCategory = new Category(),
                    Categories = context.Categories.Select(category =>
                        (category.CategoryId == id ? formGenerateCategory : category)
                    ).ToList(),
                }); ;

            try
            {

                // update category's fields
                categoryToEdit.Name = formGenerateCategory.Name;
                categoryToEdit.Symbol = formGenerateCategory.Symbol;
                categoryToEdit.Color = formGenerateCategory.Color;
                categoryToEdit.Prioritary = formGenerateCategory.Prioritary;

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
            Category? categoryToDelete = context.Categories.Find(id);

            // nullity check
            if (categoryToDelete == null)
                return NotFound();

            context.Categories.Remove(categoryToDelete);
            context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}