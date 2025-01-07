using MoShaabn.CleanArch.Enums;
using Microsoft.AspNetCore.Http;

namespace MoShaabn.CleanArch.Dtos.Auth.Commands.Register;

public class RegisterCommand
{
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public UserGenderEnum UserGender { get; set; }
    public IFormFile ProfileImage { get; set; }
}