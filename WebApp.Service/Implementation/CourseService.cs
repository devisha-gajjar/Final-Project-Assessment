using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebApp.Entities.Model;
using WebApp.Entities.ViewModel;
using WebApp.Repositories.IRepositories;
using WebApp.Service.Helper;
using WebApp.Service.IService;

namespace WebApp.Service.Implementation;

public class CourseService : ICourseService
{
    private readonly IDepartmentRepository _departmentRepo;
    private readonly ICourseRepository _courseRepo;
    private readonly IEnrollmentRepository _enrollRepo;
    private readonly IUserRepository _userRepo;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CourseService(IDepartmentRepository departmentRepo, ICourseRepository courseRepo, IHttpContextAccessor httpContextAccessor, IEnrollmentRepository enrollRepo, IUserRepository userRepo)
    {
        _departmentRepo = departmentRepo;
        _courseRepo = courseRepo;
        _httpContextAccessor = httpContextAccessor;
        _enrollRepo = enrollRepo;
        _userRepo = userRepo;
    }

    #region GetAllCourses
    public PaginatedList<CourseListViewModel> GetAllCourses(string searchTerm, string sortOrder, int page, int pageSize)
    {
        IQueryable<Course> courses = _courseRepo.GetCourseBySearch(searchTerm);

        courses = sortOrder switch
        {
            "name_desc" => courses.OrderByDescending(s => s.CourseName),
            "name_asc" => courses.OrderBy(s => s.CourseName),
            "dpname_desc" => courses.OrderByDescending(s => s.Department.DepartmentName),
            "dpname_asc" => courses.OrderBy(s => s.Department.DepartmentName),
            _ => courses.OrderBy(s => s.CourseName),
        };

        int totalCourse = courses.Count();
        decimal countedPages = Math.Ceiling((decimal)totalCourse / pageSize);

        while (page > countedPages && totalCourse != 0)
        {
            page--;
        }

        courses = courses.Skip((page - 1) * pageSize).Take(pageSize);

        List<CourseListViewModel> courseList = courses.Select(i => new CourseListViewModel
        {
            Id = i.CourseId,
            CourseName = i.CourseName,
            DepartmentName = i.Department.DepartmentName,
            Credit = i.Credit,
        }).ToList();

        return new PaginatedList<CourseListViewModel>(courseList, totalCourse, page, pageSize)
        {
            SearchTerm = searchTerm!,
            SortOrder = sortOrder
        };
    }
    #endregion


    #region GetAllDepartment
    public List<Department> GetAllDepartment()
    {
        return _departmentRepo.GetAll().Where(d => !d.IsDeleted).OrderBy(d => d.DepartmentName).ToList();
    }
    #endregion

    #region AddCourse
    public (bool isAdd, string message) AddCourse(AddCourseViewModel addCourseViewModel)
    {
        bool duplicateCourse = _courseRepo.GetAll().Any(m => m.CourseName.ToLower() == addCourseViewModel.CourseName.ToLower() && !m.IsDeleted);

        if (duplicateCourse)
        {
            return (false, "Duplicate Course Name!!");
        }

        string userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value!;

        Course course = new()
        {
            CourseName = addCourseViewModel.CourseName,
            Content = addCourseViewModel.Content,
            Credit = addCourseViewModel.Credit,
            DepartmentId = addCourseViewModel.DeptId,
            CreatedOn = DateTime.Now,
            CreatedBy = userId
        };

        _courseRepo.Add(course);
        return (true, "Course Added Successfully!!");
    }
    #endregion

    #region GetCourse
    public AddCourseViewModel GetCourse(int courseId)
    {
        Course course = _courseRepo.GetAll().FirstOrDefault(c => c.CourseId == courseId)!;

        AddCourseViewModel addCourseViewModel = new()
        {
            Id = course.CourseId,
            CourseName = course.CourseName,
            Credit = course.Credit,
            DeptId = course.DepartmentId,
            Content = course.Content,
            DepartmentList = GetAllDepartment().Select(c => new SelectListItem
            {
                Value = c.DepartmentId.ToString(),
                Text = c.DepartmentName
            }).ToList()
        };

        return addCourseViewModel;
    }
    #endregion

