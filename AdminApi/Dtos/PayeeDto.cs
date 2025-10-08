namespace AdminApi.Dtos;

public record PayeeDto(
    int PayeeID,
    string Name,
    string? Address,
    string? City,
    string? State,
    string? Postcode,
    string? Phone
    );