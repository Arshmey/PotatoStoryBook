using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Potato.Contract;
using Potato.Models;

namespace Potato.Controllers
{
    public class MainController : Controller
    {
        private readonly ILogger<MainController> logger;
        private UserSessionDTO userSession;

        public MainController(ILogger<MainController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult MainPage()
        {
            userSession = JsonSerializer.Deserialize<UserSessionDTO>(HttpContext.Session.GetString("UserSession"));
            return View(userSession);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
