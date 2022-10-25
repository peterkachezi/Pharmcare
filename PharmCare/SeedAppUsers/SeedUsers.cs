
using Microsoft.AspNetCore.Identity;
using PharmCare.DAL.Models;
using System;
namespace PharmCare.SeedAppUsers
{
    public static class SeedUsers
    {
        public static void Seed(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            seedRoles(roleManager);

            seedUsers(userManager);
        }

        private static void seedRoles(RoleManager<IdentityRole> roleManager)
        {
            try
            {
                if (!roleManager.RoleExistsAsync("Admin").Result)
                {
                    var role = new IdentityRole();

                    role.Name = "Admin";

                    roleManager.CreateAsync(role).Wait();
                }



                if (!roleManager.RoleExistsAsync("Doctor").Result)
                {
                    var role = new IdentityRole();

                    role.Name = "Doctor";

                    roleManager.CreateAsync(role).Wait();
                }



                if (!roleManager.RoleExistsAsync("Pharmacist").Result)
                {
                    var role = new IdentityRole();

                    role.Name = "Pharmacist";

                    roleManager.CreateAsync(role).Wait();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void seedUsers(UserManager<AppUser> userManager)
        {
            try
            {
                #region Admin
                var admin = userManager.FindByEmailAsync("admin@gmail.com");

                if (admin.Result == null)
                {
                    var user = new AppUser();

                    user.UserName = "admin@gmail.com";

                    user.Email = "admin@gmail.com";

                    user.PhoneNumber = "0704509484";

                    user.FirstName = "Alex";

                    user.LastName = "Jobs";

                    user.EmailConfirmed = true;

                    user.isActive = true;

                    user.CreateDate = DateTime.Now;

                    string userPWD = "Admin@2022";

                    var chkUser = userManager.CreateAsync(user, userPWD);

                    //Add default User to Role Admin    
                    if (chkUser.Result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, "Admin").Wait();

                    }
                }
                #endregion
                #region Doctor

                var doctor = userManager.FindByEmailAsync("doctor@gmail.com");

                if (doctor.Result == null)
                {
                    var user = new AppUser();

                    user.UserName = "doctor@gmail.com";

                    user.Email = "doctor@gmail.com";

                    user.PhoneNumber = "0704509484";

                    user.FirstName = "Steve";

                    user.LastName = "Jobs";

                    user.EmailConfirmed = true;

                    user.isActive = true;

                    user.CreateDate = DateTime.Now;

                    string userPWD = "Doctor@2022";

                    var chkUser = userManager.CreateAsync(user, userPWD);

                    //Add default User to Role Admin    
                    if (chkUser.Result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, "Doctor").Wait();

                    }
                }
                #endregion        

                #region Pharmacist
                var pharmacist = userManager.FindByEmailAsync("pharmacist@gmail.com");

                if (pharmacist.Result == null)
                {
                    var user = new AppUser();

                    user.UserName = "pharmacist@gmail.com";

                    user.Email = "pharmacist@gmail.com";

                    user.PhoneNumber = "0704509484";

                    user.FirstName = "Brian";

                    user.LastName = "Jordan";

                    user.EmailConfirmed = true;

                    user.isActive = true;

                    user.CreateDate = DateTime.Now;

                    string userPWD = "Pharmacist@2022";

                    var chkUser = userManager.CreateAsync(user, userPWD);

                    //Add default User to Role Admin    
                    if (chkUser.Result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, "Pharmacist").Wait();

                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

    }
}
