using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestNewLine.Core.Dtos;
using TestNewLine.Data.Models;
using TestNewLine.Services;

namespace TestNewLine.API.Controllers
{
    public class AuthController : BaseController
    {
        private IAuthService _authService;

        public AuthController(IAuthService authService,IUserService userService) : base(userService)
        {
            _authService = authService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _authService.Login(dto);
            return Ok(result);
        }

  

    }
}
