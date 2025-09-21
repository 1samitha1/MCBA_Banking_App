namespace CustomerPortal.Services;
using System.Net.Http;

public class WebService
{
    private readonly string webServiceUrl;

    public WebService(IConfiguration configuration)
    {
        webServiceUrl = configuration.GetConnectionString("webApiConnectionString")!;
    }
    
    public async Task<bool> HandleWebRequest()
    {
        try
        {
            using var client = new HttpClient();
            var jsonData = await client.GetStringAsync(webServiceUrl);
            
            Console.Write("dddd " + jsonData);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred when request web api data: {ex.Message}");
            return false;
        }
    }
}