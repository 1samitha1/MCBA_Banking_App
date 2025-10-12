using AdminApi.Data.DataManager;
using AdminApi.Dtos;
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
        
        private async Task SeedData(McbaContext context)
        {
            // Payee reference for bills
            context.Payees.AddRange(new List<Payee>
            {
                new Payee { PayeeID = 1, Name = "Telstra", Address = "Main St", City = "Melbourne", State = "VIC", Phone = "111222333", PostCode = "3000" },
                new Payee { PayeeID = 2, Name = "Vodafone", Address = "Second St", City = "Melbourne", State = "VIC", Phone = "444555666", PostCode = "3001" }
            });
            
            context.BillPay.AddRange(new List<BillPay>
            {
                new BillPay { BillPayID = 1, AccountNumber = 12345678, IsBlocked = false, PayeeID = 1 },
                new BillPay { BillPayID = 2, AccountNumber = 22233345, IsBlocked = true, PayeeID = 2 }
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
            context.Payees.RemoveRange(context.Payees);
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
            context.Payees.RemoveRange(context.Payees);
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