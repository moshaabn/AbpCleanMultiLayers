using MoShaabn.CleanArch.Entities.Base;
using MoShaabn.CleanArch.Enums;
using System;

namespace MoShaabn.CleanArch.Entities.Users;

public class Otp : BaseEntity
{
    public string PhoneCode { get; set; }
    public DateTime PhoneCodeExpireAt { get; set; }
    public string NewPhone { get; set; }
    public string NewPhoneCode { get; set; }
    public DateTime? NewPhoneCodeExpireAt { get; set; }

    public int RetryCount { get; set; }
    public OtpTypeEnum Type { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }
}