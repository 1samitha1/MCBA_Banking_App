using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

using CustomerPortal.Controllers;
using CustomerPortal.Data.Repository;
using CustomerPortal.Models;
using CustomerPortal.Services;
using CustomerPortal.Utility;
using CustomerPortal.ViewModel.Statement;

namespace CustomerPortal.Tests.Controllers
{
    public class StatementControllerTests
    {
        [Fact]
        public async Task Statement_NotSignedIn_RedirectsToLogin()
        {
            var txRepo   = new Mock<ITransactionRepository>();
            var accRepo  = new Mock<IAccountRepository>();
            var auth     = new Mock<IAuthService>();
            auth.Setup(a => a.CurrentCustomerId()).Returns((int?)null);

            var controller = new StatementController(txRepo.Object, accRepo.Object, auth.Object);

            var result = await controller.Statement(4100, 1);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
            Assert.Equal("Login", redirect.ControllerName);
        }

        [Fact]
        public async Task Print_ReturnsPrintView_WithModel()
        {
            const int customerId = 99;
            const int accountNum = 4100;

            var rows = new List<TransactionRowViewModel>(); // simple empty list
            var txRepo  = new Mock<ITransactionRepository>();
            txRepo.Setup(r => r.GetPagedTransactions(accountNum, 1, int.MaxValue))
                  .ReturnsAsync((rows, 0));

            var accRepo = new Mock<IAccountRepository>();
            accRepo.Setup(r => r.GetAccountAsync(accountNum))
                   .ReturnsAsync(new Account { AccountNumber = accountNum, CustomerID = customerId, AccountType = AccountType.C, Balance = 123m });

            var auth = new Mock<IAuthService>();
            auth.Setup(a => a.CurrentCustomerId()).Returns(customerId);

            var controller = new StatementController(txRepo.Object, accRepo.Object, auth.Object);

            var result = await controller.Print(accountNum);
            var view = Assert.IsType<ViewResult>(result);
            Assert.Equal("Print", view.ViewName);
            Assert.IsType<SingleStatementPageViewModel>(view.Model);
        }
    }
}
