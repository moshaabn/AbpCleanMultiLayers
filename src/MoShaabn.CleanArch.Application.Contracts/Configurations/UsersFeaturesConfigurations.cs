namespace MoShaabn.CleanArch.Configurations
{
    public class UsersFeaturesConfigurations
    {
        public const string SectionName = "Otp";

        public int TtlInMinutes { get; set; }

        public int MaxTry { get; set; }

        public int MaxResend { get; set; }
    }
}
