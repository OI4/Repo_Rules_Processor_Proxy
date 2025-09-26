namespace AasDemoapp.Models;

public class RuleEvaluationResult
{
    public bool WasSuccessful { get; set; }

    public Action ActionToPerform{ get; set; }
    
}

public class Action
{
    public String TargegSubmodelName { get; set; }
    public String TargetPropertyValue{ get; set; }
    public String TargetPropertyName { get; set; }
}