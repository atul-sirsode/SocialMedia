using SocialMedia.Command.Domain.Aggregates;
using SocialMedia.Core.Handlers;

namespace SocialMedia.Command.Api.Commands.Handlers
{
    public class CommandHandler : ICommandHandler
    {
        private readonly IEventSourceHandler<PostAggregate> _eventSourceHandler;

        public CommandHandler(IEventSourceHandler<PostAggregate> eventSourceHandler)
        {
            _eventSourceHandler = eventSourceHandler;
        }
        public async Task HandleAsync(NewPostCommand command)
        {
            var aggregate = new PostAggregate(command.Id, command.Author, command.Message);
            await _eventSourceHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(EditPostCommand command)
        {
            var aggregate = await _eventSourceHandler.GetByIdAsync(command.Id);
            aggregate.EditMessage(command.Message);
            await _eventSourceHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(LikePostCommand command)
        {
            var aggregate = await _eventSourceHandler.GetByIdAsync(command.Id);
            aggregate.LikePost();
            await _eventSourceHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(AddCommentCommand command)
        {
            var aggregate = await _eventSourceHandler.GetByIdAsync(command.Id);
            aggregate.AddComment(command.Comment, command.UserName);
            await _eventSourceHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(EditCommentCommand command)
        {
            var aggregate = await _eventSourceHandler.GetByIdAsync(command.Id);
            aggregate.EditComment(command.CommentId, command.Comment, command.UserName);
            await _eventSourceHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(RemoveCommentCommand command)
        {
            var aggregate = await _eventSourceHandler.GetByIdAsync(command.Id);
            aggregate.RemoveComment(command.CommentId, command.UserName);
            await _eventSourceHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(RemovePostCommand command)
        {
            var aggregate = await _eventSourceHandler.GetByIdAsync(command.Id);
            aggregate.DeletePost(command.UserName);
            await _eventSourceHandler.SaveAsync(aggregate);
        }
    }
}
