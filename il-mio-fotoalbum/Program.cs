using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using il_mio_fotoalbum.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace il_mio_fotoalbum
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services
                .AddDbContext<PhotoAlbumContext>();

            builder.Services
                .AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<PhotoAlbumContext>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

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

            app.UseAuthentication();

            app.UseAuthorization();

            CreateRolesandUsers(app);

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();

            app.Run();
        }

        private static async void CreateRolesandUsers(WebApplication app)
        {

            var ServiceProvider = app.Services.CreateScope().ServiceProvider;

            var RoleManager = ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            bool condition = await RoleManager.RoleExistsAsync("ADMIN");
            if (!condition)
            {
                // create admin creatorRole   
                var adminRole = new IdentityRole();
                adminRole.Name = "ADMIN";
                await RoleManager.CreateAsync(adminRole);
            }

            // creating Creating Manager adminRole     
            condition = await RoleManager.RoleExistsAsync("CREATOR");
            if (!condition)
            {
                var creatorRole = new IdentityRole();
                creatorRole.Name = "CREATOR";
                await RoleManager.CreateAsync(creatorRole);
            }

            string TestUserName = "test@gmail.com";
            IdentityUser TestUser = await UserManager.FindByNameAsync(TestUserName);
            if (TestUser == null)
            {
                // generate my account      
                TestUser = new IdentityUser();

                TestUser.UserName = TestUserName;
                TestUser.Email = TestUserName;
                TestUser.EmailConfirmed = true;
                string TestUserPass = "test9TEST*";

                IdentityResult checkUser = await UserManager.CreateAsync(TestUser, TestUserPass);

                //Add default User to Role Admin    
                if (checkUser.Succeeded)
                {
                    await UserManager.AddToRoleAsync(TestUser, "ADMIN");
                    await UserManager.AddToRoleAsync(TestUser, "CREATOR");
                }
            }
        }
    }
}