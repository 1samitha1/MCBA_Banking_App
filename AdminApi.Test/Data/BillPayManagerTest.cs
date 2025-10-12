using AdminApi.Data.DataManager;
using CustomerPortal.Models;
using Microsoft.EntityFrameworkCore;
using CustomerPortal.Data;

namespace AdminApi.Tests
{
    public class BillPayManagerTests
    {
        private McbaContext GetInMemoryDb()
        {
            var options = new DbContextOptionsBuilder<McbaContext>()
                .UseInMemoryDatabase(databaseName: "BillPayTestDb")
                .Options;
            return new McbaContext(options);
        }
        
        // seed data for testing
        private async Task SeedData(McbaContext context)
        {
            context.BillPay.AddRange(new List<BillPay>
            {
                new BillPay { BillPayID = 1, AccountNumber = 12345678, IsBlocked = false },
                new BillPay { BillPayID = 2, AccountNumber = 22233345, IsBlocked = true }
            });
            await context.SaveChangesAsync();
        }
        
        [Fact]
        public async Task GetAllAsyncTest()
        {

            var context = GetInMemoryDb();
            await SeedData(context);
            var manager = new BillPayManager(context);
            
            var blockedBill = await manager.GetAllAsync(true, CancellationToken.None);
            var unblockedBill = await manager.GetAllAsync(false, CancellationToken.None);
            
            Assert.Single(blockedBill);
            Assert.Single(unblockedBill);
            Assert.Equal(12345678, unblockedBill[0].AccountNumber);
            Assert.Equal(22233345, blockedBill[0].AccountNumber);
        }
        
        [Fact]
        public async Task GetAsyncTest()
        {
            
            var context = GetInMemoryDb();
            context.BillPay.RemoveRange(context.BillPay);
            await SeedData(context);
            var manager = new BillPayManager(context);
            
            var bill = await manager.GetAsync(2, CancellationToken.None);
            var invalidBill = await manager.GetAsync(99, CancellationToken.None);
            
            Assert.NotNull(bill);
            Assert.Equal(22233345, bill!.AccountNumber);
            Assert.Null(invalidBill);
        }
        
        [Fact]
        public async Task SetBlockedAsyncTest()
        {
            var context = GetInMemoryDb();
            context.BillPay.RemoveRange(context.BillPay);
            await SeedData(context);
            var manager = new BillPayManager(context);
            
            await manager.SetBlockedAsync(1, true, CancellationToken.None);
            var updated = await context.BillPay.FindAsync(1);
            Assert.True(updated.AccountNumber == 12345678);
            Assert.True(updated.IsBlocked);
            
        }
        
        [Fact]
        public async Task SetBlockedAsyncNotFoundExceptionTest()
        {
            var context = GetInMemoryDb();
            var manager = new BillPayManager(context);

            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                manager.SetBlockedAsync(99, true, CancellationToken.None));
        }
    }
}