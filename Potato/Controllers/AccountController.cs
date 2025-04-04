using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Potato.Contract;
using Potato.DataAccess;
using Potato.Models;

namespace Potato.Controllers
{
    public class AccountController : Controller
    {

        private readonly ILogger<MainController> logger;
        private readonly DataDbContext usersDbContext;

        public AccountController(ILogger<MainController> logger, DataDbContext usersDbContext)
        {
            this.logger = logger;
            this.usersDbContext = usersDbContext;
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
        public IActionResult CreateAccount([FromForm] UserDTO user)
        {
            CookieUserCreate(user);

            usersDbContext.Add(new User(user.username, user.email, user.password));
            usersDbContext.SaveChanges();

            return RedirectToAction("MainPage", "Main");
        }

        [HttpPost]
        public IActionResult SignIn([FromForm] UserDTO user)
        {
            if (ModelState.IsValid)
            {

                if (usersDbContext.Users.Any(n => n.Username == user.username
                    && n.Email == user.email
                        && n.Password == Crypto.GetCrypto(user.password)))
                {
                    CookieUserCreate(user);
                    return RedirectToAction("MainPage", "Main");
                }
            }

            return RedirectToAction("Login");
        }

        private void CookieUserCreate(UserDTO user)
        {
            CookieOptions builder = new CookieOptions
            {
                HttpOnly = true,
                IsEssential = true,
                Secure = true,
                Expires = DateTime.Now.AddYears(1)
            };

            string hex = Convert.ToHexString(JsonSerializer.SerializeToUtf8Bytes(user));
            Response.Cookies.Append("UserDTO_Cookie", hex, builder);
        }
    }
}
