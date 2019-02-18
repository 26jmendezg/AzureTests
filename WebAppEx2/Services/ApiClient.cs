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
            appSecret = configuration["AzureAd:ClientSecret"];

            this.client = client;
            this.client.BaseAddress = new Uri(configuration["Api:Baseurl"]);
        }

        public async Task SetToken()
        {
            if (!tokenSet)
            {
                var authContext = new AuthenticationContext(authority);
                var credential = new ClientCredential(appId, appSecret);
                var authResult = await authContext.AcquireTokenAsync(resourceid, credential);
                var token = authResult.AccessToken;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                tokenSet = true;
            }
        }

        public async Task<string[]> GetValues()
        {
            await SetToken();
            var response = await client.GetAsync("api/Devices");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"{response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<string[]>(content);
        }
    }
}
