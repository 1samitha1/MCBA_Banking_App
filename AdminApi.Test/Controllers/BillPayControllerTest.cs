using AdminApi.Controllers;
using AdminApi.Data.Repository;
using AdminApi.Dtos;
using AdminApi.Tests;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AdminApi.Test.Controllers;

public class BillPayControllerTest
{
    private readonly Mock<IBillPayRepository> _mockRepo;
    private readonly BillPayController _controller;

    public BillPayControllerTest()
    {
        _mockRepo = MockBillPayRepository.GetBillPayMock();
        _controller = new BillPayController(_mockRepo.Object);
    }
    
    [Fact]
    public async Task UpdateBlockBillpayAsyncTest()
    {
        var request = new BlockBillPayRequest { Blocked = true };
        var result = await _controller.UpdateBlockBillpayAsync(1, request, CancellationToken.None);
        
        Assert.IsType<NoContentResult>(result);
        _mockRepo.Verify(r => r.SetBlockedAsync(1, true, It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task UpdateBlockBillpayAsyncInvalidIdTest()
    {
        var request = new BlockBillPayRequest { Blocked = true };

        var result = await _controller.UpdateBlockBillpayAsync(101, request, CancellationToken.None);

        Assert.IsType<NotFoundResult>(result);
        _mockRepo.Verify(r => r.SetBlockedAsync(101, true, It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task UpdateBlockBillpayAsyncBadRequestTest()
    {
        _controller.ModelState.AddModelError("Blocked", "Required");
        var request = new BlockBillPayRequest { Blocked = true };

        var result = await _controller.UpdateBlockBillpayAsync(1, request, CancellationToken.None);
        
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.NotNull(badRequestResult.Value);
    }
}