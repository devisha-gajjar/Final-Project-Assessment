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

    public CourseService(IDepartmentRepository departmentRepo, ICourseRepository courseRepo)
    {
        _departmentRepo = departmentRepo;
        _courseRepo = courseRepo;
    }

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
}
