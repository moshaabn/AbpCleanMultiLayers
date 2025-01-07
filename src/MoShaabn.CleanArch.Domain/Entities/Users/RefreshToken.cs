using MoShaabn.CleanArch.Entities.Base;
using System;
using System.Security.Cryptography;

namespace MoShaabn.CleanArch.Entities.Users;

public class RefreshToken : BaseEntity
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpirationDate { get; set; }
    public bool IsExpired => DateTime.UtcNow >= ExpirationDate;
    public bool IsRevoked { get; set; }
    public bool IsActive => !IsExpired && !IsRevoked;

    public Guid UserId { get; set; }
    public User User { get; set; }

    public RefreshToken(DateTime expirationDate)
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
        }
        Token = Convert.ToBase64String(randomNumber);
        ExpirationDate = expirationDate;
    }

    public RefreshToken()
    {
    }

    public void Revoke()
    {
        IsRevoked = true;
    }
}
