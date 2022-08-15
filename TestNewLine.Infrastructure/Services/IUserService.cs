using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestNewLine.Core.Dtos;
using TestNewLine.Core.ViewModels;

namespace TestNewLine.Services
{
    public interface IUserService
    {
        Task<ResponseDto> GetAll(Pagination pagination, Query query);
        Task<List<UserViewModel>> GetAll2();
        Task<List<UserViewModel>> GetAllAPI(string serachKey);
        Task<List<UserViewModel>> GetUserFullName();
        UserViewModel GetUserByName(string UserName);
        Task<string> Create(CreateUserDto dto);
        Task<string> Update(UpdateUserDto dto);
        Task<string> Delete(string Id);
        Task<UpdateUserDto> Get(string Id);
        List<UserViewModel> GetUserAdmin();
        List<UserViewModel> GetAll2Admin(); 
        List<UserViewModel> GetUsers();
    }
}
