using SocialMedia.Core.Events;

namespace SocialMedia.Common.Events;

public class MessageUpdatedEvent : BaseEvent
{
    public MessageUpdatedEvent() : base(nameof(MessageUpdatedEvent)) { }
    public string Message { get; set; }
}