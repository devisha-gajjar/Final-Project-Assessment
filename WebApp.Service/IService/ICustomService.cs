namespace WebApp.Service.IService;

public interface ICustomService
{
    public string Hash(string password);
    public bool Verify(string password, string hashedPassword);
    public string GenerateJwtToken(string name);
    // public string ValidateToken(string token);
}
