using AdminApi.Dtos;
using CustomerPortal.Data.Repository;
using CustomerPortal.Models;
using Microsoft.AspNetCore.Mvc;
using IPayeeRepository = AdminApi.Data.Repository.IPayeeRepository;

namespace AdminApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PayeeController : ControllerBase
{
    private readonly IPayeeRepository _payeeRepository;

    public PayeeController(IPayeeRepository payeeRepository)
    {
        _payeeRepository = payeeRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<PayeeDto>>> GetAllPayee([FromQuery]string? postcode, CancellationToken ct)
    {
        List<Payee> allPayeesAsync = await _payeeRepository.GetAllPayeesAsync(postcode);
        if (allPayeesAsync is null)
        {
            Console.WriteLine("Payee not found");
            return NotFound();
        }
        Console.WriteLine("Payee found");
        if (allPayeesAsync.Count == 0)
        {
            return Ok("There are no payees");
        }
        List<PayeeDto> payeeDtos = allPayeesAsync.Select(p => new PayeeDto(
            p.PayeeID, p.Name, p.Address, p.City, p.State, p.PostCode, p.Phone
        )).ToList();
        
        return Ok(payeeDtos);
    }
    //api/Payee
    [HttpPost]
    public async void AddPayee([FromBody]PayeeDto payeeDto)
    {
        Console.WriteLine("AddPayee");
        if (payeeDto is null) BadRequest("Payee cannot be empty");
        try
        {
            await _payeeRepository.CreatePayeeAsync(payeeDto);
            Ok("Payee created");
        }
        catch (Exception ex)
        {
            BadRequest(ex.Message);
            Console.WriteLine(ex.Message);
        }
        
        
    }
    
    
}