using AuthService.Services;

namespace AuthService.Interfaces
{
    public interface ITokenService
    {
        public string CreateToken(UserModel user);
    }
}
