namespace WebApp.Entities.ViewModel;

public class UserAuthenticateViewModel
{
    public bool IsValid { get; set; } = true;
    public string Message { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}
