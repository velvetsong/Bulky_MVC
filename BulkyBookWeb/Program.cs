using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using BulkyBook.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using BulkyBook.DataAccess.DbInitializer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//  this code below  is AUTOMATICALLY GENERATED,  whenever you ADD a New Scaffolded  IDENTITY Item....  So JUST DELETE THIS LINE BELOW
//      because we ALREADY HAVE an EXISTING  AddDbContext  Service  above
//builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(connectionString));

//builder.Services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<AppDbContext>();
//   use this below if you want to define ROLES for a "Custom Identity"  to your application,  Instead of the DefaultIdentity above
//   //  and also Generate a TOKEN to Confirm the Email, within the Register.cshtml.cs
//   
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IDbInitializer,  DbInitializer>();

//builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

builder.Services.AddSingleton<IEmailSender, EmailSender>();

builder.Services.AddRazorPages();
//builder.Services.AddRazorPages().AddRazorRuntimeCompilation();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting(); 

app.UseAuthentication();    // this line MUST come BEFORE  Authorization

app.UseAuthorization();

app.UseSession();

SeedDatabase();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initialize();  //  this will  Call the Initialize  Method within the  DbInitializer CLASS  to  SEED the DB
    }
}