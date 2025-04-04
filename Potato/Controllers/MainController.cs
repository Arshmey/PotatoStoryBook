using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Potato.Contract;
using Potato.DataAccess;
using Potato.Models;

namespace Potato.Controllers
{
    public class MainController : Controller
    {
        private readonly ILogger<MainController> logger;
        private readonly DbSet<Message> messages;
        private UserDTO user;

        public MainController(ILogger<MainController> logger, DataDbContext dataDbContext)
        {
            this.logger = logger;
            messages = dataDbContext.Messages;
        }

        [HttpGet]
        public IActionResult MainPage()
        {
            if (Dechipher())
            {
                return View(user);
            }

            return RedirectToAction("Login", "Account");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private bool Dechipher()
        {
            if (Request.Cookies.ContainsKey("UserDTO_Cookie"))
            {
                byte[] dehex = Convert.FromHexString(Request.Cookies["UserDTO_Cookie"]);
                user = JsonSerializer.Deserialize<UserDTO>(dehex);
                return true;
            }

            return false;
        }
    }
}
