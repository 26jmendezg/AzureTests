using Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace WebAppEx2.Services
{
    public class ApiClient
    {
        private readonly string resourceid;
        private readonly string authority;
        private readonly string appId;
        private readonly string appSecret;
        private readonly HttpClient client;
        private bool tokenSet = false;

        public ApiClient(HttpClient client, IConfiguration configuration)
        {
            resourceid = configuration["Api:ResourceId"];
            authority = $"{configuration["AzureAd:Instance"]}{configuration["AzureAd:TenantId"]}";
            appId = configuration["AzureAd:ClientId"];
            appSecret = configuration["WebAppEx2ClientSecret"];

            this.client = client;
            this.client.BaseAddress = new Uri(configuration["Api:Baseurl"]);
        }

        public void SetToken()
        {
            if (!tokenSet)
            {
                var authContext = new AuthenticationContext(authority);
                var credential = new ClientCredential(appId, appSecret);
                var authResult = authContext.AcquireTokenAsync(resourceid, credential);
                var token = authResult.Result.AccessToken;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                tokenSet = true;
            }
        }

        public string[] GetValues()
        {
            SetToken();
            var response = client.GetAsync("api/Devices").Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"{response.StatusCode}");
            }

            var content = response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<string[]>(content.Result);
        }
    }
}
