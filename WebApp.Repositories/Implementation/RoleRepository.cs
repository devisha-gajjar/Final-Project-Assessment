using WebApp.Entities.Data;
using WebApp.Entities.Model;
using WebApp.Repositories.IRepositories;

namespace WebApp.Repositories.Implementation;

public class RoleRepository : GenericRepository<Role>, IRoleRepository
{
    private readonly WebAppFinalContext _db;

    public RoleRepository(WebAppFinalContext db) : base(db)
    {
        _db = db;
    }
}
