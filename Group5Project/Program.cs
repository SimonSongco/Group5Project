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
        static List<string> UserInfo = new List<string>();
        static List<string> DisasterInfo = new List<string>();
        static string CurrentUser = "";
        static int TargetUserLevel = 1;

        static (string, bool, bool) Login(bool UserLogin, bool LoginMenu)
        {
            while (true)
            {
                Console.Write("Username: ");
                string InputUsername = Console.ReadLine();

                Console.Write("Password: ");
                string InputPassword = Console.ReadLine();

                for (int i = 0; i < UserInfo.Count; i++)
                {
                    string[] parts = UserInfo[i].Split('|');

                    if (InputUsername == parts[0] && InputPassword == parts[1])
                    {
                        return (InputUsername, true, false);
                    }
                }

                Console.WriteLine();
                Console.WriteLine("Invalid Credentials!");
                Console.WriteLine("Press Any Key To Try Again.");
                Console.ReadKey();
                Console.Clear();
            }
        }
        static void Register()
        {
            while (true)
            {
                bool UsernameTaken = false;

                Console.Write("Username: ");
                string InputUsername = Console.ReadLine();

                for (int i = 0; i < UserInfo.Count; i++)
                {
                    string[] parts = UserInfo[i].Split('|');

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

                    string NewUserInfo = $"\n{InputUsername}|{InputPassword}|1";
                    File.AppendAllText("User_Info.txt", NewUserInfo);

                    UserInfo = File.ReadAllLines("User_Info.txt").ToList();
                    return;
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
        static List<string> DisasterGenerator(List<string> CurrentDisasters, int UserLevel)
        {
            int MaxDisasters = 0;

            if (UserLevel == 1) MaxDisasters = 3;
            else if (UserLevel == 2) MaxDisasters = 5;
            else if (UserLevel == 3) MaxDisasters = 7;
            else MaxDisasters = 3;

            Random random = new Random();
            List<int> UsedIndexes = new List<int>();

            for (int i = 0; i < MaxDisasters; i++)
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

            return CurrentDisasters;
        }
        static void NewGame()
        {
            Console.Clear();
            Console.WriteLine("=== WARNING: START NEW GAME ===");
            Console.WriteLine("This will overwrite your existing level and progress.");
            Console.Write("Are you sure you want to start a New Game? (Y/N): ");
            string ConfirmInput = Console.ReadLine().Trim().ToUpper();

            if (ConfirmInput == "Y")
            {
                for (int i = 0; i < UserInfo.Count; i++)
                {
                    string[] parts = UserInfo[i].Split('|');
                    if (parts[0] == CurrentUser)
                    {
                        string ResetUserLine = $"{parts[0]}|{parts[1]}|1";
                        UserInfo[i] = ResetUserLine;
                        break;
                    }
                }

                File.WriteAllLines("User_Info.txt", UserInfo);
                TargetUserLevel = 1;

                Console.WriteLine("\nProgress reset! Loading Level 1...");
                Thread.Sleep(1000);
                Console.Clear();

                Game();
            }
            else
            {
                Console.WriteLine("\nAction canceled. Returning to menu...");
                Thread.Sleep(1000);
                Console.Clear();
            }
        }
        static void Game()
        {
            List<string> CurrentDisasters = new List<string>();

            int CorrectAnswers = 0;
            int TotalAnswers = 0;

            int UserLineIndex = -1;

            for (int i = 0; i < UserInfo.Count; i++)
            {
                string[] parts = UserInfo[i].Split('|');
                if (parts[0] == CurrentUser)
                {
                    TargetUserLevel = int.Parse(parts[2]);
                    UserLineIndex = i;
                    break;
                }
            }

            string[] UserParts = UserInfo[UserLineIndex].Split('|');

            if (TargetUserLevel > 3)
            {
                Console.WriteLine("You Have Already Completed All The Levels");
                Console.WriteLine("Press Any Key to Go Back");
                Console.ReadKey();
                Console.Clear();
                return;
            }

            if (UserParts.Length > 3)
            {
                Console.WriteLine("--> Loading existing save...");
                Thread.Sleep(800);

                for (int i = 3; i < UserParts.Length; i++)
                {
                    string SavedDisasterID = UserParts[i].Trim();
                    string MatchedDisaster = null;

                    foreach (string DisasterLine in DisasterInfo)
                    {
                        string[] DisasterParts = DisasterLine.Split('|');

                        if (DisasterParts[0] == SavedDisasterID)
                        {
                            MatchedDisaster = DisasterLine;
                            break;
                        }
                    }

                    if (MatchedDisaster != null)
                    {
                        CurrentDisasters.Add(MatchedDisaster);
                    }
                }
            }
            else
            {
                CurrentDisasters = DisasterGenerator(CurrentDisasters, TargetUserLevel);

                List<string> GeneratedIDs = new List<string>();
                foreach (string disaster in CurrentDisasters)
                {
                    GeneratedIDs.Add(disaster.Split('|')[0]);
                }

                string updatedUserLine = UserInfo[UserLineIndex] + "|" + string.Join("|", GeneratedIDs);
                UserInfo[UserLineIndex] = updatedUserLine;

                File.WriteAllLines("User_Info.txt", UserInfo);
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

                    TotalAnswers++;

                    if (UnitChoice == CorrectUnit)
                    {
                        CorrectAnswers++;

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
                string[] parts = UserInfo[UserLineIndex].Split('|');

                double Reputation = ((double)CorrectAnswers / TotalAnswers) * 100;

                int RequiredRep = 50;

                if (TargetUserLevel == 2)
                    RequiredRep = 70;
                else if (TargetUserLevel == 3)
                    RequiredRep = 90;

                Console.Clear();
                Console.WriteLine("=========================================");
                Console.WriteLine("             LEVEL RESULTS");
                Console.WriteLine("=========================================");
                Console.WriteLine($"Correct Answers : {CorrectAnswers}");
                Console.WriteLine($"Total Answers   : {TotalAnswers}");
                Console.WriteLine($"Reputation      : {Reputation:F1}%");
                Console.WriteLine($"Required        : {RequiredRep}%");
                Console.WriteLine();

                if (Reputation >= RequiredRep)
                {
                    TargetUserLevel++;

                    string UpdatedUserLine = $"{parts[0]}|{parts[1]}|{TargetUserLevel}";
                    UserInfo[UserLineIndex] = UpdatedUserLine;

                    File.WriteAllLines("User_Info.txt", UserInfo);

                    if (TargetUserLevel > 3)
                    {
                        Console.WriteLine("=========================================");
                        Console.WriteLine("CONGRATULATIONS!");
                        Console.WriteLine("YOU HAVE COMPLETED ALL LEVELS!");
                        Console.WriteLine("=========================================");
                        Console.WriteLine("Press Any Key To Continue...");
                        Console.ReadKey();
                        Console.Clear();
                        return;
                    }

                    Console.WriteLine($"PROMOTION SUCCESSFUL!");
                    Console.WriteLine($"You advanced to Level {TargetUserLevel}!");
                }
                else
                {
                    Console.WriteLine("PROMOTION FAILED!");
                    Console.WriteLine("Your reputation is too low.");
                    Console.WriteLine("You must replay the level.");

                    string UpdatedUserLine = $"{parts[0]}|{parts[1]}|{TargetUserLevel}";
                    UserInfo[UserLineIndex] = UpdatedUserLine;

                    File.WriteAllLines("User_Info.txt", UserInfo);
                }

                Console.WriteLine();
                Console.WriteLine("[1] Play Again");
                Console.WriteLine("[2] Main Menu");
                Console.WriteLine();

                while (true)
                {
                    Console.Write("Choice: ");
                    string postGameChoice = Console.ReadLine();

                    if (postGameChoice == "1")
                    {
                        Console.Clear();
                        Game();
                        return;
                    }
                    else if (postGameChoice == "2")
                    {
                        Console.Clear();
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice!");
                    }
                }
            }
        }
        static void Main(string[] args)
        {
            UserInfo = File.ReadAllLines("User_Info.txt").ToList();
            DisasterInfo = File.ReadAllLines("Disaster_Info.txt").ToList();
            bool LoginMenu = true;
            bool UserLogin = false;

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
                        (CurrentUser, UserLogin, LoginMenu) = Login(UserLogin, LoginMenu);
                        break;
                    case 2:
                        Console.Clear();
                        Register();
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
                        Game();
                        break;
                    case 2:
                        Console.Clear();
                        NewGame();
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