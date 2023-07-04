
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NewsAppClasses;
using NewsAppClasses.Dtos;
using NewsWebApp.Controllers.ActionFilters;
using NewsWebApp.DataServices;
using NewsWebApp.Models;
using Newtonsoft.Json.Linq;
using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace NewsWebApp.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsRepoService newsRepo;
        private readonly IAuthorRepoServiceAPI authorRepo;
        private readonly ILogger<NewsController> logger;

        public NewsController(INewsRepoService newsRepoService,IAuthorRepoServiceAPI authorRepoServiceAPI, ILogger<NewsController> logger)
        {
            newsRepo = newsRepoService;
            authorRepo = authorRepoServiceAPI;
            this.logger = logger;
        }

        // GET: News
        public async Task<IActionResult> Index()
        {
            var news = await newsRepo.GetAll();
            if (news == null)
                return RedirectToAction("CustomError", "Home", new { message = "Entity does't exist" });

            ViewData["Authors"] = new SelectList(await authorRepo.GetAll(), "Name", "Name");
       
            return View(news);
        }
        public async Task<IActionResult> SortedIndex( string attribute)
        {
            var news = await newsRepo.GetAll();
            if (news == null)
                return RedirectToAction("CustomError", "Home", new { message = "Entity does't exist" });

            ViewData["Authors"] = new SelectList(await authorRepo.GetAll(), "Name", "Name");
            switch (attribute)
            {
                case "Title":
                    {
                        return View("Index", news.OrderBy(n => n.Title));
                       

                    }
                case "Author Name":
                    {
                        return View("Index", news.OrderBy(n => n.Author.Name));
                       
                    }
                case "Publication Date":
                    {
                        return View("Index", news.OrderBy(n => n.PublicationDate));
                      
                    }

                default:
                    {
                        return View("Index", news);
                       
                    }
            }

            
          
        }

        public async Task<IActionResult> FilteredIndex(string attribute,string value)
        {
            var news = await newsRepo.GetAll();
            if (news == null)
                return RedirectToAction("CustomError", "Home", new { message = "Entity does't exist" });

            ViewData["Authors"] = new SelectList(await authorRepo.GetAll(), "Name", "Name");
            switch (attribute)
            {
                case "Author Name":
                    {
                        return View("Index", news.Where(n=>n.Author.Name==value));

                    }

                default:
                    {
                        return View("Index", news);

                    }
            }

        }

        // GET: News/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return RedirectToAction("CustomError", "Home", new { message = "Entity does't exist" });
            }

            var news = await newsRepo.Get((int)id);


            if (news == null)
            {
                return RedirectToAction("CustomError", "Home", new { message = "News don't exist" });
            }

            return View(news);
        }

        // GET: News/Create
        [AuthenticationFilter]
        public async Task<IActionResult> Create()
        {
            try
            {
                ViewData["Authors"] = new SelectList(await authorRepo.GetAll(), "Id", "Name");
            }
            catch (Exception ex)
            {
                return RedirectToAction("CustomError", "Home", new { message = ex.Message });
            }

            return View();
        }

        // POST: News/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthenticationFilter]
        public async  Task<IActionResult> Create(News news,IFormFile ImageFile)
        {
            
            byte[]? newsImage = null;
             
            try
            {
                if (ModelState.IsValid)
                { 
                    newsImage = Helpers.ImageTransformationHelper.IFormImageToByteArray(ImageFile);
                    NewsWriteDto addNews = new NewsWriteDto(news.Title, news.NewsArticle, newsImage, news.PublicationDate, news.Author.Id);
                    newsRepo.Add(addNews, HttpContext.Request.Cookies["token"]!);

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewData["Authors"] = new SelectList(await authorRepo.GetAll(), "Id", "Name");
                    return View(news);
                }

            }
            catch(Exception ex)
            {
                return RedirectToAction("CustomError", "Home", new { message = ex.Message});
            }
           
            
           
        }

        // GET: News/Edit/5
        [AuthenticationFilter]
        public async Task<IActionResult> Edit(int? id)
        {
           
            try
            {
                if (id == null)
                {
                    throw new Exception("Invalid data");
                }
                var news = await newsRepo.Get((int)id);
                if (news == null)
                {
                    throw new Exception("News witht his Id don't exist");
                }
                ViewData["Authors"] = new SelectList(await authorRepo.GetAll(), "Id", "Name", news.Author.Id);
                return View(news);

            }
            catch(Exception ex)
            {
                var news = await newsRepo.Get((int)id);
                if (news == null)
                {
                    return RedirectToAction("CustomError", "Home", new { message = ex.Message });
                }
            }
            return View();



        }

        // POST: News/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthenticationFilter]
        public async Task<IActionResult> Edit(int id, News news, IFormFile ImageFile)
        {
                  
            byte[]? newsImage = null;
            if (ImageFile != null)
            {
                try
                {
                    newsImage = Helpers.ImageTransformationHelper.IFormImageToByteArray(ImageFile);
                    news.Image = newsImage;

                }
                catch (Exception ex)
                {
                    return RedirectToAction("CustomError", "Home", new { message = ex.Message });
                }
            }
            else
            {
                newsImage = news.Image;

            }

            if ( ModelState.ErrorCount<=1)
            {
                try
                {
                    NewsUpdateDto upNews = new NewsUpdateDto(news.Id, news.Title, news.NewsArticle, newsImage, news.PublicationDate, news.Author.Id);

                    bool success = await newsRepo.Update(upNews, HttpContext.Request.Cookies["token"]!);
                    if (success)
                    {
                        return RedirectToAction(nameof(Index));

                    }
                    else
                    {
                        return View(news);
                    }

                }
                catch (Exception ex)
                {
                    return RedirectToAction("CustomError", "Home", new { message = ex.Message });
                }
            }
            else
            {
                ViewData["Authors"] = new SelectList(await authorRepo.GetAll(), "Id", "Name");
                return View(news);
            }




        }

        // GET: News/Delete/5
        [AuthenticationFilter]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    throw new Exception("Invalid data");
                }

                var news = await newsRepo.Get((int)id);
                if (news == null)
                {
                    throw new Exception("News don't exist");
                }
                return View(news);

            }
            catch (Exception ex)
            {
                return RedirectToAction("CustomError", "Home", new { message = ex.Message });
            }

        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthenticationFilter]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                bool success = newsRepo.Delete(id, HttpContext.Request.Cookies["token"]!);

                if (success)
                    return RedirectToAction(nameof(Index));
                else
                    throw new Exception("Couldn't delete record");

            }
            catch (Exception ex)
            {
                return RedirectToAction("CustomError", "Home", new { message = ex.Message });
            }

        }

        
        
    }
}
