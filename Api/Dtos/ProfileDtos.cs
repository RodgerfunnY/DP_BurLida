namespace DP_BurLida.Api.Dtos
{
    public record ProfileResponse(
        int Id,
        string Email,
        string FullName,
        string Role,
        bool IsApproved,
        string Phone
    );
}

