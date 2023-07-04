using Azure;
using Microsoft.AspNetCore.Mvc;
using NewsAppClasses;
using NewsAppClasses.Dtos;
using NewsWebApp.DataServices;
using NewsWebApp.Controllers.ActionFilters;
using Microsoft.AspNetCore.Mvc.Filters;


namespace NewsWebApp.Areas.Admin.Controllers
{

    
    public class AuthorsController : Controller
    {

        private readonly IAuthorRepoServiceAPI authorRepo;
        private readonly ILogger<AuthorsController> logger;
        public AuthorsController(IAuthorRepoServiceAPI authorRepoService, ILogger<AuthorsController> logger)
        {
            authorRepo = authorRepoService;
            this.logger = logger;
        }

        // GET:Authors
        [AuthenticationFilter]
        public async Task<IActionResult> Index()
        { 
            var authors = await authorRepo.GetAll();
            if (authors == null)
                return  RedirectToAction("CustomError","Home", new { message ="Entity does't exist"});
            return View(authors);
        }
        public async Task<IActionResult> SortedIndex(string attribute)
        {
            var authors = await authorRepo.GetAll();
            if (authors == null)
                return RedirectToAction("CustomError", "Home", new { message = "Entity does't exist" });
            switch (attribute)
            {
                case "Name":
                    {
                        return View("Index", authors.OrderBy(n => n.Name));
                    }

                default:
                    {
                        return View("Index",authors);
                    }
            }

        }

        //GET: Authors/Details/5

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return RedirectToAction("CustomError", "Home", new { message = "Invalid data" });
            }

            var author = await authorRepo.Get((int)id);
         
           
            if (author == null)
            {
                return RedirectToAction("CustomError", "Home", new { message = "Author not found" });
            }

            return View(author);
        }

        // GET: Authors/Create
        [AuthenticationFilter]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Authors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthenticationFilter]
        public  ActionResult Create(AuthorWriteDto author)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    authorRepo.Add(author, HttpContext.Request.Cookies["token"]!);

                }
                catch(Exception ex)
                {
                    return RedirectToAction("CustomError", "Home", new { message = ex.Message });
                }
                
               
            }
            return RedirectToAction(nameof(Index));
        }

        // GET:Authors/Edit/5
        [AuthenticationFilter]
        public async Task<IActionResult> Edit(int? id)
        {
            Author? author;
            if (id == null )
            {
                return RedirectToAction("CustomError", "Home","Invalid data");
            }

            try
            {
                author = await authorRepo.Get((int)id);

            }
            catch (Exception ex)
            {
                return RedirectToAction("CustomError", "Home", new { message = ex.Message });
            }

            if (author == null)
            {
                return RedirectToAction("CustomError", "Home", new { message = "Author not found"});
            }
            return View(author);
        }

        // POST: Authors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthenticationFilter]
        public IActionResult Edit(int id, [Bind("Id,Name")] AuthorUpdateDto author)
        {
            if (id != author.Id)
            {
                return RedirectToAction("CustomError", "Home", "Invalid data");
            }

            if (ModelState.IsValid)
            {
                try
                {
                     authorRepo.Update(author, HttpContext.Request.Cookies["token"]!);
                     return RedirectToAction(nameof(Index));


                }
                catch (Exception ex)
                {
                    return RedirectToAction("CustomError", "Home", new { message = ex.Message });
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Authors/Delete/5
        [AuthenticationFilter]
        public async Task<IActionResult> Delete(int? id)
        {
            Author? author;
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                author = await authorRepo.Get((int)id);
            }catch(Exception ex)
            {
                return RedirectToAction("CustomError", "Home", new { message = ex.Message });
            }
            
            if (author == null)
            {
                return RedirectToAction("CustomError", "Home", new { message = "Author doesn't exist" });
            }
            return View(author);
        }

        // POST: Admin/Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthenticationFilter]
        public IActionResult DeleteConfirmed(int id)
        {

            bool success;
            try
            {
                success = authorRepo.Delete(id, HttpContext.Request.Cookies["token"]!);
            }
            catch (Exception ex)
            {
                return RedirectToAction("CustomError", "Home", new { message = ex.Message });
            }


            if (success)
                return RedirectToAction(nameof(Index));
            else
                return RedirectToAction("CustomError","Home", new { message = "Couldn't delete record" });
                
        }

        


    }
}
