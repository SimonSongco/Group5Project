using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Group5Project
{
    internal class Program
    {
        static (string, bool) Login(List<string> UserInfo, string CurrentUser, bool UserLogin)
        {
            while (true)
            {
                Console.Write("Username: ");
                string InputUsername = Console.ReadLine();

                Console.Write("Password: ");
                string InputPassword = Console.ReadLine();

                for (int i = 0; i < UserInfo.Count; i++)
                {
                    string[] parts = UserInfo[i].Split(',');

                    if (InputUsername == parts[0] && InputPassword == parts[1])
                    {
                        return (CurrentUser, UserLogin);
                    }
                }

                Console.WriteLine();
                Console.WriteLine("Invalid Credentials!");
                Console.WriteLine("Press Any Key To Try Again.");
                Console.ReadKey();
                Console.Clear();
            }
        }
        static List<string> Register(List<string> UserInfo)
        {

            while (true)
            {
                bool UsernameTaken = false;

                Console.Write("Username: ");
                string InputUsername = Console.ReadLine();

                for (int i = 0; i < UserInfo.Count; i++)
                {
                    string[] parts = UserInfo[i].Split(',');

                    if (InputUsername == parts[0])
                    {
                        Console.WriteLine("Username Already Taken!");
                        Console.WriteLine("Press Any Key To Try Again.");
                        Console.ReadKey();
                        UsernameTaken = true;
                        break;
                    }
                }

                if (UsernameTaken) continue;

                Console.Write("Password: "); 
                string InputPassword = Console.ReadLine();

                Console.Write("Confirm Password: "); 
                string InputConfirmPassword = Console.ReadLine();

                if (InputPassword == InputConfirmPassword)
                {
                    Console.WriteLine("Account Created!");
                    Console.WriteLine("Press Any Key To Go Back!");
                    Console.ReadKey();

                    string NewUserInfo = $"\n{InputUsername},{InputPassword}";
                    File.AppendAllText("User_Info.txt", NewUserInfo);

                    return File.ReadAllLines("User_Info.txt").ToList();
                }
                else
                {
                    Console.WriteLine("Credentials Not Valid!");
                    Console.WriteLine("Press Any Key To Try Again.");
                    Console.ReadKey();
                }
            }
        }
        static void Main(string[] args)
        {
            List<string> UserInfo = File.ReadAllLines("User_Info.txt").ToList();
            bool UserLogin = false;
            string CurrentUser = "";

            int MenuChoice = Convert.ToInt32(Console.ReadLine());

            switch (MenuChoice)
            {
                case 1:
                    (CurrentUser, UserLogin) = Login(UserInfo, CurrentUser, UserLogin);
                    break;
                case 2:
                    UserInfo = Register(UserInfo);
                    break;
                default:
                    break;
            }

            Console.WriteLine("Login DONE!!!");
            Console.ReadKey();
        }
    }
}
