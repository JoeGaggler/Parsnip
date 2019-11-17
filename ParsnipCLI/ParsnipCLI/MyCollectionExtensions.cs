using System;
using System.Collections.Generic;
using System.Text;

namespace ParsnipCLI
{
    public static class MyCollectionExtensions
    {
        public static String Concat<T>(this T list) where T : IEnumerable<String> => String.Join(String.Empty, list);
    }
}
