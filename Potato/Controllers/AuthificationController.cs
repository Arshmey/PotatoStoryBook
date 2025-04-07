using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Potato.Contract;
using Potato.DataAccess;
using Potato.Models;

namespace Potato.Controllers
{
    public class AuthificationController : Controller
    {

        private readonly ILogger<MainController> logger;
        private readonly DataDbContext dataDbContext;
        private readonly CookieSessionDbContext cookieSessionDbContext;

        public AuthificationController(ILogger<MainController> logger, DataDbContext dataDbContext, CookieSessionDbContext cookieSessionDbContext)
        {
            this.logger = logger;
            this.dataDbContext = dataDbContext;
            this.cookieSessionDbContext = cookieSessionDbContext;
        }

        //Async method for serach in Database. If use sync method for search in Database, this method get very slow
        public async Task<IActionResult> AuthMe()
        {
            if (Request.Cookies.ContainsKey("UserCookieDTO"))
            {
                string cookieValue = Request.Cookies["UserCookieDTO"];
                CookieDTO cookie = JsonSerializer.Deserialize<CookieDTO>(cookieValue);

                if (await cookieSessionDbContext.CookieUsers.AnyAsync(cookieUser => cookieUser.CookieID == cookie.cookieID
                    && cookieUser.UserID == cookie.userID && DateTime.UtcNow < cookieUser.DateTime))
                {
                    User enteredUser = await dataDbContext.Users.FirstOrDefaultAsync(u => u.UserId == cookie.userID);

                    string jsonSession = JsonSerializer.Serialize(new UserSessionDTO(enteredUser.Username, enteredUser.Email, enteredUser.Permission));
                    HttpContext.Session.SetString("UserSession", jsonSession);
                    return RedirectToAction("MainPage", "Main");
                }
                else
                {
                    Response.Cookies.Delete("UserCookieDTO");
                    CookieUser? cookieUser = await cookieSessionDbContext.CookieUsers.SingleOrDefaultAsync(cookieUser => cookieUser.CookieID == cookie.cookieID
                        && cookieUser.UserID == cookie.userID);
                    if (cookieUser != null)
                    {
                        cookieSessionDbContext.CookieUsers.Remove(cookieUser);
                        await cookieSessionDbContext.SaveChangesAsync();
                    }
                }
            }

            return RedirectToAction("Login", "Account");
        }
    }
}
