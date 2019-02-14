using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApiEx1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly DocumentClient client;
        public ValuesController()
        {
            client = new DocumentClient(new Uri("https://documentos1.documents.azure.com:443/"), "IfE4po27h5NRkBtM8VgtGGT4ZRZMkK6cRu0uFOaLvO7VoQzIXyIURYs6XbvZFol4l42I4R7tOpcLRUIK4xi0FA==");
            var db = client.CreateDatabaseIfNotExistsAsync(new Database() { Id = "Exercise1" });
            client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri("Exercise1"), new DocumentCollection() { Id = "Collection1" });
        }

        // GET api/values
        [HttpGet]
        public ActionResult<List<string>> Get()
        {
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };
            var resultado = client.CreateDocumentQuery<Valores>(UriFactory.CreateDocumentCollectionUri("Exercise1", "Collection1"), queryOptions).Select(x => x.valor).ToList();
            return resultado;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
            client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri("Exercise1", "Collection1"), new { valor = value });
        }

        private class Valores { public string valor { get; set; } }
    }
}
