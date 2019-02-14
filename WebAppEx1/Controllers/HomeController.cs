using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace WebAppEx1.Controllers
{
    public class HomeController : Controller
    {
        HttpClient cliente = new HttpClient();
        readonly Uri address = new Uri("https://webapiex1amvblt7gjwybi.azurewebsites.net/api/values");

        [HttpGet]
        public IActionResult Index()
        {
            var response = JsonConvert.DeserializeObject<string[]>((cliente.GetAsync(address).Result.Content.ReadAsStringAsync()).Result);
            return View(response);
        }

        [HttpPost]
        public IActionResult AddValue(string valor)
        {
            var response = cliente.PostAsJsonAsync(address, valor).Result;

            return RedirectToAction("Index");
        }
    }
}
