using Microsoft.EntityFrameworkCore;
using WebApp.Entities.Data;
using WebApp.Entities.Model;
using WebApp.Repositories.IRepositories;

namespace WebApp.Repositories.Implementation;

public class CourseRepository : GenericRepository<Course>, ICourseRepository
{
    private readonly WebAppFinalContext _db;

    public CourseRepository(WebAppFinalContext db) : base(db)
    {
        _db = db;
    }

    public IQueryable<Course> GetCourseBySearch(string searchTerm)
    {
        var query = _db.Courses.Include(d => d.Department).Where(c => !c.IsDeleted);

        if (!string.IsNullOrEmpty(searchTerm))
        {
            string trimSearchTerm = searchTerm.Trim().ToLower();
            query = query.Where(u => u.CourseName.ToLower().Contains(trimSearchTerm) || u.Department.DepartmentName.ToLower().Contains(trimSearchTerm));
        }

        return query;
    }
}
