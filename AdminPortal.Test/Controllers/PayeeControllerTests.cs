using System.Net;
using System.Net.Http.Json;
using AdminPortal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using AdminPortal.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;

namespace AdminPortal.Test.Controllers;

public class PayeeControllerTests
{
    
    private List<PayeeViewModel> GetMockPayees()
    {
        return new List<PayeeViewModel>
        {
            new()
            {
                PayeeID = 1, 
                Name = "Telstra", 
                Address = "1 Telstra Street", 
                City = "Melbourne", 
                State = "VIC", 
                Phone = "1234567843", 
                PostCode = "3000"
            },
            new()
            {
                PayeeID = 2, 
                Name = "Vodafone", 
                Address = "Vod Street 2", 
                City = "Melbourne", 
                State = "NSW", 
                Phone = "2234567843", 
                PostCode = "3000"
            }
        };
    }
    
    [Fact]
    public async Task Index_ReturnsViewWithPayees()
    {
        var payees = GetMockPayees();
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(payees)
        };
        
        // Create client mock and pass into controller
        var clientMock = MockHttpClient.Create(response);
        var controller = new PayeeController(clientMock);
        
        // calling index function of the controller
        var result = await controller.Index();
        
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<PayeeListViewModel>(viewResult.Model);
        Assert.Equal(2, model.Payees.Count);
        Assert.Contains(model.AllPostalCodes, p => p == "3000");
    }
    
    [Fact]
    public async Task Edit_ReturnsSelectedPayee()
    {

        var payees = GetMockPayees();
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(payees)
        };
        var clientMock = MockHttpClient.Create(response);
        var controller = new PayeeController(clientMock);
        
        // calling edit function of the controller
        var result = await controller.Edit(2);
        
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<PayeeListViewModel>(viewResult.Model);
        Assert.NotNull(model.SelectedPayee);
        Assert.Equal(2, model.SelectedPayee.PayeeID);
    }
    
    [Fact]
    public async Task CreatePayeeSuccessTempData()
    {
        var payee = new PayeeViewModel
        {
            Name = "electricity", 
            Address = "Street 1", 
            City = "City 1", 
            State = "VIC", 
            Phone = "1112223334", 
            PostCode = "2134"
        };
        var model = new PayeeListViewModel { NewPayee = payee };
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        
        var clientMock = MockHttpClient.Create(response);
        var controller = new PayeeController(clientMock);
        
        // mocking tempdata 
        var tempDataProvider = new Mock<ITempDataProvider>();
        controller.TempData = new TempDataDictionary(new DefaultHttpContext(), tempDataProvider.Object);
        
        var result = await controller.Create(model);
        
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirect.ActionName);
        Assert.Equal("Payee created successfully!", controller.TempData["SuccessMessage"]);
    }
    
    [Fact]
    public async Task CreatePayeeFailedTempData()
    {
        var payee = new PayeeViewModel
        {
            Name = "electricity", 
            Address = "Street 1", 
            City = "City 1", 
            State = "VIC", 
            Phone = "1112223334", 
            PostCode = "2134"
        };
        var model = new PayeeListViewModel { NewPayee = payee };
        var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
        
        var clientMock = MockHttpClient.Create(response);
        var controller = new PayeeController(clientMock);
        
        // mocking tempdata 
        var tempDataProvider = new Mock<ITempDataProvider>();
        controller.TempData = new TempDataDictionary(new DefaultHttpContext(), tempDataProvider.Object);
        
        var result = await controller.Create(model);
        
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirect.ActionName);
        Assert.Equal("Failed to create payee. Please try again.", controller.TempData["ErrorMessage"]);
    }
    
    [Fact]
    public async Task UpdatePayeeSuccessTempData()
    {
        var payee = new PayeeViewModel
        {
            PayeeID = 1,
            Name = "electricity updated", 
            Address = "Street 1", 
            City = "City 1", 
            State = "VIC", 
            Phone = "1112223334", 
            PostCode = "2134"
        };
        var model = new PayeeListViewModel { SelectedPayee = payee };
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        
        var clientMock = MockHttpClient.Create(response);
        var controller = new PayeeController(clientMock);
        
        // mocking tempdata 
        var tempDataProvider = new Mock<ITempDataProvider>();
        controller.TempData = new TempDataDictionary(new DefaultHttpContext(), tempDataProvider.Object);
        
        var result = await controller.Update(model);
        
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirect.ActionName);
        Assert.Equal("Payee updated successfully!", controller.TempData["SuccessMessage"]);
    }
    
    [Fact]
    public async Task UpdatePayeeFailedTempData()
    {
        var payee = new PayeeViewModel
        {
            PayeeID = 1,
            Name = "electricity updated", 
            Address = "Street 1", 
            City = "City 1", 
            State = "VIC", 
            Phone = "1112223334", 
            PostCode = "2134"
        };
        var model = new PayeeListViewModel { SelectedPayee = payee };
        var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
        
        var clientMock = MockHttpClient.Create(response);
        var controller = new PayeeController(clientMock);
        
        // mocking tempdata 
        var tempDataProvider = new Mock<ITempDataProvider>();
        controller.TempData = new TempDataDictionary(new DefaultHttpContext(), tempDataProvider.Object);
        
        var result = await controller.Update(model);
        
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirect.ActionName);
        Assert.Equal("Failed to update payee.", controller.TempData["ErrorMessage"]);
    }
}