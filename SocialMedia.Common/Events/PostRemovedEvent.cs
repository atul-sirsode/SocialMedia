using SocialMedia.Core.Events;

namespace SocialMedia.Common.Events
{
    public class PostRemovedEvent : BaseEvent
    {
        public PostRemovedEvent() : base(nameof(PostRemovedEvent)) { }
    }
}
