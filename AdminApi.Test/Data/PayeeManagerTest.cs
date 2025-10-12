using AdminApi.Data.DataManager;
using AdminApi.Dtos;
using CustomerPortal.Models;
using Microsoft.EntityFrameworkCore;
using CustomerPortal.Data;

namespace AdminApi.Tests
{
    public class PayeeManagerTests
    {
        private McbaContext GetInMemoryDb()
        {
            var options = new DbContextOptionsBuilder<McbaContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new McbaContext(options);
        }
        
        private async Task SeedPayees(McbaContext context)
        {
            context.Payees.AddRange(new List<Payee>
            {
                new Payee { PayeeID = 1, Name = "Telstra", Address = "Main St", City = "Melbourne", State = "VIC", Phone = "111222333", PostCode = "3000" },
                new Payee { PayeeID = 2, Name = "Vodafone", Address = "Second St", City = "Melbourne", State = "VIC", Phone = "444555666", PostCode = "3001" }
            });

            await context.SaveChangesAsync();
        }
        
        [Fact]
        public async Task GetAllPayeesAsyncTest()
        {
            var context = GetInMemoryDb();
            await SeedPayees(context);
            var manager = new PayeeManager(context);

            var result = await manager.GetAllPayeesAsync(null, CancellationToken.None);

            Assert.Equal(2, result.Count);
            Assert.Contains(result, p => p.Name == "Telstra");
            Assert.Contains(result, p => p.Name == "Vodafone");
        }
        
        [Fact]
        public async Task GetAllPayeesAsyncWithPostCodeFilterTest()
        {
            var context = GetInMemoryDb();
            await SeedPayees(context);
            var manager = new PayeeManager(context);

            var result = await manager.GetAllPayeesAsync("3000", CancellationToken.None);
            
            Assert.Single(result);
            Assert.Equal("Telstra", result[0].Name);
            Assert.Equal("3000", result[0].PostCode);
        }
        
        [Fact]
        public async Task GetAllPayeesAsyncInvalidFilterTest()
        {
            var context = GetInMemoryDb();
            await SeedPayees(context);
            var manager = new PayeeManager(context);

            var result = await manager.GetAllPayeesAsync("1234", CancellationToken.None);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetPayeeAsync_ReturnsCorrectPayee()
        {
            var context = GetInMemoryDb();
            await SeedPayees(context);
            var manager = new PayeeManager(context);

            var result = await manager.GetPayeeAsync(2);

            Assert.NotNull(result);
            Assert.Equal("Vodafone", result!.Name);
            Assert.Equal("Melbourne", result!.City);
            Assert.Equal("3001", result!.PostCode);
        }
        
        [Fact]
        public async Task GetPayeeAsyncNotExistTest()
        {
            var context = GetInMemoryDb();
            await SeedPayees(context);
            var manager = new PayeeManager(context);
            
            var result = await manager.GetPayeeAsync(999, CancellationToken.None);
            
            Assert.Null(result);
        }
        
        [Fact]
        public async Task CreatePayeeAsync_AddsNewPayee()
        {
            var context = GetInMemoryDb();
            var manager = new PayeeManager(context);

            var newPayee = new PayeeDto(
                0,           
                "Optus",
                "Second Street",
                "Melbourne",
                "VIC",
                "3002",
                "1112223334"
            );

            await manager.CreatePayeeAsync(newPayee);

            var payees = await context.Payees.ToListAsync();
            Assert.Single(payees);
            Assert.Equal("Optus", payees[0].Name);
            Assert.Equal("3002", payees[0].PostCode);
        }
        
        
        [Fact]
        public async Task UpdatePayeeAsyncTest()
        {
            var context = GetInMemoryDb();
            await SeedPayees(context);
            var manager = new PayeeManager(context);

            var payee = await manager.GetPayeeAsync(1);
            payee.Name = "Testra Updated";

            await manager.UpdatePayeeAsync(payee);

            var updated = await context.Payees.FindAsync(1);
            Assert.Equal("Testra Updated", updated.Name);
        }
        
        [Fact]
        public async Task UpdatePayeeAsyncInvalidPayeeTest()
        {
            var context = GetInMemoryDb();
            await SeedPayees(context);
            var manager = new PayeeManager(context);

            var invalidPayee = new Payee
            {
                PayeeID = 999,
                Name = "Nonexistent",
                Address = "No Address",
                City = "Nowhere",
                State = "NA",
                Phone = "000",
                PostCode = "0000"
            };
            
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(
                () => manager.UpdatePayeeAsync(invalidPayee, CancellationToken.None)
            );
        }

    }
}