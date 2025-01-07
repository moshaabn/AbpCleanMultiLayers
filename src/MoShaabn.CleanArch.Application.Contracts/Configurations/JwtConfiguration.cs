namespace MoShaabn.CleanArch.Configurations;

public class JwtConfiguration
{
    public const string SectionName = "Jwt";
    public string Issuer { get; set; }

    public string Audience { get; set; }

    public string Key { get; set; }

    public int ExpireInMinutes { get; set; }

}
