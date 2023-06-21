using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SocialMedia.Core.Consumers;
using SocialMedia.Core.Events;
using SocialMedia.Query.Infrastructure.Converters;
using SocialMedia.Query.Infrastructure.Handlers;

namespace SocialMedia.Query.Infrastructure.Consumers
{
    public class EventConsumer : IEventConsumer
    {
        private readonly ConsumerConfig _consumerConfig;
        private readonly IEventHandler _eventHandler;
        private readonly ILogger<EventConsumer> _logger;
        public EventConsumer(IOptionsMonitor<ConsumerConfig> consumerConfig, IEventHandler eventHandler, ILogger<EventConsumer> logger)
        {
            _eventHandler = eventHandler;
            _logger = logger;
            _consumerConfig = consumerConfig.CurrentValue;
        }
        public void Consume(string topic)
        {
            using var consumer = new ConsumerBuilder<string, string>(_consumerConfig)
                .SetKeyDeserializer(Deserializers.Utf8)
                .SetValueDeserializer(Deserializers.Utf8)
                .Build();

            consumer.Subscribe(topic);
            while (true)
            {
                var consumerResult = consumer.Consume();
                if (consumerResult?.Message == null) continue;

                var options = new JsonSerializerOptions
                {
                    Converters = { new EventJsonConverter() }
                };
                var @event = JsonSerializer.Deserialize<BaseEvent>(consumerResult.Message.Value, options);
                var handlerMethod = typeof(IEventHandler).GetMethod("OnHandleAsync", new[] { @event.GetType() });
                if (handlerMethod == null) throw new ArgumentNullException(nameof(handlerMethod), "could not find event handler method!");
                _logger.LogInformation("Handler {handler} consume message {message}", @event.GetType(), consumerResult.Message.Value);
                handlerMethod.Invoke(_eventHandler, new object[] { @event });
                consumer.Commit(consumerResult);
            }
        }
    }
}
