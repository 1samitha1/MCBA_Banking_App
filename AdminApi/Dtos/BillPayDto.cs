namespace AdminApi.Dtos;

public record BillPayDto(
    int BillPayID,
    int AccountNumber,
    int PayeeID,
    decimal Amount,
    DateTime ScheduleTimeUtc,
    char Period,
    bool IsBlocked
    );