using System.Text;
using Antlr4.Runtime;
using Hawk;

const string prompt = "hwk> ";

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
    var interpreter = new Interpreter(env);
    var s = ctx.Accept(interpreter);
    
    if (!string.IsNullOrWhiteSpace(s))
    {
        Console.WriteLine(s);
    }
}