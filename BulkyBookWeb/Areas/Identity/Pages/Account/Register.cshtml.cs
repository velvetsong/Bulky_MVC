// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace BulkyBookWeb.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IUnitOfWork db;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IUnitOfWork db)
        {
            _userManager = userManager;
            this.roleManager = roleManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            this.db = db;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }



        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            public string Name { get; set; }
            public string? StreetAddress { get; set; }
            public string? City { get; set; }
            public string? State { get; set; }
            public string? PostalCode { get; set; }

            public string? PhoneNumber { get; set; }

            //  from the RoleList  we will populate this  Role Property below from what was Selected within the VIEW  dropdown
            public string? Role { get; set; }

            //  from the CompanyList  we will populate this CompanyId Property below from what was Selected within the VIEW  dropdown
            public int? CompanyId { get; set; }

            [ValidateNever]
            public IEnumerable<SelectListItem> RoleList { get; set; }


            [ValidateNever]
            public IEnumerable<SelectListItem> CompanyList { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            //   here we do not need to  create the roles   if we are deploying the application,  so we comment out this code
            //if (!this.roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
            //{
            //    this.roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
            //    this.roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
            //    this.roleManager.CreateAsync(new IdentityRole(SD.Role_User_Indi)).GetAwaiter().GetResult();
            //    this.roleManager.CreateAsync(new IdentityRole(SD.Role_User_Comp)).GetAwaiter().GetResult();
            //}

            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            Input = new InputModel()    //  We "Instantiate" the Class, to "Hold" data, just like we did on the  Get()  for Product or Company
            {
                RoleList = this.roleManager.Roles.Select(x => x.Name).Select(i => new SelectListItem   //  Uses Rolemanager Helper, thus we 
                                                                                                      // do NOT need to use AppDbContext
                                 // Once the "Name" of the "Role" is SELECTED, we then  are "Projecting" that NAME to a SelectListItem
                                 //   an  IEnumerable  of a  "SelectListItem" 
                {
                    Text = i,
                    Value = i

                }),

                CompanyList = this.db.Company.GetCompanyListForDropDown()

                //CompanyList = this.db.Company.GetAll().Select(i => new SelectListItem
                //{
                //    Text = i.Name,
                //    Value = i.Id.ToString()
                //})
            };
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)  
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {

                //  here we are actually  creating an INSTANCE First of the  ApplicationUser  CLASS
                //  Then  assigning VALUES   to the  Properties of the ApplicationUser  OBJECT  after
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

                //  here we are populating the members of the ApplicationUser  OBJECT  fROM  what was entered within the Register.cshtml VIEW
                //  which thereby   is the SAME  thing as formatting the AspNetUsers   Table,  because ApplicationUser Object is the SAME as AspNetUsers table
                user.StreetAddress = Input.StreetAddress;
                user.City = Input.City;
                user.State = Input.State;
                user.Name = Input.Name;
                user.PhoneNumber = Input.PhoneNumber;


                if (Input.Role == SD.Role_User_Comp)
                {
                    user.CompanyId = Input.CompanyId;
                }


                //   This code below   ACTUALLY    CREATES & REGISTERS  the   USER  !!!!!!!!!!!!!!!!!!
                var result = await _userManager.CreateAsync(user, Input.Password);



                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");


                    if (Input.Role == null)
                    {
                        await _userManager.AddToRoleAsync(user, SD.Role_User_Indi);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, Input.Role);
                    }



                    var userId = await _userManager.GetUserIdAsync(user);



                    //  Generate a TOKEN to Confirm the Email  

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");



                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            Input.RoleList = this.roleManager.Roles.Select(x => x.Name).Select(i => new SelectListItem
            {
                Text = i,
                Value = i

            });

            Input.CompanyList = this.db.Company.GetCompanyListForDropDown();

            //Input.CompanyList = this.db.Company.GetAll().Select(i => new SelectListItem
            //{
            //    Text = i.Name,
            //    Value = i.Id.ToString()
            //});

            return Page();
        }

        //private IdentityUser CreateUser()
            private ApplicationUser CreateUser()  // really creates an instance of ApplictionUser because ApplicationUser
                                                  // is a CHILD of IdentityUser
                                                  //  public class ApplicationUser : IdentityUser
            {
                try
                {
                    //return Activator.CreateInstance<IdentityUser>();
                    return Activator.CreateInstance<ApplicationUser>();
                }
                catch
                {
                    throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                        $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                        $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
                }
            }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}
