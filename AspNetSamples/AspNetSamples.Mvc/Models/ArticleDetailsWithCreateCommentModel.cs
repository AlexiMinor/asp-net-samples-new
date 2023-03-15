namespace AspNetSamples.Mvc.Models;

public class ArticleDetailsWithCreateCommentModel 
{
    public ArticleDetailsModel ArticleDetails { get; set; }
    public List<CommentModel> Comments { get; set; }
    public CommentModel CreateComment { get; set; }

}