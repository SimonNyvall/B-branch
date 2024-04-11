using System.Text;
using CliWrap;
using TableData;

namespace Git.Base
{
    public class GitBase
    {
        protected static string GitPath { get; private set; } = string.Empty;

        public static async Task Initialize()
        {
            await TrySetGitPath();
        }

        public static async Task<string> TryGetWorkingBranch()
        {
            try
            {
                return await GetWorkingBranch();
            }
            catch
            {
                return string.Empty;
            }
        }

        private static async Task<string> GetWorkingBranch()
        {
            string HEADFile = await File.ReadAllTextAsync(Path.Combine(GitPath, "HEAD"));

            if (HEADFile.Trim().StartsWith("ref:", StringComparison.Ordinal))
            {
                string[] branchNameComponents = HEADFile.Split('/');

                string branchName = string.Join("/", branchNameComponents.Skip(2));

                return branchName;
            }

            return string.Empty;
        }

        protected static List<GitBranch> GetNamesAndLastWirte(bool all, bool remote)
        {
            Dictionary<string, DateTime> branches = [];

            string localBranchDir = Path.Combine(GitPath, "refs", "heads");
            string remoteBranchDir = Path.Combine(GitPath, "refs", "remotes");

            if (!remote)
            {
                CollectBranches(localBranchDir, ref branches);
            }

            if (all || remote)
            {
                CollectBranches(remoteBranchDir, ref branches);
            }

            IEnumerable<GitBranch> result = branches.Select(x => new GitBranch(x.Key, x.Value));

            result = result.OrderByDescending(x => x.LastCommit);

            return result.ToList();
        }

        private static void CollectBranches(
            string directory,
            ref Dictionary<string, DateTime> branches
        )
        {
            if (!Directory.Exists(directory))
            {
                throw new DirectoryNotFoundException(
                    $"Branch directory does not exist at {directory}"
                );
            }

            foreach (string file in Directory.GetFiles(directory, "*", SearchOption.AllDirectories))
            {
                string relativePath = Path.GetRelativePath(directory, file);
                string branchName = relativePath.Replace(Path.DirectorySeparatorChar, '/');
                branches[branchName] = File.GetLastWriteTime(file);
            }
        }

        public static async Task<string> GetBranchDescription(string branchName)
        {
            if (!File.Exists(Path.Combine(GitPath, "EDIT_DESCRIPTION")))
            {
                return string.Empty;
            }

            string descriptionFile = await File.ReadAllTextAsync(
                Path.Combine(GitPath, "EDIT_DESCRIPTION")
            );

            if (!descriptionFile.Contains(branchName))
            {
                return string.Empty;
            }

            string[] lines = descriptionFile.Split('\n');

            IEnumerable<string> linesWithoutComments = lines.Where(x => !x.StartsWith('#'));

            string description = string.Join(" ", linesWithoutComments);

            return description;
        }

        protected static async Task<bool> ExecuteGitCommand(string gitPath, string arguments)
        {
            CommandResult result = await Cli.Wrap("git")
                .WithWorkingDirectory(gitPath)
                .WithArguments(arguments)
                .WithValidation(CommandResultValidation.None)
                .ExecuteAsync();

            return result.ExitCode == 0;
        }

        protected static string? GetOption(Dictionary<string, string> options, params string[] keys)
        {
            foreach (string key in keys)
            {
                bool found = options.TryGetValue(key, out string? value);

                if (found)
                {
                    return value;
                }
            }

            return null;
        }

        protected static void LogError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private static async Task TrySetGitPath()
        {
            try
            {
                const string argument = "rev-parse --git-dir";
                StringBuilder stdOutBuffer = new();

                CommandResult result = await Cli.Wrap("git")
                    .WithArguments(argument)
                    .WithStandardOutputPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
                    .ExecuteAsync();

                string stdOut = stdOutBuffer.ToString().Trim();

                if (string.IsNullOrEmpty(stdOut))
                {
                    return;
                }

                GitPath = stdOut;
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("This is not a git repository");
                Console.ResetColor();
                Environment.Exit(1);
            }
        }
    }
}
