namespace HomeApp.Library.Models.BaseModels;

public class BaseReponseGeneric<T>
{
    public bool Succcess { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
    public IEnumerable<BaseError>? Errors { get; set; }
}
