using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestNewLine.Core.Dtos;
using TestNewLine.Core.ViewModels;

namespace TestNewLine.Infrastructure.Services
{
    public interface INotificationService
    {
        Task<List<NotificationViewModel>> GetAllInDay();
        int Create(CreateNotificationDto dto);
        Task<UserViewModel> GetNotificationAuthors(string Id);
        Task<List<UserViewModel>> GetUserName();
    }
}
