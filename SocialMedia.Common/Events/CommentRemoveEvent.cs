using SocialMedia.Core.Events;

namespace SocialMedia.Common.Events;

public class CommentRemoveEvent : BaseEvent
{
    public CommentRemoveEvent() : base(nameof(CommentRemoveEvent)) { }
    public Guid CommentId { get; set; }
}