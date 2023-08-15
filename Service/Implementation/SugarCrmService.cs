﻿using Core.Domain.ViewOnly;
using Core.Infrastructure;
using Newtonsoft.Json;
using Service.Interfaces;
using System.Text;

namespace Service.Implementation
{
    public class SugarCrmService : ISugarCrmService
    {
        private readonly ApplicationSettings _applicationSettings;

        public SugarCrmService(ApplicationSettings applicationSettings)
        {
            _applicationSettings = applicationSettings;
        }

        public string GetToken()
        {
            using var client = new HttpClient();

            var data = new Dictionary<string, string>
            {
                {"grant_type", "password"},
                {"client_id", _applicationSettings.ClientId},
                {"username", _applicationSettings.CrmUserName},
                {"password", _applicationSettings.CrmPassword},
                {"platform", _applicationSettings.CrmPlatform}
            };

            var res = client.PostAsync(_applicationSettings.SugarCrmUrl+ "oauth2/token", new FormUrlEncodedContent(data));

            var content = res.Result.Content.ReadAsStringAsync();

            var contentJson = JsonConvert.DeserializeObject<dynamic>(content.Result);

            if (contentJson != null)
            {
                foreach (var obj in contentJson)
                {
                    return obj.Value.Value;
                }
            }

            return string.Empty;
        }

        public string CreateProductTemplate(string name)
        {
            using var client = new HttpClient();

            var data = new
            {
                name = name,
                date_entered= DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                deleted= false,
                _module= "ProductTemplates"
            };

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + GetToken());

            var res = client.PostAsync(_applicationSettings.SugarCrmUrl + "ProductTemplates", new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json"));

            var content = res.Result.Content.ReadAsStringAsync();

            var contentJson = JsonConvert.DeserializeObject<CrmReturnObject>(content.Result);
            return contentJson.Id;
        }

        public string CreateProduct(string name,string productTemplateId)
        {
            using var client = new HttpClient();

            var data = new
            {
                name = name,
                date_entered = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                product_template_id = productTemplateId,
                deleted = false,
                _module = "Products"
            };

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + GetToken());

            var res = client.PostAsync(_applicationSettings.SugarCrmUrl + "Products", new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json"));

            var content = res.Result.Content.ReadAsStringAsync();

            var contentJson = JsonConvert.DeserializeObject<CrmReturnObject>(content.Result);
            return contentJson.Id;
        }
    }
}
