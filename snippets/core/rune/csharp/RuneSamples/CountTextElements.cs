using System;
using System.Globalization;
using System.Linq;

namespace RuneSamples
{
    partial class Program
    {
        public static void CountTextElements()
        {
            // <SnippetSample1>
            PrintTextElementCount("á");
            // Number of chars: 1
            // Number of runes: 1
            // Number of text elements: 1
            // </SnippetSample1>

            // <SnippetSample2>
            PrintTextElementCount("á");
            // Number of chars: 2
            // Number of runes: 2
            // Number of text elements: 1
            // </SnippetSample2>

            // <SnippetSample3>
            PrintTextElementCount("👩🏽‍🚒");
            // Number of chars: 7
            // Number of runes: 4
            // Number of text elements: 1
            // </SnippetSample3>

            // <SnippetTextElementMethod>
            static void PrintTextElementCount(string s)
            {
                Console.WriteLine(s);
                Console.WriteLine($"Number of chars: {s.Length}");
                Console.WriteLine($"Number of runes: {s.EnumerateRunes().Count()}");

                TextElementEnumerator enumerator = StringInfo.GetTextElementEnumerator(s);

                int textElementCount = 0;
                while (enumerator.MoveNext())
                {
                    textElementCount++;
                }

                Console.WriteLine($"Number of text elements: {textElementCount}");
                // </SnippetTextElementMethod>
            }
        }
    }
}
