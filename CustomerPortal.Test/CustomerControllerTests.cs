using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

using CustomerPortal.Controllers;
using CustomerPortal.Services;

namespace CustomerPortal.Tests.Controllers
{
    public class CustomerControllerTests
    {
        [Fact]
        public async Task Index_NotSignedIn_RedirectsToHomeIndex()
        {
            // Arrange
            var controller = new CustomerController(
                Mock.Of<ICustomerService>(),
                Mock.Of<IPasswordService>());

            
            var http = new DefaultHttpContext();
            http.Session = new InMemorySession();   
            controller.ControllerContext = new ControllerContext { HttpContext = http };

            // Act
            var result = await controller.Index();

            // Assert
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
            Assert.Equal("Home", redirect.ControllerName);
        }
    }

    
    internal sealed class InMemorySession : ISession
    {
        private readonly Dictionary<string, byte[]> _store = new();
        public IEnumerable<string> Keys => _store.Keys;
        public string Id { get; } = "test-session";
        public bool IsAvailable => true;

        public void Clear() => _store.Clear();
        public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
        public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
        public void Remove(string key) => _store.Remove(key);
        public void Set(string key, byte[] value) => _store[key] = value;
        public bool TryGetValue(string key, out byte[] value) => _store.TryGetValue(key, out value!);
    }
}