    #region UpdateCourse
    public (bool isUpdate, string message) UpdateCourse(AddCourseViewModel addCourseViewModel)
    {
        Course course = _courseRepo.GetById(addCourseViewModel.Id)!;

        if (course.CourseName != addCourseViewModel.CourseName)
        {
            if (course.CourseName.ToLower() == addCourseViewModel.CourseName.ToLower())
            {
                course.CourseName = addCourseViewModel.CourseName;
            }
            else
            {
                bool duplicateCourse = _courseRepo.GetAll().Any(m => m.CourseName.ToLower() == addCourseViewModel.CourseName.ToLower() && !m.IsDeleted);

                if (duplicateCourse)
                {
                    return (false, "Duplicate Course Name!!");
                }
            }
        }

        string userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value!;

        course.CourseName = addCourseViewModel.CourseName;
        course.Content = addCourseViewModel.Content;
        course.Credit = addCourseViewModel.Credit;
        course.DepartmentId = addCourseViewModel.DeptId;
        course.ModifiedOn = DateTime.Now;
        course.ModifiedBy = userId;

        _courseRepo.Update(course);
        return (true, "Course Updated Successfully!!");
    }

    #endregion

    #region DeleteCourse
    public (bool isDelete, string message) DeleteCourse(int id)
    {
        Course course = _courseRepo.GetById(id)!;

        bool isEnrolled = _enrollRepo.GetAll().Any(e => e.CourseId == id && !e.IsDeleted);

        if (isEnrolled)
        {
            return (false, "Can't Delete.. Student are Enrolled!!");
        }

        course.IsDeleted = true;

        _courseRepo.Update(course);

        return (true, "Course Deleted Succesfully!!");
    }
    #endregion

    #region ViewCourse
    public CourseListViewModel ViewCourse(int courseId)
    {
        Course course = _courseRepo.GetAll().Include(d => d.Department).FirstOrDefault(c => c.CourseId == courseId)!;

        CourseListViewModel courseListViewModel = new()
        {
            Id = course.CourseId,
            CourseName = course.CourseName,
            Credit = course.Credit,
            DepartmentName = course.Department.DepartmentName,
            Content = course.Content,
        };
        return courseListViewModel;
    }
    #endregion

    #region EnrollCourse
    public (bool isEnroll, string message) EnrollCourse(CourseListViewModel courseListViewModel)
    {
        string userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value!;

        bool isAlreadyEnroll = _enrollRepo.GetAll().Any(e => e.UserId.ToString() == userId && e.CourseId == courseListViewModel.Id && !e.IsDeleted);

        if (isAlreadyEnroll)
        {
            return (false, "You are alredy enrolled in this Course!!");
        }
        else
        {
            Enrollment enrollment = new()
            {
                UserId = int.Parse(userId),
                CourseId = courseListViewModel.Id,
                IsDeleted = false
            };
            _enrollRepo.Add(enrollment);
            return (true, "You are successfully Enrolled!!");
        }
    }
    #endregion

    #region GetMyCourses
    public List<CourseListViewModel> GetMyCourses()
    {
        string userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value!;

        List<Enrollment> courses = _enrollRepo.GetAll().Where(e => e.UserId.ToString() == userId && !e.IsDeleted)
                                            .Include(e => e.Course)
                                                .ThenInclude(e => e.Department).ToList();

        return courses.Select(item => new CourseListViewModel
        {
            Id = item.CourseId,
            CourseName = item.Course.CourseName,
            Credit = item.Course.Credit,
            DepartmentName = item.Course.Department.DepartmentName
        }).ToList();
    }
    #endregion

    #region CompleteCourse
    public (bool IsCompleted, string message) CompleteCourse(int id)
    {
        string userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value!;

        Enrollment? enrollment = _enrollRepo.GetAll().FirstOrDefault(e => e.CourseId == id && e.UserId.ToString() == userId);

        if (enrollment == null)
        {
            return (false, "No enrollment Found!!");
        }
        else
        {
            enrollment.IsDeleted = true;
            _enrollRepo.Update(enrollment);
            return (true, "Course Completed Successfully!!");
        }
    }
    #endregion

    #region GetStudentData
    public List<UserViewModel> GetStudentData(int courseId)
    {
        List<Enrollment> enrollments = _enrollRepo.GetAll().Include(e => e.User).Where(e => e.CourseId == courseId && !e.IsDeleted).ToList();

        return enrollments.Select(item => new UserViewModel()
        {
            Name = item.User.Name,
            UserName = item.User.UserName,
            Email = item.User.Email,
            EnrollDate = item.CreatedOn,
        }).ToList();
    }
    #endregion

    #region ShowProfile
    public UserViewModel ShowProfile()
    {
        string userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value!;

        User user = _userRepo.GetAll().FirstOrDefault(c => c.UserId.ToString() == userId)!;

        List<Enrollment> courseList = _enrollRepo.GetAll().Include(c => c.Course).Where(c => c.UserId.ToString() == userId && !c.IsDeleted).ToList();

        return new UserViewModel()
        {
            Name = user.Name,
            UserName = user.UserName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber ?? "-",
            RegisterDate = user.CreatedOn,
            TotalCourse = courseList.Count,
            TotalCredits = courseList.Sum(c => c.Course.Credit),
        };
    }
    #endregion
}
