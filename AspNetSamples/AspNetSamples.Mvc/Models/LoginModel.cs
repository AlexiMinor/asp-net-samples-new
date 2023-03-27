using System.ComponentModel.DataAnnotations;

namespace AspNetSamples.Mvc.Models;

public class LoginModel
{
    [Required]
    [EmailAddress]
   public string Email { get; set; }

    [Required]
    //[RegularExpression()]
    public string Password { get; set; }

    public string? ReturnUrl { get; set; }
}