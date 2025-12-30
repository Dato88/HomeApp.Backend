using SharedKernel;

namespace Domain.Entities.Articles;

public class ArticleCategory : AuditableEntity
{
    public int ArticleCategoryId { get; set; }

    public string Name { get; set; } = null!;
    public int? ParentCategoryId { get; set; }

    public virtual ArticleCategory? ParentCategory { get; set; }
    public virtual ICollection<ArticleCategory> Children { get; set; } = new HashSet<ArticleCategory>();
}
