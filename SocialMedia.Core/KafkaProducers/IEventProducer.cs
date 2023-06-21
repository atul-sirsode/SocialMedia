using SocialMedia.Core.Events;

namespace SocialMedia.Core.KafkaProducers
{
    public interface IEventProducer
    {
        Task ProductAsync<T>(string topic, T @event) where T : BaseEvent;
    }
}
