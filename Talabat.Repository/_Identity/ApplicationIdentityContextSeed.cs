using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models.Identity;

namespace Talabat.Repository._Identity
{
    public static class ApplicationIdentityContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager)
        {
            
            if(!userManager.Users.Any())
            {
                var user = new ApplicationUser()
                {
                    DisplayName = "Ahmed",
                    Email = "ahmedrefaat.work.2025@gmail.com",
                    UserName = "ahmed.refaat",
                    PhoneNumber = "01143564794"
                };

                await userManager.CreateAsync(user, "P@ssw0rd");
            }
        }
    }
}
