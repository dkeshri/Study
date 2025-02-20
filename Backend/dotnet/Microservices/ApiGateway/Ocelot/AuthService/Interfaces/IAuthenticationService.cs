using AuthService.Dtos;
using AuthService.Services;

namespace AuthService.Interfaces
{
    public interface IAuthenticationService
    {
        public AuthenticatedUser? Login(UserCredientialsDto userCredientials);
    }
}
