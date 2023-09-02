﻿using ClosedXML.Parser.Rolex;

namespace ClosedXML.Parser.Tests.Lexers;

public class R1C1ReferenceTokenTests
{
    [Theory]
    [MemberData(nameof(TestDataOneCorner))]
    [MemberData(nameof(TestDataTwoCorners))]
    public void Parse_extracts_information_from_token(string token, ReferenceArea expectedReference)
    {
        Assert.Equal(new[] {Token.A1_REFERENCE, Token.EofSymbolId}, RolexLexer.GetTokensR1C1(token).Select(x => x.SymbolId));
        var reference = TokenParser.ParseReference(token.AsSpan(), false);
        Assert.Equal(expectedReference, reference);
    }

    public static IEnumerable<object[]> TestDataOneCorner
    {
        get
        {
            // The `C` is a shortcut for `C[0]`
            yield return new object[]
            {
                "C",
                new ReferenceArea(new RowCol(ReferenceAxisType.Relative, 0, ReferenceAxisType.None, 0))
            };

            yield return new object[]
            {
                "C[-14]",
                new ReferenceArea(new RowCol(ReferenceAxisType.Relative, -14, ReferenceAxisType.None, 0))
            };

            yield return new object[]
            {
                "C75",
                new ReferenceArea(new RowCol(ReferenceAxisType.Absolute, 75, ReferenceAxisType.None, 0))
            };

            // The `R` is a shortcut for `R[0]`
            yield return new object[]
            {
                "R",
                new ReferenceArea(new RowCol(ReferenceAxisType.None, 0, ReferenceAxisType.Relative, 0))
            };

            yield return new object[]
            {
                "R[-14]",
                new ReferenceArea(new RowCol(ReferenceAxisType.None, 0, ReferenceAxisType.Relative, -14))
            };

            yield return new object[]
            {
                "R75",
                new ReferenceArea(new RowCol(ReferenceAxisType.None, 0, ReferenceAxisType.Absolute, 75))
            };

            yield return new object[]
            {
                "RC",
                new ReferenceArea(new RowCol(ReferenceAxisType.Relative, 0, ReferenceAxisType.Relative, 0))
            };

            yield return new object[]
            {
                "R[7]C2",
                new ReferenceArea(new RowCol(ReferenceAxisType.Absolute, 2, ReferenceAxisType.Relative, 7))
            };

            yield return new object[]
            {
                "R812C[7]",
                new ReferenceArea(new RowCol(ReferenceAxisType.Relative, 7, ReferenceAxisType.Absolute, 812))
            };
        }
    }

    public static IEnumerable<object[]> TestDataTwoCorners
    {
        get
        {
            yield return new object[]
            {
                "R1C2:R3C4",
                new ReferenceArea(
                    new RowCol(ReferenceAxisType.Absolute, 2, ReferenceAxisType.Absolute, 1),
                    new RowCol(ReferenceAxisType.Absolute, 4, ReferenceAxisType.Absolute, 3))
            };

            yield return new object[]
            {
                "C:R",
                new ReferenceArea(
                    new RowCol(ReferenceAxisType.Relative, 0, ReferenceAxisType.None, 0),
                    new RowCol(ReferenceAxisType.None, 0, ReferenceAxisType.Relative, 0))
            };

            yield return new object[]
            {
                "R[-1]C[-2]:R[-3]C[-4]",
                new ReferenceArea(
                    new RowCol(ReferenceAxisType.Relative, -2, ReferenceAxisType.Relative, -1),
                    new RowCol(ReferenceAxisType.Relative, -4, ReferenceAxisType.Relative, -3))
            };

            yield return new object[]
            {
                "R:C",
                new ReferenceArea(
                    new RowCol(ReferenceAxisType.None, 0, ReferenceAxisType.Relative, 0),
                    new RowCol(ReferenceAxisType.Relative, 0, ReferenceAxisType.None, 0))
            };
        }
    }
}