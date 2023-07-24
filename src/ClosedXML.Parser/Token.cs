﻿namespace ClosedXML.Parser;

/// <summary>
/// A token for a formula input.
/// </summary>
internal readonly struct Token
{
    /// <summary>
    /// An error symbol id.
    /// </summary>
    public const int ErrorSymbolId = -1;

    /// <summary>
    /// A token ID or TokenType. Non-negative integer. The values are from Antlr grammar, starting with 1.
    /// See <c>FormulaLexer.tokens</c>. The value -1 indicates an error and unrecognized token and is always
    /// last token.
    /// </summary>
    public readonly int SymbolId;

    /// <summary>
    /// The starting index of a token, in code units (=chars).
    /// </summary>
    public readonly int StartIndex;

    /// <summary>
    /// Length of a token in code units (=chars). For non-error tokens, must be at least 1. Ignore for error token.
    /// </summary>
    public readonly int Length;

    public Token(int symbolId, int startIndex, int length)
    {
        SymbolId = symbolId;
        StartIndex = startIndex;
        Length = length;
    }

    public bool Equals(Token other)
    {
        return SymbolId == other.SymbolId && StartIndex == other.StartIndex && Length == other.Length;
    }

    public override bool Equals(object? obj)
    {
        return obj is Token other && Equals(other);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = SymbolId;
            hashCode = (hashCode * 397) ^ StartIndex;
            hashCode = (hashCode * 397) ^ Length;
            return hashCode;
        }
    }
}