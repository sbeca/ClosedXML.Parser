﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ClosedXML.Parser;

/// <summary>
/// Extension methods for building formulas.
/// </summary>
internal static class StringBuilderExtensions
{
    public static StringBuilder AppendSheetName(this StringBuilder sb, string sheetName)
    {
        return NameUtils.EscapeName(sb, sheetName);
    }

    public static StringBuilder AppendExternalSheetName(this StringBuilder sb, int workbookIndex, string sheetName)
    {
        if (NameUtils.ShouldQuote(sheetName.AsSpan()))
        {
            return sb
                .Append('\'')
                .AppendBookIndex(workbookIndex)
                .AppendEscapedSheetName(sheetName)
                .Append('\'');
        }

        return sb
            .AppendBookIndex(workbookIndex)
            .AppendSheetName(sheetName);
    }
    public static StringBuilder AppendEscapedSheetName(this StringBuilder sb, string sheetName)
    {
        var startIndex = sb.Length;
        return sb.Append(sheetName).Replace("'", "''", startIndex, sheetName.Length);
    }

    public static StringBuilder AppendReferenceSeparator(this StringBuilder sb)
    {
        return sb.Append('!');
    }

    public static StringBuilder AppendBookIndex(this StringBuilder sb, int bookIndex)
    {
        return sb.Append('[').Append(bookIndex).Append(']');
    }

    public static StringBuilder AppendFunction(this StringBuilder sb, ReadOnlySpan<char> functionName, IReadOnlyList<TransformedSymbol> arguments)
    {
        // netstandard 2.0 doesn't have span API for StringBuilder.
        foreach (var c in functionName)
            sb.Append(c);

        return AppendArguments(sb, arguments);
    }

    public static StringBuilder AppendArguments(this StringBuilder sb, IReadOnlyList<TransformedSymbol> arguments)
    {
        sb.Append('(');
        if (arguments.Count > 0)
            sb.Append(arguments[0].AsSpan());

        for (var i = 1; i < arguments.Count; ++i)
            sb.Append(',').Append(arguments[i].AsSpan());

        return sb.Append(')');
    }

    public static StringBuilder AppendRef(this StringBuilder sb, ReferenceArea reference)
    {
        reference.Append(sb);
        return sb;
    }

    public static StringBuilder AppendRef(this StringBuilder sb, RowCol rowCol)
    {
        rowCol.Append(sb);
        return sb;
    }

#if NETSTANDARD2_0
    /// <summary>
    /// Compatibility method for NETStandard 2.0, which doesn't have methods with <c>Span</c> arguments.
    /// </summary>
    public static StringBuilder Append(this StringBuilder sb, ReadOnlySpan<char> span)
    {
        foreach (var c in span)
            sb.Append(c);

        return sb;
    }
#endif
}