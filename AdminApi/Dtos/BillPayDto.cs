using CustomerPortal.Utility;

namespace AdminApi.Dtos;

public record BillPayDto(
    int BillPayID,
    int AccountNumber,
    int PayeeID,
    string PayeeName,
    decimal Amount,
    DateTime ScheduleTimeUtc,
    BillPeriod Period,
    bool IsBlocked
    );