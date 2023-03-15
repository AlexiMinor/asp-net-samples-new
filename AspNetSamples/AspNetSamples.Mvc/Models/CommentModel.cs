namespace AspNetSamples.Mvc.Models;

public class CommentModel
{ 
    public string User { get; set; }
    public string CommentText { get; set; }

    public int ArticleId { get; set; }
}