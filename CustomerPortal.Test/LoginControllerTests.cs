// CustomerPortal.Tests/Controllers/LoginControllerTests.cs
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Xunit;
using CustomerPortal.Controllers;
using CustomerPortal.Services;
using CustomerPortal.ViewModel;

namespace CustomerPortal.Tests.Controllers
{
    public class LoginControllerTests
    {
        private static void Wire(Controller c)
        {
            var http = new DefaultHttpContext();
            c.ControllerContext = new ControllerContext { HttpContext = http };
            c.TempData = new TempDataDictionary(http, Mock.Of<ITempDataProvider>());
        }

        [Fact]
        public void Index_ReturnsView()
        {
            var auth = new Mock<IAuthService>();
            var controller = new LoginController(auth.Object);
            Wire(controller);

            var result = controller.Index();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task HandleLogin_InvalidModel_ReturnsView()
        {
            var auth = new Mock<IAuthService>();
            var controller = new LoginController(auth.Object);
            Wire(controller);
            controller.ModelState.AddModelError("LoginID", "Required");

            var vm = new LoginViewModel();
            var result = await controller.HandleLogin(vm);

            var view = Assert.IsType<ViewResult>(result);
            Assert.Same(vm, view.Model);
        }
    }
}