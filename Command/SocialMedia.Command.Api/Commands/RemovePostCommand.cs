using SocialMedia.Core.Commands;

namespace SocialMedia.Command.Api.Commands;

public class RemovePostCommand : BaseCommand
{
    public string UserName { get; set; }
}