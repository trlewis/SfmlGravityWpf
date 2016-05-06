namespace SfmlGravityWpf.Code.Helpers
{
    public static class IntHelper
    {
        /// <summary>
        /// Modulus that wraps negative numbers around to higher end of the range.
        /// Useful for going from the first element to the last by using -1 for
        /// <paramref name="number"/> and the length of the array as <paramref name="mod"/>.
        /// </summary>
        /// <param name="number">The number to mod, potentially turning positive.</param>
        /// <param name="mod">The mod value</param>
        public static int WrapMod(int number, int mod)
        {
            return ((number % mod) + mod) % mod;
        }
    }
}
