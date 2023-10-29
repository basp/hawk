namespace Hawk;

using System.Text;

public class Interpreter : HawkBaseVisitor<string>
{
    /// <summary>
    /// A client that is hosting the interpreter.
    /// </summary>
    private readonly IHost host;
    
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
    public Interpreter(
        IHost host,
        IDictionary<string, HawkParser.PatternContext> env)
        : this(new Random(), host, env)
    {
    }

    private Interpreter(
        Random rng, 
        IHost host,
        IDictionary<string, HawkParser.PatternContext> env)
    {
        this.rng = rng;
        this.host = host;
        this.env = env;
    }

    /// <summary>
    /// Visit a <strong>Hawk</strong> root expression. A root program which
    /// contains zero or more definitions and zero or more generator
    /// expressions.
    /// </summary>
    /// <param name="context">The root node context.</param>
    /// <returns>A string translated from the root expression.</returns>
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

    public override string VisitEscaped(HawkParser.EscapedContext context)
    {
        return context.GetText().Trim('"');
    }

    /// <summary>
    /// Evaluates a definition and stores it in the local environment.
    /// </summary>
    /// <param name="context">The definition node context.</param>
    /// <returns>An empty string.</returns>
    /// <remarks>
    /// This stores a definition in the local environment. It has no result.
    /// </remarks>
    public override string VisitDef(HawkParser.DefContext context)
    {
        var key = context.ID().GetText();
        var value = context.pattern();
        this.env[key] = value;
        this.host.WriteLine(key);
        return string.Empty;
    }

    /// <summary>
    /// Evaluates a parenthesized expression.
    /// </summary>
    /// <param name="context">The expression in parenthesized context.</param>
    /// <returns>
    /// A string representing the result of evaluating the parenthesized
    /// context.
    /// </returns>
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

    /// <summary>
    /// Evaluates a bracketed expression.
    /// </summary>
    /// <param name="context">The expression in bracketed context.</param>
    /// <returns>
    /// A string representing the result of evaluating the bracketed context.
    /// </returns>
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
            // Assume the number of tokens is one whatever happens.
            // Whether its available or we can parse it or not.
            int num;
            if (tc.NUM() is null)
            {
                num = 1;
            }
            else if (!int.TryParse(tc.NUM().GetText(), out num))
            {
                num = 1;
            }

            // Clone references according to their weight.
            // This will be flat-mapped later. This should always yield
            // one result (i.e. `num >= 1` at this point).
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