using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AdminApi.Data.Repository;
using AdminApi.Dtos;
using CustomerPortal.Models;
using Moq;

namespace AdminApi.Tests
{
    public static class MockPayeeRepository
    {
        public static Mock<IPayeeRepository> GetPayeeMock()
        {
            var mock = new Mock<IPayeeRepository>();

            // Sample data
            var payees = new List<Payee>
            {
                new Payee
                {
                    PayeeID = 1, 
                    Name = "Vodafone", 
                    Address = "vod Street 1", 
                    City = "Melbourne", 
                    State = "VIC", 
                    PostCode = "3000", 
                    Phone = "1234567890"
                },
                new Payee
                {
                    PayeeID = 2, 
                    Name = "Optus", 
                    Address = "opt Street 2", 
                    City = "Sydney", 
                    State = "NSW", 
                    PostCode = "2000", 
                    Phone = "9876543210"
                }
            };
            
            mock.Setup(repo => repo.GetAllPayeesAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((string postcode, CancellationToken _) =>
                {
                    if (string.IsNullOrEmpty(postcode))
                        return payees;
                    return payees.FindAll(p => p.PostCode == postcode);
                });
            
            mock.Setup(repo => repo.GetPayeeAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken _) => payees.Find(p => p.PayeeID == id));
            
            mock.Setup(repo => repo.UpdatePayeeAsync(It.IsAny<Payee>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask)
                .Callback((Payee updatedPayee, CancellationToken _) =>
                {
                    var existing = payees.Find(p => p.PayeeID == updatedPayee.PayeeID);
                    if (existing != null)
                    {
                        existing.Name = updatedPayee.Name;
                        existing.Address = updatedPayee.Address;
                        existing.City = updatedPayee.City;
                        existing.State = updatedPayee.State;
                        existing.Phone = updatedPayee.Phone;
                        existing.PostCode = updatedPayee.PostCode;
                    }
                });
            
            mock.Setup(repo => repo.CreatePayeeAsync(It.IsAny<PayeeDto>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask)
                .Callback((PayeeDto newPayee, CancellationToken _) =>
                {
                    payees.Add(new Payee
                    {
                        PayeeID = payees.Count + 1,
                        Name = newPayee.Name,
                        Address = newPayee.Address,
                        City = newPayee.City,
                        State = newPayee.State,
                        PostCode = newPayee.Postcode,
                        Phone = newPayee.Phone
                    });
                });

            return mock;
        }
    }
}
