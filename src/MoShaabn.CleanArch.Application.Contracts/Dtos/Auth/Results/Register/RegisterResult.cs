using System;

namespace MoShaabn.CleanArch.Dtos.Auth.Results.Register;

public class RegisterResult
{
    public Guid Id { get; set; }
    public string PhoneNumber { get; set; }
    public String Role { get; set; }
    public int PhoneCodeExpireAt { get; set; } 
}