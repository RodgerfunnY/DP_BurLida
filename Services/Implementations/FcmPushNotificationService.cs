using DP_BurLida.Data;
using DP_BurLida.Services.Interfaces;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DP_BurLida.Services.Implementations
{
    public class FcmPushNotificationService : IPushNotificationService
    {
        private readonly IConfiguration _configuration;
        private readonly ByrlidaContext _context;
        private static readonly object _initLock = new();
        private static bool _initialized;

        public FcmPushNotificationService(IConfiguration configuration, ByrlidaContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task SendToUserAsync(string userEmail, string title, string body, Dictionary<string, string>? data = null)
        {
            await SendToUsersAsync(new[] { userEmail }, title, body, data);
        }

        public async Task SendToUsersAsync(IEnumerable<string> userEmails, string title, string body, Dictionary<string, string>? data = null)
        {
            EnsureInitialized();

            var emails = userEmails
                .Where(e => !string.IsNullOrWhiteSpace(e))
                .Select(e => e.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (emails.Count == 0)
            {
                return;
            }

            var tokens = await _context.DeviceTokenModelData
                .AsNoTracking()
                .Where(t => emails.Contains(t.UserEmail))
                .Select(t => t.Token)
                .Distinct()
                .ToListAsync();

            if (tokens.Count == 0)
            {
                return;
            }

            var message = new MulticastMessage
            {
                Tokens = tokens,
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                },
                Data = data ?? new Dictionary<string, string>()
            };

            try
            {
                var resp = await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(message);

                // чистим невалидные токены, чтобы не копились
                var invalidTokens = new List<string>();
                for (var i = 0; i < resp.Responses.Count; i++)
                {
                    if (!resp.Responses[i].IsSuccess)
                    {
                        var ex = resp.Responses[i].Exception;
                        if (ex?.MessagingErrorCode == MessagingErrorCode.Unregistered ||
                            ex?.MessagingErrorCode == MessagingErrorCode.InvalidArgument)
                        {
                            invalidTokens.Add(tokens[i]);
                        }
                    }
                }

                if (invalidTokens.Count > 0)
                {
                    var rows = await _context.DeviceTokenModelData
                        .Where(t => invalidTokens.Contains(t.Token))
                        .ToListAsync();
                    if (rows.Count > 0)
                    {
                        _context.DeviceTokenModelData.RemoveRange(rows);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            catch
            {
                // пуши не должны ломать бизнес-операции
            }
        }

        private void EnsureInitialized()
        {
            if (_initialized)
            {
                return;
            }

            lock (_initLock)
            {
                if (_initialized)
                {
                    return;
                }

                var path = _configuration["Firebase:ServiceAccountPath"];
                var projectId = _configuration["Firebase:ProjectId"];

                if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                {
                    _initialized = true;
                    return;
                }

                if (FirebaseApp.DefaultInstance == null)
                {
                    FirebaseApp.Create(new AppOptions
                    {
                        Credential = GoogleCredential.FromFile(path),
                        ProjectId = string.IsNullOrWhiteSpace(projectId) ? null : projectId
                    });
                }

                _initialized = true;
            }
        }
    }
}

