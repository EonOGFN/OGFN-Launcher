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

        if (!Directory.Exists(UserSettings)){
            Directory.CreateDirectory(UserSettings);
        }

        while (true)
        {
            AnsiConsole.MarkupLine($"[{Project.Color}]{Project.MainScreen}[/]\n");
            Console.WriteLine("[*] Launcher made by Plague. Check it out at github.com/EonOGFN/OGFN-Launcher.\n");

            Console.WriteLine("[1] Add Fortnite Version");
            Console.WriteLine("[2] Modify Account Details");
            Console.WriteLine($"[3] Launch {Project.Name}");
            Console.WriteLine("[4] Remove Fortnite Version");
            Console.Write("[?] Please choose an option: ");

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
        AnsiConsole.MarkupLine($"[Red](!)[/] {Message} Returning in 5 Seconds..");
        await Task.Delay(5000);
        Console.Clear();
    }

    public static async Task Print(string Message)
    {
        AnsiConsole.MarkupLine($"[{Project.Color}](*)[/] {Message}");
    }

    public static async Task Question(string Message)
    {
        String Color = "[rgb(184,134,11)]"; 
        AnsiConsole.Markup($"{Color}(?)[/] {Message}");
    }
}
