using System.Text;
using Antlr4.Runtime;
using Hawk;

var example = @"
    V : o/i/u                   ;
    C : k/p/b/m/n               ;
    N : m/n                     ;
    (C)V(V)(C)V[ta/toa](V)(N)
";

var env = new Dictionary<string, HawkParser.PatternContext>();


const string prompt = "> ";

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

        if (input.TrimEnd().EndsWith('.'))
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
    var visitor = new TestVisitor(env);

    for (var n = 0; n < 5; n++)
    {
        var s = ctx.Accept(visitor);
        Console.WriteLine(s);
    }
}