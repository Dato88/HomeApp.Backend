using SharedKernel;

namespace Domain.Entities.Articles;

public class Store : AuditableEntity
{
    public int StoreId { get; set; }
    public string Name { get; set; } = null!;
    public string? WebsiteUrl { get; set; }

    public ICollection<ArticlePrice> ArticlePrices { get; set; } = new List<ArticlePrice>();
}
