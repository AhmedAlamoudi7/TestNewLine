using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestNewLine.API.Controllers;
using TestNewLine.Core.Dtos;
using TestNewLine.Core.ViewModels;
using TestNewLine.Services;
using TestNewLine.Web.Services;

namespace CMS.API.Controllers
{
    
    public class PostController : BaseController
    {
        private IPostBlogService _IPostBlogService;

        public PostController(IPostBlogService IPostBlogService, IUserService userService) : base(userService)
        {
            _IPostBlogService = IPostBlogService;
        }

        [HttpGet]
        public IActionResult GetAll(int page =1)
        {
           var post =  _IPostBlogService.GetAll(page);
           return Ok(GetRespons(post,TestNewLine.Core.Constants.Results.GetSuccessResult()));
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromForm]CreatePostBlogDto dto)
        {
            UserViewModel userID = await _IPostBlogService.GetPostAuthors(ViewBag.UserId);
            dto.AuthorId = userID.Id;
            await _IPostBlogService.Create(dto);
            return Ok(GetRespons(TestNewLine.Core.Constants.Results.AddSuccessResult()));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody]UpdatePostBlogDto dto)
        {
            await _IPostBlogService.Update(dto);
            return Ok(GetRespons(TestNewLine.Core.Constants.Results.EditSuccessResult()));
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            _IPostBlogService.Delete(id);
            return Ok(GetRespons(TestNewLine.Core.Constants.Results.DeleteSuccessResult()));
        }
        [HttpGet]
        public IActionResult getSinglePost(int id)
        {
           var post = _IPostBlogService.GetSingle(id);
            return Ok(GetRespons(post,TestNewLine.Core.Constants.Results.GetSuccessResult()));
        }
    }
}
