using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode
{
    internal static class ParseExtension
    {
        internal static bool TryAddArgs(this Dictionary<string, string> dict, List<string> args, string key)
        {
            if (args.Contains(key))
            {
                var keyIndex = args.IndexOf(key);
                if (keyIndex < args.Count - 1)
                {
                    dict[key] = args[keyIndex + 1];
                    return true;
                }
            }
            return false;
        }
    }
}
