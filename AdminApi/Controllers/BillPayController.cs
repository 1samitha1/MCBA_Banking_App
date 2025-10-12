using AdminApi.Data.Repository;
using AdminApi.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminApi.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class BillPayController :ControllerBase
{
    private readonly IBillPayRepository _billPayRepository;

    public BillPayController(IBillPayRepository billPayRepository)
    {
        _billPayRepository = billPayRepository;
    }

    [HttpPost("block/{id:int}")]
    public async Task<IActionResult> UpdateBlockBillpayAsync(int id, 
        [FromBody] BlockBillPayRequest request,
        CancellationToken ct)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            await _billPayRepository.SetBlockedAsync(id, request.Blocked, ct);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<BillPayDto>>> GetAllBillPay(CancellationToken ct)
    {
        var allBillPay = await _billPayRepository.GetAllAsync(null,ct);
        return Ok(allBillPay);
        
    }
}