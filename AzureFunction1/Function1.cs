using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AzureFunction1
{
    public static class Function1
    {
        [FunctionName("AddReading")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req, ILogger log)
        {
            const string DB = "Exercise2";
            const string Collection = "Devices";

            DocumentClient client = new DocumentClient(new Uri(GetVariable("CosmosDBUri")), GetVariable("CosmosDBAuthKey"));

            var db = client.CreateDatabaseIfNotExistsAsync(new Database() { Id = DB });
            await client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(DB), new DocumentCollection() { Id = Collection });

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Reading data = JsonConvert.DeserializeObject<Reading>(requestBody);
            data.Id = Guid.NewGuid().ToString();

            Device device = client.CreateDocumentQuery<Device>(UriFactory.CreateDocumentCollectionUri(DB, Collection), new FeedOptions()).Where(d => d.Id.Equals(data.DeviceId)).ToList().FirstOrDefault();

            if (device.Readings == null)
            {
                device.Readings = new List<Reading>();
            }

            device.Readings.Add(data);

            await client.UpsertDocumentAsync(UriFactory.CreateDocumentCollectionUri(DB, Collection), device);

            if (data.Value < device.Threshold.Low || data.Value > device.Threshold.High)
            {
                log.LogWarning($"The value provided is out of the threshold bounds [Low: {device.Threshold.Low}, High: {device.Threshold.High} ");
            }

            return data != null ? (ActionResult)new OkObjectResult($"Hello") : new BadRequestObjectResult("Please pass a Reading object on the request body");
        }

        private static string GetVariable(string name)
        {
            return Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
        }
    }
}
