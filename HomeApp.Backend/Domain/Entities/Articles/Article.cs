using SharedKernel;

namespace Domain.Entities.Articles;

public class Article : AuditableEntity
{
    public int ArticleId { get; set; }
    public int ProductId { get; set; }
    public int ManufacturerId { get; set; }

    public string Name { get; set; } = null!;
    public bool IsActive { get; set; }

    public virtual Product Product { get; set; } = null!;
    public virtual Manufacturer Manufacturer { get; set; } = null!;
    public virtual ArticleNutrition? ArticleNutrition { get; set; }

    public virtual ICollection<ArticleAllergen> ArticleAllergens { get; set; } = new HashSet<ArticleAllergen>();
    public virtual ICollection<ArticlePrice> ArticlePrices { get; set; } = new HashSet<ArticlePrice>();
}
