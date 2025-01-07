using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using MoShaabn.CleanArch.Business.Client.Profile.Results;
using MoShaabn.CleanArch.Business.OTP.Commands.SendOtp;
using MoShaabn.CleanArch.Business.OTP.Commands.VerifyOtp;
using MoShaabn.CleanArch.Business.OTP.Results;
using MoShaabn.CleanArch.Dtos.Auth.Commands.RefreshToken;
using MoShaabn.CleanArch.Dtos.Auth.Commands.Register;
using MoShaabn.CleanArch.Dtos.Auth.Results.Login;
using MoShaabn.CleanArch.Dtos.Auth.Results.Register;
using Volo.Abp.Application.Services;

namespace MoShaabn.CleanArch.Interfaces;

public interface IAuthService : IApplicationService
{
    Task<string> GenerateTokenAsync(Guid userId, List<Claim> claims = null);

    Task<AuthResult> VerifyOTPAsync(VerifyOtpCommand input);

    Task<SendOtpResult> SendOTPAsync(SendOtpCommand input);

    Task<AuthResult> RefreshTokenAsync(RefreshTokenCommand refreshTokenCommand);

    Task<RegisterResult> RegisterAsync(RegisterCommand Command);

    Task<GetProfileResult> GetProfileAsync();

    Task LogoutAsync();
}