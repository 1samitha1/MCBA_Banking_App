using System.ComponentModel.DataAnnotations;

namespace AdminApi.Dtos;

public class CustomerDto
{
    [Required, StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(11)]
    public string TFN { get; set; } = string.Empty;

    [StringLength(50)]
    public string? Address { get; set; }

    [StringLength(40)]
    public string? City { get; set; }

    [StringLength(3)]
    public string? State { get; set; }

    [StringLength(4)]
    public string? PostCode { get; set; }

    [StringLength(12)]
    public string? Mobile { get; set; }
}