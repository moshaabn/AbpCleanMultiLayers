using MoShaabn.CleanArch.Enums;
using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Identity;

namespace MoShaabn.CleanArch.Entities.Users;

public class User : IdentityUser
{
    //private readonly HashSet<RefreshToken> _refreshTokens = new HashSet<RefreshToken>();  // Correct initialization

    private User()
    {
    }

    public User(string name, string phone, string email , string userName)
    {
        Id = Guid.NewGuid();
        Name = Check.NotNull(name, nameof(name));
        PhoneNumber = phone;
        UserName = userName;
        NormalizedUserName = phone?.ToUpperInvariant();
        ConcurrencyStamp = Guid.NewGuid().ToString("N");
        SecurityStamp = Guid.NewGuid().ToString();
        Email = email;
        NormalizedEmail = Email?.ToUpperInvariant();
        IsActive = false;
    }

    public override string? Email { get; protected set; }
    public Otp Otp { get; set; }
    public  ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public string? ProfileImage { get; set; }
    // public UserGenderEnum UserGender { get; set; }

    public void Activate()
    {
        IsActive = true;
    }

    public void SetEmail(string email)
    {
        Email = email;
        NormalizedEmail = email.ToUpperInvariant();
    }

    public void SetPhoneNumber(string phoneNumber)
    {
        PhoneNumber = phoneNumber;
        UserName = phoneNumber;
        NormalizedUserName = phoneNumber.ToUpperInvariant();
        IsActive = false;
    }
}