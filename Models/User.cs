using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatalkApi.Models;

public class User
{
    public int Id { get; set; }

    [Column(TypeName = "varchar(16)")]
    [Required]
    public string Pseudo { get; set; }

    [Column(TypeName = "varchar(90)")]
    [Required]
    public string Password { get; set; }
}