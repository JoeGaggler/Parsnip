using System;

namespace ParsnipCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = "A123";
            var span = input.AsSpan();
            var parser = new MyParser();
            var result = parser.Parse(span, new Factory());
        }

        internal class Factory : IMyParserFactory
        {
        }
    }
}
