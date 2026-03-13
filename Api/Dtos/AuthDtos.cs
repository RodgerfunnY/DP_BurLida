namespace DP_BurLida.Api.Dtos
{
    public record LoginRequest(string Email, string Password);

    public record LoginResponse(
        string AccessToken,
        int UserId,
        string Email,
        string FullName,
        string Role
    );
}

