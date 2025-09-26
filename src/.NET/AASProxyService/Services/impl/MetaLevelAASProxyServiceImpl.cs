

namespace AasDemoapp.Services.impl;

public class MetaLevelAASProxyServiceImpl : AASProxyService
{
    private const string SubmodelRegistryURL = "https://designer.aas-suite.de/aas-proxy/109/sm-repo/submodels/";
    private const string ApiKey = "";

        public async Task<string> GetPropertyValue(string submodelID, string propertyName)
        {
            Console.WriteLine("Reading current value of property " + propertyName + " in submodel " + submodelID);
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("ApiKey", ApiKey);
            //var specificAssetId = new SpecificAssetId("globalAssetId", assetId);
            //var jsonString = JsonConvert.SerializeObject(specificAssetId, Formatting.Indented, settings).ToBase64UrlEncoded(Encoding.UTF8);


            var url = SubmodelRegistryURL + $"{submodelID}/submodel-elements/{propertyName}";
            using HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }

        public async Task<string> UpdatePropertyValue(string submodelID, string PropertyName, string PropertyValue)
        {
            Console.WriteLine("Changing value of property " + PropertyName + " in Submodel " + submodelID + " to: " + PropertyValue);
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("ApiKey", ApiKey);
            var url = SubmodelRegistryURL + $"{submodelID}/submodel-elements/{PropertyName}/$value";
            using HttpResponseMessage response = await client.PatchAsync(url, JsonContent.Create(PropertyValue));
            response.EnsureSuccessStatusCode();
            return null;
        }
}