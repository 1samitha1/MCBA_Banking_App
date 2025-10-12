using AdminApi.Data.Repository;
using AdminApi.Dtos;
using CustomerPortal.Models;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CustomerPortal.Utility;

namespace AdminApi.Tests
{
    public static class MockBillPayRepository
    {
        public static Mock<IBillPayRepository> GetBillPayMock()
        {
            var mockRepo = new Mock<IBillPayRepository>();

            // Return sample DTOs for GetAllAsync
            mockRepo.Setup(r => r.GetAllAsync(It.IsAny<bool?>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<BillPayDto>
                {
                    new BillPayDto(1, 12345678, 1, "Telstra", 50, System.DateTime.UtcNow, BillPeriod.OneOff, false),
                    new BillPayDto(2, 22233345, 2, "Vodafone", 75, System.DateTime.UtcNow, BillPeriod.Monthly, true)
                });

            // Return EF entity for GetAsync
            mockRepo.Setup(r => r.GetAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken _) =>
                    new BillPay { BillPayID = id, IsBlocked = false, AccountNumber = 12345678 });

            // Mock SetBlockedAsync
            mockRepo.Setup(r => r.SetBlockedAsync(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Throw for invalid ID
            mockRepo.Setup(r => r.SetBlockedAsync(101, It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .Throws<KeyNotFoundException>();

            return mockRepo;
        }
    }
}