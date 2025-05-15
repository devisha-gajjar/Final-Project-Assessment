using WebApp.Entities.Data;
using WebApp.Entities.Model;
using WebApp.Repositories.IRepositories;

namespace WebApp.Repositories.Implementation;

public class EnrollmentRepository : GenericRepository<Enrollment>, IEnrollmentRepository
{
    private readonly WebAppFinalContext _db;
    public EnrollmentRepository(WebAppFinalContext db) : base(db)
    {
        _db = db;
    }
}
