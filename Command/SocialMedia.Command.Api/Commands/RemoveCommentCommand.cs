using SocialMedia.Core.Commands;

namespace SocialMedia.Command.Api.Commands;

public class RemoveCommentCommand : BaseCommand
{
    public Guid CommentId { get; set; }
    public string UserName { get; set; }
}