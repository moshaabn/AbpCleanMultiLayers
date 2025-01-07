using MoShaabn.CleanArch.Enums;
using System;

namespace MoShaabn.CleanArch.Dtos.Auth.Results.Login;

public class AuthResult
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime AccessTokenExpireAt { get; set; }
    public DateTime RefreshTokenExpireAt { get; set; }


    public bool CompleteTeacherProfile { get; set; } = false;

    public bool AdminAcepptedTeacherProfile { get; set; } = false;
}