using Spectre.Console;
using static Manager;
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
}
