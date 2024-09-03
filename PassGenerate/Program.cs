using BCrypt.Net;
namespace PassGenerate
{
    public class Program
    {
        static void Main(string[] args)
        {
          
                string password = "Hasan123";
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
                Console.WriteLine("Hashed password: " + hashedPassword);


                Console.ReadLine();
            
        }
    }
}
