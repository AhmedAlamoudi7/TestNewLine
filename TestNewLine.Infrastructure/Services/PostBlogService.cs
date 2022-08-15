using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestNewLine.Enums;
using TestNewLine.Exceptions;
using TestNewLine.Web.Services;
using PEATestNewLineEP.Core.Constants;
using TestNewLine.Data.Data;
using TestNewLine.Core.Dtos;
using TestNewLine.Core.ViewModels;
using TestNewLine.Data.Models;
using TestNewLine.Core.ViewModel;

namespace TestNewLine.Services
{
    public class PostBlogService : IPostBlogService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public PostBlogService( IFileService fileService, ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _fileService = fileService;
        }
        public async Task<ResponseDto> GetAll(Pagination pagination, Query query)
        {
            var queryString = _db.PostBlogs.Include(x => x.Author).Where(x => !x.IsDelete).AsQueryable();
            if (queryString == null)
            {
                throw new EntityNotFoundException();
            }
            var dataCount = queryString.Count();
            var skipValue = pagination.GetSkipValue();
            var dataList = await queryString.Skip(skipValue).Take(pagination.PerPage).ToListAsync();
            var posts = _mapper.Map<List<PostBlogViewModel>>(dataList);
            if (posts == null)
            {
                throw new EntityNotFoundException();
            }
            var pages = pagination.GetPages(dataCount);
            var result = new ResponseDto
            {
                data = posts,
                meta = new Meta
                {
                    page = pagination.Page,
                    perpage = pagination.PerPage,
                    pages = pages,
                    total = dataCount,
                }
            };
            return result;
        }

        public PagingViewModel GetAll(int page)
        {

            var pages = Math.Ceiling(_db.PostBlogs.Count() / 10.0);


            if (page < 1 || page > pages)
            {
                page = 1;
            }

            var skip = (page - 1) * 10;

            var posts = _db.PostBlogs.Include(x => x.Author)
                .Select(x => new PostBlogViewModel()
                {
                    Id = x.Id,
                    Title = x.Title,
                    SubTittle = x.SubTittle,
                    Body = x.Body,
                    AuthorId = x.Author.FullName                 
                }).Skip(skip).Take(10).ToList();

            var pagingResult = new PagingViewModel();
            pagingResult.Data = posts;
            pagingResult.NumberOfPages = (int)pages;
            pagingResult.CureentPage = page;

            return pagingResult;
        }

        public async Task<List<PostBlogViewModel>> GetAll2()
        {
            var queryString = await _db.PostBlogs.Include(x => x.Author).Where(x => !x.IsDelete).ToListAsync();
                 
            var Post = _mapper.Map<List<PostBlogViewModel>>(queryString);

            if (Post == null)
            {
                throw new EntityNotFoundException();
            }
            return Post;
        }
        public async Task<List<PostBlogViewModel>> GetSingle(int id)
        //{
        //    var post = await _db.PostBlogs.Include(x => x.Author).Where(x => !x.IsDelete).Select(x => new PostBlogViewModel()
        //    {
        //        Id = x.Id,
        //        AuthorId = x.AuthorId,
        //        Body = x.Body,
        //        CreatedAt = x.CreatedAt.ToString("yyyy:MM:dd"),
        //        Image = x.Image,
        //        SubTittle = x.SubTittle,
        //        Title = x.Title

        //    }).SingleOrDefaultAsync(x => x.Id == id);
        //    if (post == null)
        //    {
        //        throw new EntityNotFoundException();
        //    }

        //    return post;
        //}
        {
            var queryString = await _db.PostBlogs.Include(x => x.Author).Where(x => !x.IsDelete && x.Id ==id).ToListAsync();

            var Post = _mapper.Map<List<PostBlogViewModel>>(queryString);

            if (Post == null)
            {
                throw new EntityNotFoundException();
            }
            return Post;
        }
        public async Task<int> Delete(int id)
        {
            var post = await _db.PostBlogs.SingleOrDefaultAsync(x => x.Id == id && !x.IsDelete);
            if (post == null)
            {
                throw new EntityNotFoundException();
            }
            post.IsDelete = true;
            _db.PostBlogs.Update(post);
            await _db.SaveChangesAsync();
            return post.Id;
        }
        public async Task<UpdatePostBlogDto> Get(int id)
        {
            var post = await _db.PostBlogs.SingleOrDefaultAsync(x => x.Id == id && !x.IsDelete);
            if (post == null)
            {
                throw new EntityNotFoundException();
            }
            var dto = _mapper.Map<UpdatePostBlogDto>(post);
            if (dto == null)
            {
                throw new EntityNotFoundException();
            }
                  
            return dto;
        }

        public async Task<UserViewModel> GetPostAuthors(string Id)
        {
            var users = await _db.Users.SingleOrDefaultAsync(x => !x.IsDelete && x.Id.Equals(Id));
            if (users == null)
            {
                throw new EntityNotFoundException();
            }
            return _mapper.Map<UserViewModel>(users);
        }
      
     
        public async Task<int> Create(CreatePostBlogDto dto)
        {
            var post = _mapper.Map<PostBlog>(dto);
            if (post == null)
            {
                throw new EntityNotFoundException();
            }
            if (dto.Image != null)
            {
                post.Image = await _fileService.SaveFile(dto.Image, FolderNames.ImagesFolder);
            }
            await _db.PostBlogs.AddAsync(post);
            await _db.SaveChangesAsync();
            
            return post.Id;
        }
        public async Task<int> Update(UpdatePostBlogDto dto)
        {
            var post = await _db.PostBlogs.SingleOrDefaultAsync(x => x.Id == dto.Id && !x.IsDelete);
            if (post == null)
            {
                throw new EntityNotFoundException();
            }
            var updatedPost = _mapper.Map(dto, post);
            if (dto.Image != null)
            {
                updatedPost.Image = await _fileService.SaveFile(dto.Image, FolderNames.ImagesFolder);
            }

            _db.PostBlogs.Update(updatedPost);
            _db.SaveChanges();
           
            return updatedPost.Id;
        }
      

      
      
    }
}
