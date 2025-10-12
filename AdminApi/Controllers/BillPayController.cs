using AdminApi.Data.Repository;
using AdminApi.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminApi.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/admin/[controller]")]
[ValidateAntiForgeryToken]
public class BillPayController :ControllerBase
{
    private readonly IBillPayRepository _billPayRepository;

    public BillPayController(IBillPayRepository billPayRepository)
    {
        _billPayRepository = billPayRepository;
    }

    [HttpPatch("block/{Id:int}")]
    public async Task<IActionResult> UpdateBlockBillpayAsync(int billPayId, [FromBody] BlockBillPayRequest request,
        CancellationToken ct)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            await _billPayRepository.SetBlockedAsync(billPayId, request.Blocked, ct);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}