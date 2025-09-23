namespace CustomerPortal.Services;

public interface IWebService
{
    Task<bool> HandleWebRequest();
}