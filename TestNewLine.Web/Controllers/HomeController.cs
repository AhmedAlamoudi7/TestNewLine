using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TestNewLine.Services;
using TestNewLine.Data.Models;

namespace TestNewLine.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IUserService userService) : base(userService)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}