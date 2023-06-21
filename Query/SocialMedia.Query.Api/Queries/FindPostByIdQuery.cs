using SocialMedia.Core.Queries;

namespace SocialMedia.Query.Api.Queries;

public class FindPostByIdQuery : BaseQuery
{
    public Guid Id { get; set; }
}