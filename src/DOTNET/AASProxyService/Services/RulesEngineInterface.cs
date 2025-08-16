using AasDemoapp.Models;

namespace AasDemoapp.Services;

public interface RulesEngineInterface
{
    RuleEvaluationResult ExecuteRule(Rule rule);
    
}