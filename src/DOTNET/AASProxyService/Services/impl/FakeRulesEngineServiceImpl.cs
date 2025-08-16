using AasDemoapp.Models;

namespace AasDemoapp.Services.impl;

public class FakeRulesEngineServiceImpl: RulesEngineInterface
{
    public RuleEvaluationResult ExecuteRule(Rule rule)
    {
        Console.WriteLine(rule.Name);
        var result =  new RuleEvaluationResult();
        result.WasSuccessful = true;
        return result;;
    }
}