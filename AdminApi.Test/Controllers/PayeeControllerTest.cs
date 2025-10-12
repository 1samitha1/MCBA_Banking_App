using System.Text.Json;
using AdminApi.Controllers;
using AdminApi.Data.Repository;
using AdminApi.Dtos;
using AdminApi.Tests;
using CustomerPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AdminApi.Test.Controllers;

public class PayeeControllerTest
{
    private readonly Mock<IPayeeRepository> _mockRepo;
    private readonly PayeeController _controller;

    public PayeeControllerTest()
    {
        _mockRepo = MockPayeeRepository.GetPayeeMock();
        _controller = new PayeeController(_mockRepo.Object);
    }

    [Fact]
    public async Task GetAllPayeeTest()
    {
        var result = await _controller.GetAllPayee(null, CancellationToken.None);
        
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var payees = Assert.IsAssignableFrom<List<PayeeDto>>(okResult.Value);
        Assert.Equal(2, payees.Count);
        Assert.Equal("Vodafone", payees[0].Name);
        Assert.Equal("Melbourne", payees[0].City);
        Assert.Equal("Optus", payees[1].Name);
        Assert.Equal("NSW", payees[1].State);
    }
    
    [Fact]
    public async Task GetAllPayeeByPostCodeTest()
    {
        var result = await _controller.GetAllPayee("3000", CancellationToken.None);
        
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var payees = Assert.IsAssignableFrom<List<PayeeDto>>(okResult.Value);
        Assert.Single(payees);
        Assert.Equal("3000", payees[0].Postcode);
        Assert.Equal("Vodafone", payees[0].Name);
    }
    
    [Fact]
    public async Task CreatePayeeTest()
    {

        var payeeDto = new PayeeDto(
            0,           
            "Electricity",
            "Main Street",
            "Melbourne",
            "VIC",
            "3000",
            "1234567891"
        );
        
        var result = await _controller.CreatePayee(payeeDto, CancellationToken.None);
        
        var created = Assert.IsType<CreatedResult>(result);
        var json = JsonSerializer.Serialize(created.Value);
        using var jsonDoc = JsonDocument.Parse(json);
        var message = jsonDoc.RootElement.GetProperty("message").GetString();
        Assert.Equal("Payee Created", message);
        _mockRepo.Verify(r => r.CreatePayeeAsync(It.IsAny<PayeeDto>(), It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task CreatePayeeBadRequestTest()
    {
        // empty values
        var invalidPayeeDto = new PayeeDto(
            0, 
            "", 
            "", 
            "", 
            "", 
            "", 
            "");
        
        _controller.ModelState.AddModelError("Name", "Name is required");
        
        var result = await _controller.CreatePayee(invalidPayeeDto, CancellationToken.None);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequest.StatusCode);
        _mockRepo.Verify(r => r.CreatePayeeAsync(It.IsAny<PayeeDto>(), It.IsAny<CancellationToken>()), Times.Never);
    }
    
    [Fact]
    public async Task UpdatePayeeTest()
    {
        var payee = new Payee
        {
            PayeeID = 1,
            Name = "Electricity",
            Address = "Main Street",
            City = "Melbourne",
            State = "VIC",
            PostCode = "3000",
            Phone = "1234567891"
        };
        
        var result = await _controller.UpdatePayee(payee, CancellationToken.None);
        
        var okResult = Assert.IsType<OkObjectResult>(result);
        var json = JsonSerializer.Serialize(okResult.Value);
        using var jsonDoc = JsonDocument.Parse(json);
        var message = jsonDoc.RootElement.GetProperty("message").GetString();
        Assert.Equal("Payee Updated", message);
        _mockRepo.Verify(r => r.UpdatePayeeAsync(It.IsAny<Payee>(), It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task UpdatePayeeBadRequestTest()
    {
        var payee = new Payee { PayeeID = 1, Name = null, Address = null, City = null, State = null, Phone = null};
        _controller.ModelState.AddModelError("Name", "Name is required");
        _controller.ModelState.AddModelError("Address", "Address is required");
        _controller.ModelState.AddModelError("City", "City is required");
        
        var result = await _controller.UpdatePayee(payee, CancellationToken.None);
        
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequest.StatusCode);
        _mockRepo.Verify(r => r.UpdatePayeeAsync(It.IsAny<Payee>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}