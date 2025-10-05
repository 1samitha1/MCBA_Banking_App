using System.ComponentModel.DataAnnotations;

namespace AdminApi.Dtos;

public class BlockBillPayRequest
{
    [Required]
    public int BillPayId { get; set; }
    [Required]
    public bool Blocked { get; set; }
}