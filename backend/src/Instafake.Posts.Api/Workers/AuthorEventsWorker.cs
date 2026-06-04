using Instafake.Posts.Application.Events;

namespace Instafake.Posts.Api.Workers;

public class AuthorEventsWorker([FromKeyedServices("authors")] IEventsConsumer eventsConsumer) : BackgroundService
{
    private readonly IEventsConsumer _eventsConsumer = eventsConsumer;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _eventsConsumer.ExecuteAsync(stoppingToken);
    }
}
