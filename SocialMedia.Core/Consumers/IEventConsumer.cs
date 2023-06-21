namespace SocialMedia.Core.Consumers
{
    public interface IEventConsumer
    {
        void Consume(string topic);
    }
}
