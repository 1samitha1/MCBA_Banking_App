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
    public async Task<IActionResult> CreatePayee([FromBody]PayeeDto payeeDto, CancellationToken ct)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _payeeRepository.CreatePayeeAsync(payeeDto,ct);
        return Created("", new{message="Payee Created"});
        
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdatePayee([FromBody]Payee payee, CancellationToken ct)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _payeeRepository.UpdatePayeeAsync(payee, ct);
        return Created("", new{message="Payee Updated"});
        
    }
}