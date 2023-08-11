using System;
using System.Collections.Generic;

namespace HoleyCodegenPoC.Lib
{
    public static class Lookup
    {
        private static readonly IDictionary<string, string> entries = new Dictionary<string, string>();

        public static void AddEntries(IDictionary<string, string> additionalEntries)
        {
            foreach (var entry in additionalEntries)
                entries.Add(entry.Key, entry.Value);
        }

        public static bool TryGetValue(string key, out string value) => entries.TryGetValue(key, out value);
    }
}
