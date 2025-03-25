﻿using Server.Interfaces.IServices;

namespace Server.Services
{
    public class BcryptService : IBcryptService
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }  
    }
}
