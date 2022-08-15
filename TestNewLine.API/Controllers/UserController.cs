using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestNewLine.API.Controllers;
using TestNewLine.Core.Dtos;
using TestNewLine.Services;

namespace CMS.API.Controllers
{
    
    public class UserController : BaseController
    {

        public UserController(IUserService userService) : base(userService)
        {
        }

        [HttpGet]
        public IActionResult GetAll()
        {
           var users =  _userService.GetUsers();
           return Ok(GetRespons(users,TestNewLine.Core.Constants.Results.GetSuccessResult()));
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateUserDto dto)
        {
            await _userService.Create(dto);
            return Ok(GetRespons(TestNewLine.Core.Constants.Results.AddSuccessResult()));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody]UpdateUserDto dto)
        {
            await _userService.Update(dto);
            return Ok(GetRespons(TestNewLine.Core.Constants.Results.EditSuccessResult()));
        }

        [HttpDelete]
        public IActionResult Delete(string id)
        {
            _userService.Delete(id);
            return Ok(GetRespons(TestNewLine.Core.Constants.Results.DeleteSuccessResult()));
        }

    }
}
