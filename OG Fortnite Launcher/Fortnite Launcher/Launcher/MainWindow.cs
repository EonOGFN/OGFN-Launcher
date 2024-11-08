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

            Console.WriteLine("[1] Add a New Fortnite Version");
            Console.WriteLine("[2] Modify Account Information");
            Console.WriteLine("[3] Start Fortnite Game");
            Console.WriteLine("[4] Remove a Fortnite Version");
            Console.WriteLine("[5] Configure Launcher Settings");

            Console.Write("\n[?] Please select an option: ");

            string Input = Console.ReadLine();

            switch (Input)
            {
                case "1": await AddVersion(); break;
                case "2": await ModifyAccountDetails(); break;
                case "3": await Launch(); break;
                case "4": await RemoveVersion(); break;
                case "5": await Options(); break;
                default: Console.Clear(); break;
            }
        }
    }

    public static async Task Return(string Message, bool SkipLine)
    {
        const int CloseTime = 5;
        AnsiConsole.MarkupLine($"{(SkipLine ? "\n" : "")}[Red](!)[/] {Message} Returning in {CloseTime} Seconds...");
        await Task.Delay(TimeSpan.FromSeconds(CloseTime));
        Console.Clear();
    }

    public static async Task Print(string Message, bool SkipLine)
    {
        AnsiConsole.MarkupLine($"{(SkipLine ? "\n" : "")}[{Project.Color}](*)[/] {Message}");
    }

    public static async Task Success(string Message, bool SkipLine)
    {
        AnsiConsole.MarkupLine($"{(SkipLine ? "\n" : "")}[{Project.Color}](*)[/] {Message}");
        Print("Press any key to continue...", false);
        Console.ReadKey();
        Console.Clear();
    }

    public static async Task Question(string Message, bool SkipLine)
    {
        string color = "[rgb(184,134,11)]";
        AnsiConsole.Markup($"{(SkipLine ? "\n" : "")}{color}(?)[/] {Message}");
    }
}
