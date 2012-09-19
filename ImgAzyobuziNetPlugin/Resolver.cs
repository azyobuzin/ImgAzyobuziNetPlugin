using System;
using System.Linq;
using System.Text.RegularExpressions;
using Acuerdo.External.Uploader;

namespace ImgAzyobuziNetPlugin
{
    public class Resolver : IResolver
    {
        public static Regex[] RegexArray = new Regex[] { };

        public bool IsResolvable(string url)
        {
            return RegexArray.Any(r => r.IsMatch(url));
        }

        public string Resolve(string url)
        {
            if (IsResolvable(url))
                return "http://img.azyobuzi.net/api/redirect?size=large&uri=" + Uri.EscapeDataString(url);
            else
                return null;
        }
    }
}
