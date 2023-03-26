using System.ComponentModel.DataAnnotations;

namespace ChatalkApi.Dtos.Users;

public class LoginDto
{
    [Required]
    public string Pseudo { get; set; }

    [Required]
    [MinLength(8)]
    public string Password { get; set; }
}