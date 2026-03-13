namespace DP_BurLida.Api.Dtos
{
    public record NotificationResponse(
        int Id,
        int? OrderId,
        string Message,
        bool IsRead,
        DateTime CreatedAt
    );
}

