using Spectre.Console;
using static Launcher;
public class MainWindow
{
    public static string Credentials;
    public static async Task Main(string[] args)
    {
        Console.Title = $"{Project.Name} Launcher";

         string UserSettings = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"{Project.Name} Launcher");
         Credentials = Path.Combine(UserSettings, "Credentials.ini");

        Directory.CreateDirectory(UserSettings);
        if (!File.Exists(Credentials)){
            File.Create(Credentials).Dispose();
        }

        while (true)
        {
            AnsiConsole.MarkupLine($"[{Project.Color}]{Project.MainScreen}[/]\n");
            Console.WriteLine("[*] Launcher made by Plague. Check it out at github.com/EonOGFN/OGFN-Launcher.\n");

            Console.WriteLine("[1] Add Fortnite Version");
            Console.WriteLine("[2] Modify Account Details");
            Console.WriteLine($"[3] Launch {Project.Name}");
            Console.WriteLine("[4] Remove Fortnite Version");

            Console.Write("\n[?] Please choose an option: ");

            string Input = Console.ReadLine();

            switch (Input)
            {
                case "1": await AddVersion(); break;
                case "2": await ModifyAccountDetails(); break;
                case "3": await Launch(); break;
                case "4": await RemoveVersion(); break;
                default: await Return(""); break;
            }
        }
    }

    public static async Task Return(string Message)
    {
        const int CloseTime = 5;
        AnsiConsole.MarkupLine($"[Red](!)[/] {Message} Returning in {CloseTime} Seconds..");
        await Task.Delay(TimeSpan.FromSeconds(CloseTime));
        Console.Clear();
    }

    public static async Task Print(string Message)
    {
        AnsiConsole.MarkupLine($"[{Project.Color}](*)[/] {Message}");
    }

    public static async Task Success(string Message)
    {
        AnsiConsole.MarkupLine($"[{Project.Color}](*)[/] {Message}");
        Print("Press any key to continue...");
        Console.ReadKey();
        Console.Clear();
    }

    public static async Task Question(string Message)
    {
        String Color = "[rgb(184,134,11)]"; 
        AnsiConsole.Markup($"{Color}(?)[/] {Message}");
    }
}
