//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.12.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from .\Hawk.g4 by ANTLR 4.12.0

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using Antlr4.Runtime.Misc;
using IParseTreeListener = Antlr4.Runtime.Tree.IParseTreeListener;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete listener for a parse tree produced by
/// <see cref="HawkParser"/>.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.12.0")]
public interface IHawkListener : IParseTreeListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="HawkParser.root"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterRoot([NotNull] HawkParser.RootContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="HawkParser.root"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitRoot([NotNull] HawkParser.RootContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="HawkParser.def"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterDef([NotNull] HawkParser.DefContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="HawkParser.def"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitDef([NotNull] HawkParser.DefContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="HawkParser.pattern"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterPattern([NotNull] HawkParser.PatternContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="HawkParser.pattern"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitPattern([NotNull] HawkParser.PatternContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="HawkParser.escaped"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterEscaped([NotNull] HawkParser.EscapedContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="HawkParser.escaped"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitEscaped([NotNull] HawkParser.EscapedContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="HawkParser.parens"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterParens([NotNull] HawkParser.ParensContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="HawkParser.parens"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitParens([NotNull] HawkParser.ParensContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="HawkParser.brackets"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBrackets([NotNull] HawkParser.BracketsContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="HawkParser.brackets"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBrackets([NotNull] HawkParser.BracketsContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="HawkParser.toklist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterToklist([NotNull] HawkParser.ToklistContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="HawkParser.toklist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitToklist([NotNull] HawkParser.ToklistContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="HawkParser.tok"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTok([NotNull] HawkParser.TokContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="HawkParser.tok"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTok([NotNull] HawkParser.TokContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="HawkParser.filter"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFilter([NotNull] HawkParser.FilterContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="HawkParser.filter"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFilter([NotNull] HawkParser.FilterContext context);
}
