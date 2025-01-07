using MoShaabn.CleanArch.Business.Client.Profile.Results;
using MoShaabn.CleanArch.Business.OTP.Commands.SendOtp;
using MoShaabn.CleanArch.Business.OTP.Commands.VerifyOtp;
using MoShaabn.CleanArch.Business.OTP.Results;
using MoShaabn.CleanArch.Dtos.Auth.Commands.RefreshToken;
using MoShaabn.CleanArch.Dtos.Auth.Commands.Register;
using MoShaabn.CleanArch.Dtos.Auth.Results.Login;
using MoShaabn.CleanArch.Dtos.Auth.Results.Register;
using MoShaabn.CleanArch.Interfaces;
using MoShaabn.CleanArch.Routes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MoShaabn.CleanArch.Controllers;

public class AuthController(IAuthService authService) : CleanArchController
{
    [AllowAnonymous]
    [HttpPost(ApiRoutes.Auth.Register)]
    public async Task<RegisterResult> RegisterAsync([FromForm] RegisterCommand command, CancellationToken cancellationToken = default)
    {

        return await authService.RegisterAsync(command);
    }

    [AllowAnonymous]
    [HttpPost(ApiRoutes.Auth.SendOtp)]
    public async Task<SendOtpResult> SendOTPAsync([FromBody] SendOtpCommand sendOtpRequest)
    {
        return await authService.SendOTPAsync(sendOtpRequest);
    }

    [AllowAnonymous]
    [HttpPost(ApiRoutes.Auth.VerifyOtp)]
    public async Task<AuthResult> VerifyOTPAsync([FromBody] VerifyOtpCommand verifyOtpRequest)
    {
        return await authService.VerifyOTPAsync(verifyOtpRequest);
    }



    [HttpPost(ApiRoutes.Auth.RefreshToken)]
    public async Task<AuthResult> RefreshTokenAsync([FromBody] RefreshTokenCommand refreshTokenCommand)
    {
        return await authService.RefreshTokenAsync(refreshTokenCommand);
    }

    [Authorize]
    [HttpGet(ApiRoutes.Auth.MyInfo)]
    public async Task<GetProfileResult> GetProfileAsync()
    {
        return await authService.GetProfileAsync();
    }

    [Authorize]
    [HttpPost(ApiRoutes.Auth.Logout)]
    public async Task LogoutAsync()
    {
        await authService.LogoutAsync();
    }
}