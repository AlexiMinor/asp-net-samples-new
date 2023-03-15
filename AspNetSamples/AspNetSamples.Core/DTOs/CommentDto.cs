namespace AspNetSamples.Core.DTOs
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string User { get; set; }
        public string CommentText{ get; set; }
        public int ArticleId { get; set; }
    }
}