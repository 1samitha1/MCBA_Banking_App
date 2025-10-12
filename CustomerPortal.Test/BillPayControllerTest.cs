
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

using CustomerPortal.Controllers;
using CustomerPortal.Services;
using CustomerPortal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace CustomerPortal.Tests.Controllers
{
    public class BillPayControllerTests
    {
        [Fact]
        public async Task Index_ReturnsView()
        {
            var billSvc = new Mock<IBillPayService>();
            var accSvc  = new Mock<IAccountService>();
            var auth    = new Mock<IAuthService>();

            auth.Setup(a => a.CurrentCustomerId()).Returns(10);
            billSvc.Setup(s => s.GetBills(10)).ReturnsAsync(new List<BillPay>());
            billSvc.Setup(s => s.GetPayees()).ReturnsAsync(new List<Payee>());
            accSvc.Setup(s => s.GetCustomerAccounts(10)).ReturnsAsync(new List<Account>());

            var controller = new BillPayController(billSvc.Object, accSvc.Object, auth.Object);

            var result = await controller.Index();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Remove_ReturnsIndexView_WithSuccessModel()
        {
            // Arrange
            var billSvc = new Moq.Mock<IBillPayService>();
            var accSvc  = new Moq.Mock<IAccountService>();
            var auth    = new Moq.Mock<IAuthService>();

            
            auth.Setup(a => a.CurrentCustomerId()).Returns(10);

            
            billSvc.Setup(s => s.RemoveBill(123))
                .ReturnsAsync((true, "ok"));

            
            billSvc.Setup(s => s.GetBills(10))
                .ReturnsAsync(new List<CustomerPortal.Models.BillPay>());

            var controller = new BillPayController(billSvc.Object, accSvc.Object, auth.Object);

            // Act
            var result = await controller.Remove(123);

            // Assert
            var view = Assert.IsType<ViewResult>(result);
            Assert.Equal("Index", view.ViewName); 

            var model = Assert.IsType<CustomerPortal.ViewModel.BillPay.BillPayListViewModel>(view.Model);
            Assert.True(model.IsSuccess);         
            Assert.NotNull(model.Bills);          
            Moq.Mock.Get(billSvc.Object).Verify(s => s.RemoveBill(123), Moq.Times.Once);
            Moq.Mock.Get(billSvc.Object).Verify(s => s.GetBills(10), Moq.Times.Once);
        }

    }
}