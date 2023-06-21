using SocialMedia.Core.Commands;

namespace SocialMedia.Command.Api.Commands
{
    public class EditPostCommand : BaseCommand
    {
        public string Message { get; set; }
    }
}
