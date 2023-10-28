namespace Hawk;

using System.Text;

public class Interpreter : HawkBaseVisitor<string>
{
    /// <summary>
    /// Random number generator.
    /// </summary>
    /// <remarks>
    /// This generator is used to evaluate parens and token lists where
    /// a dynamic (random) choice needs to be made.
    /// </remarks>
    private readonly Random rng;
    
    /// <summary>
    /// The environment of predefined sub-patterns.
    /// </summary>
    /// <remarks>
    /// Sub-patterns are just patterns but they are associated with a
    /// single capital letter (their identifier). Akin to a variable they
    /// can be re-used in other patterns.
    /// </remarks>
    private readonly IDictionary<string, HawkParser.PatternContext> env;

    /// <summary>
    /// Constructs a new <see cref="Interpreter"/> instance.
    /// </summary>
    /// <param name="env">The sub-pattern environment.</param>
    /// <remarks>
    /// <p>Since the host is almost involved in manipulating the
    /// sub-pattern environment, it is best if it is passed along.</p>
    /// <p>In the future there will probably be an improved host interface as
    /// well as an abstraction for the random number generator.</p> 
    /// </remarks>
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
        var key = context.ID().GetText();
        var value = context.pattern();
        this.env[key] = value;
        return string.Empty;
    }

    public override string VisitParens(HawkParser.ParensContext context)
    {
        var roll = this.rng.NextDouble();
        if (roll < 0.5)
        {
            return string.Empty;
        }

        var buf = new StringBuilder();
        Array.ForEach(
            context.pattern(),
            pat => buf.Append(pat.Accept(this)));
        return buf.ToString();
    }

    public override string VisitBrackets(HawkParser.BracketsContext context)
    {
        var buf = new StringBuilder();
        Array.ForEach(
            context.pattern(),
            pat => buf.Append(pat.Accept(this)));
        return buf.ToString();
    }

    public override string VisitTok(HawkParser.TokContext context)
    {
        return context.ID() is null
            ? context.TEXT().GetText()
            : this
                .env[context.ID().GetText()]
                .Accept(this);
    }

    public override string VisitToklist(
        HawkParser.ToklistContext context)
    {
        // There is a certain weight for each token to be generated, with
        // the default being one. The algorithm below works as follows:\
        // 
        // 1. For each token:

        // 1.1 Try to parse its *num* value (set num = 1 by otherwise/default)
        // 1.2 Generate list of *num* number of tokens (references)
        static IEnumerable<HawkParser.TokContext> Dup(
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

        // 2. Concatenate all generated lists into a flat list
        // 3. Shuffle this list by ordering by a random number
        // 4. Take the token at the head of the list
        var tok = context
            .tok()
            .SelectMany(Dup)
            .OrderBy(_ => rng.Next())
            .First();

        // 5. Send the selected token to the interpreter.
        return tok.Accept(this);
    }
}