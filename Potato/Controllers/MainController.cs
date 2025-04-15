using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Potato.Contract;
using Potato.DataAccess;
using Potato.Models;

namespace Potato.Controllers
{
    [RequireHttps]
    public class MainController : Controller
    {
        private readonly ILogger<MainController> logger;
        private readonly DataDbContext dataDbContext;
        private readonly Dictionary<string, object> model;
        private readonly CancellationToken cancellationToken;
        private UserSessionDTO? userSession;

        public MainController(ILogger<MainController> logger, DataDbContext dataDbContext)
        {
            this.logger = logger;
            this.dataDbContext = dataDbContext;
            cancellationToken = new CancellationToken();
            model = new Dictionary<string, object>();
        }

        public async Task<IActionResult> MainPage()
        {
            GetUserSession();

            //If messages counted five or more, get order sorted by desc and conver list with range.
            if (await dataDbContext.Messages.CountAsync(cancellationToken) >= 5)
            {
                var lastMessage = dataDbContext.Messages.OrderByDescending(t => t.DateCreation)
                    .ToList()
                    .GetRange(0, 5);
                model.Add("newMessage", lastMessage);
            }

            return View(model);
        }

        public async Task<IActionResult> UserCabinet()
        {
            GetUserSession();

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //Method for get UserSession in session.
        private void GetUserSession()
        {
            userSession = JsonSerializer.Deserialize<UserSessionDTO>(HttpContext.Session.Get("UserSession"));
            model.Add("user", userSession);
        }
    }
}
