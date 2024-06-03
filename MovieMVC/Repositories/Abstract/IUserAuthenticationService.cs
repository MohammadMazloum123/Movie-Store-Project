using MovieMVC.Models.DTO;

namespace MovieMVC.Repositories.Abstract
{
    public interface IUserAuthenticationService
    {
        Task<Status> LoginAsync(LoginModel model);
        Task LogoutAsync();
        Task<Status> RegisterAsync(RegisterationModel model);
    }
}
