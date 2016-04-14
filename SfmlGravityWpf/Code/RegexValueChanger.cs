namespace SfmlGravityWpf.Code
{
    using System;
    using System.Text.RegularExpressions;

    public static class RegexValueChanger
    {
        /// <summary>
        /// Format for changing a value by a percentage. "+#%" adds a percentage and "-#%" subtracts
        /// a percentage. Where # is an integer.
        /// </summary>
        private const string ByPercentPattern = @"^([+-])(\d+)%$";

        /// <summary>
        /// Format for changing a value by adding/subtracting to it. "+=#" adds # and "-=#" subtracts #.
        /// Where # is an integer.
        /// </summary>
        private const string ByQuantityPattern = @"^([+-])=(\d+)$";

        /// <summary>
        /// Format for changing a value to the number specified. "+#" and "#" sets it to positive #. "-#"
        /// sets it to negative #. Where # is an integer.
        /// </summary>
        private const string ToQuantityPattern = @"^([+-])?(\d+)$";

        /// <summary>
        /// Format for multiplying a value by an amount. "x#", wherer # is an integer.
        /// </summary>
        private const string MultiplyPattern = @"^x(\d+)$";

        /// <summary>
        /// Modifies the value passed in according to directions passed in.
        /// </summary>
        /// <param name="initial">The value to change</param>
        /// <param name="directions">How to change the value and by how much</param>
        /// <returns>The changed value</returns>
        public static float Change(float initial, string directions)
        {
            if (Regex.IsMatch(directions, ByPercentPattern))
                return ChangeByPercent(initial, directions);
            if (Regex.IsMatch(directions, ByQuantityPattern))
                return ChangeByQuantity(initial, directions);
            if (Regex.IsMatch(directions, ToQuantityPattern))
                return Single.Parse(directions);
            if (Regex.IsMatch(directions, MultiplyPattern))
                return MultiplyByAmount(initial, directions);

            throw new ArgumentException("Not a recognized change format", "directions");
        }

        private static float ChangeByPercent(float initial, string directions)
        {
            var match = Regex.Match(directions, ByPercentPattern, RegexOptions.Singleline);
            var increasing = match.Groups[1].Value == "+";
            var byPercent = Single.Parse(match.Groups[2].Value);
            var delta = (byPercent/100f)*initial*(increasing ? 1f : -1f);
            return initial + delta;
        }

        private static float ChangeByQuantity(float initial, string directions)
        {
            var match = Regex.Match(directions, ByQuantityPattern, RegexOptions.Singleline);
            var delta = Single.Parse(match.Groups[1].Value + match.Groups[2].Value);
            return initial + delta;
        }

        private static float MultiplyByAmount(float initial, string directions)
        {
            var match = Regex.Match(directions, MultiplyPattern, RegexOptions.Singleline);
            var multiplier = Single.Parse(match.Groups[1].Value);
            return initial*multiplier;
        }
    }
}
