using SocialMedia.Core.Events;

namespace SocialMedia.Common.Events;

public class CommendAddedEvent : BaseEvent
{
    public CommendAddedEvent() : base(nameof(CommendAddedEvent)) { }
    public Guid CommentId { get; set; }
    public string Comment { get; set; }
    public string UserName { get; set; }
    public DateTime CommentDate { get; set; }
}