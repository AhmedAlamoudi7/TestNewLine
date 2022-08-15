using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TestNewLine.Core.Dtos;
using TestNewLine.Infrastructure.Services;
using TestNewLine.Services;

namespace TestNewLine.Web.Controllers
{
    [Area("ControlPanel")]
    public class NotificationsController : BaseController
    {
        private readonly INotificationService _notificationService;
        public NotificationsController(INotificationService notificationService, IUserService userService) : base( userService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["auther"] = await _notificationService.GetNotificationAuthors(ViewBag.UserId);
            ViewData["Userss"] = new SelectList(await _notificationService.GetUserName(), "Id", "Email");
            return View();
        }
        //[HttpPost]
        //public async Task<IActionResult> Index(CreateNotificationDto dto)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        ViewData["auther"] = await _notificationService.GetNotificationAuthors(ViewBag.UserId);
        //        ViewData["Userss"] = new SelectList(await _notificationService.GetUserName(), "Id", "Email");
        //        await _notificationService.Create(dto);
        //        return Redirect("/Home/Index");
        //    }
        //    return View(dto);
        //}


    }
}