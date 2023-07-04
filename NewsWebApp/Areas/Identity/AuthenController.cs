using Microsoft.AspNetCore.Mvc;
using NewsAppClasses.Dtos;
using NewsWebApp.DataServices;
using NewsWebApp.Models;
using System.Diagnostics;

namespace NewsWebApp.Areas.Identity.Controllers
{
    public class AuthenController : Controller
    {
        private readonly ILogger<AuthenController> _logger;
        private readonly IAuthenRepoService authenRepo;

        public AuthenController(IAuthenRepoService authenRepoService,ILogger<AuthenController> logger)
        {
            _logger = logger;
            authenRepo = authenRepoService;
        }

        public async Task<IActionResult> Login(LogInDTO creds)
        {
            string? token;
            if (creds != null)
            {
                token=await authenRepo.Login(creds);
                if(token== "Username doesn't exist" || token=="Wrong password")
                {
                    return RedirectToAction("CustomError", "Home", new { message = token});
                }
                else if (token != null)
                {
                    Response.Cookies.Delete("token");
                    Response.Cookies.Append("token", token);
                }

                else RedirectToAction("CustomError", "Home", new { message = "Error in logging in , please try again" });

            }


            return RedirectToAction("Index", "News");
        }

        public  ActionResult Logout()
        {
            Response.Cookies.Delete("token");
            return RedirectToAction("Index", "News");
        }

       
    }
}