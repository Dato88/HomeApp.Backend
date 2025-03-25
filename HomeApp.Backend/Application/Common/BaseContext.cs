using Application.Abstractions.Data;

namespace Application.Common;

public class BaseContext(IHomeAppContext dbContext)
{
    protected readonly IHomeAppContext DbContext = dbContext;
}
