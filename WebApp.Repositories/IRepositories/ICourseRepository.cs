using WebApp.Entities.Model;

namespace WebApp.Repositories.IRepositories;

public interface ICourseRepository : IGenericRepository<Course>
{
    public IQueryable<Course> GetCourseBySearch(string searchTerm);
}
