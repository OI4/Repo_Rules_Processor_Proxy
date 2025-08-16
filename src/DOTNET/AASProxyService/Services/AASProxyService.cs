namespace AasDemoapp.Services;

public interface AASProxyService
{
    Task<string> GetPropertyValue(String submodelID, String PropertyName);
    
    Task<string> UpdatePropertyValue(String submodelID, String PropertyName, String PropertyValue);
}
