using System;

namespace MoShaabn.CleanArch.Business.OTP.Results;

public class SendOtpResult
{
    public DateTime ExpiryTimeByMinute { get; init; }
    public string OtpCode { get; init; }
}