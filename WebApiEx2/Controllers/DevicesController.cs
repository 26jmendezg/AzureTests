using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApiEx2.Controllers
{
    [Authorize]
    public class DevicesController : Controller
    {
        private readonly DocumentClient client;
        const string DB = "Exercise2";
        const string Collection = "Devices";

        public DevicesController()
        {
            client = new DocumentClient(new Uri("https://documentos1.documents.azure.com:443/"), "IfE4po27h5NRkBtM8VgtGGT4ZRZMkK6cRu0uFOaLvO7VoQzIXyIURYs6XbvZFol4l42I4R7tOpcLRUIK4xi0FA==");
            var db = client.CreateDatabaseIfNotExistsAsync(new Database() { Id = DB });
            client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(DB), new DocumentCollection() { Id = Collection });
        }

        [HttpGet]
        [Route("api/Devices")]
        public IEnumerable<Device> Get()
        {
            var devices = client.CreateDocumentQuery<Device>(UriFactory.CreateDocumentCollectionUri(DB, Collection), new FeedOptions()).ToList();
            return devices;
        }

        [HttpGet]
        [Route("api/Devices/{id}")]
        public Device GetDevice(string id)
        {
            var device = client.CreateDocumentQuery<Device>(UriFactory.CreateDocumentCollectionUri(DB, Collection), new FeedOptions()).Where(d => d.Id.Equals(id)).FirstOrDefault();
            return device;
        }

        [HttpPost]
        [Route("api/CreateDevice")]
        public IActionResult CreateDevice(Device objDevice)
        {
            client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DB, Collection), objDevice);
            return Ok();
        }
    }
}