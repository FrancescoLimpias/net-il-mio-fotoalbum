using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using il_mio_fotoalbum;
using il_mio_fotoalbum.Models;
using il_mio_fotoalbum.Areas.Identity.Data;

namespace il_mio_fotoalbum.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GuestController : ControllerBase
    {
        private readonly PhotoAlbumContext context;

        public GuestController(PhotoAlbumContext context)
        {
            this.context = context;
        }

        // GET: api/APIPhoto
        [HttpGet]
        public ActionResult GetPhotos(string? searchedWord)
        {

            return Ok(
                context.Photos
                .Where(photo =>
                    !photo.Private
                    && (searchedWord == null || photo.Title.ToLower().StartsWith(searchedWord.ToLower())))
                .Include(photo => photo.Album)
                .Include(photo => photo.Categories)
                .ToList()
                );
        }

        // POST: api/APIPhoto
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult PostMessage(Message message)
        {

            context.Messages.Add(message);
            context.SaveChanges();

            return Ok();
        }
    }
}
