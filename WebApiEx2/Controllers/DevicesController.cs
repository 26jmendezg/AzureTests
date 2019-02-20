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
            client = new DocumentClient(new Uri("https://documents1.documents.azure.com:443/"), "YDdZO1XRWjXwXFQ3qjtt8cXy1sV3pEfcyz7fAj6iEwpvB7A25iF2F58mhqALezh5nmlzd8ZqYm6bXGHLbt9JEg==");
            var db = client.CreateDatabaseIfNotExistsAsync(new Database() { Id = DB });
            client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(DB), new DocumentCollection() { Id = Collection });
        }

        [HttpGet]
        [Route("api/Devices")]
        public ICollection<Device> Get()
        {
            Device[] devices = client.CreateDocumentQuery<Device>(UriFactory.CreateDocumentCollectionUri(DB, Collection), new FeedOptions()).ToArray();
            return devices;
        }

        [HttpGet]
        [Route("api/Devices/{id}")]
        public Device GetDevice(string id)
        {
            List<Device> device = client.CreateDocumentQuery<Device>(UriFactory.CreateDocumentCollectionUri(DB, Collection), new FeedOptions()).Where(d => d.Id.Equals(id)).ToList();
            return device.FirstOrDefault();
        }

        [HttpPost]
        [Route("api/CreateDevice")]
        public IActionResult CreateDevice([FromBody]Device objDevice)
        {
            client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DB, Collection), objDevice);
            return Ok();
        }
    }
}