using Core.Domain;
using Core.Domain.ViewOnly;
using Core.Infrastructure;
using Newtonsoft.Json;
using Service.Interfaces;
using System.Text;
using Microsoft.IdentityModel.Tokens;

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

        public string CreateProductTemplate(string token, string name)
        {
            using var client = new HttpClient();

            var data = new
            {
                name = name,
                date_entered= DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                deleted= false,
                _module= "ProductTemplates"
            };

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            var res = client.PostAsync(_applicationSettings.SugarCrmUrl + "ProductTemplates", new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json"));

            var content = res.Result.Content.ReadAsStringAsync();

            var contentJson = JsonConvert.DeserializeObject<CrmReturnObject>(content.Result);
            return contentJson.Id;
        }
        public string CreateOppertunities(string token, Project project)
        {
            using var client = new HttpClient();

            var data = new
            {
                name = project.ProjectName,
                date_entered = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                deleted = false,
                _module = "Products"
            };

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            var res = client.PostAsync(_applicationSettings.SugarCrmUrl + "Opportunities", new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json"));

            var content = res.Result.Content.ReadAsStringAsync();

            var contentJson = JsonConvert.DeserializeObject<CrmReturnObject>(content.Result);
            return contentJson.Id;
        }
        public string CreateProduct(string token, ProjectDetail projectDetail, string productTemplateId, string oppertunityId)
        {
            using var client = new HttpClient();

            var data = new
            {
                name = projectDetail.ItemName,
                date_entered = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                product_template_id = productTemplateId,
                cost_price = projectDetail.LineItemCharge,
                list_price = projectDetail.LineItemCharge,
                length_in_c = projectDetail.Length,
                wid_c = projectDetail.Width,
                height_in_c = projectDetail.Height,
                product_category_c = projectDetail.Category,
                linear_feet_c = projectDetail.TotalLf,
                cubic_feet_c = projectDetail.TotalActualNominalValue.Equals("1")?projectDetail.TotalActCf : projectDetail.TotalNomCf,
                takeoff_color_c = projectDetail.TakeOffColor,
                description = projectDetail.DispositionSpecialNote,
                est_qty_c = projectDetail.Pieces,
                cd_detail_c = projectDetail.DetailPage,
                opportunity_id = oppertunityId,
                deleted = false,
                _module = "Products"
            };

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            var res = client.PostAsync(_applicationSettings.SugarCrmUrl + "Products", new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json"));

            var content = res.Result.Content.ReadAsStringAsync();

            var contentJson = JsonConvert.DeserializeObject<CrmReturnObject>(content.Result);
            return contentJson.Id;
        }

        public SugarCrmOppertunityList SearchOppertunities(string searchString)
        {
            using var client = new HttpClient();
            var jsObject = new Root()
            {
                name = new Name() 
                {
                    starts = searchString
                }
            };

            var jsonData = JsonConvert.SerializeObject(jsObject);

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + GetToken());

            var opp = client.GetAsync(_applicationSettings.SugarCrmUrl + "Opportunities?filter=[" + jsonData + "]").Result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<SugarCrmOppertunityList>(opp.Result);
        }
    }
}
