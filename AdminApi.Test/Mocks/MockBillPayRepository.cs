using AdminApi.Data.Repository;
using CustomerPortal.Models;
using Moq;

namespace AdminApi.Tests
{
    public static class MockBillPayRepository
    {
        public static Mock<IBillPayRepository> GetBillPayMock()
        {
            var mockRepo = new Mock<IBillPayRepository>();

            // Corrected properties
            mockRepo.Setup(r => r.GetAllAsync(It.IsAny<bool?>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<BillPay>
                {
                    new BillPay { BillPayID = 1, IsBlocked = false },
                    new BillPay { BillPayID = 2, IsBlocked = true }
                });

            mockRepo.Setup(r => r.GetAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken _) =>
                    new BillPay { BillPayID = id, IsBlocked = false });

            mockRepo.Setup(r => r.SetBlockedAsync(1, It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            mockRepo.Setup(r => r.SetBlockedAsync(101, It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .Throws<KeyNotFoundException>();

            return mockRepo;
        }
    }
}