using Microsoft.EntityFrameworkCore;
using SocialMedia.Query.Domain.Entities;
using SocialMedia.Query.Domain.Repositories;
using SocialMedia.Query.Infrastructure.DataContext;

namespace SocialMedia.Query.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly DatabaseContextFactory _contextFactory;

        public PostRepository(DatabaseContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task CreatePostAsync(PostEntity post)
        {
            await using var context = _contextFactory.CreateDbContext();
            await context.Posts.AddAsync(post);
            await context.SaveChangesAsync();
        }

        public async Task UpdatePostAsync(PostEntity postId)
        {
            await using var context = _contextFactory.CreateDbContext();
            context.Posts.Update(postId);
            await context.SaveChangesAsync();
        }

        public async Task DeletePostAsync(Guid postId)
        {
            await using var context = _contextFactory.CreateDbContext();
            var post = await GetPostByIdAsync(postId);
            if (post == null) return;
            context.Posts.Remove(post);
            await context.SaveChangesAsync();
        }

        public Task<bool> PostExistsAsync(Guid postId)
        {
            throw new NotImplementedException();
        }

        public async Task<PostEntity> GetPostByIdAsync(Guid postId)
        {
            await using var context = _contextFactory.CreateDbContext();
            return await context.Posts
                .Include(_ => _.Comments)
                .FirstOrDefaultAsync(x => x.Id == postId);
        }

        public async Task<List<PostEntity>> GetPostsAsync()
        {
            await using var context = _contextFactory.CreateDbContext();
            return await context.Posts
                .AsNoTracking()
                .Include(_ => _.Comments)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<PostEntity>> GetPostsByAuthorAsync(string author)
        {
            await using var context = _contextFactory.CreateDbContext();
            return await context.Posts
                .AsNoTracking()
                .Include(_ => _.Comments)
                .AsNoTracking()
                .Where(x => x.Author == author)
                .ToListAsync();
        }

        public async Task<List<PostEntity>> GetPostWithLikesAsync(int numberOfLikes)
        {
            await using var context = _contextFactory.CreateDbContext();
            return await context.Posts
                .AsNoTracking()
                .Include(_ => _.Comments)
                .AsNoTracking()
                .Where(x => x.Likes == numberOfLikes)
                .ToListAsync();
        }

        public async Task<List<PostEntity>> GetPostWithCommentsAsync()
        {
            await using var context = _contextFactory.CreateDbContext();
            return await context.Posts
                .AsNoTracking()
                .Include(_ => _.Comments)
                .AsNoTracking()
                .Where(_ => _.Comments != null && _.Comments.Any())
                .ToListAsync();
        }
    }
}
