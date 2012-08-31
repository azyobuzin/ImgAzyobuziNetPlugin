using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Acuerdo.External.Uploader;

namespace ImgAzyobuziNetPlugin
{
    public class Resolver : IResolver
    {
        static Resolver()
        {
            RegexList = new List<Regex>();
        }

        public static List<Regex> RegexList { get; private set; }

        public bool IsResolvable(string url)
        {
            return RegexList.Any(r => r.IsMatch(url));
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
