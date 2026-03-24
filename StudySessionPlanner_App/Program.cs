using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudySessionPlanner_App.Data;
using StudySessionPlanner_App.Models;

namespace StudySessionPlanner_App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                db.Database.Migrate();

                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();


                string[] roles = { "Administrator", "User" };

                foreach (var role in roles)
                {
                    if (roleManager.RoleExistsAsync(role).GetAwaiter().GetResult())
                    {
                        roleManager.CreateAsync(new IdentityRole(role)).GetAwaiter().GetResult();
                    }
                }

                
                string adminEmail = "admin@studysessionplanner.com";
                string adminPassword = "Admin123!";

                var adminUser = userManager.FindByEmailAsync(adminEmail).GetAwaiter().GetResult();

                if (adminUser == null)
                {
                    adminUser = new ApplicationUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        EmailConfirmed = true
                    };

                    var result = userManager.CreateAsync(adminUser, adminPassword).GetAwaiter().GetResult();

                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(adminUser, "Administrator").GetAwaiter().GetResult();
                    }
                }


                if (!db.Topics.Any())
                {
                    db.Topics.AddRange(
                        new Topic { Name = "Databases" },
                        new Topic { Name = "C# Fundamentals" },
                        new Topic { Name = "ASP.NET Core MVC" }
                    );
                    db.SaveChangesAsync();
                }
            }


            app.Run();
        }
    }
}
