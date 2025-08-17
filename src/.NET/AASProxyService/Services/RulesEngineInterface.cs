using AasDemoapp.Models;

namespace AasDemoapp.Services;

public interface RulesEngineInterface
{
    Task<RuleEvaluationResult> ExecuteRule(String sourceSubmodelName, String sourcePropertyName, String sourcePropertyValue, Rule rule);
    
}