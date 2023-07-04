
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsAPI.Data.Contexts;
using NewsAppClasses;
using NewsAppClasses.Dtos;

namespace NewsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly MainDBContext _context;

        public NewsController(MainDBContext context)
        {
            _context = context;
        }

        // GET: api/News
        [HttpGet]
        public async Task<ActionResult<IEnumerable<News>>> GetNews()
        {
          if (_context.News == null)
          {
              return NotFound();
          }
            return await _context.News.Include(n=>n.Author).ToListAsync();
        }

        // GET: api/News/5
        [HttpGet("{id}")]
        public async Task<ActionResult<News>> GetNews(int id)
        {
          if (_context.News == null)
          {
              return NotFound();
          }
            var news = await _context.News.Include(n => n.Author).FirstAsync(n=>n.Id==id);

            if (news == null)
            {
                return NotFound();
            }

            return news;
        }

        // PUT: api/News/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Policy = "AllowAdmins")]
        public async Task<IActionResult> PutNews(int? id, NewsUpdateDto news)
        {
            if (id != news.Id)
            {
                return BadRequest();
            }
            try
            {

                var dbNews = _context.News.Find(id);
                dbNews.Title=news.Title;
                dbNews.PublicationDate=news.PublicationDate;
                dbNews.NewsArticle = news.NewsArticle;
                dbNews.Author.Id = news.AuthorID;
                dbNews.Image=news.Image;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewsExists(news.Id))
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

        // POST: api/News
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Policy = "AllowAdmins")]
        public async Task<ActionResult<News>> PostNews(NewsWriteDto news)
        {
            

            Author? author = _context.Authors.Find(news.AuthorID);
            if (author == null)
                return Problem("Author's id doesn't exist");

            News newess = new News() {Title=news.Title,
                NewsArticle=news.NewsArticle,
                PublicationDate=news.PublicationDate,
                Image=news.Image,
                Author = author

            };
            _context.News.Add(newess);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNews", new { id = newess.Id }, news);
        }

        // DELETE: api/News/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "AllowAdmins")]
        public async Task<IActionResult> DeleteNews(int id)
        {
            if (_context.News == null)
            {
                return NotFound();
            }
            var news = await _context.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }

            _context.News.Remove(news);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NewsExists(int id)
        {
            return (_context.News?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
