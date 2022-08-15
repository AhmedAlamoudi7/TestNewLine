using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using TestNewLine.Data.Models;
using TestNewLine.Enums;

namespace TestNewLine.Data.Data
{
    public static class DbSeeder
    {
        public static IHost SeedDb(this IHost webHost)
        {
            using (var scope = webHost.Services.CreateScope())
            {
                try
                {
                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                    // Seed Roles for Enum 
                    var RoleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                    RoleManager.SeedRoles().Wait();
                    userManager.SeedUser().Wait();


                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    //context.SeedCategory().Wait();


                    //   //context.SeedAdvertisement().Wait();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
            return webHost;
        }

        public static async Task SeedUser(this UserManager<User> userManager)
        {
            if (await userManager.Users.AnyAsync())
            {
                return;
            }

            //var users = new List<User>();

            var user = new User()
            {
                Email = "PEAEPSuperAdmin@PEAEP.com",
                FullName = "PEAEP",
                UserName = "PEAEPSuperAdmin@PEAEP.com",
                PhoneNumber = "0595555555",
                DOB = DateTime.Now,
                CreatedAt = DateTime.Now,
                // new DateTime(); to add my date
                UserType = UserType.SuperAdmin,
                PhoneNumberConfirmed = true,
                EmailConfirmed = true,
                IsDelete = false,
            };

       

            // User 1 Create
            await userManager.CreateAsync(user, "PEAEP2020$$");

            if (user.UserType == UserType.SuperAdmin)
            {
                await userManager.AddToRoleAsync(user, "SuperAdmin");
            }
   
         

        }
        //Seed Roles for Enum
        public static async Task SeedRoles(this RoleManager<IdentityRole> RoleManager)
        {

            if (!RoleManager.Roles.Any())
            {

                var roles = new List<string>();
                roles.Add(TestNewLine.Enums.UserType.SuperAdmin.ToString());
                foreach (var role in roles)
                {
                    await RoleManager.CreateAsync(new IdentityRole(role));
                }

            }

        }




        public static async Task SeedPostBlogs(this ApplicationDbContext context)
        {

            if (await context.PostBlogs.AnyAsync())
            {
                return;
            }

        var categoires = new List<PostBlog>();

            var postBlog = new PostBlog();
            postBlog.Title = "new line";
            postBlog.SubTittle = "new line";
            postBlog.Body = "new line";
            postBlog.Image = "~/Images/Logo/newLine.jfif";
            postBlog.AuthorId = "123";
            postBlog.CreatedAt = DateTime.Now;

            var postBlog2 = new PostBlog();
            postBlog2.Title = "new line";
            postBlog2.SubTittle = "new line";
            postBlog2.Body = "new line";
            postBlog2.Image = "~/Images/Logo/newLine.jfif";
            postBlog2.AuthorId = "123";
            postBlog2.CreatedAt = DateTime.Now;



            categoires.Add(postBlog);
            categoires.Add(postBlog2);


            await context.PostBlogs.AddRangeAsync(categoires);
            await context.SaveChangesAsync();


        }

    




    }
}

