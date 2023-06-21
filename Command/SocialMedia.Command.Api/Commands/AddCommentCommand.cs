using SocialMedia.Core.Commands;

namespace SocialMedia.Command.Api.Commands
{
    public class AddCommentCommand : BaseCommand
    {
        public string Comment { get; set; }
        public string UserName { get; set; }
    }
}
