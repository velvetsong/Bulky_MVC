using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using BulkyBook.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly AppDbContext db;

        public DbInitializer(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            AppDbContext  db)    //  because we want to use the Helper Methods within the AppDbContext,  instead of using IUnitOfWork db; 
        {
            _userManager = userManager;
            this.roleManager = roleManager;
            this.db = db;
        }
        public void Initialize()   // this method is responsible for Creating the Admin User, and the  User Roles
        {
            //  apply migrations  if they are not applied

            // create roles if they are not created

            //  if roles are not created,  then we will create admin user as well

            try
            {
                if (this.db.Database.GetPendingMigrations().Count() > 0)  //  to see if there are EXISTING Migrations,
                                                                          //  that have not been applied to DB
                {
                    this.db.Database.Migrate();   //  do this ,  instead of running the command   PM> update-database
                }
            }
            catch (Exception ex) { }

            //  this code below is the same as doing the OnGetAsync()  method WITHIN THE Register.cshtml.cs FILE

            //    CREATE the  Roles,   if they do NOT exist
            if (!this.roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
            {
                this.roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                this.roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
                this.roleManager.CreateAsync(new IdentityRole(SD.Role_User_Indi)).GetAwaiter().GetResult();
                this.roleManager.CreateAsync(new IdentityRole(SD.Role_User_Comp)).GetAwaiter().GetResult();

                _userManager.CreateAsync(new ApplicationUser     //  creating the User and Populating its Properties  AT THE SAME TIME
                                                                 //  This Command will ACTUALLY  CREATE / SAVE  the USER in the Bulky1 DATABASE
                {
                    UserName = "admin@dotnetmastery.com",
                    Email = "admin@dotnetmastery.com",
                    Name = "Bhrugen Patel",
                    PhoneNumber = "1112223333",
                    StreetAddress = "test 123 Ave",
                    State = "IL",
                    City = "Chicago"
                }, "Admin123*").GetAwaiter().GetResult();

                ApplicationUser   user = 
                    this.db.ApplicationUsers.FirstOrDefault(u => u.Email == "admin@dotnetmastery.com");


                //  Now Assign a  Role to this NEW User

                _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();  // Await means, do NOT continue UNTIL the Async method is done
            }

            return;
        }
    }
}