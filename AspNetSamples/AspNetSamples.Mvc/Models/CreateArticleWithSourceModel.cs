using System.ComponentModel.DataAnnotations;

namespace AspNetSamples.Mvc.Models;

public class CreateArticleWithSourceModel
{
    [Required]
    public string Title { get; set; }
    [Required]
    [StringLength(240, MinimumLength = 3, ErrorMessage = "Short desc must be between 3 and 240 symbols")]
    public string ShortDescription { get; set; }
    [Required]
    public string FullText { get; set; }
    
    [Required(ErrorMessage = "You should enter the name")]
    public string SourceName { get; set; }
}