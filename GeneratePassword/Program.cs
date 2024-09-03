using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace GeneratePassword
{
    public class Program
    {
        static void Main(string[] args)
        {
            
                string password = "Hasan123";
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
                Console.WriteLine("Hashed password: " + hashedPassword);
            
        }
    }
}
