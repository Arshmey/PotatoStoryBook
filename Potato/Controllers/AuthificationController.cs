using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Potato.Contract;
using Potato.DataAccess;
using Potato.Models;

namespace Potato.Controllers
{
    public class AuthificationController : Controller
    {

        private readonly ILogger<MainController> logger;
        private readonly DataDbContext dataDbContext;

        public AuthificationController(ILogger<MainController> logger, DataDbContext dataDbContext)
        {
            this.logger = logger;
            this.dataDbContext = dataDbContext;
        }

        public IActionResult AuthMe()
        {
            if (Request.Cookies.ContainsKey("UserDTO_Cookie"))
            {
                byte[] dehex = Convert.FromHexString(Request.Cookies["UserDTO_Cookie"]);
                UserDTO userCookie = JsonSerializer.Deserialize<UserDTO>(dehex);

                if (dataDbContext.Users.Any(n => n.Username == userCookie.username && n.Password == Crypto.GetCrypto(userCookie.password)))
                {
                    return RedirectToAction("MainPage", "Main");
                }
            }

            return RedirectToAction("Login", "LoginRegistration");
        }
    }
}
