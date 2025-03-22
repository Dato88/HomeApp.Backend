﻿namespace SharedKernel;

public class BaseReponseGeneric<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
    public BaseError? Error { get; set; }
}
