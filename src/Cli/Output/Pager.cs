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
            //RedirectStandardOutput = true,
            StandardInputEncoding = System.Text.Encoding.UTF8,
            //StandardOutputEncoding = System.Text.Encoding.UTF8,
            UseShellExecute = false,
        };

        using var process = Process.Start(processStartInfo);

        if (process == null)
            return;

        using (var writer = process.StandardInput)
        {
            writer.Write(input);
        }

        process.WaitForExit();
    }
}
