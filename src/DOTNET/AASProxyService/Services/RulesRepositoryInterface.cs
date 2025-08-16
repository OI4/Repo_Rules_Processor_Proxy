using AasDemoapp.Models;

namespace AasDemoapp.Services;

public interface RulesRepositoryInterface
{
    List<Rule> GetPreRulesForProperty(string propertyName);

    List<Rule> GetPostRulesForProperty(string propertyName);
}