using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework_Client.Managers
{
    public static class AccountManager
    {
        public static string Token { get; set; } = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiMzc1MDAwMDAwMDAwIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJuYmYiOjE2MjIwOTU1MDIsImV4cCI6MTYyMjExNzEwMiwiaXNzIjoiUGhvbmV0aWNTZXJ2ZXIiLCJhdWQiOiJQaG9uZXRpY0NsaWVudCJ9.n-OGl3dNQopmSH8jxVW0Bf_dGSNSmdKZ6RDDyT-A75s";

        public static string Role { get; set; } = "Admin";
    }
}
