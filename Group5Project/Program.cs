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
        static void Game(List<string> DisasterInfo)
        {
            Random random = new Random();
            List<int> UsedIndexes = new List<int>();
            List<string> CurrentDisasters = new List<string>();

            for (int i = 0; i < 3; i++)
            {
                int randomIndex;
                do
                {
                    randomIndex = random.Next(0, DisasterInfo.Count);
                }
                while (UsedIndexes.Contains(randomIndex));

                UsedIndexes.Add(randomIndex);
                CurrentDisasters.Add(DisasterInfo[randomIndex]);
            }

            while (CurrentDisasters.Count > 0)
            {
                Console.Clear();
                Console.WriteLine("=== EMERGENCY DISPATCH CONTROL CENTER ===");
                Console.WriteLine($"Active Emergencies Left: {CurrentDisasters.Count}");
                Console.WriteLine("-----------------------------------------");
                Console.WriteLine();

                for (int i = 0; i < CurrentDisasters.Count; i++)
                {
                    string[] parts = CurrentDisasters[i].Split('|');
                    Console.WriteLine("[{0}] {1}", i + 1, parts[1]);
                    Console.WriteLine("    Severity: {0,-6} | Location: {1}", parts[2], parts[3]);
                    Console.WriteLine();
                }

                Console.Write("Select an emergency to respond to (or type '0' to go back to menu): ");
                string ChoiceInput = Console.ReadLine();

                if (ChoiceInput == "0")
                {
                    Console.WriteLine("\nReturning to menu...");
                    Thread.Sleep(1000);
                    Console.Clear();
                    break;
                }

                if (int.TryParse(ChoiceInput, out int choice) && choice >= 1 && choice <= CurrentDisasters.Count)
                {
                    string ChosenDisaster = CurrentDisasters[choice - 1];
                    string[] chosenParts = ChosenDisaster.Split('|');

                    Console.Clear();
                    Console.WriteLine($"---> RESPONDING TO: {chosenParts[1]} <---");
                    Console.WriteLine($"Details: {chosenParts[4]}");
                    Console.WriteLine("--------------------------------------------------");
                    Console.WriteLine();

                    Console.WriteLine("Choose the BEST department to dispatch:");
                    Console.WriteLine("[1] Police");
                    Console.WriteLine("[2] Firemen");
                    Console.WriteLine("[3] Healthcare");
                    Console.WriteLine("[4] First Aid");
                    Console.WriteLine("[5] Rescue Team");
                    Console.WriteLine();
                    Console.Write("Your Selection (1-5): ");
                    string UnitChoice = Console.ReadLine();
                    Console.WriteLine();

                    string CorrectUnit = chosenParts[5].Trim();

                    string[] UnitNames = { "Police", "Firemen", "Healthcare", "First Aid", "Rescue Team" };

                    int correctIndex = int.Parse(CorrectUnit) - 1;
                    string CorrectUnitNames = UnitNames[correctIndex];

                    if (UnitChoice == CorrectUnit)
                    {
                        Console.WriteLine("SUCCESS! You deployed the correct emergency response unit.");
                        Console.WriteLine("The area is secure. Removing emergency tracking card.");

                        CurrentDisasters.RemoveAt(choice - 1);
                    }
                    else
                    {
                        Console.WriteLine("FAILURE! You deployed an ineffective department.");
                        Console.WriteLine($"The situation has deteriorated. The correct unit was: [{CorrectUnit}] {CorrectUnitNames}.");

                        CurrentDisasters.RemoveAt(choice - 1);
                    }

                    Console.WriteLine();
                    Console.WriteLine("Press any key to refresh operational status board...");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("\nInvalid dispatch selection! Try again.");
                    Thread.Sleep(1200);
                }
            }

            if (CurrentDisasters.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("=========================================");
                Console.WriteLine("MISSION COMPLETE: All sectors clear!");
                Console.WriteLine("=========================================");
                Console.WriteLine("Press any key to go back to main menu.");
                Console.ReadKey();
                Console.Clear();
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
                Console.Clear();
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
                        Console.Clear();
                        Game(DisasterInfo);
                        break;
                    case 3:
                        Console.Clear();
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