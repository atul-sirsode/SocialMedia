using SocialMedia.Command.Domain.Aggregates;
using SocialMedia.Command.Infrastructure.Exceptions;
using SocialMedia.Core.Domain;
using SocialMedia.Core.Events;
using SocialMedia.Core.Infrastructure;
using SocialMedia.Core.KafkaProducers;

namespace SocialMedia.Command.Infrastructure.Stores
{
    public class EventStore : IEventStore
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IEventProducer _eventProducer;
        public EventStore(IEventStoreRepository eventStoreRepository, IEventProducer eventProducer)
        {
            _eventStoreRepository = eventStoreRepository;
            _eventProducer = eventProducer;
        }
        public async Task SaveEventsAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion)
        {
            var eventStream = await GetEvents(aggregateId);
            if (eventStream != null && eventStream.Any() &&
                eventStream.Last().Version != expectedVersion &&
                expectedVersion != -1)
                throw new ConcurrencyException();

            var version = expectedVersion;

            foreach (var @event in events)
            {
                version++;
                @event.Version = version;
                var eventType = @event.GetType().Name;
                var eventModel = new EventModel
                {
                    AggregateIdentifier = aggregateId,
                    EventData = @event,
                    Version = version,
                    EventType = eventType,
                    TimeStamp = DateTime.Now,
                    AggregateType = nameof(PostAggregate)
                };

                await _eventStoreRepository.SaveAsync(eventModel);

                var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC");
                await _eventProducer.ProductAsync(topic, @event);
            }
        }

        public async Task<List<BaseEvent>> GetEventsForAggregateAsync(Guid aggregateId)
        {
            var eventStream = await GetEvents(aggregateId);
            if (!(eventStream?.Any() ?? false))
                throw new AggregateNotFoundException($"Aggregate with id {aggregateId} not found");

            return eventStream.OrderBy(x => x.Version).Select(x => x.EventData).ToList();
        }

        private async Task<List<EventModel>> GetEvents(Guid aggregateId) => await _eventStoreRepository.FindByAggregateId(aggregateId);
    }
}
