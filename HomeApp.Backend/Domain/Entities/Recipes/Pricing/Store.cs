using SharedKernel;

namespace Domain.Entities.Recipes.Pricing;

public class Store : AuditableEntity
{
    public int StoreId { get; set; }
    public string Name { get; set; } = null!;
    public string? WebsiteUrl { get; set; }

    public ICollection<IngredientPrice> IngredientPrices { get; set; } = new List<IngredientPrice>();
}
