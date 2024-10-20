using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VJN.Models;

namespace VJN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaItemsController : ControllerBase
    {
        private readonly VJNDBContext _context;

        public MediaItemsController(VJNDBContext context)
        {
            _context = context;
        }

        // GET: api/MediaItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MediaItem>>> GetMediaItems()
        {
          if (_context.MediaItems == null)
          {
              return NotFound();
          }
            return await _context.MediaItems.ToListAsync();
        }

        // GET: api/MediaItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MediaItem>> GetMediaItem(int id)
        {
          if (_context.MediaItems == null)
          {
              return NotFound();
          }
            var mediaItem = await _context.MediaItems.FindAsync(id);

            if (mediaItem == null)
            {
                return NotFound();
            }

            return mediaItem;
        }

        // PUT: api/MediaItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMediaItem(int id, MediaItem mediaItem)
        {
            if (id != mediaItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(mediaItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MediaItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/MediaItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MediaItem>> PostMediaItem(MediaItem mediaItem)
        {
          if (_context.MediaItems == null)
          {
              return Problem("Entity set 'VJNDBContext.MediaItems'  is null.");
          }
            _context.MediaItems.Add(mediaItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMediaItem", new { id = mediaItem.Id }, mediaItem);
        }

        // DELETE: api/MediaItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMediaItem(int id)
        {
            if (_context.MediaItems == null)
            {
                return NotFound();
            }
            var mediaItem = await _context.MediaItems.FindAsync(id);
            if (mediaItem == null)
            {
                return NotFound();
            }

            _context.MediaItems.Remove(mediaItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MediaItemExists(int id)
        {
            return (_context.MediaItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
