namespace DP_BurLida.Services.Interfaces
{
    public interface IPushNotificationService
    {
        Task SendToUserAsync(string userEmail, string title, string body, Dictionary<string, string>? data = null);
        Task SendToUsersAsync(IEnumerable<string> userEmails, string title, string body, Dictionary<string, string>? data = null);
    }
}

