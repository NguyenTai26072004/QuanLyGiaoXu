using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuanLyGiaoXu.Backend.Entities;
using QuanLyGiaoXu.Backend.Enums;
using System;
using System.Threading.Tasks;

namespace QuanLyGiaoXu.Backend.Data
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(Roles.Admin.ToString()))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            }

            if (!await roleManager.RoleExistsAsync(Roles.GLV.ToString()))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.GLV.ToString()));
            }

            if (await userManager.FindByNameAsync("admin") == null)
            {
                var adminUser = new User
                {
                    UserName = "admin",
                    FullName = "Quản Trị Viên",
                    IsActive = true
                };

                var result = await userManager.CreateAsync(adminUser, "123456");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, Roles.Admin.ToString());
                }
            }
        }
    }
}