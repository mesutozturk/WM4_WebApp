using System.Collections.Generic;

namespace ItServiceApp.Models
{
    public static class RoleNames
    {
        public static string Admin = "Admin";
        public static string User = "User";
        public static string Passive = "Passive";
        
        public static List<string> Roles => new List<string>() { Admin, User, Passive };
    }
}