using WebApp.Entities.ViewModel;
using WebApp.Service.Helper;

namespace WebApp.Service.IService;

public interface ICourseService
{
    public PaginatedList<CourseListViewModel> GetAllCourses(string searchTerm, string sortOrder, int page, int pageSize);
}
