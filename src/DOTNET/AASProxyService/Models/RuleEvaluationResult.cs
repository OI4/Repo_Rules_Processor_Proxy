namespace AasDemoapp.Models;

public class RuleEvaluationResult
{
    public bool WasSuccessful { get; set; }

    public List<Action> ActionToPerform{ get; set; }
    
}

public class Action
{
    public String PropertyValue{ get; set; }
    public String PropertyName { get; set; }
}