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
