using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using TestNewLine.Data.Data;
using TestNewLine.Data.Models;
using TestNewLine.Services;

public class Noti : Hub
{
    protected readonly UserManager<User> _userManager;
    protected readonly ApplicationDbContext _db;
    protected readonly IUserService _userService;
    public Noti(IUserService userService, UserManager<User> userManager, ApplicationDbContext db)
    {
        _userManager = userManager;
        _db = db;
        _userService = userService;
    }


    public async Task SendFollow(string myId, string UserTo, string ResultStatus)
    {
        //var userAll = _userService.GetAll2SingleAdmin(myId);
        var user = _db.Users.Find(UserTo);
        var ser = _db.Users.Find(myId);
        // var AdminId = _db.UserRoles.Where(x => x.UserId == UserTo);
        await Clients.User(UserTo).SendAsync("ReciveF", new noti { Title = _db.Users.Find(UserTo).Email, Text = _db.Users.Find(UserTo).FullName });
        _db.Notifications.Add(new Notification { CreatedAt = DateTime.Now, isCheked = false, Title = ser.FullName + " " + ResultStatus, UserTo = UserTo });
        await _db.SaveChangesAsync();
        //foreach (var item in userAll)
        //{
        //    var user = _db.Users.Find(item.Id);
        //    var ser = _db.Users.Find(myId);
        //    var AdminId = _db.UserRoles.Where(x => x.UserId == item.Id);
        //    await Clients.User(item.Id).SendAsync("ReciveF", new noti { Title = _db.Users.Find(myId).Email, Text = _db.Users.Find(myId).FullName });
        //    _db.Notifications.Add(new Notification { Date = DateTime.Now, isCheked = false, Title = ser.FullName + " " + ResultStatus, UserId = item.Id });
        //    await _db.SaveChangesAsync();
        //}
    }
    class noti
    {
        public string Title { get; set; }
        public string Text { get; set; }
    }
}
