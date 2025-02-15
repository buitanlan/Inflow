namespace Inflow.Shared.Abstractions.Kernel.Types;

public class AggregateRoot<T>
{
    public T Id { get; protected set; }
    public int Version { get; protected set; } = 1;
    public IEnumerable<IDomainEvent> Events => _events;

    private readonly List<IDomainEvent> _events = [];
    private bool _versionIncremented;

    protected void AddEvent(IDomainEvent @event)
    {
        if (_events.Count == 0 && !_versionIncremented)
        {
            Version++;
            _versionIncremented = true;
        }

        _events.Add(@event);
    }

    public void ClearEvents() => _events.Clear();

    protected void IncrementVersion()
    {
        if (_versionIncremented)
        {
            return;
        }

        Version++;
        _versionIncremented = true;
    }
}

public abstract class AggregateRoot : AggregateRoot<AggregateId>;
