using System.Diagnostics;
using static MainWindow;
using static Values;
public class Launcher
{
    public static async Task AddVersion()
    {
        Console.Clear();
        Question("Enter a name to identify the Fortnite Build: ", false);
        string Version_Name = Console.ReadLine();
        string Version_Path;

        var Existing_Line = File.Exists(Credentials) ? File.ReadAllLines(Credentials).ToList() : new List<string>();
        bool Existing_Version = Existing_Line.Any(line => line.StartsWith(Version_Name + "="));

        if (Existing_Version)
        {
            Question("A build with this name already exists. Would you like to overwrite it? (Yes/No): ", false);
            string Answer = Console.ReadLine();

            if (Answer.ToLower() != "yes")
            {
                await Return("Name already used. Please choose a different name.", false);
                return;
            }
        }

        Question($"Specify the gamepath for the {Version_Name} Build: ", false);

        Version_Path = Console.ReadLine();

        if (!Directory.Exists(Path.Combine(Version_Path, "FortniteGame")) || !Directory.Exists(Path.Combine(Version_Path, "Engine")))
        {
            await Return("Please select the correct Fortnite path that contains 'FortniteGame' and 'Engine' folders.", false);
            return;
        }

        Existing_Line.RemoveAll(Line => Line.StartsWith(Version_Name + "="));
        Existing_Line.Add($"{Version_Name}={Version_Path}");

        File.WriteAllLines(Credentials, Existing_Line);

        Success("Version added/modified successfully.", true);
    }

    public static async Task Launch()
    {
        try
        {
            var Configuration = File.Exists(Credentials) ? File.ReadAllLines(Credentials).ToList() : new List<string>();

            string Email = GetString(Configuration, "Email");
            string Password = GetString(Configuration, "Password");

            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password)) {
                await Return("Email and/or password missing. Please set your account credentials.", false);
                return;
            }

            var Versions = GetVersions(Configuration);

            if (!Versions.Any()) {
                await Return("No Fortnite Builds are set. Please add a build to your configuration.", false);
                return;
            }

            Console.WriteLine("\nFortnite Builds:\n");
            for (int i = 0; i < Versions.Count; i++)
                Console.WriteLine($"* [{i + 1}] {Versions[i]}");

            Question("Select the version number to launch: ", true);

