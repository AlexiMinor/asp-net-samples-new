using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace AspNetSamples.Mvc.Models;

public class RegisterModel
{
    [Required]
    [EmailAddress]
    [Remote("CheckIsUserEmailIsValidAndNotExists", 
        "Account", 
        ErrorMessage = "This email already used")]
    public string Email { get; set; }

    [Required]
    //[RegularExpression()]
    public string Password { get; set; }

    [Compare(nameof(Password))]
    public string PasswordConfirmation { get; set; }
}