using SocialMedia.Query.Domain.Entities;

namespace SocialMedia.Query.Domain.Repositories
{
    public interface IPostRepository
    {
        Task CreatePostAsync(PostEntity post);
        Task UpdatePostAsync(PostEntity post);
        Task DeletePostAsync(Guid postId);
        Task<bool> PostExistsAsync(Guid postId);
        Task<PostEntity> GetPostByIdAsync(Guid postId);
        Task<List<PostEntity>> GetPostsAsync();
        Task<List<PostEntity>> GetPostsByAuthorAsync(string author);
        Task<List<PostEntity>> GetPostWithLikesAsync(int numberOfLikes);
        Task<List<PostEntity>> GetPostWithCommentsAsync();
    }
}
