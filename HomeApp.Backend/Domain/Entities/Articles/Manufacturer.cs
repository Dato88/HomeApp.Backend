using SharedKernel;

namespace Domain.Entities.Articles;

public class Manufacturer : AuditableEntity
{
    public int ManufacturerId { get; set; }

    public string Name { get; set; } = null!;

    public string? WebsiteUrl { get; set; }

    public ICollection<Article> Articles { get; set; } = new HashSet<Article>();
}
