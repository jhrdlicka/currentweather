using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using currentweather.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;

namespace currentweather.Controllers
{
    [AllowAnonymous]
    //    [Authorize(Policy = "PCMUsersOnly")]
    [Authorize]
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class doc_documentController : ControllerBase
    {
        private readonly CurrentWeatherContext _context;

        public doc_documentController(CurrentWeatherContext context)
        {
            _context = context;
        }

        // GET: api/doc_document
        [HttpGet]
        public async Task<ActionResult<IEnumerable<doc_document>>> Getdoc_document()
        {
            return await _context.doc_document.ToListAsync();
        }

        // GET: api/doc_document/5
        [HttpGet("{id}")]
        public async Task<ActionResult<doc_document>> Getdoc_document(long id)
        {
            var doc_document = await _context.doc_document.FindAsync(id);

            if (doc_document == null)
            {
                return NotFound();
            }

            return doc_document;
        }

        // PUT: api/doc_document/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> Putdoc_document(long id, doc_document doc_document)
        {
            if (id != doc_document.id)
            {
                return BadRequest();
            }

            _context.Entry(doc_document).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!doc_documentExists(id))
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

        // POST: api/doc_document
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<doc_document>> Postdoc_document(doc_document doc_document)
        {
            _context.doc_document.Add(doc_document);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getdoc_document", new { id = doc_document.id }, doc_document);
        }

        // DELETE: api/doc_document/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<doc_document>> Deletedoc_document(long id)
        {
            var doc_document = await _context.doc_document.FindAsync(id);
            if (doc_document == null)
            {
                return NotFound();
            }

            _context.doc_document.Remove(doc_document);
            await _context.SaveChangesAsync();

            return doc_document;
        }

        private bool doc_documentExists(long id)
        {
            return _context.doc_document.Any(e => e.id == id);
        }
    }
}
