namespace Instafake.Posts.Application.Events;

public interface IEventsConsumer
{
    Task ExecuteAsync(CancellationToken cancellationToken);
}
