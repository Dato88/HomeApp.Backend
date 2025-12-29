using SharedKernel;

namespace Domain.Entities.Articles;

public class ArticlePrice : AuditableEntity
{
    public int ArticlePriceId { get; set; }
    public int ArticleId { get; set; }
    public int StoreId { get; set; }
    public int UnitId { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; } = "EUR";
    public DateTime ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }

    public virtual Article Article { get; set; } = null!;
    public virtual Store Store { get; set; } = null!;
    public virtual Unit Unit { get; set; } = null!;
}
