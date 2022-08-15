using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using TestNewLine.Services;
using TestNewLine.Web.Controllers;
using System.Reflection.Metadata;
using Microsoft.VisualBasic;
using TestNewLine.Core.Dtos;

namespace Tyaf.Web.Controllers
{
    public class UserController : BaseController
    {


        public UserController(IUserService userService) : base(userService)
        {
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GetUserData(Pagination pagination,Query query)
        {
            var result = await _userService.GetAll(pagination, query);
            return  Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserDto dto)
        {
            if (ModelState.IsValid)
            {
                await _userService.Create(dto);
                return Ok(TestNewLine.Core.Constants.Results.AddSuccessResult());
            }
            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var user = await _userService.Get(id);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromForm] UpdateUserDto dto)
        {
            if (ModelState.IsValid)
            {
                await _userService.Update(dto);
                return Ok(TestNewLine.Core.Constants.Results.EditSuccessResult());
            }
            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            await _userService.Delete(id);
            return Ok(TestNewLine.Core.Constants.Results.DeleteSuccessResult());
        }

     

    }
}
