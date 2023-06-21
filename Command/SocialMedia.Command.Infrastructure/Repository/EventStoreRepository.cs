using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SocialMedia.Command.Infrastructure.Config;
using SocialMedia.Core.Domain;
using SocialMedia.Core.Events;

namespace SocialMedia.Command.Infrastructure.Repository
{
    public class EventStoreRepository : IEventStoreRepository
    {
        private readonly IMongoCollection<EventModel> _mongoCollection;

        public EventStoreRepository(IOptionsMonitor<MongoDbConfig> config)
        {
            var mongoClient = new MongoClient(config.CurrentValue.ConnectionString);
            var database = mongoClient.GetDatabase(config.CurrentValue.Database);
            _mongoCollection = database.GetCollection<EventModel>(config.CurrentValue.Collection);
        }
        public async Task SaveAsync(EventModel @event)
        {
            await _mongoCollection.InsertOneAsync(@event).ConfigureAwait(false);
        }

        public async Task<List<EventModel>> FindByAggregateId(Guid aggregateId)
        {
            return await _mongoCollection.Find(x => x.AggregateIdentifier == aggregateId).ToListAsync().ConfigureAwait(false);
        }
    }
}
