using System;
using System.Buffers;
using System.Text;
using System.Linq;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace RuneSamples
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\n----- Print string chars");
            PrintStringChars();

            Console.WriteLine("\n----- Convert string to uppercase");
            ConvertToUpper();

            Console.WriteLine("\n----- Split string on character count");
            InsertNewlines();

            Console.WriteLine("----- Instantiate runes");
            InstantiateRunes();

            GetValueOfRune();
            Console.WriteLine("-----");

            char[] s = "!Hello!".ToCharArray();
            ReadOnlySpan<char> span = new ReadOnlySpan<char>(s);
            Console.WriteLine(span.ToString());
            ReadOnlySpan<char> newSpan = TrimNonLettersAndNonDigits(span);
            Console.WriteLine(newSpan.ToString());
            Console.WriteLine("-----");

            ConvertToUtf8Utf16(new Rune("🐂".ToCharArray()[0], "🐂".ToCharArray()[1]));
            SplitStringOnCharValue();

            Console.WriteLine("----- Count text elements");
            CountTextElements();

        }
        public static void PrintStringChars()
        {
            // <SnippetPrintStringChars>
            // <SnippetHello>
            PrintChars("Hello");
            // </SnippetHello>
            // <SnippetNiHao>
            PrintChars("你好");
            // </SnippetNiHao>
            // <SnippetOsage>
            PrintChars("𐓏𐓘𐓻𐓘𐓻𐓟 𐒻𐓟");
            // </SnippetOx>
            PrintChars("🐂");
            // </SnippetOx>

            // <SnippetPrintChars>
            static void PrintChars(string s)
            {
                Console.WriteLine($"\"{s}\".Length = {s.Length}");
                for (int i = 0; i < s.Length; i++)
                {
                    Console.WriteLine($"s[{i}] = '{s[i]}' ('\\u{(int)s[i]:x4}')");
                }
                Console.WriteLine();
            }
            // </SnippetPrintChars>
            // </SnippetPrintStringChars>
        }

        public static void ConvertToUpper()
        {

            // <SnippetConvertToUpper>
            string testString = "abc𐑉";
            Console.WriteLine($"String to be converted to uppercase: {testString}");
            PrintChars(testString);

            string testStringUppercase = ConvertToUpper(testString);
            Console.WriteLine($"String converted to uppercase using correct code: {testStringUppercase}");
            PrintChars(testStringUppercase);

            testStringUppercase = ConvertToUpperBadExample(testString);
            Console.WriteLine($"String converted to uppercase using incorrect code: {testStringUppercase}");
            PrintChars(testStringUppercase);

            // <SnippetConvertToUpperGoodExample>
            static string ConvertToUpper(string input)
            {
                StringBuilder builder = new StringBuilder(input.Length);
                foreach (Rune rune in input.EnumerateRunes())
                {
                    builder.Append(Rune.ToUpperInvariant(rune));
                }
                return builder.ToString();
            }
            // </SnippetConvertToUpperGoodExample>

            // <SnippetConvertToUpperBadExample>
            // THE FOLLOWING METHOD SHOWS INCORRECT CODE.
            // DO NOT DO THIS IN A PRODUCTION APPLICATION.
            static string ConvertToUpperBadExample(string input)
            {
                StringBuilder builder = new StringBuilder(input.Length);
                for (int i = 0; i < input.Length; i++) /* or 'foreach' */
                {
                    builder.Append(char.ToUpperInvariant(input[i]));
                }
                return builder.ToString();
            }
            // </SnippetConvertToUpperBadExample>

            static void PrintChars(string s)
            {
                Console.WriteLine($"\"{s}\".Length = {s.Length}");
                for (int i = 0; i < s.Length; i++)
                {
                    Console.WriteLine($"s[{i}] = '{s[i]}' ('\\u{(int)s[i]:x4}')");
                }
                Console.WriteLine();
            }
            Console.WriteLine("-----");
            // <SnippetConvertToUpper>

        }


        public static void InstantiateRunes()
        {
            Rune a = new Rune('a'); // OK, 'a' (U+0061) is a valid scalar value.
            Console.WriteLine($"Rune a: {a}");
            Rune b = new Rune(0x0061); // OK, this is a valid scalar value.
            Console.WriteLine($"Rune b: {b}");
            try
            {
                Rune c = new Rune('\ud801'); // Throws, U+D801 is not a valid scalar value.
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Rune c: Exception: {ex.Message}");
            }
            Rune d = new Rune(0x10421); // OK, this is a valid scalar value.
            Console.WriteLine($"Rune d: {d}");
            Rune e = new Rune('\ud801', '\udc21'); // OK, this is equivalent to the above.
            Console.WriteLine($"Rune e: {e}");
            try
            {
                Rune f = new Rune(0x12345678); // Throws, outside the range of valid scalar values.
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Rune f: Exception: {ex.Message}");
            }
        }

        // Using char.IsLetter(char) could produce incorrect results.
        public static bool StringConsistsEntirelyOfLetters(string input)
        {
            foreach (Rune rune in input.EnumerateRunes())
            {
                if (!Rune.IsLetter(rune))
                {
                    return false;
                }
            }
            return true;
        }

        public static void InstantiateRunes2()
        {
            // The calls below all create a Rune with value U+20AC EURO SIGN ('€')
            Rune a = new Rune('€');
            Console.WriteLine($"Rune a: {a}");
            Rune b = new Rune('\u20ac');
            Console.WriteLine($"Rune b: {b}");
            Rune c = new Rune(0x20AC);
            Console.WriteLine($"Rune c: {c}");

            // The calls below all create a Rune with value U+1F52E CRYSTAL BALL ('🔮')
            Rune d = new Rune('\ud83d', '\udd2e');
            Console.WriteLine($"Rune d: {d}");
            Rune e = new Rune(0x1F52E);
            Console.WriteLine($"Rune e: {e}");
        }

        public static void GetValueOfRune()
        {
            Rune rune = new Rune('\ud83d', '\udd2e'); // U+1F52E CRYSTAL BALL ('🔮')
            int codePoint = rune.Value; // = 128302 decimal (= 0x1F52E hexadecimal)
            Console.WriteLine($"Crystal ball code point: {codePoint}");
        }
        public static ReadOnlySpan<char> TrimNonLettersAndNonDigits(ReadOnlySpan<char> span)
        {
            // First, trim from the front. If any Rune can't be decoded (return value is anything other
            // than "Done"), or if the Rune is a letter or digit, stop trimming from the front and
            // instead work from the end.
            while (Rune.DecodeFromUtf16(span, out Rune rune, out int charsConsumed) == OperationStatus.Done)
            {
                if (Rune.IsLetterOrDigit(rune))
                { break; }
                span = span[charsConsumed..];
            }

            // Next, trim from the end. If any Rune can't be decoded, or if the Rune is a letter or digit,
            // break from the loop, and we're finished.
            while (Rune.DecodeLastFromUtf16(span, out Rune rune, out int charsConsumed) == OperationStatus.Done)
            {
                if (Rune.IsLetterOrDigit(rune))
                { break; }
                span = span[..^charsConsumed];
            }

            return span; // this is now trimmed on both sides
        }

        public static void ConvertToUtf8Utf16(Rune rune)
        {
            Console.WriteLine($"Rune value: {rune.Value}");

            // Convert to UTF-16 char[]
            char[] chars = new char[rune.Utf16SequenceLength];
            int numCharsWritten = rune.EncodeToUtf16(chars);
            Console.WriteLine($"Number of chars: {numCharsWritten}");

            // Shortcut to convert to UTF-16 string
            string theString = rune.ToString();

            // Convert to UTF-8 byte[]
            byte[] bytes = new byte[rune.Utf8SequenceLength];
            int numBytesWritten = rune.EncodeToUtf8(bytes);
            Console.WriteLine($"Number of bytes: {numBytesWritten}");

        }

        // THE FOLLOWING METHOD SHOWS INCORRECT CODE.
        // DO NOT DO THIS IN A PRODUCTION APPLICATION.
        public static int CountLettersInStringBadExample(string s)
        {
            int letterCount = 0;

            foreach (char ch in s)
            {
                if (char.IsLetter(ch))
                { letterCount++; }
            }

            return letterCount;
        }

        // THE FOLLOWING METHOD SHOWS INCORRECT CODE.
        // DO NOT DO THIS IN A PRODUCTION APPLICATION.
        public static int CountLettersInSpanBadExample(ReadOnlySpan<char> span)
        {
            int letterCount = 0;

            foreach (char ch in span)
            {
                if (char.IsLetter(ch))
                { letterCount++; }
            }

            return letterCount;
        }

        // This sample shows correct usage of the Rune type.
        public static int CountLettersInString(string s)
        {
            int letterCount = 0;

            foreach (Rune rune in s.EnumerateRunes())
            {
                if (Rune.IsLetter(rune))
                { letterCount++; }
            }

            return letterCount;
        }

        // This sample shows correct usage of the Rune type.
        public static int CountLettersInSpan(ReadOnlySpan<char> span)
        {
            int letterCount = 0;

            foreach (Rune rune in span.EnumerateRunes())
            {
                if (Rune.IsLetter(rune))
                { letterCount++; }
            }

            return letterCount;
        }

        // Example of code which performs manual char surrogate checks.
        public static void ProcessStringHandleSurrogates(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (!char.IsSurrogate(s[i]))
                {
                    ProcessCodePoint(s[i]);
                }
                else if (i + 1 < s.Length && char.IsSurrogatePair(s[i], s[i + 1]))
                {
                    int codePoint = char.ConvertToUtf32(s[i], s[i + 1]);
                    ProcessCodePoint(codePoint);
                    i++; // so that when the loop iterates it's actually +2
                }
                else
                {
                    throw new Exception("String was not well-formed UTF-16.");
                }
            }
        }

        private static void ProcessCodePoint(int codePoint) { /* ... */ }

        // Example of code which uses Rune instead of performing manual char surrogate checks.
        public static void ProcessStringUseRunes(string s)
        {
            for (int i = 0; i < s.Length;)
            {
                if (!Rune.TryGetRuneAt(s, i, out Rune rune))
                {
                    throw new Exception("String was not well-formed UTF-16.");
                }

                ProcessCodePoint(rune.Value);
                i += rune.Utf16SequenceLength; // increment the iterator by the number of chars in this Rune
            }
        }
        // This code returns the index of the first char 'A' - 'Z' that appears in the string.
        // It demonstrates valid usage of the char type.
        public static int GetIndexOfFirstAToZ(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                char thisChar = s[i];
                if ('A' <= thisChar && thisChar <= 'Z')
                {
                    return i; // found a match
                }
            }

            return -1; // didn't find 'A' - 'Z' in the input string
        }

        public static void SplitStringOnCharValue()
        {
            // These lines demonstrate valid usage of the string.Split method and the char type.

            string inputString = "🐂, 🐄, 🐆";
            string[] splitOnSpace = inputString.Split(' ');
            Array.ForEach(splitOnSpace, s => Console.WriteLine(s));
            string[] splitOnComma = inputString.Split(',');
            Array.ForEach(splitOnComma, s => Console.WriteLine(s));
        }
    }
}
