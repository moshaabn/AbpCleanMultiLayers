using MoShaabn.CleanArch.Business.Client.Profile.Results;
using MoShaabn.CleanArch.Business.OTP.Commands.SendOtp;
using MoShaabn.CleanArch.Business.OTP.Commands.VerifyOtp;
using MoShaabn.CleanArch.Business.OTP.Results;
using MoShaabn.CleanArch.Dtos.Auth.Commands.RefreshToken;
using MoShaabn.CleanArch.Dtos.Auth.Commands.Register;
using MoShaabn.CleanArch.Dtos.Auth.Results.Login;
using MoShaabn.CleanArch.Dtos.Auth.Results.Register;
using MoShaabn.CleanArch.Entities.Users;
using AutoMapper;

namespace MoShaabn.CleanArch.MappingProfiles
{
    public class UserMapper : Profile
    {
        public UserMapper() {

            CreateMap<RegisterCommand, User>();
            CreateMap<VerifyOtpCommand, User>();
            CreateMap<SendOtpCommand, User>();
            CreateMap<RefreshTokenCommand, User>();

            CreateMap<User, RegisterResult>();
            CreateMap<User, AuthResult>();
            CreateMap<User, SendOtpResult>();
            CreateMap<User, GetProfileResult>();
        }
    }
}
