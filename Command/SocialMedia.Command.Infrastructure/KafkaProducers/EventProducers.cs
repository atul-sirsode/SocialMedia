using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using SocialMedia.Core.Events;
using SocialMedia.Core.KafkaProducers;

namespace SocialMedia.Command.Infrastructure.KafkaProducers
{
    public class EventProducers : IEventProducer
    {
        private readonly ProducerConfig _producerConfig;

        public EventProducers(IOptionsMonitor<ProducerConfig> producerConfig)
        {
            _producerConfig = producerConfig.CurrentValue;
        }
        public async Task ProductAsync<T>(string topic, T @event) where T : BaseEvent
        {
            using var producer = new ProducerBuilder<string, string>(_producerConfig)
                .SetKeySerializer(Serializers.Utf8)
                .SetValueSerializer(Serializers.Utf8)
                .Build();


            var message = new Message<string, string>
            {
                Key = Guid.NewGuid().ToString(),
                Value = JsonSerializer.Serialize(@event, @event.GetType())
            };

            var deliveryResult = await producer.ProduceAsync(topic, message);

            if (deliveryResult is { Status: PersistenceStatus.NotPersisted })
            {
                throw new Exception($"could not produce {@event.GetType().Name} message to topic - {topic} due to following reason : {deliveryResult.Message}!");
            }
        }
    }
}
