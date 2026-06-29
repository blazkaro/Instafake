namespace Instafake.BuildingBlocks.Domain;

public abstract class DomainEntity
{
    private readonly List<IDomainEvent> _events = [];
    public IReadOnlyCollection<IDomainEvent> Events => _events.AsReadOnly();

    public void AddEvent<TEvent>(TEvent ev)
        where TEvent : IDomainEvent
    {
        _events.Add(ev);
    }
}
