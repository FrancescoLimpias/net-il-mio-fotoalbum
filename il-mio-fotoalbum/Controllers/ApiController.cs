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
    public class ApiController : ControllerBase
    {
        private readonly PhotoAlbumContext context;

        public ApiController(PhotoAlbumContext context)
        {
            this.context = context;
        }

        // GET: api/APIPhoto
        [HttpGet]
        public ActionResult Index(string? searchedWord)
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
        /*[HttpPost]
        public async Task<ActionResult<Photo>> PostPhoto(Photo photo)
        {
            if (context.Photos == null)
            {
                return Problem("Entity set 'PhotoAlbumContext.Photos'  is null.");
            }
            context.Photos.Add(photo);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetPhoto", new { id = photo.PhotoId }, photo);
        }*/
    }
}
