using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppEx2.Services;

namespace WebAppEx2.Controllers
{
    [Authorize]
    public class DevicesController : Controller
    {
        private readonly ApiClient client;

        public DevicesController(ApiClient client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            var devices = client.GetValues();
            return View(devices);
        }
    }
}