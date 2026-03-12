using System;
using System.Collections.Generic;
using System.Text;

namespace Application.AuthUser
{
    public interface IPasswordHasherService
    {
        string HashPassword(string password);
    }
}
