namespace Routes
{

    public static class ApiRoutes
    {
        public const string V1 = "api/v1";
        public const string V2 = "api/v2";

        // Mønstre du kan genbruge:
        public const string V1Root = V1;
        public const string Auth = V1 + "/" + "auth";
        public const string Secret = V1 + "/secret";
        public const string User = V1 + "/users";

        public const string v2Users = V2 + "/users";
    }
}