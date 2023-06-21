using Microsoft.EntityFrameworkCore;
using SocialMedia.Query.Domain.Entities;
using SocialMedia.Query.Domain.Repositories;
using SocialMedia.Query.Infrastructure.DataContext;

namespace SocialMedia.Query.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DatabaseContextFactory _contextFactory;

        public CommentRepository(DatabaseContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task CreateCommentAsync(CommentEntity comment)
        {
            await using var context = _contextFactory.CreateDbContext();
            await context.Comments.AddAsync(comment);
            await context.SaveChangesAsync();
        }

        public async Task UpdateCommentAsync(CommentEntity comment)
        {
            await using var context = _contextFactory.CreateDbContext();
            context.Comments.Update(comment);
            await context.SaveChangesAsync();
        }

        public async Task DeleteCommentAsync(Guid commentId)
        {
            await using var context = _contextFactory.CreateDbContext();
            var comment = await GetCommentByIdAsync(commentId);
            if (comment == null) return;
            context.Comments.Remove(comment);
            await context.SaveChangesAsync();
        }

       
        public async Task<CommentEntity> GetCommentByIdAsync(Guid commentId)
        {
            await using var context = _contextFactory.CreateDbContext();
            return await context.Comments
                .FirstOrDefaultAsync(x => x.CommentId == commentId);
        }
        public Task<bool> CommentExistsAsync(Guid commentId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CommentEntity>> GetCommentsAsync()
        {
            await using var context = _contextFactory.CreateDbContext();
            return await context.Comments
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<CommentEntity>> GetCommentsByAuthorAsync(string author)
        {
            await using var context = _contextFactory.CreateDbContext();
            return await context.Comments
                .AsNoTracking()
                .Where(x => x.UserName == author)
                .ToListAsync();
        }

        public async Task<IEnumerable<CommentEntity>> GetCommentsByPostIdAsync(Guid postId)
        {
            await using var context = _contextFactory.CreateDbContext();
            return await context.Comments
                .AsNoTracking()
                .Where(x => x.PostId == postId)
                .ToListAsync();
        }
    }
}
