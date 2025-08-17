using AasDemoapp.Models;
using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;
using RestSharp.Deserializers;

namespace AasDemoapp.Services.impl;

public class DroolsRulesEngineServiceImpl : RulesEngineInterface
{
    private const string BASE_URL = "http://localhost:8180/kie-server/services/rest/server/containers/AASAtom_1.0.0-SNAPSHOT";
    


    public async Task<RuleEvaluationResult> ExecuteRule(String sourceSubmodelName, String sourcePropertyName, String sourcePropertyValue, Rule rule)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("ApiKey", "");
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var url = BASE_URL + "/dmn";
        string[] modelContext = rule.Name.Split("_");
        string modelName = modelContext[0];
        string decisionName = modelContext[1];
        
        // Build JSON body matching the requested structure
        var body = new Dictionary<string, object>
        {
            ["model-namespace"] = "https://kiegroup.org/dmn/_A6136E3C-7F17-48C5-B9D3-D3A0B0AAB27B",
            ["model-name"] = modelName,
            ["decision-name"] = new[]
            {
                decisionName,
            },
            ["dmn-context"] = new Dictionary<string, object>
            {
                [sourceSubmodelName + "_" + sourceSubmodelName] = false // todo: exchange false with sourcePropertyValue with correct datatype
            }
        };

        var json = JsonSerializer.Serialize(body);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");

        using HttpResponseMessage response = await client.PostAsync(url, content);
        var responseBody = await response.Content.ReadAsStringAsync();

        // Convert responseBody JSON to a Dictionary
        Dictionary<string, System.Text.Json.JsonElement>? responseDict = null;
        responseDict = JsonSerializer.Deserialize<Dictionary<string, System.Text.Json.JsonElement>>(responseBody, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        Console.WriteLine(responseDict);
        return null;
    }
}