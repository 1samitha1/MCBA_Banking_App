using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

using CustomerPortal.Controllers;
using CustomerPortal.Data.Repository;
using CustomerPortal.Services;
using CustomerPortal.ViewModel;

namespace CustomerPortal.Tests.Controllers
{
    public class DepositControllerTests
    {
        [Fact]
        public async Task HandleDeposit_NotSignedIn_RedirectsToLogin()
        {
            var accRepo = new Mock<IAccountRepository>();
            var auth    = new Mock<IAuthService>();
            var dep     = new Mock<IDepositService>();

            auth.Setup(a => a.CurrentCustomerId()).Returns((int?)null);

            var controller = new DepositController(accRepo.Object, auth.Object, dep.Object);

            var vm = new DepositViewModel { AccountNumber = 4100, Amount = 50m, Comment = "Top up" };
            var result = await controller.HandleDeposit(vm);

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
            Assert.Equal("Login", redirect.ControllerName);
        }

        [Fact]
        public void DepositSuccess_ReturnsViewWithModel()
        {
            var controller = new DepositController(Mock.Of<IAccountRepository>(), Mock.Of<IAuthService>(), Mock.Of<IDepositService>());

            var result = controller.DepositSuccess(4100, 150m);
            var view = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<DepositViewModel>(view.Model);
            Assert.Equal(4100, model.AccountNumber);
            Assert.Equal(150m, model.Amount);
        }
    }
}