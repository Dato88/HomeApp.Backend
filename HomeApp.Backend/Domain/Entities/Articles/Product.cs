using SharedKernel;

namespace Domain.Entities.Articles;

public class Product : AuditableEntity
{
    public int ProductId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int? ArticleCategoryId { get; set; }
    public ArticleCategory? Category { get; set; }

    public ICollection<Article> Articles { get; set; } = new HashSet<Article>();
}
