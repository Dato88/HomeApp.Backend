using SharedKernel;

namespace Domain.Entities.Articles;

public class ArticleNutrition : AuditableEntity
{
    public int ArticleId { get; set; }

    public decimal? CaloriesKcalPer100g { get; set; }
    public decimal? ProteinGPer100g { get; set; }
    public decimal? CarbsGPer100g { get; set; }
    public decimal? SugarGPer100g { get; set; }
    public decimal? FatGPer100g { get; set; }
    public decimal? SaturatedFatGPer100g { get; set; }
    public decimal? FiberGPer100g { get; set; }
    public decimal? SaltGPer100g { get; set; }
    public string? LastSource { get; set; }

    public Article Article { get; set; } = null!;
}
