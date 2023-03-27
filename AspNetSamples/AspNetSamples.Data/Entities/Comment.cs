using AspNetSamples.Core;

namespace AspNetSamples.Data.Entities;

public class Comment : IBaseEntity
{
    public int Id { get; set; }
    public string CommentText { get; set; }

    public int ArticleId { get; set; }
    public Article Article{ get; set; }

    public int UserId { get; set; }
    public User User { get; set; }
}