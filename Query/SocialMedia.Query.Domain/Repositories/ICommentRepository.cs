using SocialMedia.Query.Domain.Entities;

namespace SocialMedia.Query.Domain.Repositories
{
    public interface ICommentRepository
    {
        Task CreateCommentAsync(CommentEntity comment);
        Task UpdateCommentAsync(CommentEntity comment);
        Task DeleteCommentAsync(Guid commentId);
        Task<bool> CommentExistsAsync(Guid commentId);
        Task<CommentEntity> GetCommentByIdAsync(Guid commentId);
        Task<IEnumerable<CommentEntity>> GetCommentsAsync();
        Task<IEnumerable<CommentEntity>> GetCommentsByAuthorAsync(string author);
        Task<IEnumerable<CommentEntity>> GetCommentsByPostIdAsync(Guid postId);
    }
}
