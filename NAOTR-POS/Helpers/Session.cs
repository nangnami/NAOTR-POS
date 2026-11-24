
using NAOTR_POS.Models;

namespace NAOTR_POS.Helpers
{
    public static class Session
    {
        public static User CurrentUser { get; private set; }

        public static bool IsLoggedIn
        {
            get { return CurrentUser != null; }
        }

        public static void Login(User user)
        {
            CurrentUser = user;
        }

        public static void Logout()
        {
            CurrentUser = null;
        }

        public static bool IsInRole(string role)
        {
            return CurrentUser != null &&
                   CurrentUser.RoleName == role;
        }
    }
}
