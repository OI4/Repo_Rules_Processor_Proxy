using Microsoft.AspNetCore.Mvc;
using AasDemoapp.Models;
using AasDemoapp.Services;
using AasDemoapp.Services.impl;

namespace AasDemoapp.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ProxyController : ControllerBase
{
    private readonly RulesEngineInterface _rulesEngineService;
    private readonly AASProxyService _proxyService;
    private readonly RulesRepositoryInterface _rulesRepositoryService;

    public ProxyController()
    {
        _rulesEngineService = new FakeRulesEngineServiceImpl();
        _proxyService = new MetaLevelAASProxyServiceImpl();
        _rulesRepositoryService = new InMemoryRulesRepositoryServiceImpl();
    }

    [HttpPatch]
    public async void UpdateProperty(String submodelID, String propertyName, String propertyValue)
    {

        _proxyService.UpdatePropertyValue(submodelID, propertyName, propertyValue);
        
        List<Rule> preRules = _rulesRepositoryService.GetPreRulesForProperty(propertyName);
        foreach (Rule rule in preRules)
        {
            RuleEvaluationResult result = await _rulesEngineService.ExecuteRule(submodelID, propertyName, propertyValue, rule);
            if(!result.WasSuccessful)
            {
                return; // todo: error message + correct status code here
            }
        }
        List<Rule> postRules = _rulesRepositoryService.GetPostRulesForProperty(propertyName);
        foreach (Rule rule in postRules)
        {
            RuleEvaluationResult result = await _rulesEngineService.ExecuteRule(submodelID, propertyName, propertyValue, rule);
            if(!result.WasSuccessful)
            {
                return; // todo: error message + correct status code here
            }
            // todo: execute action
            // todo-discuss: kommt hier wirklich die Aktion von Drools zurück, oder wird die lediglich in Drools selbst ausgeführt, und man muss dann einen Diff Check machen
        }
    }



}
