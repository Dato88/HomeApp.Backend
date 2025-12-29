using SharedKernel;

namespace Domain.Entities.Articles;

public class Unit : AuditableEntity
{
    public int UnitId { get; set; }

    public string Name { get; set; } = null!;
    public string Abbreviation { get; set; } = null!;
}
