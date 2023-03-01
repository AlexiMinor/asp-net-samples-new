using System.ComponentModel.DataAnnotations;

namespace AspNetSamples.Mvc.Models;

public class CreateSourceModel
{
    [Required(ErrorMessage = "You should enter the name")]
    public string Name { get; set; }

    public string? SourceUrl { get; set; }

}