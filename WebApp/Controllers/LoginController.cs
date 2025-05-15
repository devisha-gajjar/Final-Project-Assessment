using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApp.Entities.Model;
using WebApp.Service.IService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebApp.Entities.ViewModel;

namespace WebApp.Controllers;

public class LoginController : Controller
{
    private readonly ILoginService _loginService;

    public LoginController(ILoginService loginService)
    {
        _loginService = loginService;
    }

    public IActionResult Index()
    {
        return View();
    }

    #region Login
    [HttpPost]
    public IActionResult Index(LoginViewModel loginViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(loginViewModel);
        }

        UserAuthenticateViewModel userAuthenticateViewModel = _loginService.AuthenticateUser(loginViewModel);

        if (!userAuthenticateViewModel.IsValid)
        {
            TempData["error"] = userAuthenticateViewModel.Message;
            return View(loginViewModel);
        }
        else
        {
            CookieOptions options = new()
            {
                Domain = "localhost",
                Path = "/",
                Secure = false,
                Expires = DateTime.Now.AddHours(2),
                HttpOnly = true,
                IsEssential = true
            };
            Response.Cookies.Append("Token", userAuthenticateViewModel.Token!, options);

            string role = new JwtSecurityTokenHandler().ReadJwtToken(userAuthenticateViewModel.Token)
                       .Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value!;

            TempData["success"] = "Login success";

            return role switch
            {
                "Admin" => RedirectToAction("index", "Admin"),
                "User" => RedirectToAction("index", "User"),
                _ => RedirectToAction("index", "Login")
            };
        }
    }
    #endregion

    #region Logout
    public IActionResult Logout()
    {
        Response.Cookies.Append("Token", "", new CookieOptions
        {
            Expires = DateTime.UtcNow.AddDays(-1) // Settting expiration in the past
        });
        Response.Cookies.Delete("Token");

        return RedirectToAction("index", "login");
    }
    #endregion


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
