using WebApp.Entities.ViewModel;

namespace WebApp.Service.IService;

public interface ILoginService
{
    public UserAuthenticateViewModel AuthenticateUser(LoginViewModel model);
    public (bool isRegister, string message) RegisterUser(RegisterViewModel registerViewModel);
}

