using Spectre.Console;

class UI {
    public static async Task Return(string Message, bool SkipLine)
    {
        const int CloseTime = 5;
        AnsiConsole.MarkupLine($"{(SkipLine ? "\n" : "")}[Red](!)[/] {Message} [gray](Returning in {CloseTime} Seconds)[/]");
        await Task.Delay(TimeSpan.FromSeconds(CloseTime));
        Console.Clear();
    }

    public static async Task Print(string Message, bool SkipLine)
    {
        AnsiConsole.Markup($"{(SkipLine ? "\n" : "")}[{Project.Color}](*)[/] {Message}");
    }

    public static async Task Success(string Message, bool SkipLine)
    {
        AnsiConsole.MarkupLine($"{(SkipLine ? "\n" : "")}[green](!)[/] {Message} [gray](Press any key to continue)[/]");
        Console.ReadKey();
        Console.Clear();
    }

    public static async Task Question(string Message, bool SkipLine)
    {
        string color = "[rgb(184,134,11)]";
        AnsiConsole.Markup($"{(SkipLine ? "\n" : "")}{color}(?)[/] {Message}");
    }
}