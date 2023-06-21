using SocialMedia.Command.Domain.Aggregates;
using SocialMedia.Core.Domain;
using SocialMedia.Core.Handlers;
using SocialMedia.Core.Infrastructure;

namespace SocialMedia.Command.Infrastructure.Handlers
{
    public class EventSourceHandler : IEventSourceHandler<PostAggregate>
    {
        private readonly IEventStore _eventStore;

        public EventSourceHandler(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }
        public async Task SaveAsync(AggregateRoot aggregate)
        {
            await _eventStore.SaveEventsAsync(aggregate.Id, aggregate.GetUnCommittedChanges(), aggregate.Version);
        }

        public async Task<PostAggregate> GetByIdAsync(Guid aggregateId)
        {
            var aggregate = new PostAggregate();
            var events = await _eventStore.GetEventsForAggregateAsync(aggregateId);
            if (events != null && !events.Any())
                return aggregate;

            aggregate.ReplayEvents(events);

            aggregate.Version = events!.Select(x => x.Version).Max();
            return aggregate;
        }
    }
}
