using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[Authorize(Roles = "User")]
public class UserController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
