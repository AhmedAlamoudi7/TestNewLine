using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TestNewLine.Core.ViewModel;
using TestNewLine.Data.Models;
using TestNewLine.Services;
 using System.Security.Claims;

namespace TestNewLine.API.Controllers
{

    [Route("api/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BaseController : Controller

    {
        protected readonly IUserService _userService;
        private readonly UserManager<User> _userManager;

        public BaseController(IUserService userService)
        {
            _userService = userService;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userService.GetUserByName(userId);
            ViewBag.FullName = user.FullName;
            ViewBag.UserType = user.UserType;
            ViewBag.UserImg = user.ImageUrl;
            ViewBag.UserId = user.Id;
        }

        protected APIResponseViewModel GetRespons(object data = null, string message = "Done")
        {
            var result = new APIResponseViewModel();
            result.Status = true;
            result.Message = message;
            result.Data = data;
            return result;
        }
    }
}
