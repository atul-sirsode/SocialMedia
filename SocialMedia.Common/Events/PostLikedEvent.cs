using SocialMedia.Core.Events;

namespace SocialMedia.Common.Events;

public class PostLikedEvent : BaseEvent
{
    public PostLikedEvent() : base(nameof(PostLikedEvent)) { }
}