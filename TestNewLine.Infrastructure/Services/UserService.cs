using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PEATestNewLineEP.Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestNewLine.Core.Dtos;
using TestNewLine.Core.ViewModels;
using TestNewLine.Data.Data;
using TestNewLine.Data.Models;
using TestNewLine.Exceptions;

namespace TestNewLine.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserService( ApplicationDbContext db, IMapper mapper, UserManager<User> userManager, IFileService fileService, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _mapper = mapper;
            _userManager = userManager;
            _fileService = fileService;
            _roleManager = roleManager;
        }
        // mrthods to retub All Ptoparites in Base Controller to View In Cpanel
        public UserViewModel GetUserByName(string UserName)
        {
            var users = _userManager.Users.SingleOrDefault(x => x.UserName == UserName && !x.IsDelete);
            if (users == null)
            {
                throw new EntityNotFoundException();
            }
            return _mapper.Map<UserViewModel>(users);
        }
        public async Task<ResponseDto> GetAll(Pagination pagination, Query query)
        {
            var queryString = _db.Users.Where(x => !x.IsDelete && (x.FullName.Contains(query.GeneralSearch) || string.IsNullOrWhiteSpace(query.GeneralSearch) || x.Email.Contains(query.GeneralSearch) || x.PhoneNumber.Contains(query.GeneralSearch))).AsQueryable();
            if (queryString == null)
            {
                throw new EntityNotFoundException();
            }
            var dataCount = queryString.Count();
            var skipValue = pagination.GetSkipValue();
            var dataList = await queryString.Skip(skipValue).Take(pagination.PerPage).ToListAsync();
            var users = _mapper.Map<List<UserViewModel>>(dataList);
            if (users == null)
            {
                throw new EntityNotFoundException();
            }
            var pages = pagination.GetPages(dataCount);
            var result = new ResponseDto
            {
                data = users,
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
        public async Task<List<UserViewModel>> GetAllAPI(string serachKey)
        {
            var users = await _db.Users.Where(x => x.FullName.Contains(serachKey) || x.PhoneNumber.Contains(serachKey) || string.IsNullOrWhiteSpace(serachKey)).ToListAsync();
            if (users == null)
            {
                throw new EntityNotFoundException();
            }
            return _mapper.Map<List<UserViewModel>>(users);
        }
        public List<UserViewModel> GetUsers()
        {
            var users = _db.Users.Where(x => !x.IsDelete).Select(x => new UserViewModel()
            {
                Id = x.Id,
                FullName = x.FullName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                DOB = x.DOB
            }).ToList();

            return users;
        }
        public async Task<List<UserViewModel>> GetAll2()
        {
            var queryString = await _db.Users.Where(x => !x.IsDelete).ToListAsync();

            var Users = _mapper.Map<List<UserViewModel>>(queryString);
            if (Users == null)
            {
                throw new EntityNotFoundException();
            }
            return Users;
        }
        public List<UserViewModel> GetAll2Admin()
        {
            var queryString = _db.Users.Where(x => !x.IsDelete).Where(x => x.UserType == Enums.UserType.SuperAdmin ).ToList();

            var Users = _mapper.Map<List<UserViewModel>>(queryString);
            if (Users == null)
            {
                throw new EntityNotFoundException();
            }
            return Users;
        }
        public async Task<List<UserViewModel>> GetUserFullName()
        {
            var user = await _db.Users.Where(x => !x.IsDelete).ToListAsync();
            if (user == null)
            {
                throw new EntityNotFoundException();
            }
            return _mapper.Map<List<UserViewModel>>(user);
        }


        public List<UserViewModel> GetUserAdmin()
        {
            var user = _db.Users.Where(x => !x.IsDelete && x.UserType == Enums.UserType.SuperAdmin).ToList();
            if (user == null)
            {
                throw new EntityNotFoundException();
            }
            return _mapper.Map<List<UserViewModel>>(user);
        }
        public async Task<string> Create(CreateUserDto dto)
        {
            var emailOrPhoneIsExist = _db.Users.Any(x => !x.IsDelete && (x.Email == dto.Email || x.PhoneNumber == dto.PhoneNumber));
            if (emailOrPhoneIsExist)
            {
                throw new DuplicateEmailOrPhoneException();
            }
            var user = _mapper.Map<User>(dto);
            if (user == null)
            {
                throw new EntityNotFoundException();
            }
            user.UserName = dto.Email;
            if (dto.Image != null)
            {
                user.ImageUrl = await _fileService.SaveFile(dto.Image, FolderNames.ImagesFolder);
            }
            var password = GenratePassword();
            try
            {
                var result = await _userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    throw new OperationFailedException();
                }
                // to make Role To user by enum Constant and userManage
                            
                if (user.UserType == Enums.UserType.SuperAdmin)
                {
                    await _userManager.AddToRoleAsync(user, Enums.UserType.SuperAdmin.ToString());
                }
                return user.Id;
            }
            catch (Exception e)
            {
            }
            return user.Id;
        }
        public async Task<string> Update(UpdateUserDto dto)
        {
            var emailOrPhoneIsExist = _db.Users.Any(x => !x.IsDelete && (x.Email == dto.Email || x.PhoneNumber == dto.PhoneNumber && x.Id == dto.Id) && x.Id != dto.Id);
            if (emailOrPhoneIsExist)
            {
                throw new DuplicateEmailOrPhoneException();
            }
            var user = await _db.Users.FindAsync(dto.Id);
            var updatedUser = _mapper.Map<UpdateUserDto, User>(dto, user);
            if (dto.Image != null)
            {
                updatedUser.ImageUrl = await _fileService.SaveFile(dto.Image, FolderNames.ImagesFolder);
            }
            _db.Users.Update(updatedUser);
            await _db.SaveChangesAsync();
            await _userManager.AddToRoleAsync(user, user.UserType.ToString());
            return user.Id;
        }
        public async Task<string> Delete(string Id)
        {
            var user = await _db.Users.SingleOrDefaultAsync(x => x.Id == Id && !x.IsDelete);
            if (user == null)
            {
                throw new EntityNotFoundException();
            }
            user.IsDelete = true;
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
            return user.Id;
        }
        public async Task<UpdateUserDto> Get(string Id)
        {
            var user = await _db.Users.SingleOrDefaultAsync(x => x.Id == Id && !x.IsDelete);
            if (user == null)
            {
                throw new EntityNotFoundException();
            }
            return _mapper.Map<UpdateUserDto>(user);
        }
    
        private string GenratePassword()
        {
            return Guid.NewGuid().ToString().Substring(1, 8);
        }
       
    }
}
