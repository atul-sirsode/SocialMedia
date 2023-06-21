using SocialMedia.Core.Events;

namespace SocialMedia.Core.Infrastructure
{
    public interface IEventStore
    {
        Task SaveEventsAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion);
        Task<List<BaseEvent>> GetEventsForAggregateAsync(Guid aggregateId);
    }
}
