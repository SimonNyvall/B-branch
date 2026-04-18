using System.Diagnostics;

namespace Bbranch.CLI.Output;

internal static class Pager
{
    public static void StartLess(string input, string lessCommandPath)
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = lessCommandPath,
            Arguments = "-R -F -X",

            RedirectStandardInput = true,

            UseShellExecute = false,
        };

        processStartInfo.Environment["LESSCHARSET"] = "utf-8";

        using var process = Process.Start(processStartInfo);
        if (process == null)
            return;

        process.StandardInput.Write(input);
        process.StandardInput.Close();

        process.WaitForExit();
    }
}
