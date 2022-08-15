using AutoMapper;
using Microsoft.EntityFrameworkCore;
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

namespace TestNewLine.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public NotificationService(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<List<NotificationViewModel>> GetAllInDay()
        {
            var queryString = await _db.Notifications.Where(x => !x.IsDelete && x.CreatedAt.Day == DateTime.Now.Day).ToListAsync();

            var notification = _mapper.Map<List<NotificationViewModel>>(queryString);

            if (notification == null)
            {
                throw new EntityNotFoundException();
            }
            return notification;
        }


        public  int Create(CreateNotificationDto dto)
        {
            var notification = _mapper.Map<Notification>(dto);
            if (notification == null)
            {
                throw new EntityNotFoundException();
            }
             _db.Notifications.Add(notification);
             _db.SaveChanges();
            return notification.Id;
        }
        public async Task<UserViewModel> GetNotificationAuthors(string Id)
        {
            var users = await _db.Users.SingleOrDefaultAsync(x => !x.IsDelete && x.Id.Equals(Id));
            if (users == null)
            {
                throw new EntityNotFoundException();
            }
            return _mapper.Map<UserViewModel>(users);
        }
        public async Task<List<UserViewModel>> GetUserName()
        {
            var category = await _db.Users.Where(x => !x.IsDelete).ToListAsync();
            if (category == null)
            {
                throw new EntityNotFoundException();
            }
            return _mapper.Map<List<UserViewModel>>(category);
        } 
    }
}