using FirebaseAdmin.Messaging;
using Instafake.Notifications.DbContexts;
using Instafake.Notifications.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Security.Authentication;
using System.Security.Claims;

namespace Instafake.Notifications.Endpoints;

public class CreateDeviceRequest
{
    public string? DeviceToken { get; set; }
}

public static class CreateDeviceEndpoint
{
    public static async Task<IResult> Create(CreateDeviceRequest? req, DevicesDbContext dbContext, HttpContext context, CancellationToken cancellationToken)
    {
        var valid = await IsTokenValid(req?.DeviceToken);
        if (!valid)
        {
            return Results.BadRequest("Device token is invalid");
        }

        var atSub = context.User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(atSub))
            throw new AuthenticationException("User is not authenticated");

        var device = new Device { UserId = atSub, DeviceToken = req.DeviceToken };

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
        catch (FirebaseMessagingException)
        {
            return false;
        }
    }
}
