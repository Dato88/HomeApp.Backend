using SharedKernel;

namespace Domain.Entities.Articles;

public class Article : AuditableEntity
{
    public int ArticleId { get; set; }

    public string Name { get; set; } = null!;
    public int? ArticleCategoryId { get; set; }
    public bool IsActive { get; set; }

    public virtual ArticleCategory? Category { get; set; }
    public virtual ArticleNutrition? ArticleNutrition { get; set; }

    public virtual ICollection<ArticleAllergen> ArticleAllergens { get; set; } = new HashSet<ArticleAllergen>();
    public virtual ICollection<ArticlePrice> ArticlePrices { get; set; } = new HashSet<ArticlePrice>();
}
