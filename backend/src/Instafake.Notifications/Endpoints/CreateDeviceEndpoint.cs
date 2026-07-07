using FirebaseAdmin.Messaging;
using Instafake.Notifications.DbContexts;
using Instafake.Notifications.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Security.Authentication;

namespace Instafake.Notifications.Endpoints;

public static class CreateDeviceEndpoint
{
    public static async Task<IResult> Create(string? deviceToken, DevicesDbContext dbContext, HttpContext context, CancellationToken cancellationToken)
    {
        var valid = await IsTokenValid(deviceToken);
        if (!valid)
        {
            return Results.BadRequest("Device token is invalid");
        }

        var atSub = context.User.Identity?.Name;
        if (string.IsNullOrEmpty(atSub))
            throw new AuthenticationException("User is not authenticated");

        var device = new Device { UserId = atSub, DeviceToken = deviceToken };

        try
        {
            dbContext.Devices.Add(device);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            // ignore duplicates
            return Results.Conflict();
        }

        return Results.Ok();
    }

    private static async Task<bool> IsTokenValid([NotNullWhen(true)] string? deviceToken)
    {
        if (string.IsNullOrEmpty(deviceToken))
            return false;

        var message = new Message()
        {
            Token = deviceToken,
            Notification = new Notification()
            {
                Title = "Validation"
            }
        };

        try
        {
            await FirebaseMessaging.DefaultInstance.SendAsync(message, dryRun: true);
            return true;
        }
        catch (FirebaseMessagingException ex)
        {
            if (ex.MessagingErrorCode == MessagingErrorCode.Unregistered)
                return false;

            throw;
        }
    }
}
