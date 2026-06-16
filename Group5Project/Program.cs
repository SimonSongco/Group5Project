using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Linq.Expressions;

namespace Group5Project
{
    internal class Program
    {
        static bool isRunning = true;
        static (string, bool, bool) Login(List<string> UserInfo, string CurrentUser, bool UserLogin, bool LoginMenu)
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
                        return (CurrentUser, true, false);
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
                        Console.Clear();
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
                    Console.Clear();

                    string NewUserInfo = $"\n{InputUsername},{InputPassword},1";
                    File.AppendAllText("User_Info.txt", NewUserInfo);

                    return File.ReadAllLines("User_Info.txt").ToList();
                }
                else
                {
                    Console.WriteLine("Credentials Not Valid!");
                    Console.WriteLine("Press Any Key To Try Again.");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }
        static List<string> SaveProgress(List<string> UserInfo, string CurrentUser, int CurrentLevel)
        {
            for (int i = 0; i < UserInfo.Count; i++)
            {
                string[] parts = UserInfo[i].Split(',');

                if (CurrentUser == parts[0])
                {
                    UserInfo[i] = $"{parts[0]},{parts[1]},{CurrentLevel}";
                    break;
                }
            }

            File.WriteAllLines("User_Info.txt", UserInfo);
            Console.WriteLine("Progress saved successfully!");
            return UserInfo;
        }
        static void Game(List<string> DisasterInfo)
        {
            Random Random = new Random();
            List<string> UsedDisasters = new List<string>();

            for (int i = 0; i < 3; i++)
            {
                int RandomIndex;

                do
                {
                    RandomIndex = Random.Next(0, DisasterInfo.Count);
                }
                while (UsedDisasters.Contains(RandomIndex));
                {
                    UsedDisasters.Add(RandomIndex);
                }
            }
        }
        static void Main(string[] args)
        {
            List<string> UserInfo = File.ReadAllLines("User_Info.txt").ToList();
            List<string> DisasterInfo = File.ReadAllLines("Disaster_Info.txt").ToList();
            bool LoginMenu = true;
            bool UserLogin = false;
            string CurrentUser = "";

            while (LoginMenu)
            {
                Console.WriteLine("[1] Login");
                Console.WriteLine("[2] Register");
                Console.WriteLine("[3] Exit");
                Console.WriteLine();

                Console.Write("Choice: ");
                string Input = Console.ReadLine();

                int.TryParse(Input, out int MenuChoice);

                switch (MenuChoice)
                {
                    case 1:
                        Console.Clear();
                        (CurrentUser, UserLogin, LoginMenu) = Login(UserInfo, CurrentUser, UserLogin, LoginMenu);
                        break;
                    case 2:
                        Console.Clear();
                        UserInfo = Register(UserInfo);
                        break;
                    case 3:
                        Console.Clear();
                        LoginMenu = false;
                        break;
                    default:
                        Console.WriteLine();
                        Console.WriteLine("Enter A Valid Choice! Try Again.");
                        Console.WriteLine("Press Any Key To Try Again.");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                }
            }

            while (UserLogin)
            {
                Console.WriteLine("[1] Load Game");
                Console.WriteLine("[2] New Game");
                Console.WriteLine("[3] Logout");
                Console.WriteLine();

                Console.Write("Choice: ");
                string Input = Console.ReadLine();

                int.TryParse(Input, out int MenuChoice);

                switch (MenuChoice)
                {
                    case 1:
                        Console.Clear();
                        Game(DisasterInfo);
                        break;
                    case 2:
                        break;
                    case 3:
                        LoginMenu = true;
                        UserLogin = false;
                        break;
                    default:
                        Console.WriteLine();
                        Console.WriteLine("Enter A Valid Choice! Try Again.");
                        Console.WriteLine("Press Any Key To Try Again.");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                }
            }

            Console.WriteLine("Went Out Of Main Menu!");
            Console.ReadKey();
        }
    }
}
