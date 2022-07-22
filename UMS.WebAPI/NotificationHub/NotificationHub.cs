using Microsoft.AspNetCore.SignalR;
using SignalRSwaggerGen.Attributes;

namespace UMS.WebAPI.NotificationHub;

public class NotificationHub : Hub
{
    public async Task SomeMethod(int arg1, object arg2, [SignalRHidden] CancellationToken cancellationToken = default)
    {
        await Clients.All.SendAsync(nameof(SomeMethod), arg1, arg2, cancellationToken);
        
    }

    public async Task SomeOtherMethod(int arg1, object arg2, [SignalRHidden] CancellationToken cancellationToken = default)
    {
        await Clients.All.SendAsync(nameof(SomeOtherMethod), arg1, arg2, cancellationToken);
    }
}