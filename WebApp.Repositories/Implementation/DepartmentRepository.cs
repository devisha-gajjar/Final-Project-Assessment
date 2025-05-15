using WebApp.Entities.Data;
using WebApp.Entities.Model;
using WebApp.Repositories.IRepositories;

namespace WebApp.Repositories.Implementation;

public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
{
    private readonly WebAppFinalContext _db;

    public DepartmentRepository(WebAppFinalContext db) : base(db)
    {
        _db = db;
    }
}
