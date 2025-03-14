using System.Diagnostics;
using System.Runtime.InteropServices;
public class FNProc
{
    public static Process Launch(string GamePath, bool FreezeProc, string Arg = "")
    {
        Process Process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = GamePath,
                Arguments = "-epicapp=Fortnite -epicenv=Prod -epiclocale=en-us -epicportal -nobe -fromfl=eac " + Arg
            }
        };
        Process.Start();
        if (FreezeProc)
        {
            foreach (ProcessThread processThread in Process.Threads)
            {
                SuspendThread(OpenThread(2, false, processThread.Id));
            }
        }
        return Process;
    }

    [DllImport("kernel32.dll")]
    public static extern int SuspendThread(IntPtr hThread);

    [DllImport("kernel32.dll")]
    public static extern IntPtr OpenThread(int dwDesiredAccess, bool bInheritHandle, int dwThreadId);
}
public static class ConfigHelper
{
    public static List<string> Excludes = new List<string>
    {
        "LaunchMultipleGames", "CloseAfterLaunch",
        "Password", "Email",
    };
}
public class Values
{
    public static string GetString(List<string> Settings, string Attribute)
    {
        var Line = Settings.FirstOrDefault(X => X.StartsWith($"{Attribute}="));
        return Line != null && Line.Contains("=") ? Line.Substring(Line.IndexOf('=') + 1) : null;
    }
    public static bool GetBool(List<string> Settings, string Attribute)
    {
        var Line = Settings.FirstOrDefault(X => X.StartsWith($"{Attribute}="));
        return Line?.Split('=')[1].ToLower() == "true";
    }

    public static List<string> GetVersions(List<string> Configuration)
    {
        return Configuration
            .Where(Line => !ConfigHelper.Excludes.Any(Exclude => Line.Contains(Exclude)))
            .Select(Line => Line.Split('=')[0])
            .ToList();
    }

    public static void UpdateSetting(List<string> Settings, string Attribute, bool Status)
    {
        Settings.RemoveAll(X => X.StartsWith($"{Attribute}="));
        Settings.Add($"{Attribute}={Status}");
    }
    public static void Status(bool Status)
    {
        Console.ForegroundColor = Status ? ConsoleColor.Green : ConsoleColor.Red;
        Console.WriteLine(Status ? "Enabled" : "Disabled");
        Console.ResetColor();
    }
}
