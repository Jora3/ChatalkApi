using System.ComponentModel.DataAnnotations;

namespace ChatalkApi.Dtos.Users;

public class SignupDto
{
    [Required]
    public string Pseudo { get; set; }

    [Required]
    [MinLength(8)]
    public string Password { get; set; }

    [Required]
    [MinLength(8)]
    [Compare(nameof(Password))]
    public string PasswordConfirmation { get; set; }
}