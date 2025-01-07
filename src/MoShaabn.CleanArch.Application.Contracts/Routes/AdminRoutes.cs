namespace MoShaabn.CleanArch.Routes;

public static class AdminRoutes
{
    private const string BaseUrl = "api/v1/Admin/";

    public static class Users
    {
        private const string ModuleUrl = BaseUrl + "users/";
        public const string List = ModuleUrl;
        public const string Single = ModuleUrl + "{id}";
        public const string Accept = ModuleUrl + "Accept/{id}";
    }

    public static class Pages
    {
        private const string ModuleUrl = BaseUrl + "pages/";
        public const string GetBySlug = ModuleUrl + "{slug}";
        public const string Update = ModuleUrl + "{slug}";
    }

}
