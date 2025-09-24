using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demochka
{
    public static class AuthService
    {
        private static int _currentUserId;
        private static readonly object _lock = new object();

        public static int CurrentUserId
        {
            get
            {
                lock (_lock)
                {
                    return _currentUserId;
                }
            }
        }

        public static void Login(int userId)
        {
            lock (_lock)
            {
                _currentUserId = userId;
            }
        }

        public static void Logout()
        {
            lock (_lock)
            {
                _currentUserId = 0;
            }
        }

        public static bool IsAuthenticated => CurrentUserId > 0;
    }
}
