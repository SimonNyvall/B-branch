using System.Diagnostics;

namespace Bbranch.CLI.Output;

internal class Pager : IPager
{
    public async Task StartLess(IAsyncEnumerable<string> batches, string lessCommandPath)
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = lessCommandPath,
            Arguments = "-R -r -F -X",

            RedirectStandardInput = true,

            UseShellExecute = false,
        };

        processStartInfo.Environment["LESSCHARSET"] = "utf-8";

        using var process = Process.Start(processStartInfo);
        if (process == null)
            return;

        try
        {
            await foreach (var line in batches)
            {
                process.StandardInput.Write(line);
            }

            process.StandardInput.Flush();
            process.StandardInput.Close();
        }
        catch (IOException)
        {
            // Ignore error
        }

        await process.WaitForExitAsync();
    }
}
