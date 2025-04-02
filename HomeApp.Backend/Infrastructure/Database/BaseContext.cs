using Application.Abstractions.Data;

namespace Infrastructure.Database;

public class BaseContext(IHomeAppContext dbContext)
{
    protected readonly IHomeAppContext DbContext = dbContext;
}
