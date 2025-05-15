using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using WebApp.Entities.Model;
using WebApp.Entities.ViewModel;
using WebApp.Repositories.IRepositories;
using WebApp.Service.IService;

namespace WebApp.Service.Implementation;

public class LoginService : ILoginService
{
    private readonly IUserRepository _userRepo;
    private readonly ICustomService _customService;
    private readonly IHttpContextAccessor _httpContext;

    public LoginService(IUserRepository userRepo, ICustomService customService, IHttpContextAccessor httpContext)
    {
        _userRepo = userRepo;
        _customService = customService;
        _httpContext = httpContext;
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

    #region RegisterUser
    public (bool isRegister, string message) RegisterUser(RegisterViewModel registerViewModel)
    {
        bool userExisting = _userRepo.GetAll().Any(u => u.Email == registerViewModel.Email);

        if (userExisting)
        {
            return (false, "You already have an Account!!");
        }
        else
        {
            User addUser = new()
            {
                Name = registerViewModel.Name,
                UserName = registerViewModel.UserName,
                Email = registerViewModel.Email,
                PhoneNumber = registerViewModel.Phone,
                Password = _customService.Hash(registerViewModel.Password),
                RoleId = 2,
                IsDeleted = false,
            };

            _userRepo.Add(addUser);

            return (true, "Register Successfully!!");
        }
    }
    #endregion
}
