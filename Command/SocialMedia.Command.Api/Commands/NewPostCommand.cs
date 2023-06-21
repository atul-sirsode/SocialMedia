using SocialMedia.Core.Commands;

namespace SocialMedia.Command.Api.Commands
{
    public class NewPostCommand : BaseCommand
    {
        public string Author { get; set; }
        public string Message { get; set; }

    }
}
