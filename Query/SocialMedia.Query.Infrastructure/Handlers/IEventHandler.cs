using SocialMedia.Common.Events;

namespace SocialMedia.Query.Infrastructure.Handlers
{
    public interface IEventHandler
    {
        Task OnHandleAsync(PostCreatedEvent @event);
        Task OnHandleAsync(MessageUpdatedEvent @event);
        Task OnHandleAsync(PostLikedEvent @event);
        Task OnHandleAsync(PostRemovedEvent @event);
        Task OnHandleAsync(CommendAddedEvent @event);
        Task OnHandleAsync(CommentRemoveEvent @event);
        Task OnHandleAsync(CommentUpdatedEvent @event);
        
    }
}
