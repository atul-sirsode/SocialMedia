using SocialMedia.Core.Infrastructure;
using SocialMedia.Core.Queries;
using SocialMedia.Query.Domain.Entities;

namespace SocialMedia.Query.Infrastructure.Dispatcher
{
    public class QueryDispatcher : IQueryDispatcher<PostEntity>
    {
        private readonly Dictionary<Type, Func<BaseQuery, Task<List<PostEntity>>>> _handlers = new();
        public void RegisterHandler<TQuery>(Func<TQuery, Task<List<PostEntity>>> handler) where TQuery : BaseQuery
        {
            if (_handlers.ContainsKey(typeof(TQuery)))
            {
                throw new IndexOutOfRangeException($"You cannot register same query handler twice!");
            }
            _handlers.Add(typeof(TQuery), query => handler((TQuery)query));
        }

        public async Task<List<PostEntity>> SendAsync(BaseQuery query)
        {
            if (_handlers.TryGetValue(query.GetType(), out var handler))
            {
                return await handler(query);
            }
            throw new IndexOutOfRangeException($"Query handler for {query.GetType()} not found!");
        }
    }
}
