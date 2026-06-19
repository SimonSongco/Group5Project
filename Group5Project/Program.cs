using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace Group5Project
{
    internal class Program
    {
        static List<string> UserInfo = new List<string>();
        static List<string> DisasterInfo = new List<string>();
        static string CurrentUser = "";
        static int TargetUserLevel = 1;
        static int Reputation = 40;

        static void PrintDarkHeader()
        {
            string header1 = @"
            ██████╗ ██╗   ██╗████████╗██████╗ ██████╗ ███████╗ █████╗ ██╗  ██╗
           ██╔═══██╗██║   ██║╚══██╔══╝██╔══██╗██╔══██╗██╔════╝██╔══██╗██║ ██╔╝
           ██║   ██║██║   ██║   ██║   ██████╔╝██████╔╝█████╗  ███████║█████╔╝ 
           ██║   ██║██║   ██║   ██║   ██╔══██╗██╔══██╗██╔══╝  ██╔══██║██╔═██╗ 
           ╚██████╔╝╚██████╔╝   ██║   ██████╔╝██║  ██║███████╗██║  ██║██║  ██╗
            ╚═════╝  ╚═════╝    ╚═╝   ╚═════╝ ╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝╚═╝  ╚═╝            
            ";
            string header2 = @"                                                      
                            ███████╗███████╗██████╗  ██████╗                        
                            ╚══███╔╝██╔════╝██╔══██╗██╔═══██╗                       
                              ███╔╝ █████╗  ██████╔╝██║   ██║                       
                             ███╔╝  ██╔══╝  ██╔══██╗██║   ██║                       
                            ███████╗███████╗██║  ██║╚██████╔╝                       
                            ╚══════╝╚══════╝╚═╝  ╚═╝ ╚═════╝                        
            ";
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(header1);

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(header2);
            Console.ResetColor();
        }

        static (string, bool, bool) Login(bool UserLogin, bool LoginMenu)
        {
            string m = new string(' ', 31);

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(m + "--------------------------");
                Console.WriteLine(m + "     ACCOUNT LOGIN        ");
                Console.WriteLine(m + "--------------------------");
                Console.ResetColor();
                Console.WriteLine(m + "  Press Enter to Go Back  ");
                Console.WriteLine();
                Console.Write(m + "Username: ");
                string InputUsername = Console.ReadLine();

                if (InputUsername == "") return (InputUsername, false, true);

                Console.Write(m + "Password: ");
                string InputPassword = Console.ReadLine();

                for (int i = 0; i < UserInfo.Count; i++)
                {
                    string[] parts = UserInfo[i].Split('|');

                    if (InputUsername == parts[0] && InputPassword == parts[1])
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            Console.Clear();
                            PrintDarkHeader();
                            Console.WriteLine(new string(' ', 39) + "Logging In.");
                            Thread.Sleep(200);
                            Console.Clear();
                            PrintDarkHeader();
                            Console.WriteLine(new string(' ', 39) + "Logging In..");
                            Thread.Sleep(200);
                            Console.Clear();
                            PrintDarkHeader();
                            Console.WriteLine(new string(' ', 39) + "Logging In...");
                            Thread.Sleep(200);
                            Console.Clear();
                        }
                        return (InputUsername, true, false);
                    }
                }

                Console.WriteLine();
                Console.WriteLine(m + "Invalid Credentials! Please Try Again.");
                Thread.Sleep(1000);
                Console.Clear();
                PrintDarkHeader();
            }
        }

        static void Register()
        {
            string m = new string(' ', 31);

            while (true)
            {
                bool UsernameTaken = false;

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(m + "--------------------------");
                Console.WriteLine(m + "     ACCOUNT REGISTER     ");
                Console.WriteLine(m + "--------------------------");
                Console.ResetColor();
                Console.WriteLine(m + "  Press Enter to Go Back  ");
                Console.WriteLine();
                Console.Write(m + "Username: ");
                string InputUsername = Console.ReadLine();

                if (InputUsername == "") return;

                for (int i = 0; i < UserInfo.Count; i++)
                {
                    string[] parts = UserInfo[i].Split('|');

                    if (InputUsername == parts[0])
                    {
                        Console.WriteLine();
                        Console.WriteLine(m + "Username Already Taken!");
                        Console.WriteLine(m + "Press Any Key To Try Again.");
                        Console.ReadKey();
                        Console.Clear();
                        PrintDarkHeader();
                        UsernameTaken = true;
                        break;
                    }
                }

                if (UsernameTaken) continue;

                Console.Write(m + "Password: ");
                string InputPassword = Console.ReadLine();

                Console.Write(m + "Confirm Password: ");
                string InputConfirmPassword = Console.ReadLine();

                Console.WriteLine();

                if (InputPassword == InputConfirmPassword)
                {
                    Console.WriteLine(m + "Account Created!");
                    Thread.Sleep(1000);
                    Console.Clear();

                    string NewUserInfo = $"\n{InputUsername}|{InputPassword}|1|40";
                    File.AppendAllText("User_Info.txt", NewUserInfo);

                    UserInfo = File.ReadAllLines("User_Info.txt").ToList();
                    return;
                }
                else
                {
                    Console.WriteLine(m + "Credentials Not Valid! Please Try Again.");
                    Thread.Sleep(1000);
                    Console.Clear();
                    PrintDarkHeader();
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
            string m = new string(' ', 20);
            string m2 = new string(' ', 29);

            Console.Clear();
            PrintDarkHeader();
            Console.WriteLine(m2 + "=== WARNING: START NEW GAME ===");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(m + "This will overwrite your existing level and progress.");
            Console.ResetColor();
            Console.WriteLine();
            Console.Write(m + "Are you sure you want to start a New Game? (Y/N): ");
            string ConfirmInput = Console.ReadLine().Trim().ToUpper();

            if (ConfirmInput == "Y")
            {
                for (int i = 0; i < UserInfo.Count; i++)
                {
                    string[] parts = UserInfo[i].Split('|');
                    if (parts[0] == CurrentUser)
                    {
                        string ResetUserLine = $"{parts[0]}|{parts[1]}|1|40";
                        UserInfo[i] = ResetUserLine;
                        break;
                    }
                }

                File.WriteAllLines("User_Info.txt", UserInfo);
                TargetUserLevel = 1;
                Reputation = 40;

                Console.WriteLine();
                Console.WriteLine(m + "Progress Reset! Loading Level 1...");
                Thread.Sleep(1000);
                Console.Clear();

                Game();
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine(m + "Action Canceled. Returning to Menu...");
                Thread.Sleep(1000);
                Console.Clear();
            }
        }

        static void Game()
        {
            bool Game = true;

            while (Game)
            {
                List<string> CurrentDisasters = new List<string>();
                int UserLineIndex = -1;
                int StartingReputation = 40;

                for (int i = 0; i < UserInfo.Count; i++)
                {
                    string[] parts = UserInfo[i].Split('|');
                    if (parts[0] == CurrentUser)
                    {
                        TargetUserLevel = int.Parse(parts[2]);
                        Reputation = int.Parse(parts[3]);
                        StartingReputation = Reputation;
                        UserLineIndex = i;
                        break;
                    }
                }

                string[] UserParts = UserInfo[UserLineIndex].Split('|');

                if (TargetUserLevel > 3)
                {
                    string m = new string(' ', 22);
                    Console.WriteLine(m + "You Have Already Completed All The Levels!");
                    Thread.Sleep(1500);
                    Console.Clear();
                    return;
                }

                if (UserParts.Length > 4)
                {
                    Console.WriteLine(new string(' ', 30) + "--> Loading existing save...");
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
                    string m = new string(' ', 22);
                    Console.Clear();
                    Console.WriteLine(m + "=== EMERGENCY DISPATCH CONTROL CENTER ===");
                    Console.WriteLine(m + $"Active Emergencies Left: {CurrentDisasters.Count}");
                    Console.Write(m + $"Current Reputation: ");

                    if (Reputation <= 40)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"{Reputation}%");
                        Console.ResetColor();
                    }
                    else if (Reputation <= 70)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"{Reputation}%");
                        Console.ResetColor();
                    }
                    else if (Reputation <= 100)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"{Reputation}%");
                        Console.ResetColor();
                    }

                    Console.WriteLine(m + "-----------------------------------------");
                    Console.WriteLine();

                    for (int i = 0; i < CurrentDisasters.Count; i++)
                    {
                        string[] parts = CurrentDisasters[i].Split('|');
                        Console.WriteLine(m + "[{0}] {1}", i + 1, parts[1]);
                        Console.WriteLine(m + "   Severity: {0,-6} | Location: {1}", parts[2], parts[3]);
                        Console.WriteLine();
                    }

                    Console.Write(m + "Select an emergency to respond to (or '0' to exit): ");
                    string ChoiceInput = Console.ReadLine();

                    if (ChoiceInput == "0")
                    {
                        Console.WriteLine();
                        Console.WriteLine(m + "Returning to Menu...");
                        Thread.Sleep(1000);
                        Console.Clear();
                        Game = false;
                        break;
                    }

                    if (int.TryParse(ChoiceInput, out int choice) && choice >= 1 && choice <= CurrentDisasters.Count)
                    {
                        string ChosenDisaster = CurrentDisasters[choice - 1];
                        string[] chosenParts = ChosenDisaster.Split('|');
                        bool SelectionValid = true;
                        string UnitChoice = "";
                        string m2 = new string(' ', 18);

                        while (SelectionValid)
                        {
                            Console.Clear();
                            Console.WriteLine(m2 + $"---> RESPONDING TO: {chosenParts[1]} <---");
                            Console.Write(m2 + "Details: ");
                            string[] words = chosenParts[4].Split(' ');

                            string currentLine = "";
                            int maxLen = 50;

                            foreach (string word in words)
                            {
                                if ((currentLine + word).Length > maxLen)
                                {
                                    Console.WriteLine(currentLine.TrimEnd());
                                    Console.Write(m2 + "         ");
                                    currentLine = "";
                                }
                                currentLine += word + " ";
                            }
                            if (currentLine.Length > 0)
                            {
                                Console.WriteLine(currentLine.TrimEnd());
                            }

                            Console.WriteLine(m2 + "--------------------------------------------------");
                            Console.WriteLine();

                            Console.WriteLine(m2 + "Choose the BEST department to dispatch:");
                            Console.WriteLine(m2 + "[1] Police");
                            Console.WriteLine(m2 + "[2] Firemen");
                            Console.WriteLine(m2 + "[3] First Aid");
                            Console.WriteLine(m2 + "[4] Rescue Team");
                            Console.WriteLine();
                            Console.Write(m2 + "Your Selection (1-4): ");
                            UnitChoice = Console.ReadLine();
                            Console.WriteLine();

                            if (UnitChoice == "" || !int.TryParse(UnitChoice, out int unitNum) || unitNum <= 0 || unitNum >= 5)
                            {
                                Console.WriteLine(m2 + "Enter a Valid Selection (1-4)!");
                                Thread.Sleep(750);
                                continue;
                            }
                            else
                            {
                                SelectionValid = false;
                            }
                        }

                        string CorrectUnit = chosenParts[5].Trim();
                        string[] UnitNames = { "Police", "Firemen", "First Aid", "Rescue Team" };
                        int correctIndex = int.Parse(CorrectUnit) - 1;
                        string CorrectUnitNames = UnitNames[correctIndex];

                        if (UnitChoice == CorrectUnit)
                        {
                            Reputation += 10;
                            if (Reputation > 100) Reputation = 100;

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(m2 + "SUCCESS! ");
                            Console.ResetColor();
                            Console.WriteLine("You deployed the correct emergency response unit.");
                            Console.WriteLine(m2 + "The area is secure. Removing emergency tracking card.");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine(m2 + "+10 Reputation");
                            Console.ResetColor();

                            CurrentDisasters.RemoveAt(choice - 1);
                        }
                        else
                        {
                            Reputation -= 5;
                            if (Reputation < 0) Reputation = 0;

                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(m2 + "FAILURE! ");
                            Console.ResetColor();
                            Console.WriteLine("You deployed an ineffective department.");
                            Console.WriteLine(m2 + $"The situation has deteriorated. The correct unit was: [{CorrectUnit}] {CorrectUnitNames}.");
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(m2 + "-5 Reputation");
                            Console.ResetColor();

                            CurrentDisasters.RemoveAt(choice - 1);
                        }

                        Console.WriteLine();
                        Console.WriteLine(m2 + "Press Any Key to Refresh Operational Status Board...");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine(m + "Invalid Dispatch Selection! Try Again.");
                        Thread.Sleep(750);
                    }
                }

                if (CurrentDisasters.Count == 0)
                {
                    string m = new string(' ', 22);
                    string[] parts = UserInfo[UserLineIndex].Split('|');
                    int RequiredRep = TargetUserLevel == 3 ? 90 : (TargetUserLevel == 2 ? 70 : 50);

                    Console.Clear();
                    Console.WriteLine(m + "=========================================");
                    Console.WriteLine(m + "             LEVEL RESULTS");
                    Console.WriteLine(m + "=========================================");
                    Console.Write(m + $"Current Reputation: ");

                    if (Reputation <= 40)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"{Reputation}%");
                        Console.ResetColor();
                    }
                    else if (Reputation <= 70)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"{Reputation}%");
                        Console.ResetColor();
                    }
                    else if (Reputation <= 100)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"{Reputation}%");
                        Console.ResetColor();
                    }


                    Console.Write(m + $"Required Reputation: ");

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{RequiredRep}%");
                    Console.ResetColor();

                    Console.WriteLine();

                    if (Reputation >= RequiredRep)
                    {
                        TargetUserLevel++;
                        string UpdatedUserLine = $"{parts[0]}|{parts[1]}|{TargetUserLevel}|{Reputation}";
                        UserInfo[UserLineIndex] = UpdatedUserLine;
                        File.WriteAllLines("User_Info.txt", UserInfo);

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(m + "PROMOTION SUCCESSFUL!");
                        Console.ResetColor();

                        if (TargetUserLevel > 3)
                        {
                            Console.WriteLine(m + "YOU HAVE COMPLETED ALL LEVELS!");
                            Console.ReadKey();
                            return;
                        }

                        Console.WriteLine(m + $"You are now Level {TargetUserLevel}!");
                    }
                    else
                    {
                        Reputation = StartingReputation;
                        string UpdatedUserLine = $"{parts[0]}|{parts[1]}|{TargetUserLevel}|{Reputation}";
                        UserInfo[UserLineIndex] = UpdatedUserLine;
                        File.WriteAllLines("User_Info.txt", UserInfo);

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(m + "PROMOTION FAILED!");
                        Console.ResetColor();
                        Console.WriteLine(m + "Your reputation is too low.");
                        Console.WriteLine(m + "Your reputation has been restored. Replay level to try again.");
                    }

                    Console.WriteLine();
                    Console.Write(m + "Would You Like to Proceed? (Y/N): ");
                    string Choice = Console.ReadLine();

                    if (Choice.ToUpper().Trim() == "Y")
                    {
                        continue;
                    }
                    else if (Choice.ToUpper().Trim() == "N")
                    {
                        Game = false;
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            if (!File.Exists("User_Info.txt")) File.Create("User_Info.txt").Close();
            if (!File.Exists("Disaster_Info.txt")) File.Create("Disaster_Info.txt").Close();

            UserInfo = File.ReadAllLines("User_Info.txt").ToList();
            DisasterInfo = File.ReadAllLines("Disaster_Info.txt").ToList();

            bool LoginMenu = true;
            bool UserLogin = false;
            bool Menu = true;

            string m = new string(' ', 38);

            while (Menu)
            {
                while (LoginMenu)
                {
                    Console.Clear();
                    PrintDarkHeader();
                    Console.WriteLine(m + "[1] Login");
                    Console.WriteLine(m + "[2] Register");

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(m + "[3] Exit");
                    Console.ResetColor();
                    Console.WriteLine();

                    Console.Write(m + "Choice: ");
                    string Input = Console.ReadLine();

                    int.TryParse(Input, out int MenuChoice);

                    switch (MenuChoice)
                    {
                        case 1:
                            Console.Clear();
                            PrintDarkHeader();
                            (CurrentUser, UserLogin, LoginMenu) = Login(UserLogin, LoginMenu);
                            break;
                        case 2:
                            Console.Clear();
                            PrintDarkHeader();
                            Register();
                            break;
                        case 3:
                            Console.Clear();
                            LoginMenu = false;
                            Menu = false;
                            break;
                        default:
                            Console.WriteLine();
                            Console.WriteLine(m + "Enter A Valid Choice! Try Again.");
                            Console.WriteLine(m + "Press Any Key To Try Again.");
                            Console.ReadKey();
                            break;
                    }
                }

                while (UserLogin)
                {
                    Console.Clear();
                    PrintDarkHeader();
                    Console.WriteLine(m + "[1] Load Game");
                    Console.WriteLine(m + "[2] New Game");
                    Console.WriteLine(m + "[3] Logout");
                    Console.WriteLine();

                    Console.Write(m + "Choice: ");
                    string Input = Console.ReadLine();

                    int.TryParse(Input, out int MenuChoice);

                    switch (MenuChoice)
                    {
                        case 1:
                            Console.Clear();
                            Game();
                            break;
                        case 2:
                            NewGame();
                            break;
                        case 3:
                            Console.Clear();
                            LoginMenu = true;
                            UserLogin = false;
                            PrintDarkHeader();
                            Console.WriteLine(m + $"Logged Out as {CurrentUser}");
                            Console.WriteLine();
                            Thread.Sleep(1000);
                            break;
                        default:
                            Console.WriteLine();
                            Console.WriteLine(m + "Enter A Valid Choice! Try Again.");
                            Console.WriteLine(m + "Press Any Key To Try Again.");
                            Console.ReadKey();
                            break;
                    }
                }
            }
        }
    }
}