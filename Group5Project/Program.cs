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
        static List<string> PerPlayerDisasterInfo = new List<string>();
        static string CurrentUser = "";
        static int TargetUserLevel = 1;
        static int Reputation = 0;

        static void PrintDarkHeader()
        {
            string header1 = "\n" +
            "\t      ██████╗ ██╗   ██╗████████╗██████╗ ██████╗ ███████╗ █████╗ ██╗  ██╗\n" +
            "\t     ██╔═══██╗██║   ██║╚══██╔══╝██╔══██╗██╔══██╗██╔════╝██╔══██╗██║ ██╔╝\n" +
            "\t     ██║   ██║██║   ██║   ██║   ██████╔╝██████╔╝█████╗  ███████║█████╔╝ \n" +
            "\t     ██║   ██║██║   ██║   ██║   ██╔══██╗██╔══██╗██╔══╝  ██╔══██║██╔═██╗ \n" +
            "\t     ╚██████╔╝╚██████╔╝   ██║   ██████╔╝██║  ██║███████╗██║  ██║██║  ██╗\n" +
            "\t      ╚═════╝  ╚═════╝    ╚═╝   ╚═════╝ ╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝╚═╝  ╚═╝  " +
            " ";

            string header2 = "" +
            "                            ███████╗███████╗██████╗  ██████╗                        \n" +
            "                            ╚══███╔╝██╔════╝██╔══██╗██╔═══██╗                       \n" +
            "                              ███╔╝ █████╗  ██████╔╝██║   ██║                       \n" +
            "                             ███╔╝  ██╔══╝  ██╔══██╗██║   ██║                       \n" +
            "                            ███████╗███████╗██║  ██║╚██████╔╝                       \n" +
            "                            ╚══════╝╚══════╝╚═╝  ╚═╝ ╚═════╝                        \n" +
            " ";

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
                Console.WriteLine(m + "      ACCOUNT LOGIN       ");
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
                            Console.Write(new string(' ', 39) + "Logging In");
                            Thread.Sleep(200);
                            Console.Write('.');
                            Thread.Sleep(200);
                            Console.Write('.');
                            Thread.Sleep(200);
                            Console.Write('.');
                            Thread.Sleep(200);
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

                if (InputPassword == "")
                {
                    Console.WriteLine();
                    Console.WriteLine(m + "Password Cannot Be Empty!");
                    Console.WriteLine(m + "Press Any Key To Try Again.");
                    Console.ReadKey();
                    Console.Clear();
                    PrintDarkHeader();
                    continue;
                }

                Console.Write(m + "Confirm Password: ");
                string InputConfirmPassword = Console.ReadLine();

                Console.WriteLine();

                if (InputPassword == InputConfirmPassword)
                {
                    Console.WriteLine(m + "Account Created!");
                    Thread.Sleep(1000);
                    Console.Clear();

                    string NewUserInfo = $"\n{InputUsername}|{InputPassword}|0|0|0";
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
            else if (UserLevel == 3) MaxDisasters = 6;
            else if (UserLevel == 4) MaxDisasters = 8;
            else if (UserLevel == 5) MaxDisasters = 10;

            List<string> previouslyUsedIDs = new List<string>();
            foreach (string line in PerPlayerDisasterInfo)
            {
                string[] parts = line.Split('|');
                if (parts[0] == CurrentUser)
                {
                    for (int i = 1; i < parts.Length; i++)
                    {
                        previouslyUsedIDs.Add(parts[i]);
                    }
                    break;
                }
            }

            int availableDisasters = DisasterInfo.Count - previouslyUsedIDs.Count;
            if (MaxDisasters > availableDisasters) MaxDisasters = availableDisasters;

            Random random = new Random();
            List<int> UsedIndexes = new List<int>();

            for (int i = 0; i < MaxDisasters; i++)
            {
                int randomIndex;
                string newDisasterID;
                do
                {
                    randomIndex = random.Next(0, DisasterInfo.Count);
                    newDisasterID = DisasterInfo[randomIndex].Split('|')[0];
                }
                while (UsedIndexes.Contains(randomIndex) || previouslyUsedIDs.Contains(newDisasterID));

                UsedIndexes.Add(randomIndex);
                CurrentDisasters.Add(DisasterInfo[randomIndex]);
            }

            return CurrentDisasters;
        }
        static void NewGame()
        {
            string m = new string(' ', 20);
            string m2 = new string(' ', 29);

            int currentUserLevel = 0;
            int accumulatedPoints = 0;

            for (int i = 0; i < UserInfo.Count; i++)
            {
                string[] parts = UserInfo[i].Split('|');
                if (parts[0] == CurrentUser)
                {
                    currentUserLevel = int.Parse(parts[2]);
                    if (parts.Length >= 5) int.TryParse(parts[4], out accumulatedPoints);
                    break;
                }
            }

            bool proceedWithNewGame = false;

            if (currentUserLevel == 0)
            {
                proceedWithNewGame = true;
            }
            else
            {
                Console.Clear();
                PrintDarkHeader();
                Console.WriteLine(m2 + "=== WARNING: START NEW GAME ===");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(m + "This will overwrite your level but preserve accumulated points.");
                Console.ResetColor();
                Console.WriteLine();
                Console.Write(m + "Are you sure you want to start a New Game? (Y/N): ");
                string ConfirmInput = Console.ReadLine().Trim().ToUpper();

                if (ConfirmInput == "Y")
                {
                    proceedWithNewGame = true;
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine(m + "Action Canceled. Returning to Menu...");
                    Thread.Sleep(1000);
                    Console.Clear();
                    return;
                }
            }

            if (proceedWithNewGame)
            {
                for (int i = 0; i < UserInfo.Count; i++)
                {
                    string[] parts = UserInfo[i].Split('|');
                    if (parts[0] == CurrentUser)
                    {
                        string ResetUserLine = $"{parts[0]}|{parts[1]}|1|0|{accumulatedPoints}";
                        UserInfo[i] = ResetUserLine;
                        break;
                    }
                }
                File.WriteAllLines("User_Info.txt", UserInfo);

                for (int i = 0; i < PerPlayerDisasterInfo.Count; i++)
                {
                    if (PerPlayerDisasterInfo[i].Split('|')[0] == CurrentUser)
                    {
                        PerPlayerDisasterInfo.RemoveAt(i);
                        break;
                    }
                }
                File.WriteAllLines("PerPlayer_Disaster_Info.txt", PerPlayerDisasterInfo);

                TargetUserLevel = 1;
                Reputation = 40;

                Console.WriteLine();
                if (currentUserLevel == 0)
                {
                    Console.Clear();
                    PrintDarkHeader();
                    Console.WriteLine(m + "       Starting First Game! Loading Level 1.");
                }
                else
                {
                    Console.WriteLine(m + "          Progress Reset! Loading Level 1.");
                }

                Thread.Sleep(1000);
                Console.Clear();

                Game();
            }
        }
        static void Game()
        {
            bool Game = true;

            while (Game)
            {
                List<string> CurrentDisasters = new List<string>();
                int UserLineIndex = -1;
                int StartingReputation = 0;
                int TotalPoints = 0;

                for (int i = 0; i < UserInfo.Count; i++)
                {
                    string[] parts = UserInfo[i].Split('|');
                    if (parts[0] == CurrentUser)
                    {
                        TargetUserLevel = int.Parse(parts[2]);
                        Reputation = int.Parse(parts[3]);
                        if (parts.Length >= 5) int.TryParse(parts[4], out TotalPoints);
                        StartingReputation = Reputation;
                        UserLineIndex = i;
                        break;
                    }
                }

                string[] UserParts = UserInfo[UserLineIndex].Split('|');
                string m = new string(' ', 22);

                if (TargetUserLevel > 5)
                {
                    Console.WriteLine(m + "You Have Already Completed All The Levels!");
                    Thread.Sleep(1500);
                    Console.Clear();
                    return;
                }

                if (UserParts.Length > 5)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Console.Clear();
                        PrintDarkHeader();
                        Console.Write(new string(' ', 33) + "Loading existing save");
                        Console.Write('.');
                        Thread.Sleep(200);
                        Console.Write('.');
                        Thread.Sleep(200);
                        Console.Write('.');
                        Thread.Sleep(200);
                    }

                    for (int i = 5; i < UserParts.Length; i++)
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
                    if (int.Parse(UserParts[2]) <= 1)
                    {
                        string m2 = new string(' ', 19);
                        Console.WriteLine("\n\n\n\n\n\n\n\n\n" + m2 + "[1] You must Dispatch the Best Unit for the Current Situation.");
                        Console.WriteLine(m2 + "[2] You must Match the required Reputation each level.");
                        Console.WriteLine(m2 + "[3] Each level increases required Reputation and decreases");
                        Console.WriteLine(m2 + "[4] Rep gain if you succeed and increases Rep loss when you");
                        Console.WriteLine(m2 + "[5] fail on a dispatch call.");
                        Console.WriteLine();
                        Console.WriteLine(m2 + "Press Enter to Continue to the Game.");
                        Console.ReadKey();

                        for (int j = 0; j < 3; j++)
                        {
                            Console.Clear();
                            PrintDarkHeader();
                            Console.Write(new string(' ', 34) + "Generating Disasters");
                            Thread.Sleep(200);
                            Console.Write('.');
                            Thread.Sleep(200);
                            Console.Write('.');
                            Thread.Sleep(200);
                            Console.Write('.');
                            Thread.Sleep(200);
                        }
                    }

                    CurrentDisasters = DisasterGenerator(CurrentDisasters, TargetUserLevel);

                    List<string> GeneratedIDs = new List<string>();
                    foreach (string disaster in CurrentDisasters)
                    {
                        GeneratedIDs.Add(disaster.Split('|')[0]);
                    }

                    string updatedUserLine = $"{UserParts[0]}|{UserParts[1]}|{UserParts[2]}|{UserParts[3]}|{TotalPoints}|" + string.Join("|", GeneratedIDs);
                    UserInfo[UserLineIndex] = updatedUserLine;

                    File.WriteAllLines("User_Info.txt", UserInfo);
                }

                int RequiredRep = 100;
                if (TargetUserLevel == 2) RequiredRep = 60;
                else if (TargetUserLevel == 3) RequiredRep = 75;
                else if (TargetUserLevel == 4) RequiredRep = 90;
                else if (TargetUserLevel == 5) RequiredRep = 100;

                int PointsGainedThisLevel = 0;
                int DisastersSolvedThisLevel = 0;

                while (CurrentDisasters.Count > 0)
                {
                    int gainAmount = 0;
                    int loseAmount = 0;

                    switch (TargetUserLevel)
                    {
                        case 1: gainAmount = 35; loseAmount = 20; break;
                        case 2: gainAmount = 20; loseAmount = 20; break;
                        case 3: gainAmount = 20; loseAmount = 25; break;
                        case 4: gainAmount = 15; loseAmount = 35; break;
                        case 5: gainAmount = 10; loseAmount = 40; break;
                    }

                    Console.Clear();
                    Console.WriteLine(m + "=== EMERGENCY DISPATCH CONTROL CENTER ===");
                    Console.WriteLine(m + $"Current Level: {TargetUserLevel} | Points Earned So Far: {TotalPoints}");
                    Console.WriteLine(m + $"Active Emergencies Left: {CurrentDisasters.Count}");
                    Console.Write(m + "Current Reputation: ");

                    if (Reputation <= RequiredRep * .50)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"{Reputation}%");
                        Console.ResetColor();
                    }
                    else if (Reputation <= RequiredRep * .70)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"{Reputation}%");
                        Console.ResetColor();
                    }
                    else if (Reputation >= RequiredRep)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"{Reputation}%");
                        Console.ResetColor();
                    }

                    Console.Write(m + $"Required Reputation: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{RequiredRep}%");
                    Console.ResetColor();

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
                        for (int i = 0; i < 3; i++)
                        {
                            string m2 = new string(' ', 36);
                            Console.Clear();
                            PrintDarkHeader();
                            Console.Write(m2 + "Returning to Menu");
                            Thread.Sleep(200);
                            Console.Write('.');
                            Thread.Sleep(200);
                            Console.Write('.');
                            Thread.Sleep(200);
                            Console.Write('.');
                        }
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
                            Reputation += gainAmount;
                            if (Reputation > 100) Reputation = 100;

                            PointsGainedThisLevel += TargetUserLevel;
                            DisastersSolvedThisLevel++;

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(m2 + "SUCCESS! ");
                            Console.ResetColor();
                            Console.WriteLine("You deployed the correct emergency response unit.");
                            Console.WriteLine(m2 + "The area is secure. Removing emergency tracking card.");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine(m2 + $"+{gainAmount}% Reputation");
                            Console.ResetColor();

                            CurrentDisasters.RemoveAt(choice - 1);
                        }
                        else
                        {
                            Reputation -= loseAmount;
                            if (Reputation < 0) Reputation = 0;

                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(m2 + "FAILURE! ");
                            Console.ResetColor();
                            Console.WriteLine("You deployed an ineffective department.");
                            Console.WriteLine(m2 + $"The situation has deteriorated. The correct unit was: [{CorrectUnit}] {CorrectUnitNames}.");
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(m2 + $"-{loseAmount}% Reputation");
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
                    string[] parts = UserInfo[UserLineIndex].Split('|');

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
                        TotalPoints += PointsGainedThisLevel;

                        string[] currentUserParts = UserInfo[UserLineIndex].Split('|');
                        List<string> newlyUsedIDs = new List<string>();

                        for (int i = 5; i < currentUserParts.Length; i++)
                        {
                            newlyUsedIDs.Add(currentUserParts[i]);
                        }

                        int playerRecordIndex = -1;
                        for (int i = 0; i < PerPlayerDisasterInfo.Count; i++)
                        {
                            if (PerPlayerDisasterInfo[i].Split('|')[0] == CurrentUser)
                            {
                                playerRecordIndex = i;
                                break;
                            }
                        }

                        if (playerRecordIndex != -1)
                        {
                            if (newlyUsedIDs.Count > 0)
                                PerPlayerDisasterInfo[playerRecordIndex] += "|" + string.Join("|", newlyUsedIDs);
                        }
                        else
                        {
                            if (newlyUsedIDs.Count > 0)
                                PerPlayerDisasterInfo.Add(CurrentUser + "|" + string.Join("|", newlyUsedIDs));
                        }

                        File.WriteAllLines("PerPlayer_Disaster_Info.txt", PerPlayerDisasterInfo);

                        TargetUserLevel++;

                        string UpdatedUserLine = $"{parts[0]}|{parts[1]}|{TargetUserLevel}|{Reputation}|{TotalPoints}";
                        UserInfo[UserLineIndex] = UpdatedUserLine;
                        File.WriteAllLines("User_Info.txt", UserInfo);

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(m + "PROMOTION SUCCESSFUL!");
                        Console.ResetColor();

                        Console.WriteLine(m + $"Disasters Solved: {DisastersSolvedThisLevel}");
                        Console.WriteLine(m + $"Points Earned: {PointsGainedThisLevel}");
                        Console.WriteLine(m + $"Total Accumulated Points: {TotalPoints}");
                        Console.WriteLine();

                        if (TargetUserLevel > 5)
                        {
                            PrintDarkHeader();
                            Console.WriteLine($"{m}YOU HAVE COMPLETED ALL THE LEVELS {CurrentUser}!");
                            Console.WriteLine($"{m}Start a New Game to keep grinding for the Leaderboards!");
                            Console.WriteLine(new string(' ', 27) + "Press Any Key To Go To The Main Menu.");
                            Console.ReadKey();

                            return;
                        }

                        Console.WriteLine(m + $"You are now Level {TargetUserLevel}!");
                    }
                    else
                    {
                        Reputation = StartingReputation;

                        string UpdatedUserLine = $"{parts[0]}|{parts[1]}|{TargetUserLevel}|{Reputation}|{TotalPoints}";
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
                        Console.Clear();
                        PrintDarkHeader();
                        Console.WriteLine(new string(' ', 31) + "Returning to Menu.");
                        Thread.Sleep(1000);
                        Game = false;
                    }
                }
            }
        }
        static void Leaderboard()
        {
            Console.Clear();
            PrintDarkHeader();

            string m = new string(' ', 28);

            Console.WriteLine(m + "===== LEADERBOARD =====\n");

            List<(string username, int points)> players = new List<(string, int)>();

            foreach (string user in UserInfo)
            {
                string[] parts = user.Split('|');

                if (parts.Length >= 5)
                {
                    string username = parts[0];
                    int points = 0;
                    int.TryParse(parts[4], out points);

                    players.Add((username, points));
                }
            }

            players = players.OrderByDescending(p => p.points).ToList();

            Console.WriteLine(m + "Rank   Player            Total Points ");
            Console.WriteLine(m + "--------------------------------------");

            int rank = 1;

            foreach (var player in players)
            {
                Console.WriteLine(m + $"{rank,-6} {player.username,-17} {player.points}");
                rank++;

                if (rank > 10) break;
            }

            Console.WriteLine();
            Console.WriteLine(m + "Press any key to return...");
            Console.ReadKey();
        }
        static void Main(string[] args)
        {
            if (!File.Exists("User_Info.txt")) File.Create("User_Info.txt").Close();
            if (!File.Exists("Disaster_Info.txt")) File.Create("Disaster_Info.txt").Close();
            if (!File.Exists("PerPlayer_Disaster_Info.txt")) File.Create("PerPlayer_Disaster_Info.txt").Close();

            UserInfo = File.ReadAllLines("User_Info.txt").ToList();
            DisasterInfo = File.ReadAllLines("Disaster_Info.txt").ToList();
            PerPlayerDisasterInfo = File.ReadAllLines("PerPlayer_Disaster_Info.txt").ToList();

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
                            Thread.Sleep(750);
                            break;
                    }
                }

                while (UserLogin)
                {
                    Console.Clear();
                    PrintDarkHeader();
                    Console.WriteLine(m + "[1] Load Game");
                    Console.WriteLine(m + "[2] New Game");
                    Console.WriteLine(m + "[3] Leaderboard");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(m + "[4] Logout");
                    Console.WriteLine();
                    Console.ResetColor();

                    Console.Write(m + "Choice: ");
                    string Input = Console.ReadLine();

                    int.TryParse(Input, out int MenuChoice);

                    switch (MenuChoice)
                    {
                        case 1:
                            int checkLevel = 0;
                            foreach (var user in UserInfo)
                            {
                                string[] parts = user.Split('|');
                                if (parts[0] == CurrentUser)
                                {
                                    checkLevel = int.Parse(parts[2]);
                                    break;
                                }
                            }

                            if (checkLevel == 0)
                            {
                                Console.WriteLine();
                                Console.WriteLine(new string(' ', 22) + "No save data found. Please start a New Game!");
                                Thread.Sleep(1500);
                            }
                            else
                            {
                                Console.Clear();
                                Game();
                            }
                            break;
                        case 2:
                            NewGame();
                            break;
                        case 3:
                            Leaderboard();
                            break;
                        case 4:
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
                            Thread.Sleep(750);
                            break;
                    }
                }
            }
        }
    }
}
