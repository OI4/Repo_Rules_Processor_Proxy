using AasDemoapp.Models;

namespace AasDemoapp.Services.impl;

public class InMemoryRulesRepositoryServiceImpl : RulesRepositoryInterface
{
    
    private static Dictionary<String, List<Rule>> PRE_RULES = new Dictionary<string, List<Rule>>();
    private static Dictionary<String, List<Rule>> POST_RULES = new Dictionary<string, List<Rule>>();


    public List<Rule> GetPreRulesForProperty(string propertyName)
    {
        return InMemoryRulesRepositoryServiceImpl.PRE_RULES[propertyName];
    }

    public List<Rule> GetPostRulesForProperty(string propertyName)
    {
        return InMemoryRulesRepositoryServiceImpl.POST_RULES[propertyName];
    }
}