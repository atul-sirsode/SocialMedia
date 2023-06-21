using SocialMedia.Core.Events;

namespace SocialMedia.Core.Domain
{
    public abstract class AggregateRoot
    {
        protected Guid _Id;
        public Guid Id => _Id;
        private readonly List<BaseEvent> _changes = new();
        public int Version { get; set; } = -1;

        public IEnumerable<BaseEvent> GetUnCommittedChanges() => _changes.AsEnumerable();
        public void MarkChangesAsCommitted() => _changes.Clear();

        public void ApplyChanges(BaseEvent @event, bool isNew)
        {
            var method = this.GetType().GetMethod("Apply", new[] { @event.GetType() });
            if (method is null) throw new ArgumentException($"Apply method not found in Aggregate root for event {@event.GetType()}");

            method.Invoke(this, new object?[] { @event });

            if (isNew) { _changes.Add(@event); }
        }

        protected void RaiseEvent(BaseEvent @event) => ApplyChanges(@event, true);

        public void ReplayEvents(IEnumerable<BaseEvent> events)
        {
            foreach (var @event in events)
            {
                ApplyChanges(@event, false); 
                Version++;
            }

        }
    }
}
