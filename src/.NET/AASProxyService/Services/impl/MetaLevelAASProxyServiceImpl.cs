

namespace AasDemoapp.Services.impl;

public class MetaLevelAASProxyServiceImpl : AASProxyService
{
    private const string SubmodelRegistryURL = "https://designer.aas-suite.de/aas-proxy/109/sm-repo/submodels/";

        public async Task<string> GetPropertyValue(string submodelID, string propertyName)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("ApiKey", "");
            //var specificAssetId = new SpecificAssetId("globalAssetId", assetId);
            //var jsonString = JsonConvert.SerializeObject(specificAssetId, Formatting.Indented, settings).ToBase64UrlEncoded(Encoding.UTF8);


            var url = SubmodelRegistryURL + "aHR0cHM6Ly9leGFtcGxlLmNvbS8vU3VibW9kZWwvSW5jb21pbmdHb29kSW5mb3MvNjg0N184MTk2XzMyMjNfODY2NQ/submodel-elements/{propertyName}";
            using HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }

        public async Task<string> UpdatePropertyValue(string submodelID, string PropertyName, string PropertyValue)
        {
            throw new NotImplementedException();
        }
}