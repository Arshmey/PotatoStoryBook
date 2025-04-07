using Potato.DataAccess;

namespace Potato
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSession();
            builder.Services.AddScoped<DataDbContext>();
            builder.Services.AddScoped<CookieSessionDbContext>();

            var app = builder.Build();
            var scope = app.Services.CreateScope();
            var database = scope.ServiceProvider.GetService<DataDbContext>();
            database.Database.EnsureCreated();
            var cookiebase = scope.ServiceProvider.GetService<CookieSessionDbContext>();
            cookiebase.Database.EnsureCreated();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            else
            {
                app.UseHsts();
            }

            app.UseSession();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "user_open_site",
                pattern: "{controller=Authification}/{action=AuthMe}/{id?}");

            app.Run();
        }
    }
}
