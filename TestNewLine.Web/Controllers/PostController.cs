using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TestNewLine.Web.Services;
using TestNewLine.Services;
using TestNewLine.Core.Dtos;
using TestNewLine.Infrastructure.Services;

namespace TestNewLine.Web.Controllers
{
    public class PostController : BaseController
    {
        private readonly IPostBlogService _IPostBlogService;
        private readonly INotificationService _notificationService;

        public PostController(INotificationService notificationService, IPostBlogService IPostBlogService, IUserService userService) : base(userService)
        {
            _IPostBlogService = IPostBlogService;
            _notificationService = notificationService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var post = await _IPostBlogService.GetAll2();
            return View(post);
        }
        public async Task<JsonResult> GetPostData(Pagination pagination, Query query)
        {
            var result = await _IPostBlogService.GetAll(pagination, query);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["auther"] = await _IPostBlogService.GetPostAuthors(ViewBag.UserId);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreatePostBlogDto dto)
        {

            if (ModelState.IsValid)
            {

                ViewData["auther"] = await _IPostBlogService.GetPostAuthors(ViewBag.UserId);
                await _IPostBlogService.Create(dto);

                ViewData["auther"] = await _notificationService.GetNotificationAuthors(ViewBag.UserId);
                ViewData["Userss"] = new SelectList(await _notificationService.GetUserName(), "Id", "Email");
                var noti = new CreateNotificationDto();
                noti.Title = dto.Title;
                noti.UserTo = ViewBag.auther.Id;
                noti.UserFrom = ViewBag.auther.Id;
                noti.isCheked = false;
                noti.Href = "";
                noti.Author = ViewBag.auther.Id;
                //_notificationService.Create(noti);

                return Ok(TestNewLine.Core.Constants.Results.AddSuccessResult());
            }
            return View(dto);
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            ViewData["auther"] = await _IPostBlogService.GetPostAuthors(ViewBag.UserId);

            var post = await _IPostBlogService.Get(id);
            return View(post);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdatePostBlogDto dto)
        {
            if (ModelState.IsValid)
            {
                ViewData["auther"] = await _IPostBlogService.GetPostAuthors(ViewBag.UserId);
                await _IPostBlogService.Update(dto);
                return Ok(TestNewLine.Core.Constants.Results.EditSuccessResult());
            }
            return View(dto);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _IPostBlogService.Delete(id);
            return Ok(TestNewLine.Core.Constants.Results.DeleteSuccessResult());
        }


    }
}
