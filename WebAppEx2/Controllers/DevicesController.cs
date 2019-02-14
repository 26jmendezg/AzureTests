using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using WebAppEx2.Services;

namespace WebAppEx2.Controllers
{
    [Authorize]
    public class DevicesController : Controller
    {
        private readonly ApiClient client;
        private readonly Uri address = new Uri("https://localhost:44387/api/devices");

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