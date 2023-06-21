using SocialMedia.Query.Domain.Entities;
using SocialMedia.Query.Domain.Repositories;

namespace SocialMedia.Query.Api.Queries.Handlers
{
    public class QueryHandler : IQueryHandler
    {
        private readonly IPostRepository _postRepository;

        public QueryHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        public async Task<List<PostEntity>> HandleAsync(FindAllPostQuery query)
        {
            return await _postRepository.GetPostsAsync();
        }

        public async Task<List<PostEntity>> HandleAsync(FindPostByAuthorQuery query)
        {
            return await _postRepository.GetPostsByAuthorAsync(query.Author);
        }

        public async Task<List<PostEntity>> HandleAsync(FindPostByIdQuery query)
        {
            var post = await _postRepository.GetPostByIdAsync(query.Id);
            return new List<PostEntity> { post };
        }

        public async Task<List<PostEntity>> HandleAsync(FindPostWithCommentsQuery query)
        {
            return await _postRepository.GetPostWithCommentsAsync();
        }

        public async Task<List<PostEntity>> HandleAsync(FindPostWithLikesQuery query)
        {
            return await _postRepository.GetPostWithLikesAsync(query.NumberOfLikes);
        }
    }
}
