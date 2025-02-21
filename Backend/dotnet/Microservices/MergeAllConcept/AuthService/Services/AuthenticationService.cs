using AuthService.Dtos;
using AuthService.Interfaces;

namespace AuthService.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        ITokenService tokenService;
        public AuthenticationService(ITokenService tokenService)
        {
            this.tokenService = tokenService;
        }

        public AuthenticatedUser? Login(UserCredientialsDto userCredientials)
        {
            User? user = GetUser(userCredientials);

            if (user == null) {
                return null;
            }
            UserModel userModel = new UserModel()
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role
            };
            string token = tokenService.CreateToken(userModel);
            AuthenticatedUser authenticatedUser = new AuthenticatedUser()
            {
                
                UserName = userModel.Username,
                Token = token,
                
            };
            return authenticatedUser;

        }

        private User? GetUser(UserCredientialsDto userCredientialsDto)
        {
            List<User> users = new List<User>()
            {
                new User() {
                    Username = "dkeshri",
                    Id = 1,
                    Role = "Admin",
                    Password = "123456"
                },
                new User() {
                    Username = "pk",
                    Id = 1,
                    Role = "Admin",
                    Password = "123456"
                }
            };

            User? user = users.FirstOrDefault(user => 
            user.Username.Equals(userCredientialsDto.UserName) 
            && user.Password.Equals(userCredientialsDto.Password));
            return user;
        }
    }
}
