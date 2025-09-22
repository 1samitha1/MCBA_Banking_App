using System.Globalization;
using System.Text.Json.Serialization;
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
            // check for existing customers
            if (mcbacontext.Customers.Any())
            {
                Console.WriteLine("No web requests will be processed as customers exists in DB.");
                return true;
            }

            using var client = new HttpClient();
            var jsonData = await client.GetStringAsync(webServiceUrl);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            };

            var customers = JsonSerializer.Deserialize<List<Customer>>(jsonData, options);
            if (customers == null)
            {
                Console.WriteLine("No customer data found in web service.");
                return false;
            }

            foreach (var customer in customers)
            {
                if (customer.Login != null)
                {
                    // create login data
                    customer.Login.CustomerID = customer.CustomerID;
                    customer.Login.Customer = customer;
                }
                
                foreach (var account in customer.Accounts)
                {
                    account.Customer = customer;
                    decimal balance = 0;

                    foreach (var transaction in account.Transactions)
                    {
                        // setting transactions
                        transaction.Account = account;
                        transaction.TransactionType = TransactionType.Deposit;
                        transaction.AccountNumber = account.AccountNumber;

                        if (!string.IsNullOrWhiteSpace(transaction.TransactionTimeUtcJson))
                        {
                            // converting time
                            transaction.TransactionTimeUtc = DateTime.ParseExact(
                                transaction.TransactionTimeUtcJson,
                                "dd/MM/yyyy hh:mm:ss tt",
                                CultureInfo.InvariantCulture);
                        }

                        balance += transaction.Amount;
                    }

                    account.Balance = balance;
                }
                mcbacontext.Customers.Add(customer);
            }

            await mcbacontext.SaveChangesAsync();
            return true;
        }
        catch (Exception ex) {
            Console.WriteLine($"An error occurred when request web api data: {ex.Message}");
            return false;
        }
    }
}