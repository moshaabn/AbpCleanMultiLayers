namespace MoShaabn.CleanArch.Routes;

public static class ApiRoutes
{
    private const string BaseUrl = "api/v1/";

    public static class Auth
    {
        private const string ModuleUrl = BaseUrl + "auth/";
        public const string Login = ModuleUrl + "login";
        public const string RefreshToken = ModuleUrl + "refresh-token";
        public const string ForgetPassword = ModuleUrl + "forget-password";
        public const string ValidateForgetPasswordOtp = ModuleUrl + "verify-forget-password-otp";
        public const string ResendForgetPasswordOtp = ModuleUrl + "resend-forget-password-otp";
        public const string ChangePassword = ModuleUrl + "change-password";
        public const string Register = ModuleUrl + "register";

        public const string VerifyOtp = ModuleUrl + "verify-otp";
        public const string SendOtp = ModuleUrl + "send-otp";
        public const string MyInfo = ModuleUrl + "my-info";
        public const string Logout = ModuleUrl + "logout";
    }
    //refrence to for CRUD routes to minimize code duplication
    //please read 
    public static class Sample {
        private const string ModuleUrl = BaseUrl + "samples/";
        public const string Base = ModuleUrl; //For Get All and Create
        public const string Single = ModuleUrl + "{id}"; //For Get Single, Update and Delete
    }
}