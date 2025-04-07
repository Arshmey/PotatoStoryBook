using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Potato.Contract;
using Potato.DataAccess;
using Potato.Models;

namespace Potato.Controllers
{
    public class AccountController : Controller
    {

        private readonly ILogger<MainController> logger;
        private readonly DataDbContext dataDbContext;
        private readonly CookieSessionDbContext cookieSessionDbContext;

        public AccountController(ILogger<MainController> logger, DataDbContext dataDbContext, CookieSessionDbContext cookieSessionDbContext)
        {
            this.logger = logger;
            this.dataDbContext = dataDbContext;
            this.cookieSessionDbContext = cookieSessionDbContext;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromForm] UserDTO user)
        {
            User newUser = new User(user.username, user.email, Crypto.GetCrypto(user.password));

            await dataDbContext.AddAsync(newUser);
            await dataDbContext.SaveChangesAsync();

            CookieUser cookieUser = new CookieUser(newUser.UserId);

            await cookieSessionDbContext.AddAsync(cookieUser);
            await cookieSessionDbContext.SaveChangesAsync();

            CookieUserCreate(new CookieDTO(cookieUser.CookieID, cookieUser.UserID));

            UserSessionCreate(newUser);

            return RedirectToAction("MainPage", "Main");
        }

        [HttpPost]
        public async Task<IActionResult> SignIn([FromForm] UserDTO user)
        {

            if (await dataDbContext.Users.AnyAsync(u => u.Username == user.username && u.Email == user.email
                && u.Password == Crypto.GetCrypto(user.password)))
            {
                User oldUser = await dataDbContext.Users.SingleOrDefaultAsync(u => u.Username == user.username && u.Email == user.email);
                CookieUser cookieUser = new CookieUser(oldUser.UserId);

                await cookieSessionDbContext.AddAsync(cookieUser);
                await cookieSessionDbContext.SaveChangesAsync();

                CookieUserCreate(new CookieDTO(cookieUser.CookieID, cookieUser.UserID));

                UserSessionCreate(oldUser);
                return RedirectToAction("MainPage", "Main");
            }

            return RedirectToAction("Login");
        }

        private void CookieUserCreate(CookieDTO cookie)
        {
            CookieOptions builder = new CookieOptions
            {
                HttpOnly = true,
                IsEssential = true,
                Secure = true,
                Expires = DateTime.UtcNow.AddYears(1)
            };

            Response.Cookies.Append("UserCookieDTO", JsonSerializer.Serialize(cookie), builder);
        }

        private void UserSessionCreate(User user)
        {
            string jsonSession = JsonSerializer.Serialize(new UserSessionDTO(user.Username, user.Email, user.Permission));
            HttpContext.Session.SetString("UserSession", jsonSession);
        }
    }
}
