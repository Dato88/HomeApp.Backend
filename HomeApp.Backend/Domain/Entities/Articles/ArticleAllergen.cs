using SharedKernel;

namespace Domain.Entities.Articles;

public class ArticleAllergen : AuditableEntity
{
    public int ArticleId { get; set; }
    public int AllergenId { get; set; }

    public virtual Article Article { get; set; } = null!;
    public virtual Allergen Allergen { get; set; } = null!;
}
