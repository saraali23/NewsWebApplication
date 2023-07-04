using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsAPI.Data.Contexts;
using NewsAppClasses;
using NewsAppClasses.Dtos;

namespace NewsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly MainDBContext _context;

        public AuthorsController(MainDBContext context)
        {
            _context = context;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        {
          if (_context.Authors == null)
          {
              return NotFound();
          }
            return await _context.Authors.ToListAsync();
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetAuthor(int id)
        {
          if (_context.Authors == null)
          {
              return NotFound();
          }
            var author = await _context.Authors.FindAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            return author;
        }

        // PUT: api/Authors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Policy = "AllowAdmins")]
        public async Task<IActionResult> PutAuthor(int id, AuthorUpdateDto author)
        {
            if (id != author.Id)
            {
                return BadRequest();
            }

            var auth = _context.Authors.Find(id);
            
            if(auth!=null)
            {
                auth.Name = author.Name;
                await _context.SaveChangesAsync();
            }
            else
            {
                return NotFound();
            }
                    
            return NoContent();
        }

        // POST: api/Authors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Policy = "AllowAdmins")]
        public async Task<ActionResult> PostAuthor(AuthorWriteDto author)
        {
          if (_context.Authors == null)
          {
              return Problem("Entity set 'MainDBContext.Authors'  is null.");
          }
           Author auth=new Author() { Name= author.Name };
           _context.Authors.Add(auth);
           await _context.SaveChangesAsync();

           return Ok();
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "AllowAdmins")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            if (_context.Authors == null)
            {
                return NotFound();
            }
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
