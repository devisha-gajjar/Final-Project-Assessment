using WebApp.Entities.Model;
using WebApp.Entities.ViewModel;
using WebApp.Service.Helper;

namespace WebApp.Service.IService;

public interface ICourseService
{
    public List<Department> GetAllDepartment();
    public PaginatedList<CourseListViewModel> GetAllCourses(string searchTerm, string sortOrder, int page, int pageSize);
    public (bool isAdd, string message) AddCourse(AddCourseViewModel addCourseViewModel);
    public AddCourseViewModel GetCourse(int courseId);
    public (bool isUpdate, string message) UpdateCourse(AddCourseViewModel addCourseViewModel);
    public (bool isDelete, string message) DeleteCourse(int id);
    public CourseListViewModel ViewCourse(int courseId);
    public (bool isEnroll, string message) EnrollCourse(CourseListViewModel courseListViewModel);
    public List<CourseListViewModel> GetMyCourses();
    public (bool IsCompleted, string message) CompleteCourse(int id);
    public List<UserViewModel> GetStudentData(int courseId);
    public UserViewModel ShowProfile();
}
