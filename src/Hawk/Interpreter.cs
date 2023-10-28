using System.Text;

namespace Hawk;

public class Interpreter : HawkBaseVisitor<string>
{
    private readonly Random rng;
    private readonly IDictionary<string, HawkParser.PatternContext> env;

    public Interpreter(IDictionary<string, HawkParser.PatternContext> env)
        : this(new Random(), env)
    {
    }

    private Interpreter(
        Random rng, 
        IDictionary<string, HawkParser.PatternContext> env)
    {
        this.rng = rng;
        this.env = env;
    }

    public override string VisitRoot(HawkParser.RootContext context)
    {
        Array.ForEach(
            context.def(),
            pat => pat.Accept(this));
        
        var buf = new StringBuilder();
        Array.ForEach(
            context.pattern(),
            pat => buf.Append(pat.Accept(this)));
        return buf.ToString();
    }

    public override string VisitDef(HawkParser.DefContext context)
    {
        Console.WriteLine("DEF");
        var key = context.ID().GetText();
        var value = context.pattern();
        this.env[key] = value;
        return string.Empty;
    }
    
    public override string VisitBrackets(HawkParser.BracketsContext context)
    {
        var buf = new StringBuilder();
        Array.ForEach(
            context.pattern(),
            pat => buf.Append(pat.Accept(this)));
        return buf.ToString();
    }

    public override string VisitToklist(
        HawkParser.ToklistContext context)
    {
        IEnumerable<HawkParser.TokContext> DuplicateTokens(
            HawkParser.TokContext tc)
        {
            int num;
            if (tc.NUM() is null)
            {
                num = 1;
            }
            else if (!int.TryParse(tc.NUM().GetText(), out num))
            {
                num = 1;
            }

            return Enumerable
                .Range(0, num)
                .Select(_ => tc);
        }

        var tok = context
            .tok()
            .SelectMany(DuplicateTokens)
            .OrderBy(_ => rng.Next())
            .First();

        return tok.ID() is null
            ? tok.TEXT().GetText()
            : this.env[tok.ID().GetText()].Accept(this);
    }
}