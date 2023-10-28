using System.Text;

namespace Hawk;

using System.Linq;

public class TestVisitor : HawkBaseVisitor<string>
{
    private static readonly Random rng = new();

    private readonly IDictionary<string, HawkParser.PatternContext> env;
    
    public TestVisitor(IDictionary<string, HawkParser.PatternContext> env)
    {
        this.env = env;
    }

    public override string VisitSub(HawkParser.SubContext context)
    {
        var key = context.ID().GetText();
        this.env[key] = context.pattern();
        return string.Empty;
    }

    public override string VisitRoot(HawkParser.RootContext context)
    {
        foreach (var sub in context.sub())
        {
            sub.Accept(this);
        }
        
        var s = new StringBuilder();
        foreach (var pat in context.pattern())
        {
            s.Append(pat.Accept(this));
        }

        return s.ToString();
    }

    public override string VisitBgroup(HawkParser.BgroupContext context)
    {
        var buf = new StringBuilder();
        Array.ForEach(
            context.pattern(),
            n => buf.Append(n.Accept(this)));
        return buf.ToString();
    }

    public override string VisitPgroup(HawkParser.PgroupContext context)
    {
        var roll = rng.NextDouble();
        if (roll <= 0.5)
        {
            return string.Empty;
        }

        var buf = new StringBuilder();
        Array.ForEach(
            context.pattern(),
            n => buf.Append(n.Accept(this)));
        return buf.ToString();
    }

    public override string VisitToklist(HawkParser.ToklistContext context)
    {
        var tok = context
            .tok()
            .SelectMany(t =>
            {
                var num = 1;
                if (t.TEXT() is null)
                {
                    num = 1;
                }
                else if (!int.TryParse(t.TEXT().GetText(), out num))
                {
                    num = 1;
                }
                
                return Enumerable
                    .Range(0, num)
                    .Select(_ => t);
            })
            .OrderBy(_ => rng.Next())
            .First();

        return tok.ID() is null
            ? tok.TEXT().GetText()
            : this.env[tok.ID().GetText()].Accept(this);
    }
}