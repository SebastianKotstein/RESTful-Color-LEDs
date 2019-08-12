using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.app.ledserver.tools
{
    public class Scopes
    {
        public const string LED_READ = "led-read";
        public const string LED_WRITE = "led-write";
        public const string GROUP_READ = "group-read";
        public const string GROUP_WRITE = "group-write";
        public const string CONTROLLER_READ = "controller-read";
        public const string CONTROLLER_WRITE = "controller-write";
        public const string FIRMWARE_READ = "firmware-read"; //handle with care as this scope grants access to the wifi credentials!!
        public const string FIRMWARE_WRITE = "firmware-write";
        public const string AUTH_CLIENT_READ = "auth-client-read";
        public const string AUTH_CLIENT_WRITE = "auth-client-write";
        public const string AUTH_SCOPE_READ = "auth-scope-read";
        public const string AUTH_USER_READ = "auth-user-read";
        public const string AUTH_USER_WRITE = "auth-user-write";
    }
}
