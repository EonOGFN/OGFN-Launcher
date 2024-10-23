using System.Diagnostics;
using static MainWindow;
public class Launcher
{
    public static async Task AddVersion()
    {
        Console.Clear();
        Question("Enter a name to identify this Fortnite build: ");
        string Version_Name = Console.ReadLine();
        string Version_Path;

        var Existing_Line = File.Exists(Credentials) ? File.ReadAllLines(Credentials).ToList() : new List<string>();
        bool Existing_Version = Existing_Line.Any(line => line.StartsWith(Version_Name + "="));

        if (Existing_Version)
        {
            Question("A build with this name already exists. Would you like to overwrite it? (Yes/No): ");
            string Answer = Console.ReadLine();

            if (Answer.ToLower() != "yes")
            {
                await Return("Name already used. Please choose a different name.");
                return;
            }
        }

        Question("Enter the path to your Fortnite build: ");
        Version_Path = Console.ReadLine();

        if (!Directory.Exists(Path.Combine(Version_Path, "FortniteGame")) || !Directory.Exists(Path.Combine(Version_Path, "Engine")))
        {
            await Return("Please select the correct Fortnite path that contains 'FortniteGame' and 'Engine' folders.");
            return;
        }

        Existing_Line.RemoveAll(Line => Line.StartsWith(Version_Name + "="));
        Existing_Line.Add($"{Version_Name}={Version_Path}");

        File.WriteAllLines(Credentials, Existing_Line);

        Success("Version added/modified successfully.");
    }

    public static async Task Launch()
    {
        var Lines = File.ReadAllLines(Credentials).ToList();

        var Versions = Lines
            .Where(Line => !Line.StartsWith("Email=") && !Line.StartsWith("Password="))
            .Select(Line => Line.Split('=')[0])
            .ToList();

        Console.WriteLine("\nFortnite Builds:\n");

        for (int i = 0; i < Versions.Count; i++)
        {
            Console.WriteLine($"* [{i + 1}] {Versions[i]}");
        }

        Console.WriteLine();
        Question("Enter the number of the version to launch: ");

        if (int.TryParse(Console.ReadLine(), out int Build) && Build > 0 && Build <= Versions.Count)
        {
            string GamePath = Lines.First(line => line.StartsWith(Versions[Build - 1] + "=")).Split('=')[1];
            string Email = Lines.First(line => line.StartsWith("Email=")).Split('=')[1];
            string Password = Lines.First(line => line.StartsWith("Password=")).Split('=')[1];

            Process FortniteLauncher = FNProc.Launch($"{GamePath}\\FortniteGame\\Binaries\\Win64\\FortniteLauncher.exe", true, "");
            Process FortniteClientBE = FNProc.Launch($"{GamePath}\\FortniteGame\\Binaries\\Win64\\FortniteClient-Win64-Shipping_BE.exe", true, "");
            Process FortniteClientEAC = FNProc.Launch($"{GamePath}\\FortniteGame\\Binaries\\Win64\\FortniteClient-Win64-Shipping_EAC.exe", true, "");
            Process FortniteClient64 = FNProc.Launch($"{GamePath}\\FortniteGame\\Binaries\\Win64\\FortniteClient-Win64-Shipping.exe", false, $"-AUTH_LOGIN={Email} -AUTH_PASSWORD={Password} -nobe -fromfl=eac -fltoken=5d67b47b83ad5793a18a5746 -AUTH_TYPE=epic -epicapp=Fortnite -epicenv=Prod -epiclocale=en-us -epicportal -skippatchcheck -nobe -fromfl=eac -fltoken=5d67b47b83ad5793a18a5746");
            Print($"{Project.Name} has been successfully launched.");
            Thread.Sleep(Timeout.Infinite);
        }
        else
        {
            await Return("Invalid selection. Please try again.");
        }
    }

    public static async Task RemoveVersion()
    {
        Console.Clear();
        var Lines = File.ReadAllLines(Credentials).ToList();

        var Versions = Lines
            .Where(Line => !Line.StartsWith("Email=") && !Line.StartsWith("Password="))
            .Select(Line => Line.Split('=')[0])
            .ToList();

        if (!Versions.Any())
        {
            await Return("No versions available to remove.");
            return;
        }


        Console.WriteLine("Available Fortnite Versions:\n");

        for (int i = 0; i < Versions.Count; i++)
        {
            Console.WriteLine($"* [{i + 1}] {Versions[i]}");
        }

        Console.WriteLine();
        Question("Enter the number of the version to remove: ");


        if (int.TryParse(Console.ReadLine(), out int Index) && Index > 0 && Index <= Versions.Count)
        {
            var VersionToRemove = Lines.First(Line => Line.StartsWith(Versions[Index - 1] + "="));
            Lines.Remove(VersionToRemove);

            File.WriteAllLines(Credentials, Lines);

            Success("Version removed successfully.");
        }
    }

    public static async Task ModifyAccountDetails()
    {
    Retry:

        try
        {
            Console.Clear();
            Question("Enter your Email: ");
            string AUTH_LOGIN = Console.ReadLine();

            Question("Enter your Password: ");
            string AUTH_PASSWORD = Console.ReadLine();

            Question("Are you sure these are your correct details? (Yes/No) ");
            string Answer = Console.ReadLine();

            if (Answer.ToLower() == "no")
            {
                await Return("Please re-enter your credentials correctly.");
                goto Retry;
            }
            var NewDetails = File.Exists(Credentials) ? File.ReadAllLines(Credentials).ToList() : new List<string>();

            NewDetails.RemoveAll(Line => Line.StartsWith("Email="));
            NewDetails.RemoveAll(Line => Line.StartsWith("Password="));

            NewDetails.Add($"Email={AUTH_LOGIN}");
            NewDetails.Add($"Password={AUTH_PASSWORD}");

            File.WriteAllLines(Credentials, NewDetails);

            Success("Account details modified successfully.");
        }
        catch (Exception ex)
        {
            Print($"Whoops! Something went wrong. We've encountered an issue on our end. Please try reopening the {Project.Name} Launcher.");
        }
    }
}
