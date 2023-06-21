using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SocialMedia.Query.Domain.Entities;

[Table("Comment")]
public class CommentEntity
{
    [Key]
    public Guid CommentId { get; set; }
    public string UserName { get; set; }
    public string Comment { get; set; }
    public DateTime DateCommented { get; set; }
    public bool Edited { get; set; }
    public Guid PostId { get; set; }

    [JsonIgnore]
    public virtual PostEntity Post { get; set; }
}