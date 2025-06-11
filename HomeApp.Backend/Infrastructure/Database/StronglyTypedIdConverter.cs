using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Database;

public class StronglyTypedIdConverter<T> : ValueConverter<T, int>
    where T : struct
{
    public StronglyTypedIdConverter()
        : base(
            id => (int)typeof(T).GetProperty("Value")!.GetValue(id)!,
            value => (T)Activator.CreateInstance(typeof(T), value)!
        )
    {
    }
}
