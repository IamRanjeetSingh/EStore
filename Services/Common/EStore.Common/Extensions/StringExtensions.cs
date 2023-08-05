namespace EStore.Common.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveSuffix(this string thisString, string suffix)
        {
            if (string.IsNullOrEmpty(suffix))
                throw new ArgumentException($"'{nameof(suffix)}' is null or empty.", paramName: nameof(suffix));

            //suffix is definitely not available in thisString
            if (suffix.Length > thisString.Length)
                return thisString;

            int suffixStartIndexInThisString = thisString.Length - suffix.Length;
            string suffixSubstringInThisString = thisString.Substring(suffixStartIndexInThisString, suffix.Length);

            bool hasSuffix = string.Equals(suffix, suffixSubstringInThisString);

            if (!hasSuffix)
                return thisString;

            string thisStringWithoutSuffix = thisString.Substring(0, thisString.Length - suffix.Length);

            return thisStringWithoutSuffix;
        }
    }
}
