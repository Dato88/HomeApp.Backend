﻿namespace HomeApp.Identity.Entities.DataTransferObjects.Register;

public class RegistrationResponseDto
{
    public bool IsSuccessfulRegistration { get; set; }
    public IEnumerable<string>? Errors { get; set; }
}
