using SocialMedia.Core.Commands;

namespace SocialMedia.Core.Infrastructure
{
    public interface ICommandDispatcher
    {
        void RegisterHandler<T>(Func<T, Task> handler) where T : BaseCommand;
        Task DispatchAsync(BaseCommand command);
    }
}
