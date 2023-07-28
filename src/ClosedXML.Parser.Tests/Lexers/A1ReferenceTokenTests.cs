﻿using Xunit;

namespace ClosedXML.Parser.Tests.Lexers;

/// <summary>
/// Test of a parsing of a token <c>A1_REFERENCE</c>.
/// <code>
/// A1_REFERENCE
///        : A1_COLUMN ':' A1_COLUMN
///        | A1_ROW ':' A1_ROW
///        | A1_CELL
///        | A1_AREA
///        ;
/// </code>
/// </summary>
public class A1ReferenceTokenTests
{
    private const int MaxCol = 16384;
    private const int MaxRow = 1048576;

    [Fact]
    public void Parse_a1_cell()
    {
        // Check A1_CELL path
        AssertAreaReferenceToken("$B$3", new CellArea(new Reference(true, 2, true, 3)));
        AssertAreaReferenceToken("A1", new CellArea(1, 1));
        AssertAreaReferenceToken("XFD1", new CellArea(16384, 1));
        AssertAreaReferenceToken("A1048576", new CellArea(1, 1048576));
        AssertAreaReferenceToken("$XFD$1048576", new CellArea(new Reference(true, 16384, true, 1048576)));
    }

    [Fact]
    public void Parse_row_range()
    {
        // Check A1_ROW ':' A1_ROW path
        AssertAreaReferenceToken("1:1", new CellArea(new Reference(true, 1, false, 1), new Reference(true, MaxCol, false, 1)));
        AssertAreaReferenceToken("$5:10", new CellArea(new Reference(true, 1, true, 5), new Reference(true, MaxCol, false, 10)));
        AssertAreaReferenceToken("7:$3", new CellArea(new Reference(true, 1, false, 7), new Reference(true, MaxCol, true, 3)));
        AssertAreaReferenceToken("$1048576:$1048576", new CellArea(new Reference(true, 1, true, 1048576), new Reference(true, MaxCol, true, 1048576)));
    }

    [Fact]
    public void Parse_column_range()
    {
        // Check A1_COLUMN ':' A1_COLUMN path
        AssertAreaReferenceToken("A:A", new CellArea(new Reference(false, 1, true, 1), new Reference(false, 1, true, MaxRow)));
        AssertAreaReferenceToken("RW:ST", new CellArea(new Reference(false, 491, true, 1), new Reference(false, 514, true, MaxRow)));
        AssertAreaReferenceToken("$C:D", new CellArea(new Reference(true, 3, true, 1), new Reference(false, 4, true, MaxRow)));
        AssertAreaReferenceToken("E:$C", new CellArea(new Reference(false, 5, true, 1), new Reference(true, 3, true, MaxRow)));
        AssertAreaReferenceToken("$XFD:$XFD", new CellArea(new Reference(true, MaxCol, true, 1), new Reference(true, MaxCol, true, MaxRow)));
    }

    [Fact]
    public void Parse_area()
    {
        // Check A1_AREA path
        AssertAreaReferenceToken("A1:A1", new CellArea(new Reference(false, 1, false, 1)));
        AssertAreaReferenceToken("Z1:AB25", new CellArea(new Reference(false, 26, false, 1), new Reference(28, 25)));
        AssertAreaReferenceToken("$XFC$1048575:$XFD$1048576", new CellArea(new Reference(true, MaxCol - 1, true, MaxRow - 1), new Reference(true, MaxCol, true, MaxRow)));
    }

    private static void AssertAreaReferenceToken(string token, CellArea expectedReference)
    {
        AssertFormula.AssertTokenType(token, FormulaLexer.A1_REFERENCE);
        var reference = TokenParser.ParseA1Reference(token);
        Assert.Equal(expectedReference, reference);
    }
}