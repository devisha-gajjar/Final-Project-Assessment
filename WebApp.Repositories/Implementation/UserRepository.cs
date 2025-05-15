using WebApp.Entities.Data;
using WebApp.Entities.Model;
using WebApp.Repositories.IRepositories;

namespace WebApp.Repositories.Implementation;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    private readonly WebAppFinalContext _db;
    public UserRepository(WebAppFinalContext db) : base(db)
    {
        _db = db;
    }
}
    