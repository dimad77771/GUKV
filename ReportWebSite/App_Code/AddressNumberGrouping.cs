using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace GUKV
{
    public enum AddressNumberTokenKind
    {
        Letter,
        Digit
    }

    public sealed class AddressNumberToken
    {
        public AddressNumberToken(AddressNumberTokenKind kind, string value)
        {
            Kind = kind;
            Value = value ?? string.Empty;
        }

        public AddressNumberTokenKind Kind { get; private set; }

        public string Value { get; private set; }
    }

    public sealed class AddressNumberCandidate
    {
        public AddressNumberCandidate(int buildingId, string displayNumber)
        {
            BuildingId = buildingId;
            DisplayNumber = (displayNumber ?? string.Empty).Trim();
        }

        public AddressNumberCandidate(int buildingId, string number1, string number2, string number3)
            : this(buildingId, AddressNumberGrouping.FormatDisplayNumber(number1, number2, number3))
        {
        }

        public int BuildingId { get; private set; }

        public string DisplayNumber { get; private set; }
    }

    public sealed class AddressNumberGroup
    {
        internal AddressNumberGroup(string tokenKey, bool isTokenless, int representativeBuildingId,
            string displayNumber, IEnumerable<int> buildingIds)
        {
            TokenKey = tokenKey;
            IsTokenless = isTokenless;
            RepresentativeBuildingId = representativeBuildingId;
            DisplayNumber = displayNumber;
            BuildingIds = new ReadOnlyCollection<int>(buildingIds.OrderBy(id => id).ToList());
        }

        public string TokenKey { get; private set; }

        public bool IsTokenless { get; private set; }

        public int RepresentativeBuildingId { get; private set; }

        public string DisplayNumber { get; private set; }

        public ReadOnlyCollection<int> BuildingIds { get; private set; }
    }

    /// <summary>
    /// Pure address-number grouping rules shared by the address picker and create flow.
    /// This class deliberately has no SQL, WebForms, or DevExpress dependencies.
    /// </summary>
    public static class AddressNumberGrouping
    {
        private sealed class TokenAccumulator
        {
            public AddressNumberTokenKind Kind;
            public readonly StringBuilder Value = new StringBuilder();
        }

        private sealed class SpellingChoice
        {
            public string Spelling;
            public int Frequency;
            public int CleanlinessPenalty;
            public int MinimumBuildingId;
        }

        public static string FormatDisplayNumber(string number1, string number2, string number3)
        {
            string[] parts = new[] { number1, number2, number3 }
                .Select(value => (value ?? string.Empty).Trim())
                .Where(value => value.Length > 0)
                .ToArray();

            return string.Join(" ", parts);
        }

        public static IList<AddressNumberToken> Tokenize(string value)
        {
            List<AddressNumberToken> tokens = new List<AddressNumberToken>();
            TokenAccumulator current = null;
            string text = (value ?? string.Empty).Normalize(NormalizationForm.FormC);

            for (int index = 0; index < text.Length;)
            {
                UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(text, index);
                int charLength = char.IsSurrogatePair(text, index) ? 2 : 1;
                string character = text.Substring(index, charLength);
                AddressNumberTokenKind? kind = IsApostropheLike(character)
                    ? (AddressNumberTokenKind?)null
                    : GetTokenKind(category);

                if (!kind.HasValue)
                {
                    FlushToken(tokens, ref current);
                    index += charLength;
                    continue;
                }

                if (current == null || current.Kind != kind.Value)
                {
                    FlushToken(tokens, ref current);
                    current = new TokenAccumulator { Kind = kind.Value };
                }

                current.Value.Append(MapHomoglyphs(character.ToLowerInvariant()));
                index += charLength;
            }

            FlushToken(tokens, ref current);
            return new ReadOnlyCollection<AddressNumberToken>(tokens);
        }

        public static string GetTokenKey(string value)
        {
            IList<AddressNumberToken> tokens = Tokenize(value);
            if (tokens.Count == 0)
            {
                return string.Empty;
            }

            StringBuilder key = new StringBuilder();
            foreach (AddressNumberToken token in tokens)
            {
                key.Append(token.Kind == AddressNumberTokenKind.Letter ? 'L' : 'D');
                key.Append(token.Value.Length.ToString(CultureInfo.InvariantCulture));
                key.Append(':');
                key.Append(token.Value);
                key.Append(';');
            }

            return key.ToString();
        }

        public static int GetCleanlinessPenalty(string value)
        {
            int penalty = 0;
            bool previousWasWhitespace = false;
            string text = (value ?? string.Empty).Normalize(NormalizationForm.FormC);

            for (int index = 0; index < text.Length;)
            {
                UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(text, index);
                int charLength = char.IsSurrogatePair(text, index) ? 2 : 1;
                string character = text.Substring(index, charLength);

                if (char.IsWhiteSpace(text, index))
                {
                    if (previousWasWhitespace)
                    {
                        penalty++;
                    }

                    previousWasWhitespace = true;
                }
                else
                {
                    previousWasWhitespace = false;
                    // Keep representative selection aligned with Tokenize: every
                    // non-letter/non-decimal-digit separator is an extra sign.
                    // This includes invisible format/control and bidi characters.
                    if (IsApostropheLike(character) || !GetTokenKind(category).HasValue)
                    {
                        penalty++;
                    }
                }

                index += charLength;
            }

            return penalty;
        }

        public static IList<AddressNumberGroup> Group(IEnumerable<AddressNumberCandidate> candidates)
        {
            if (candidates == null)
            {
                throw new ArgumentNullException("candidates");
            }

            List<AddressNumberCandidate> source = candidates
                .Where(candidate => candidate != null && candidate.BuildingId > 0)
                .ToList();

            List<AddressNumberGroup> result = new List<AddressNumberGroup>();

            foreach (IGrouping<string, AddressNumberCandidate> tokenGroup in source
                .Where(candidate => GetTokenKey(candidate.DisplayNumber).Length > 0)
                .GroupBy(candidate => GetTokenKey(candidate.DisplayNumber), StringComparer.Ordinal))
            {
                SpellingChoice representative = tokenGroup
                    .GroupBy(candidate => candidate.DisplayNumber, StringComparer.Ordinal)
                    .Select(spellingGroup => new SpellingChoice
                    {
                        Spelling = spellingGroup.Key,
                        Frequency = spellingGroup.Count(),
                        CleanlinessPenalty = GetCleanlinessPenalty(spellingGroup.Key),
                        MinimumBuildingId = spellingGroup.Min(candidate => candidate.BuildingId)
                    })
                    .OrderByDescending(choice => choice.Frequency)
                    .ThenBy(choice => choice.CleanlinessPenalty)
                    .ThenBy(choice => choice.Spelling, StringComparer.OrdinalIgnoreCase)
                    .ThenBy(choice => choice.MinimumBuildingId)
                    .First();

                result.Add(new AddressNumberGroup(
                    tokenGroup.Key,
                    false,
                    representative.MinimumBuildingId,
                    representative.Spelling,
                    tokenGroup.Select(candidate => candidate.BuildingId).Distinct()));
            }

            // A tokenless value cannot be compared safely. Keep every row as its own group.
            foreach (AddressNumberCandidate candidate in source
                .Where(item => GetTokenKey(item.DisplayNumber).Length == 0))
            {
                result.Add(new AddressNumberGroup(
                    "TOKENLESS:" + candidate.BuildingId.ToString(CultureInfo.InvariantCulture),
                    true,
                    candidate.BuildingId,
                    candidate.DisplayNumber,
                    new[] { candidate.BuildingId }));
            }

            return new ReadOnlyCollection<AddressNumberGroup>(result
                .OrderBy(group => group.DisplayNumber, StringComparer.OrdinalIgnoreCase)
                .ThenBy(group => group.RepresentativeBuildingId)
                .ToList());
        }

        public static AddressNumberGroup FindGroup(IEnumerable<AddressNumberCandidate> candidates,
            string number1, string number2, string number3)
        {
            string tokenKey = GetTokenKey(FormatDisplayNumber(number1, number2, number3));
            if (tokenKey.Length == 0)
            {
                return null;
            }

            return Group(candidates).FirstOrDefault(group =>
                !group.IsTokenless && string.Equals(group.TokenKey, tokenKey, StringComparison.Ordinal));
        }

        public static AddressNumberGroup FindGroupByBuildingId(IEnumerable<AddressNumberGroup> groups, int buildingId)
        {
            if (groups == null || buildingId <= 0)
            {
                return null;
            }

            return groups.FirstOrDefault(group => group.BuildingIds.Contains(buildingId));
        }

        private static AddressNumberTokenKind? GetTokenKind(UnicodeCategory category)
        {
            switch (category)
            {
                case UnicodeCategory.UppercaseLetter:
                case UnicodeCategory.LowercaseLetter:
                case UnicodeCategory.TitlecaseLetter:
                case UnicodeCategory.ModifierLetter:
                case UnicodeCategory.OtherLetter:
                    return AddressNumberTokenKind.Letter;

                case UnicodeCategory.DecimalDigitNumber:
                    return AddressNumberTokenKind.Digit;

                default:
                    return null;
            }
        }

        private static bool IsApostropheLike(string value)
        {
            if (string.IsNullOrEmpty(value) || value.Length != 1)
            {
                return false;
            }

            switch (value[0])
            {
                case '\u0027': // apostrophe
                case '\u0060': // grave accent
                case '\u00B4': // acute accent
                case '\u02BC': // modifier letter apostrophe (common Ukrainian form)
                case '\u055A': // Armenian apostrophe
                case '\u2018': // left single quotation mark
                case '\u2019': // right single quotation mark
                case '\u201B': // single high-reversed-9 quotation mark
                case '\u2032': // prime
                case '\uFF07': // fullwidth apostrophe
                    return true;
            }

            return false;
        }

        private static void FlushToken(ICollection<AddressNumberToken> tokens, ref TokenAccumulator current)
        {
            if (current != null && current.Value.Length > 0)
            {
                tokens.Add(new AddressNumberToken(current.Kind, current.Value.ToString()));
            }

            current = null;
        }

        private static string MapHomoglyphs(string value)
        {
            StringBuilder mapped = new StringBuilder(value.Length);
            foreach (char character in value)
            {
                switch (character)
                {
                    case 'a': mapped.Append((char)0x0430); break; // Latin A -> Cyrillic A
                    case 'b': mapped.Append((char)0x0432); break; // Latin B -> Cyrillic V
                    case 'c': mapped.Append((char)0x0441); break; // Latin C -> Cyrillic S
                    case 'e': mapped.Append((char)0x0435); break; // Latin E -> Cyrillic E
                    case 'h': mapped.Append((char)0x043d); break; // Latin H -> Cyrillic N
                    case 'i': mapped.Append((char)0x0456); break; // Latin I -> Ukrainian I
                    case 'k': mapped.Append((char)0x043a); break; // Latin K -> Cyrillic K
                    case 'm': mapped.Append((char)0x043c); break; // Latin M -> Cyrillic M
                    case 'o': mapped.Append((char)0x043e); break; // Latin O -> Cyrillic O
                    case 'p': mapped.Append((char)0x0440); break; // Latin P -> Cyrillic R
                    case 't': mapped.Append((char)0x0442); break; // Latin T -> Cyrillic T
                    case 'x': mapped.Append((char)0x0445); break; // Latin X -> Cyrillic H
                    default: mapped.Append(character); break;
                }
            }

            return mapped.ToString();
        }
    }
}
