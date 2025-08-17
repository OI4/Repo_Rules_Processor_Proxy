using AasDemoapp.Models;

namespace AasDemoapp.Services.impl;

public class FakeRulesEngineServiceImpl: RulesEngineInterface
{

    public async Task<RuleEvaluationResult> ExecuteRule(string submodelName, string propertyName, string propertyValue, Rule rule)
    {
        Console.WriteLine(rule.Name);
        var result =  new RuleEvaluationResult();
        result.WasSuccessful = true;
        return result;
    }
}