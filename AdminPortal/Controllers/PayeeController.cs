using System.Text;
using System.Text.Json;
using AdminPortal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminPortal.Controllers;

[Authorize(Roles = "Admin")]
public class PayeeController: Controller
{
    private readonly HttpClient _client;
    
    public PayeeController(IHttpClientFactory clientFactory)
    {
        _client = clientFactory.CreateClient("AdminApi");
    }
    
    
    public async Task<IActionResult> Index()
    {
        
        var payees = await GetAllPayees();

        var model = new PayeeListViewModel
        {
            Payees = payees,
            AllPostalCodes = payees.Select(p => p.PostCode)
                .Where(p => !string.IsNullOrEmpty(p))
                .Distinct()
                .ToList(),
            SelectedPostalCode = null,
         
        };

        return View(model);
    }

    [HttpGet]
    public async Task<List<PayeeViewModel>> GetAllPayees(string? postCode = null)
    {
        string url = "api/payee";
        // if post code is provided, it will add 
        if (!string.IsNullOrEmpty(postCode))
        {
            url += $"?postCode={Uri.EscapeDataString(postCode)}";
        }
        
        using var response = await _client.GetAsync(url);
    
        response.EnsureSuccessStatusCode();

        var payees = await response.Content.ReadFromJsonAsync<List<PayeeViewModel>>();

        return payees ?? new List<PayeeViewModel>();
    }
    
  
    public async Task<IActionResult> Filter(string? postalCode)
    {
        // Call your existing function to get payees
        var FilteredPayees = await GetAllPayees(postalCode); // your function should accept postalCode as optional
        var allPayees = await GetAllPayees();
        // Create the view model
        var viewModel = new PayeeListViewModel
        {
            Payees = FilteredPayees,
            AllPostalCodes = allPayees.Select(p => p.PostCode)
                .Where(p => !string.IsNullOrEmpty(p))
                .Distinct()
                .ToList(),
            SelectedPostalCode = postalCode,
            SelectedPayee = null
        };

        // Return the same view (Index) with filtered data
        return View("Index", viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int payeeId)
    {
        // Get all payees
        var payees = await GetAllPayees();

        // Find the selected one
        var selected = payees.FirstOrDefault(p => p.PayeeID == payeeId);

        // Prepare the full model for the same Index view
        var model = new PayeeListViewModel
        {
            Payees = payees,
            AllPostalCodes = payees.Select(p => p.PostCode)
                .Where(p => !string.IsNullOrEmpty(p))
                .Distinct()
                .ToList(),
            SelectedPostalCode = null,
            SelectedPayee = selected
        };

        return View("Index", model);
    }

    [HttpPost]
    public async Task<IActionResult> Update(PayeeListViewModel model)
    {
        var payee = model.SelectedPayee;

        string url = "api/Payee";
        var json = JsonSerializer.Serialize(payee);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        using var response = await _client.PutAsync(url, content);

        if (response.IsSuccessStatusCode)
            TempData["SuccessMessage"] = "Payee updated successfully!";
        else
        {
            TempData["ErrorMessage"] = "Failed to update payee.";
        }

        return RedirectToAction("Index");
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(PayeeListViewModel model)
    {
        var payee = model.NewPayee;

        string url = "api/payee";
        var json = JsonSerializer.Serialize(payee);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        using var response = await _client.PostAsync(url, content);

        if (response.IsSuccessStatusCode)
        {
            TempData["SuccessMessage"] = "Payee created successfully!";
        }
        else
        {
            TempData["ErrorMessage"] = "Failed to create payee. Please try again.";
        }

        return RedirectToAction("Index");
    }
    
}