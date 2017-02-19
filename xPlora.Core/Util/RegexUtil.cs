using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace xPlora.Core.Util
{
    public static class RegexUtil
    {
        // this regex tests if the token has measure > 0 [at least one VC].
        public static Regex MGr0 = new Regex("^(" + RegexPatterns.C + ")?" + RegexPatterns.V + RegexPatterns.C);

        // m equals 1: token has measure == 1
        public static Regex MEq1 = new Regex("^(" + RegexPatterns.C + ")?" + RegexPatterns.V + RegexPatterns.C + "(" + RegexPatterns.V + ")?$");

        // m greater than 1: token has measure > 1
        public static Regex MGr1 = new Regex(MGr0 + RegexPatterns.V + RegexPatterns.C);

        // vowel: token has a vowel after the first (optional) C
        public static Regex HasVowel = new Regex("^(" + RegexPatterns.C + ")?" + RegexPatterns.V);

        // double consonant: token ends in two consonants that are the same, 
        //			unless they are L, S, or Z. (look up "backreferencing" to help 
        //			with this)
        public static Regex HasDblConsonant = new Regex(@"([^aeiouylsz])\1$");

        // m equals 1, Cvc: token is in Cvc form, where the last c is not w, x, 
        //			or y.
        public static Regex MEq1cvcFormLastNotWXY = new Regex("^(" + RegexPatterns.C + ")+" + RegexPatterns.v + "[^aeiouwxy]$");

        /// <summary>
        /// 
        /// </summary>
        public static Regex RemoveNonAlphanumeric = new Regex("(^[^a-zA-Z0-9 -]*)|([^a-zA-Z0-9 -]*$)");

        public static Regex RemoveScriptTags = new Regex(@"(\\<script(.+?)\\</script\\>)|(\\<style(.+?)\\</style\\>)", RegexOptions.Singleline | RegexOptions.IgnoreCase);

        public static Regex RemoveHtmlTags = new Regex("(<.*?>\\s*)+", RegexOptions.Singleline);

        public static string ExtractHref(string source)
        {
            Regex filter = new Regex("hrf:'(.+?)'");
            var match = Regex.Match(source, "href='(.+?)'");
            string result = match.Groups[1].Value;
            return result;
        }
    }

    public static class RegexPatterns
    {
        // a single consonant
        public const string c = "[^aeiou]";
        // a single vowel
        public const string v = "[aeiouy]";

        // a sequence of consonants; the second/third/etc consonant cannot be 'y'
        public const string C = c + "[^aeiouy]*";
        // a sequence of vowels; the second/third/etc cannot be 'y'
        public const string V = v + "[aeiou]*";
    }

}
