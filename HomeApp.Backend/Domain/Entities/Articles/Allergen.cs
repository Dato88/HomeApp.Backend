using SharedKernel;

namespace Domain.Entities.Articles;

public class Allergen : AuditableEntity
{
    public int AllergenId { get; set; }

    public string Name { get; set; } = null!;
    public string? Code { get; set; }

    public virtual ICollection<ArticleAllergen> ArticleAllergens { get; set; } = new HashSet<ArticleAllergen>();
}
