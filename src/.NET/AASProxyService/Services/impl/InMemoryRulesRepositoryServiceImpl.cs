using AasDemoapp.Models;

namespace AasDemoapp.Services.impl;

public class InMemoryRulesRepositoryServiceImpl : RulesRepositoryInterface
{
    
    private Dictionary<String, List<Rule>> PRE_RULES = new Dictionary<string, List<Rule>>();
    private Dictionary<String, List<Rule>> POST_RULES = new Dictionary<string, List<Rule>>();

    public InMemoryRulesRepositoryServiceImpl()
    {
        // pre rules for "QualityCheckResult"
        List<Rule> preRules = new List<Rule>();
        PRE_RULES.Add("QualityCheckResult", preRules);
        
        // post rules for "QualityCheckResult"
        List<Rule> postRules = new List<Rule>();
        Rule postRuleOne = new Rule();
        postRuleOne.Name = "The Rule";
        postRuleOne.TargetSubmodelName = "aHR0cHM6Ly9leGFtcGxlLmNvbS8vU3VibW9kZWwvSW5jb21pbmdHb29kSW5mb3MvNjg0N184MTk2XzMyMjNfODY2NQ";
        postRuleOne.TargetPropertyName = "StorageLocation";
        postRules.Add(postRuleOne);
        POST_RULES.Add("QualityCheckResult", postRules);
    }    
    
    public List<Rule> GetPreRulesForProperty(string propertyName)
    {
        return PRE_RULES[propertyName];
    }

    public List<Rule> GetPostRulesForProperty(string propertyName)
    {
        return POST_RULES[propertyName];
    }
}