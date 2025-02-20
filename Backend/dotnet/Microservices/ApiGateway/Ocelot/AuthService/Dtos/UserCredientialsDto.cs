using System.ComponentModel.DataAnnotations;

namespace AuthService.Dtos
{
    public class UserCredientialsDto
    {
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}
