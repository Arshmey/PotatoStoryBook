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
        private readonly CancellationToken cancellationToken;

        public AuthificationController(ILogger<MainController> logger, DataDbContext dataDbContext, CookieSessionDbContext cookieSessionDbContext)
        {
            this.logger = logger;
            this.dataDbContext = dataDbContext;
            this.cookieSessionDbContext = cookieSessionDbContext;
            cancellationToken = new CancellationToken();
        }

        //Async method for serach in Database. If use sync method for search in Database, this method get very slow
        public async Task<IActionResult> AuthMe()
        {
            //Check have cookie in user browser storage
            if (Request.Cookies.ContainsKey("UserCookieDTO"))
            {
                string cookieValue = Request.Cookies["UserCookieDTO"];
                CookieDTO cookie = JsonSerializer.Deserialize<CookieDTO>(cookieValue);

                //Seeking cookie in database
                if (await cookieSessionDbContext.CookieUsers.AnyAsync(cookieUser => cookieUser.CookieID == cookie.cookieID
                    && cookieUser.UserID == cookie.userID && DateTime.UtcNow < cookieUser.DateTime, cancellationToken))
                {
                    User enteredUser = await dataDbContext.Users.FirstOrDefaultAsync(u => u.UserId == cookie.userID, cancellationToken);

                    UserSessionDTO DTO = new UserSessionDTO(enteredUser.Username, enteredUser.Email, enteredUser.Permission);
                    HttpContext.Session.Set("UserSession", JsonSerializer.SerializeToUtf8Bytes(DTO));
                    return RedirectToAction("MainPage", "Main");
                }
                else
                {
                    Response.Cookies.Delete("UserCookieDTO");
                    CookieUser? cookieUser = await cookieSessionDbContext.CookieUsers.SingleOrDefaultAsync(cookieUser => cookieUser.CookieID == cookie.cookieID
                        && cookieUser.UserID == cookie.userID, cancellationToken);
                    if (cookieUser != null)
                    {
                        cookieSessionDbContext.CookieUsers.Remove(cookieUser);
                        await cookieSessionDbContext.SaveChangesAsync(cancellationToken);
                    }
                }
            }

            return RedirectToAction("Login", "Account");
        }
    }
}
