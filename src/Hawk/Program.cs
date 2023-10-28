using System.Text;
using Antlr4.Runtime;
using Hawk;

const string example = @"
    V : o/i/u                   ;
    C : k/p/b/m/n               ;
    N : m/n                     ;
    (C)V(V)(C)V[ta/toa](V)(N)
";

const string prompt = "> ";

const int count = 50;

var env = new Dictionary<string, HawkParser.PatternContext>();

while (true)
{
    var buf = new StringBuilder();
    Console.Write(prompt);
    
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
        
        Console.Write(string.Empty.PadRight(prompt.Length));
    }

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