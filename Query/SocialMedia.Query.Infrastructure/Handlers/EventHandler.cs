using SocialMedia.Common.Events;
using SocialMedia.Query.Domain.Entities;
using SocialMedia.Query.Domain.Repositories;

namespace SocialMedia.Query.Infrastructure.Handlers
{
    public class EventHandler : IEventHandler
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        public EventHandler(IPostRepository postRepository, ICommentRepository commentRepository)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
        }
        public async Task OnHandleAsync(PostCreatedEvent @event)
        {
            var post = new PostEntity
            {
                Id = @event.Id,
                Author = @event.Author,
                DatePosted = @event.DatePosted,
                Message = @event.Message
            };
            await _postRepository.CreatePostAsync(post);
        }

        public async Task OnHandleAsync(CommendAddedEvent @event)
        {
            var comment = new CommentEntity
            {
                PostId = @event.Id,
                CommentId = @event.CommentId,
                DateCommented = @event.CommentDate,
                UserName = @event.UserName,
                Edited = false

            };
            await _commentRepository.CreateCommentAsync(comment);
        }

        public async Task OnHandleAsync(PostLikedEvent @event)
        {
            var post = await _postRepository.GetPostByIdAsync(@event.Id);
            if (post == null) return;
            post.Likes += 1;
            await _postRepository.UpdatePostAsync(post);
        }

        public async Task OnHandleAsync(CommentUpdatedEvent @event)
        {
            var comment = await _commentRepository.GetCommentByIdAsync(@event.CommentId);
            if (comment == null) return;
            comment.Comment = @event.Comment;
            comment.Edited = true;
            comment.DateCommented = @event.EditDate;
            await _commentRepository.UpdateCommentAsync(comment);
        }
        public async Task OnHandleAsync(MessageUpdatedEvent @event)
        {
            var post = await _postRepository.GetPostByIdAsync(@event.Id);
            if (post == null) return;
            post.Message = @event.Message;
            await _postRepository.UpdatePostAsync(post);
        }
        public async Task OnHandleAsync(CommentRemoveEvent @event)
        {
            await _commentRepository.DeleteCommentAsync(@event.CommentId);
        }
        public async Task OnHandleAsync(PostRemovedEvent @event)
        {
            await _postRepository.DeletePostAsync(@event.Id);
        }

    }
}
