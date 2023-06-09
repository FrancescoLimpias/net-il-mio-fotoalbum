﻿using il_mio_fotoalbum.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace il_mio_fotoalbum.Controllers
{
    public class HomeController : Controller
    {

        readonly PhotoAlbumContext context = new();

        // GET: HomeController
        public ActionResult Index(string? searchedWord)
        {
            return View("Index", searchedWord);
        }

        // GET: HomeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
