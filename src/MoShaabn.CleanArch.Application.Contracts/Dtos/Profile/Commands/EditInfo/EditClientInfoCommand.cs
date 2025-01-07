using System;

namespace MoShaabn.CleanArch.Business.Client.Profile.Commands.EditInfo;

public class EditClientInfoCommand
{
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public Guid CityId { get; set; }
    public Guid NeighborhoodId { get; set; }
}