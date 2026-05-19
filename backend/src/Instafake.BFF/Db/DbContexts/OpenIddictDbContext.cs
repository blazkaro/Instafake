using Microsoft.EntityFrameworkCore;

namespace Instafake.BFF.Db.DbContexts;

public class OpenIddictDbContext(DbContextOptions<OpenIddictDbContext> options) : DbContext(options)
{
}
