using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebApp.Entities.Model;
using WebApp.Repositories.IRepositories;
using WebApp.Service.IService;

namespace WebApp.Service.Implementation;

public class CustomService : ICustomService
{
    private readonly IConfiguration _config;
    private readonly IUserRepository _userRepo;
    public CustomService(IConfiguration config, IUserRepository userRepo)
    {
        _config = config;
        _userRepo = userRepo;
    }

    #region PasswordHash
    //for db store purpose only
    public string Hash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool Verify(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
    #endregion PasswordHash

    #region Token
    public string GenerateJwtToken(string name)
    {
        User? user = _userRepo.GetAll().Include(u => u.Role).FirstOrDefault(u => u.UserName == name);

        JwtSecurityTokenHandler tokenHandler = new();
        byte[] key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]!);

        Claim[]? authClaims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.RoleName),
                new Claim(ClaimTypes.Name,user.UserId.ToString())
            };

        var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

        JwtSecurityToken token = new(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Issuer"],
            claims: authClaims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials
        );

        return tokenHandler.WriteToken(token);
    }
    #endregion Token

}



