
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestNewLine.Core.Dtos;
using TestNewLine.Core.ViewModel;

namespace TestNewLine.Services
{
    public interface IAuthService
    {
        Task<LoginResponseViewModel> Login(LoginDto dto);

        Task<bool> ChangePassword(string userId, ChangePasswordDto dto);
    }
}
