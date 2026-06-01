using Instafake.IdentityEventsIngress.Events;
using System.Threading.Channels;

namespace Instafake.IdentityEventsIngress.Retry;

public class RetryQueue<TEvent>
    where TEvent : EventBase, new()
{
    private readonly Channel<TEvent> _channel = Channel.CreateUnbounded<TEvent>();

    public void Enqueue(TEvent ev) => _channel.Writer.TryWrite(ev);
    public IAsyncEnumerable<TEvent> ReadAll() => _channel.Reader.ReadAllAsync();
}
