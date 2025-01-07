using MoShaabn.CleanArch.Business.Client.Profile.Results;
using MoShaabn.CleanArch.Business.OTP.Commands.SendOtp;
using MoShaabn.CleanArch.Business.OTP.Commands.VerifyOtp;
using MoShaabn.CleanArch.Business.OTP.Results;
using MoShaabn.CleanArch.Configurations;
using MoShaabn.CleanArch.Dtos.Auth.Commands.RefreshToken;
using MoShaabn.CleanArch.Dtos.Auth.Commands.Register;
using MoShaabn.CleanArch.Dtos.Auth.Results.Login;
using MoShaabn.CleanArch.Dtos.Auth.Results.Register;
using MoShaabn.CleanArch.Entities.Users;
using MoShaabn.CleanArch.Enums;
using MoShaabn.CleanArch.Helpers;
using MoShaabn.CleanArch.Integrations.Storage;
using MoShaabn.CleanArch.Interfaces;
using MoShaabn.CleanArch.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Uow;
using Volo.Abp.Users;
using User = MoShaabn.CleanArch.Entities.Users.User;

namespace MoShaabn.CleanArch.Services;

public class AuthService(
     IRepository<RefreshToken, Guid> refreshTokenRepository,
    IRepository<User, Guid> userRepository,
    IRepository<Otp, Guid> otpRepository,
    IdentityUserManager userManager,
    JwtConfiguration jwtConfig,
    IStringLocalizer<CleanArchResource> localizer,
    IdentityUserManager identityUserManager,
    IStorageService storageService,
    ICurrentUser currentUser
    ) : BaseAppService, IAuthService
{
    [UnitOfWork]
    public async Task<RegisterResult> RegisterAsync(RegisterCommand Command)
    {
        // Declare the variable to hold the newly created user
        User userCreated;


        // Check if the phone number already exists
        User userFind = await userRepository.FirstOrDefaultAsync(x => x.PhoneNumber == Command.PhoneNumber);
        if (userFind != null)
        {
            // If the phone number already exists, throw an exception
            throw new UserFriendlyException(localizer["phone_number_already_exist"], "400");
        }

        // Create the user object with the provided phone number
        userCreated = new User(Command.Name, Command.PhoneNumber, "", Command.PhoneNumber);

        // userCreated.UserGender = Command.UserGender;


        // Create the user in the Identity system
        var createUserResult = await identityUserManager.CreateAsync(userCreated);

        // Check if user creation failed
        if (!createUserResult.Succeeded)
        {
            // If user creation fails, throw an exception with the error message
            throw new UserFriendlyException(localizer["failed_to_create_user"], "400", createUserResult.Errors.FirstOrDefault()?.Description);
        }

        // Assign the "Student" role to the newly created user
        var assignRoleResult = await identityUserManager.AddToRoleAsync(userCreated, RoleEnum.USER.ToString());

        // Check if role assignment failed
        if (!assignRoleResult.Succeeded)
        {
            // If role assignment fails, collect error messages and throw an exception
            var errorMessage = string.Join(", ", assignRoleResult.Errors.Select(e => e.Description));
            throw new UserFriendlyException(localizer["failed_to_assign_role"], "400", errorMessage);
        }

        // Generate OTP for phone/email verification
        var otp = new Otp
        {
            UserId = userCreated.Id,
            PhoneCode = GenerateOtp.GetOtp(),
            PhoneCodeExpireAt = DateTime.UtcNow.AddMinutes(1),
            Type = OtpTypeEnum.ConfirmPhone
        };

        // Insert OTP into the database
        var otpSaved = await otpRepository.InsertAsync(otp, true);

        // Handle profile image if provided
        if (Command.ProfileImage != null && Command.ProfileImage.Length > 0)
        {
            // Upload the profile image to storage
            var url = await storageService.Upload(Command.ProfileImage);
            userCreated.ProfileImage = url;  // Set the profile image URL for the user
        }
        await userRepository.UpdateAsync(userCreated);

        // Return the result with user ID, phone number, email, role, and OTP expiry time
        return new RegisterResult
        {
            Id = userCreated.Id,
            PhoneNumber = userCreated.PhoneNumber,
            Role = RoleEnum.USER.ToString(),
            PhoneCodeExpireAt = (int)(otp.PhoneCodeExpireAt - DateTime.UtcNow).TotalSeconds,  // Return OTP expiration in seconds
        };
    }
    public async Task<SendOtpResult> SendOTPAsync(SendOtpCommand Command)
    {
        User userFind;


        // Find user by phone number
        userFind = await userRepository.WithDetailsAsync(x => x.Otp)
                                       .Result
                                       .FirstOrDefaultAsync(x => x.PhoneNumber == Command.PhoneNumber);
        if (userFind == null)
        {
            throw new UserFriendlyException(localizer["user_not_exist"], "400"); // User not found
        }



        // Step 2: Delete existing OTP if it exists
        if (userFind.Otp != null)
        {


            // Hard delete the existing OTP
            await otpRepository.HardDeleteAsync(userFind.Otp, true);
        }

        // Step 3: Generate a new OTP
        var newOtp = new Otp
        {

            UserId = userFind.Id,
            PhoneCode = GenerateOtp.GetOtp(),
            PhoneCodeExpireAt = DateTime.UtcNow.AddMinutes(1),
            Type = OtpTypeEnum.ConfirmPhone


        };

        // Step 4: Save the new OTP
        await otpRepository.InsertAsync(newOtp);

        // Step 5: Return the result with OTP details
        return new SendOtpResult
        {
            ExpiryTimeByMinute = newOtp.PhoneCodeExpireAt, // Expiration time of the OTP
            OtpCode = newOtp.PhoneCode // Generated OTP code
        };
    }


    [UnitOfWork]
    public async Task<AuthResult> VerifyOTPAsync(VerifyOtpCommand Command)
    {

        // Step 1: Find the user based on AuthType (Phone or Email)
        User userFind;


        // Fetch user by phone number with OTP and RefreshTokens details
        userFind = await userRepository.WithDetailsAsync(x => x.Otp, x => x.RefreshTokens)
                                       .Result
                                       .FirstOrDefaultAsync(x => x.PhoneNumber == Command.PhoneNumber);



        // Step 2: Handle user not found case
        if (userFind == null)
        {
            throw new UserFriendlyException(localizer["user_not_exist"], "400"); // User does not exist
        }

        // Step 3: Check if OTP exists for the user
        if (userFind.Otp == null)
        {
            throw new UserFriendlyException(localizer["otp_not_sent"], "400"); // OTP not sent
        }

        // Step 4: Validate OTP code
        if (userFind.Otp.PhoneCode != Command.Otp)
        {
            throw new UserFriendlyException(localizer["invalid_otp"], "400"); // Invalid OTP code
        }

        // Step 5: Check if OTP has expired
        if (userFind.Otp.PhoneCodeExpireAt < DateTime.UtcNow)
        {
            throw new UserFriendlyException(localizer["otp_expired"], "400"); // OTP expired
        }

        // Step 6: Activate the user if inactive
        if (!userFind.IsActive)
        {
            userFind.Activate(); // Activate user if not already active
        }

        // Step 7: Map the user entity to AuthResult DTO
        var result = ObjectMapper.Map<User, AuthResult>(userFind);

        var userRoles = await userManager.GetRolesAsync(userFind); // Assuming GetRolesAsync fetches roles for the user
        result.Role = userRoles[0];

        // Step 8: Generate and assign access token
        result.AccessToken = await GenerateTokenAsync(userFind.Id);
        result.AccessTokenExpireAt = DateTime.UtcNow.AddMinutes(jwtConfig.ExpireInMinutes); // Set expiration for access token



        // Step 9: Check for active refresh tokens and assign or generate new one
        if (userFind.RefreshTokens.Any(rt => rt.IsActive))
        {

            // If there is an active refresh token, use it
            var activeRefreshToken = userFind.RefreshTokens.FirstOrDefault(rt => rt.IsActive);
            result.RefreshToken = activeRefreshToken.Token;
            result.RefreshTokenExpireAt = activeRefreshToken.ExpirationDate;
        }
        else
        {

            // Otherwise, generate a new refresh token
            var newToken = await GenerateNewRefreshTokenAsync(userFind.Id);
            result.RefreshToken = newToken.Token;
            result.RefreshTokenExpireAt = newToken.ExpirationDate;
        }

        // Step 10: Update user details (e.g., activate user, etc.)
        await userRepository.UpdateAsync(userFind);

        // Step 11: Return the authentication result with tokens
        return result;
    }

    public async Task<AuthResult> RefreshTokenAsync(RefreshTokenCommand refreshTokenCommand)
    {
        var user = await (await userRepository.WithDetailsAsync(x => x.RefreshTokens))
            .FirstOrDefaultAsync(c => c.RefreshTokens.Any(x => x.Token == refreshTokenCommand.RefreshToken))
            ?? throw new UserFriendlyException(localizer["user_not_found"], "404");

        var refreshToken = user.RefreshTokens.FirstOrDefault(rt => rt.Token == refreshTokenCommand.RefreshToken);

        if (refreshToken is null || !refreshToken.IsActive)
        {
            throw new UserFriendlyException(localizer["invalid_refresh_token"], "400");
        }

        refreshToken.Revoke();

        var result = ObjectMapper.Map<User, AuthResult>(user);

        result.AccessToken = await GenerateTokenAsync(user.Id);

        result.AccessTokenExpireAt = DateTime.Now.AddMinutes(jwtConfig.ExpireInMinutes);
        var newToken = await GenerateNewRefreshTokenAsync(user.Id);

        result.RefreshToken = newToken.Token;

        result.RefreshTokenExpireAt = newToken.ExpirationDate;

        return result;
    }
    public async Task<string> GenerateTokenAsync(Guid userId, List<Claim>? claims = null)
    {
        var user = await userRepository.GetAsync(userId);
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(jwtConfig.ExpireInMinutes);

        claims ??= [];
        claims.AddRange(
        [
            new (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new (ClaimTypes.MobilePhone, user.PhoneNumber),
            new (ClaimTypes.Role,(await userManager.GetRolesAsync(user))[0])
        ]);

        var token = new JwtSecurityToken(
            issuer: jwtConfig.Issuer,
            audience: jwtConfig.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private async Task<RefreshToken?> GenerateNewRefreshTokenAsync(Guid userId)
    {
        var user = await userRepository.GetAsync(userId);
        var refreshToken = new RefreshToken(DateTime.UtcNow.AddDays(10));
        refreshToken.UserId = userId;

        await refreshTokenRepository.InsertAsync(refreshToken);
        //user.AddRefreshToken(refreshToken);

        //await userRepository.UpdateAsync(user);

        return refreshToken;
    }
    public async Task<GetProfileResult> GetProfileAsync()
    {
        var userId = currentUser.GetId();

        var user = await userRepository.WithDetailsAsync(u => u.ProfileImage!)
            .ContinueWith(x => x.Result
                .FirstOrDefault(x => x.Id == userId));

        return ObjectMapper.Map<User, GetProfileResult>(user);
    }

    public async Task LogoutAsync()
    {
        var userId = currentUser.GetId();

        var user = await userRepository.GetAsync(userId);

        user.RefreshTokens.ToList().ForEach(rt => rt.Revoke());

        await userRepository.UpdateAsync(user);
    }


}