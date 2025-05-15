using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Entities.ViewModel;
using WebApp.Service.Helper;
using WebApp.Service.IService;

namespace WebApp.Controllers;

[Authorize(Roles = "User")]
public class UserController : Controller
{
    private readonly ICourseService _courseService;

    public UserController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    public IActionResult Index()
    {
        return View();
    }

    #region GetCourses
    [HttpGet]
    public IActionResult GetCourses(string searchTerm, string sortOrder, int page = 1, int pageSize = 5)
    {
        PaginatedList<CourseListViewModel> courses = _courseService.GetAllCourses(searchTerm, sortOrder, page, pageSize);
        return PartialView("_courseList", courses);
    }
    #endregion GetCourses

    #region ViewCourse
    public IActionResult ViewCourse(int id)
    {
        CourseListViewModel courseListViewModel = _courseService.ViewCourse(id);
        return PartialView("_enrollCourse", courseListViewModel);
    }
    #endregion

    #region EnrollCourse
    public IActionResult EnrollCourse(CourseListViewModel courseListViewModel)
    {
        var (isEnroll, message) = _courseService.EnrollCourse(courseListViewModel);
        if (isEnroll)
        {
            return Json(new { success = true, message });
        }
        else
        {
            return Json(new { success = false, message });
        }
    }
    #endregion

    #region MyCourses
    public IActionResult MyCourses()
    {
        List<CourseListViewModel> courses = _courseService.GetMyCourses();
        return PartialView("_myCourses", courses);
    }
    #endregion

    #region CompleteCourse
    public IActionResult CompleteCourse(int id)
    {
        var (isCompleted, message) = _courseService.CompleteCourse(id);

        if (!isCompleted)
        {
            return Json(new { success = false, message });
        }
        else
        {
            return Json(new { success = true, message });
        }
    }
    #endregion

    #region ShowProfile
    public IActionResult ShowProfile()
    {
        UserViewModel userViewModel = _courseService.ShowProfile();
        return PartialView("_myProfile", userViewModel);
    }
    #endregion
}
