using SocialMedia.Common.Events;
using SocialMedia.Core.Domain;

namespace SocialMedia.Command.Domain.Aggregates
{
    public class PostAggregate : AggregateRoot
    {
        private bool _active;
        private string _author;
        private readonly Dictionary<Guid, Tuple<string, string>> _comments = new();

        public bool Active
        {
            get => _active; set => _active = value;
        }
        public PostAggregate() { }

        public PostAggregate(Guid id, string author, string messages)
        {
            RaiseEvent(new PostCreatedEvent
            {
                Id = id,
                Author = author,
                Message = messages,
                DatePosted = DateTime.Now
            });
        }

        public void Apply(PostCreatedEvent @event)
        {
            _Id = @event.Id;
            _author = @event.Author;
            _active = true;
        }

        public void EditMessage(string message)
        {
            if (!_active)
            {
                throw new InvalidOperationException("cannot edit an inactive post");
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException($"value of {nameof(message)} cannot be null or empty. Please provide valid {nameof(message)}");
            }
            RaiseEvent(new MessageUpdatedEvent
            {
                Id = _Id,
                Message = message
            });
        }

        public void Apply(MessageUpdatedEvent @event)
        {
            _Id = @event.Id;
        }

        public void LikePost()
        {
            if (!_active)
            {
                throw new InvalidOperationException("you cannot like inactive post");
            }
            RaiseEvent(new PostLikedEvent
            {
                Id = _Id
            });
        }

        public void Apply(PostLikedEvent @event)
        {
            _Id = @event.Id;
        }

        public void AddComment(string comment, string userName)
        {
            if (!_active)
            {
                throw new InvalidOperationException("you cannot comment on inactive post");
            }

            if (string.IsNullOrWhiteSpace(comment))
            {
                throw new ArgumentException($"value of {nameof(comment)} cannot be null or empty. Please provide valid {nameof(comment)}");
            }
            RaiseEvent(new CommendAddedEvent
            {
                Id = _Id,
                Comment = comment,
                UserName = userName,
                CommentId = Guid.NewGuid(),
                CommentDate = DateTime.UtcNow
            });

        }

        public void Apply(CommendAddedEvent @event)
        {
            _Id = @event.Id;
            _comments.Add(@event.CommentId, new Tuple<string, string>(@event.Comment, @event.UserName));
        }

        public void EditComment(Guid commentId, string comment, string userName)
        {
            if (!_active)
            {
                throw new InvalidOperationException("you cannot edit comment on inactive post");
            }

            if (!_comments[commentId].Item2.Equals(userName, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException(
                    "You are not allowed too edit a comment that was made by another user!");
            }
            RaiseEvent(new CommentUpdatedEvent
            {
                Id = _Id,
                CommentId = commentId,
                Comment = comment,
                UserName = userName,
                EditDate = DateTime.UtcNow
            });
        }

        public void Apply(CommentUpdatedEvent @event)
        {
            _Id = @event.Id;
            _comments[@event.CommentId] = new Tuple<string, string>(@event.Comment, @event.UserName);
        }

        public void RemoveComment(Guid commentId, string userName)
        {
            if (!_active)
            {
                throw new InvalidOperationException("you cannot remove comment on inactive post");
            }

            if (!_comments[commentId].Item2.Equals(userName, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException(
                                       "You are not allowed too remove a comment that was made by another user!");
            }
            RaiseEvent(new CommentRemoveEvent
            {
                Id = _Id,
                CommentId = commentId,
            });
        }

        public void Apply(CommentRemoveEvent @event)
        {
            _Id = @event.Id;
            _comments.Remove(@event.CommentId);
        }

        public void DeletePost(string userName)
        {
            if (!_active)
            {
                throw new InvalidOperationException("you cannot delete an inactive post");
            }

            if (!_author.Equals(userName, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException(
                                       "You are not allowed too delete a post that was made by another user!");
            }
            RaiseEvent(new PostRemovedEvent
            {
                Id = _Id
            });

        }

        public void Apply(PostRemovedEvent @event)
        {
            _Id = @event.Id;
            _active = false;
        }
    }
}
