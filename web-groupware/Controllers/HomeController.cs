using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using web_groupware.Models;
using Microsoft.AspNetCore.Authorization;
using web_groupware.Data;

namespace web_groupware.Controllers
{
    public class HomeController : BaseController
    {

        public HomeController(IConfiguration configuration, ILogger<BaseController> logger, web_groupwareContext context, IWebHostEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor) : base(configuration, logger, context, hostingEnvironment, httpContextAccessor) { 
        }
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }
        //close colorbox and reload parent window
        [Authorize]
        public IActionResult Back_to_parent()
        {
            return View();
        }


    }
}