            if (int.TryParse(Console.ReadLine(), out int Build) && Build > 0 && Build <= Versions.Count) 
            {
                bool LaunchMultipleGames = GetBool(Configuration, "LaunchMultipleGames");
                bool CloseAfterLaunch = GetBool(Configuration, "CloseAfterLaunch");

                int Processes = 1;
                if (LaunchMultipleGames)
                {
                    Question("Enter the number of instances to launch: ", false);
                    if (!int.TryParse(Console.ReadLine(), out Processes) || Processes <= 0)
                        Processes = 1;
                }

                for (int i = 0; i < Processes; i++)
                {
                    string GamePath = Configuration.First(Line => Line.StartsWith(Versions[Build - 1] + "=")).Split('=')[1];
                    Process FortniteLauncher = FNProc.Launch($"{GamePath}\\FortniteGame\\Binaries\\Win64\\FortniteLauncher.exe", true, "");
                    Process FortniteClientBE = FNProc.Launch($"{GamePath}\\FortniteGame\\Binaries\\Win64\\FortniteClient-Win64-Shipping_BE.exe", true, "");
                    Process FortniteClientEAC = FNProc.Launch($"{GamePath}\\FortniteGame\\Binaries\\Win64\\FortniteClient-Win64-Shipping_EAC.exe", true, "");
                    Process FortniteClient64 = FNProc.Launch($"{GamePath}\\FortniteGame\\Binaries\\Win64\\FortniteClient-Win64-Shipping.exe", false, $"-AUTH_LOGIN={Email} -AUTH_PASSWORD={Password} -nobe -fromfl=eac -fltoken=5d67b47b83ad5793a18a5746 -AUTH_TYPE=epic -epicapp=Fortnite -epicenv=Prod -epiclocale=en-us -epicportal -skippatchcheck -nobe -fromfl=eac -fltoken=5d67b47b83ad5793a18a5746");
                    Print($"{Project.Name} has been successfully launched.", false);
                }

                if (!CloseAfterLaunch)
                    Thread.Sleep(Timeout.Infinite);
                else
                    Thread.Sleep(30000);
                    Environment.Exit(0);
            }
        }
        catch (Exception Error)
        {
            if (Error.Message.Contains("An error occurred trying to start process"))
                await Return($"{Project.Name} Launcher supports only Chapter 1 Season 3 and above. If your build is compatible but still encountering issues, it may be corrupted, and you should consider reinstalling.", true);
        }
    }

    public static async Task RemoveVersion()
    {
        Console.Clear();
        var Lines = File.ReadAllLines(Credentials).ToList();

        var Configuration = File.Exists(Credentials) ? File.ReadAllLines(Credentials).ToList() : new List<string>();
        var Versions = GetVersions(Configuration);

        if (!Versions.Any()){
            await Return("No versions available to remove.", false);
            return;
        }

        Console.WriteLine("Available Fortnite Versions:\n");

        for (int i = 0; i < Versions.Count; i++)
            Console.WriteLine($"* [{i + 1}] {Versions[i]}");

        Console.WriteLine();
        Question("Enter the number of the version to remove: ", false);


        if (int.TryParse(Console.ReadLine(), out int Index) && Index > 0 && Index <= Versions.Count)
        {
            var VersionToRemove = Lines.First(Line => Line.StartsWith(Versions[Index - 1] + "="));
            Lines.Remove(VersionToRemove);

            File.WriteAllLines(Credentials, Lines);

            Success("Version removed successfully.", true);
        }
    }

    public static async Task ModifyAccountDetails()
    {
    Retry:

        try
        {
            Console.Clear();
            Print("Enter your Email: ", false);
            string AUTH_LOGIN = Console.ReadLine();

            Print("Enter your Password: ", false);
            string AUTH_PASSWORD = Console.ReadLine();

            Question("Are you sure these are your correct details? (Yes/No) ", false);
            string Answer = Console.ReadLine();

            if (Answer.ToLower() == "no")
            {
                await Return("Please re-enter your credentials correctly.", false);
                goto Retry;
            }
            var NewDetails = File.Exists(Credentials) ? File.ReadAllLines(Credentials).ToList() : new List<string>();

            NewDetails.RemoveAll(Line => Line.StartsWith("Email="));
            NewDetails.RemoveAll(Line => Line.StartsWith("Password="));

            NewDetails.Add($"Email={AUTH_LOGIN}");
            NewDetails.Add($"Password={AUTH_PASSWORD}");

            File.WriteAllLines(Credentials, NewDetails);

            Success("Account details modified successfully.", true);
        }
        catch (Exception ex)
        {
            Print($"Whoops! Something went wrong. We've encountered an issue on our end. Please try reopening the {Project.Name} Launcher.", true);
        }
    }

    public static async Task Options()
    {
    Retry:
        try
        {
            Console.Clear();

            var Configuration = File.Exists(Credentials) ? File.ReadAllLines(Credentials).ToList() : new List<string> 
            { 
                "LaunchMultipleGames=false", 
                "CloseAfterLaunch=false" 
            };

            bool Config_1 = GetBool(Configuration, "LaunchMultipleGames");
            bool Config_2 = GetBool(Configuration, "CloseAfterLaunch");

            Console.Write("[1] Launch Multiple Games: ");
            Status(Config_1);

            Console.Write("[2] Close after Launching Fortnite: ");
            Status(Config_2);

            Question("Select an option to toggle: ", false);

            switch (Console.ReadLine())
            {
                case "1": Config_1 = !Config_1; UpdateSetting(Configuration, "LaunchMultipleGames", Config_1); break;
                case "2": Config_2 = !Config_2; UpdateSetting(Configuration, "CloseAfterLaunch", Config_2); break;
                default: goto Retry;
            }

            File.WriteAllLines(Credentials, Configuration);
            Success("Option updated successfully.", true);
        }
        catch (Exception ex)
        {
            await Return($"Whoops! Something went wrong. We've encountered an issue on our end. Please try reopening the {Project.Name} Launcher.", true);
        }
    }
}
