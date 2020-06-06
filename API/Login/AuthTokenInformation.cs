using System;

namespace API.Login
{
    public struct AuthTokenInformation
    {
        internal AuthTokenInformation(string username)
        {
            Username = username;
            LastAccess = DateTime.Now;
        }
        public string Username { get; }
        internal DateTime LastAccess { get; set; }
    }
}