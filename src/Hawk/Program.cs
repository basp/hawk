namespace  Hawk;

using System.CommandLine;
using System.Text;
using Antlr4.Runtime;

internal class Program
{
    private const string prompt = "hwk> ";

    private static void RunInteractive()
    {
        var env = new Dictionary<string, HawkParser.PatternContext>();

        while (true)
        {
            var buf = new StringBuilder();
            Console.Write(prompt);

            // This inner loop reads a multi-line prompt into
            // a buffer until a period ('.') is encountered.
            // This signals the interactive to evaluate the
            // given input by jumping (breaking) back into the
            // outer loop.
            while (true)
            {
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    continue;
                }

                buf.Append(input.TrimEnd('.'));
                if (input.EndsWith('.'))
                {
                    break;
                }

                // Indent the prompt to give a visual indication
                // that we are working with a multi-line prompt.
                Console.Write(string.Empty.PadRight(prompt.Length));
            }

            // Parse commands here (in the outer loop) since
            // we want to support multiple lines for them as well.
            // Also the period ('.') is part of the interactive
            // and should be used consistently.

            // In order to keep our Hawk grammar small and clean
            // we should not invoke Antlr unless we are dealing
            // with something it can actually parse (i.e. no
            // interpreter commands). These should be short-circuited
            // right here.

            var stream = new AntlrInputStream(buf.ToString());
            var lexer = new HawkLexer(stream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new HawkParser(tokens);
            var ctx = parser.root();
            var host = new ConsoleHost();
            var interpreter = new Interpreter(host, env);
            var s = ctx.Accept(interpreter);

            if (!string.IsNullOrWhiteSpace(s))
            {
                Console.WriteLine(s);
            }
        }
    }

    private static IEnumerable<string> Generate(FileSystemInfo file, int count)
    {
        var stream = new AntlrFileStream(file.FullName);
        var lexer = new HawkLexer(stream);
        var tokens = new CommonTokenStream(lexer);
        var parser = new HawkParser(tokens);
        var ctx = parser.root();
        var host = new NullHost();
        var env = new Dictionary<string, HawkParser.PatternContext>();
        var interpreter = new Interpreter(host, env);
        return Enumerable
            .Range(0, count)
            .Select(_ => ctx.Accept(interpreter));
    }

    public static async Task<int> Main(string[] args)
    {
        var fileArgument = new Argument<FileInfo?>();
        var countOption = new Option<int>(
            "--count", 
            () => 10);
        var generateCommand = new Command(
            "generate",
            "Generates words from a definition file.");
        generateCommand.AddArgument(fileArgument);
        generateCommand.AddOption(countOption);
        generateCommand.SetHandler(
            (file, count) =>
            {
                var words = Generate(file!, count)
                    .ToHashSet()
                    .Order()
                    .ToArray();
                var output = string.Join(" ", words);
                Console.WriteLine(output);
            }, 
            fileArgument, 
            countOption);
        
        var rootCommand = new RootCommand(
            "Execute a Hawk command.");
        rootCommand.AddCommand(generateCommand);
        rootCommand.SetHandler(RunInteractive);
        return await rootCommand.InvokeAsync(args);
    }

    private class NullHost : IHost
    {
        public void Write(string s)
        {
        }

        public void WriteLine(string s)
        {
        }
    }

    private class ConsoleHost : IHost
    {
        public void Write(string s)
        {
            Console.Write(s);
        }

        public void WriteLine(string s)
        {
            Console.WriteLine(s);
        }
    }
}
