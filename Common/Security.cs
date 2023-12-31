﻿using BC = BCrypt.Net.BCrypt;
namespace Common
{
    public static class Security
    {
        public static string HashPassword(string Password)
        {
            return BC.HashPassword(Password);
        }
        public static bool VerifyPassword(string Password, string HashPassword)
        {
            return BC.Verify(Password, HashPassword);
        }
    }
}
