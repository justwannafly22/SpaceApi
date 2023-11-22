using System.ComponentModel.DataAnnotations;

namespace IdentityApi.Boundary;

public class LoginRequestModel
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}
