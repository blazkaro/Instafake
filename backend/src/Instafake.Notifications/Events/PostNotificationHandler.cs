using FirebaseAdmin.Messaging;
using Instafake.Notifications.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Wolverine.Attributes;

namespace Instafake.Notifications.Events;

public class PostNotificationHandler
{
    private const int GOOGLE_FCM_BATCH_SIZE = 500; // lower if connection exhaustion happens, timeouts, FCM rate limiting

    [NonTransactional]
    public async Task Handle(PostNotification ev, DevicesDbContext dbContext, CancellationToken cancellationToken)
    {
        var tokens = await dbContext.Devices
            .AsNoTracking()
            .Where(p => ev.FollowerIds.Contains(p.UserId)) // evalutes to ANY() instead of IN() since EF 8
            .Select(p => p.DeviceToken)
            .ToListAsync(cancellationToken);

        var authorJson = JsonSerializer.Serialize(ev.Author);

        var tokenBatches = tokens.Chunk(GOOGLE_FCM_BATCH_SIZE);
        var sendTasks = tokenBatches.Select(async tokensBatch =>
        {
            var message = new FirebaseAdmin.Messaging.MulticastMessage()
            {
                Tokens = tokensBatch,
                Notification = new FirebaseAdmin.Messaging.Notification()
                {
                    Title = $"{ev.Author.Name} just posted \U0001F440"
                },
                Data = new Dictionary<string, string>
                {
                    { "author", authorJson },
                    { "post_id", ev.PostId.ToString() }
                }
            };

            // no matter what policy is set, we don't want to retry this handler
            // TODO: maybe some background batch retry, or split into batch handlers?
            try
            {
                // TODO: collect and batch delete stale tokens?
                return await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(message, cancellationToken);
            }
            catch
            {
                return null;
            }
        });

        await Task.WhenAll(sendTasks);
    }
}
