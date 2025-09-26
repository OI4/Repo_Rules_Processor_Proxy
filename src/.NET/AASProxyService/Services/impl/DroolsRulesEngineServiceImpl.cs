using AasDemoapp.Models;
using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;
using RestSharp.Deserializers;
using Action = AasDemoapp.Models.Action;

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
        // Basic Authorization header: username:password (replace with secure config in production)
        var basicCreds = Convert.ToBase64String(Encoding.ASCII.GetBytes($"admin:admin"));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicCreds);

        var url = BASE_URL + "/dmn";
        
        // Build JSON body matching the requested structure
        var body = new Dictionary<string, object>
        {
            ["model-namespace"] = "https://kiegroup.org/dmn/_A6136E3C-7F17-48C5-B9D3-D3A0B0AAB27B",
            ["model-name"] = rule.Name,
            ["decision-name"] = new[]
            {
                rule.TargetSubmodelName + "_" + rule.TargetPropertyName,
            },
            ["dmn-context"] = new Dictionary<string, object>
            {
                [sourceSubmodelName + "_" + sourcePropertyName] = sourcePropertyValue // todo: exchange false with sourcePropertyValue with correct datatype
            }
        };

        var json = JsonSerializer.Serialize(body);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");

        using HttpResponseMessage response = await client.PostAsync(url, content);
        var responseBody = await response.Content.ReadAsStringAsync();

        // Convert responseBody JSON into a Dictionary<string, object>
        string resultValue = DroolsRulesEngineServiceImpl.GetEvaluationResultPropertyValueFromResponseString(responseBody);
        Action resultAction = new Action();
        resultAction.TargegSubmodelName = rule.TargetSubmodelName;
        resultAction.TargetPropertyName = rule.TargetPropertyName;
        resultAction.TargetPropertyValue = resultValue;
        RuleEvaluationResult result = new RuleEvaluationResult();
        result.WasSuccessful = true;
        result.ActionToPerform = resultAction;
        return result;
    }

    private static string GetEvaluationResultPropertyValueFromResponseString(string responseString)
    {
        Dictionary<string, object> responseDict = JsonToDictionary(responseString);
 
        Dictionary<string, object> result = (Dictionary<string, object>)responseDict["result"];
        Dictionary<string, object> dmnEvaluationResult = (Dictionary<string, object>)result["dmn-evaluation-result"];
        Dictionary<string, object> decisionResults = (Dictionary<string, object>)dmnEvaluationResult["decision-results"];
        Dictionary<string, object> targetPropertyValueResult = (Dictionary<string, object>)decisionResults.First().Value;
        string targetPropertyValue = (string)targetPropertyValueResult["result"];
        return targetPropertyValue;
    }
    
    private static Dictionary<string, object> JsonToDictionary(string json)
    {
        using var doc = JsonDocument.Parse(json);
        if (doc.RootElement.ValueKind != JsonValueKind.Object)
        {
            return new Dictionary<string, object>();
        }
        var converted = JsonElementToObject(doc.RootElement) as Dictionary<string, object?>;
        // Coerce nullable values to object for the return type
        var result = new Dictionary<string, object>();
        if (converted != null)
        {
            foreach (var kv in converted)
            {
                result[kv.Key] = kv.Value!; // permit nulls; caller can check via TryGetValue
            }
        }
        return result;
    }
    
    // Recursively convert a JsonElement to .NET objects (Dictionary/List/scalars)
    private static object? JsonElementToObject(JsonElement element)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
            {
                var dict = new Dictionary<string, object?>();
                foreach (var prop in element.EnumerateObject())
                {
                    dict[prop.Name] = JsonElementToObject(prop.Value);
                }
                return dict;
            }
            case JsonValueKind.Array:
            {
                var list = new List<object?>();
                foreach (var item in element.EnumerateArray())
                {
                    list.Add(JsonElementToObject(item));
                }
                return list;
            }
            case JsonValueKind.String:
                return element.GetString();
            case JsonValueKind.Number:
                if (element.TryGetInt64(out var l)) return l;
                if (element.TryGetDouble(out var d)) return d;
                return element.GetRawText(); // fallback as string of the number
            case JsonValueKind.True:
                return true;
            case JsonValueKind.False:
                return false;
            case JsonValueKind.Null:
            case JsonValueKind.Undefined:
            default:
                return null;
        }
    }

    
}