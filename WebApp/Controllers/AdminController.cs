using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Entities.Model;
using WebApp.Entities.ViewModel;
using WebApp.Service.Helper;
using WebApp.Service.IService;

namespace WebApp.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ICourseService _courseService;

    public AdminController(ICourseService courseService)
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

    #region GetCourseModal
    public IActionResult GetCourse(int courseId)
    {
        if (courseId < 0)
        {
            return Json(new { success = false, message = "Invalid ID. It must be a positive integer!!" });
        }

        AddCourseViewModel model = new()
        {
            DepartmentList = _courseService.GetAllDepartment().Select(c => new SelectListItem
            {
                Value = c.DepartmentId.ToString(),
                Text = c.DepartmentName
            }).ToList()
        };

        if (courseId != 0)
        {
            model = _courseService.GetCourse(courseId);
        }
        return PartialView("_addCourse", model);
    }
    #endregion

    #region AddEditCourse
    public IActionResult AddEditCourse([FromForm] AddCourseViewModel addCourseViewModel)
    {
        addCourseViewModel.DepartmentList = _courseService.GetAllDepartment().Select(c => new SelectListItem
        {
            Value = c.DepartmentId.ToString(),
            Text = c.DepartmentName
        }).ToList();

        if (!ModelState.IsValid)
            return PartialView("_addCourse", addCourseViewModel);
        else if (addCourseViewModel.Id == 0)
        {
            var (addCourse, message) = _courseService.AddCourse(addCourseViewModel);
            if (addCourse)
                return Json(new { success = true, message });
            else
                return Json(new { success = false, message });

        }
        else if (addCourseViewModel.Id > 0)
        {
            var (update, message) = _courseService.UpdateCourse(addCourseViewModel);
            if (update)
                return Json(new { success = true, message });
            else
                return Json(new { success = false, message });
        }
        else
        {
            return Json(new { success = false, message = "Invalid ID. It must be a positive integer!!" });
        }
    }
    #endregion

    #region DeleteCourse
    [HttpPost]
    public IActionResult DeleteCourse(int id)
    {
        if (id <= 0)
        {
            return Json(new { success = false, message = "Invalid ID. It must be a positive integer!!" });
        }

        var (isDeleted, message) = _courseService.DeleteCourse(id);

        if (!isDeleted)
        {
            return Json(new { success = false, message });
        }
        else
        {
            return Json(new { success = true, message });
        }
    }
    #endregion

    #region GetStudentData
    public IActionResult GetStudentData(int courseId)
    {
        List<UserViewModel> userViewModels = _courseService.GetStudentData(courseId);
        return PartialView("_studentList", userViewModels);
    }
    #endregion
}
