using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using Domain;
using Microsoft.Azure.Documents.Client;
using System;
using Microsoft.Azure.Documents;
using System.Linq;

namespace AzureFunction1
{
    public static class Function1
    {
        [FunctionName("AddReading")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req, ILogger log)
        {
            const string DB = "Exercise2";
            const string Collection = "Devices";

            DocumentClient client = new DocumentClient(new Uri("https://documents1.documents.azure.com:443/"), "YDdZO1XRWjXwXFQ3qjtt8cXy1sV3pEfcyz7fAj6iEwpvB7A25iF2F58mhqALezh5nmlzd8ZqYm6bXGHLbt9JEg==");
            var db = client.CreateDatabaseIfNotExistsAsync(new Database() { Id = DB });
            await client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(DB), new DocumentCollection() { Id = Collection });

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Reading data = JsonConvert.DeserializeObject<Reading>(requestBody);

            Device device = client.CreateDocumentQuery<Device>(UriFactory.CreateDocumentCollectionUri(DB, Collection), new FeedOptions()).Where(d => d.id.Equals(data.DeviceId)).FirstOrDefault();

            device.Readings.Append(data);

            if (data.Value < device.Threshold.Low || data.Value > device.Threshold.High)
            {
                log.LogWarning($"The value provided is out of the threshold bounds [Low: {device.Threshold.Low}, High: {device.Threshold.High} ");
            }

            return data != null ? (ActionResult)new OkObjectResult($"Hello") : new BadRequestObjectResult("Please pass a Reading object on the request body");
        }
    }
}
