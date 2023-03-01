using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspNetSamples.Mvc.Models;

public class CreateArticleModel
{
    [Required]
    public string Title { get; set; }
    [Required]
    [StringLength(240, MinimumLength = 3, ErrorMessage = "Short desc must be between 3 and 240 symbols")]
    public string ShortDescription { get; set; }
    [Required]
    public string FullText { get; set; }
    [Required]
    public int SourceId { get; set; }

    //[ValidateNever]
    public List<SelectListItem>? AvailableSources { get; set; }
}