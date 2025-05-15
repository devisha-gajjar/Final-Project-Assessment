using WebApp.Entities.Model;
using WebApp.Entities.ViewModel;
using WebApp.Repositories.IRepositories;
using WebApp.Service.IService;

namespace WebApp.Service.Implementation;

public class LoginService : ILoginService
{
    private readonly IUserRepository _userRepo;
    private readonly ICustomService _customService;

    public LoginService(IUserRepository userRepo, ICustomService customService)
    {
        _userRepo = userRepo;
        _customService = customService;
    }

    #region AuthenticateUser
    public UserAuthenticateViewModel AuthenticateUser(LoginViewModel model)
    {
        User? user = _userRepo.GetAll().Where(u => !u.IsDeleted).FirstOrDefault(u => u.Email == model.Email);

        UserAuthenticateViewModel userAuthenticateViewModel = new();

        if (user == null)
        {
            userAuthenticateViewModel.IsValid = false;
            userAuthenticateViewModel.Message = "No User Found!!";
            return userAuthenticateViewModel;
        }

        bool passMatch = _customService.Verify(model.Password, user.Password);

        if (!passMatch)
        {
            userAuthenticateViewModel.IsValid = false;
            userAuthenticateViewModel.Message = "Invalid Username or Password!!";
            return userAuthenticateViewModel;
        }

        string token = _customService.GenerateJwtToken(user.UserName);

        if (!string.IsNullOrEmpty(token))
        {
            userAuthenticateViewModel.Token = token;
            return userAuthenticateViewModel;
        }
        userAuthenticateViewModel.IsValid = false;
        userAuthenticateViewModel.Message = "Error at Authenticating User!!";
        return userAuthenticateViewModel;
    }

    #endregion AuthenticateUser
}
