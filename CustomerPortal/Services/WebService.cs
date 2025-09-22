using System.Globalization;
using CustomerPortal.Data;
using CustomerPortal.Models;

namespace CustomerPortal.Services;
using System.Net.Http;
using System.Text.Json;

public class WebService
{
    private readonly string webServiceUrl;
    private readonly McbaContext mcbacontext;

    public WebService(IConfiguration configuration, McbaContext context)
    {
        webServiceUrl = configuration.GetConnectionString("webApiConnectionString")!;
        mcbacontext = context;
    }
    
    public async Task<bool> HandleWebRequest()
    {
        try
        {
            if (mcbacontext == null)
            {
                Console.WriteLine("mcbacontext is null!");
                return false;
            }
            
            using var client = new HttpClient();
            var jsonData = await client.GetStringAsync(webServiceUrl);
            
            Console.Write("before " + jsonData);
            
            var customers = JsonSerializer.Deserialize<List<Customer>>(jsonData);
            
            foreach (var customer in customers)
            {
                // Process Login
                // if (customer.Login != null)
                // {
                //
                //     customer.Login.CustomerID = customer.CustomerID;
                //     customer.Logins.Add(customer.Login);
                //     customer.Login.Customer = customer;
                // }
            
                // Process Accounts
                foreach (var account in customer.Accounts)
                {
                    account.Customer = customer;
                    decimal balance = 0;
            
                    if (account.Transactions != null)
                    {
                        foreach (var transaction in account.Transactions)
                        {
                            if (transaction == null) continue;
            
                            transaction.Account = account;
                            transaction.TransactionType = TransactionType.Deposit;
                            transaction.AccountNumber = account.AccountNumber;
            
                            // Use the correct JSON property name
                            if (!string.IsNullOrWhiteSpace(transaction.TransactionTimeUtcJson))
                            {
                                transaction.TransactionTimeUtc = DateTime.ParseExact(
                                    transaction.TransactionTimeUtcJson,
                                    "dd/MM/yyyy hh:mm:ss tt",
                                    CultureInfo.InvariantCulture);
                            }
            
                            balance += transaction.Amount;
                        }
                    }
            
                    account.Balance = balance;
                }
            
                // Add the full graph to DbContext
                mcbacontext.Customers.Add(customer);
            }
            
            await mcbacontext.SaveChangesAsync();
            return true;
            
           
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred when request web api data: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
            }
            return false;
            
        }
    }
}