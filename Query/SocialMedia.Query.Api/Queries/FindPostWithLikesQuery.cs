using SocialMedia.Core.Queries;

namespace SocialMedia.Query.Api.Queries;

public class FindPostWithLikesQuery : BaseQuery
{
    public int NumberOfLikes { get; set; }
}