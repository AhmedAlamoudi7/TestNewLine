using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TestNewLine.Core.Dtos;
using TestNewLine.Core.ViewModel;
using TestNewLine.Core.ViewModels;
using TestNewLine.Data.Data;
using TestNewLine.Data.Models;
using TestNewLine.Exceptions;

namespace TestNewLine.Services
{
    public class AuthService : IAuthService
    {

        private readonly ApplicationDbContext _DB;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        // used just to access appsetting 
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration,RoleManager<IdentityRole> roleManager, ApplicationDbContext DB, IMapper mapper, UserManager<User> userManager)
        {
            _DB = DB;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }


        public async Task<LoginResponseViewModel> Login(LoginDto dto)
        {
            var user = _DB.Users.SingleOrDefault(x => x.UserName == dto.Username && !x.IsDelete);

            if(user == null)
            {
                throw new InvalidUsernameOrPasswordException();
            }

            var result = await _userManager.CheckPasswordAsync(user, dto.Password);

            if (!result)
            {
                throw new InvalidUsernameOrPasswordException();
            }

            var accessToken = await GenrateAccessToken(user);
            var userVm = _mapper.Map<UserViewModel>(user);

            return new LoginResponseViewModel()
            {
                AccessToken = accessToken,
                User = userVm
            };
        }
        // we use find instead of first or default cuz we take the user id from token from base controller
        public async Task<bool> ChangePassword(string userId,ChangePasswordDto dto)
        {
            var user = _DB.Users.Find(userId);
            var result = await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);
            return result.Succeeded;
        }

        private async Task<string> GenrateAccessToken(User user)
        {
            // GetRolesAsync : used to get user and back all Roles to this user   
            var roles = await _userManager.GetRolesAsync(user);

            /* Diffrent be tween Claims && Role
             * Claims : هو عبارة البيانات للمستخدم مش صلاحية يعني ما بقدر يعطي صلاحية للمستخدم
             * Role: بتعطي صلاحية للمستخدم زي الأدمن 
             */
            var claims = new List<Claim>(){
              new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
              new Claim(JwtRegisteredClaimNames.Email, user.Email),
              new Claim("UserId",user.Id),
              new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
             };
            /* if Role to user was Found add roles to claims 
             * join used : if user have more than Roles 
             */
            if (roles.Any())
            {
                claims.Add(new Claim(ClaimTypes.Role, string.Join(",", roles)));
            }


            var expires = DateTime.Now.AddMonths(1);
            // key encrypted: Jwt there the name in appsetting Signinkey
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Signinkey"]));
            // SecurityAlgorithms.HmacSha256 : its thw type of encrypted we can choose wat ever you want
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            var accessToken = new JwtSecurityToken(_configuration["Jwt:Issure"],
                _configuration["Jwt:Site"],
                claims,
                expires: expires,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(accessToken);
        }



    }
}
