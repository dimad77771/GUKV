using System.Text.RegularExpressions;

namespace Gukv.UiTests.Infrastructure;

public sealed record VisualAddress(string District, string Street, string Number)
{
    private static readonly Regex Whitespace = new(@"\s+", RegexOptions.Compiled);

    public VisualAddress Normalize()
    {
        return new VisualAddress(
            NormalizePart(District),
            NormalizePart(Street),
            NormalizePart(Number));
    }

    public override string ToString()
    {
        VisualAddress normalized = Normalize();
        return $"{normalized.District} | {normalized.Street} | {normalized.Number}";
    }

    public static string JoinNumber(params string?[] parts)
    {
        return string.Join(" ", parts
            .Select(NormalizePart)
            .Where(part => part.Length > 0));
    }

    private static string NormalizePart(string? value)
    {
        return Whitespace.Replace(value ?? string.Empty, " ").Trim();
    }
}
