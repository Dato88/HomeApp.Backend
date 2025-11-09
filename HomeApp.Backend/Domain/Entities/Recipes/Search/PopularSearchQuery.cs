using SharedKernel;

namespace Domain.Entities.Recipes.Search;

public class PopularSearchQuery : AuditableEntity
{
    public int QueryId { get; set; }
    public string QueryText { get; set; } = null!;
    public int SearchCount { get; set; }
    public DateTime LastSearchedAt { get; set; }
}
