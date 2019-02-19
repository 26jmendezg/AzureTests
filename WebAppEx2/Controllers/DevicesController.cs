using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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

        public IActionResult Create()
        {
            
            return View(new Device() { id = Guid.NewGuid().ToString() });
        }

        [HttpPost]
        public IActionResult Create(Device objDevice)
        {
            client.CreateDevice(objDevice);
            return RedirectToAction("Index");
        }

        public IActionResult Details(string id)
        {
            Device objDevice = client.GetDetail(id);
            return View(objDevice);
        }
    }
}