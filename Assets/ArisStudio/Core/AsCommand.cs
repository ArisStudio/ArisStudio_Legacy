using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ArisStudio.Core
{
    public static class AsCommand
    {
        public static string[] Parse(string textCommand)
        {
            /*
            * Split command with space delimiter then return as array.
            *
            * You can write a word without using single or double quote.
            * Example: txt Arona Hello
            *
            * But, you must use either single or double quote if
            * you write a sentence that have 'Space' character.
            * Example: txt Arona 'Hello, Sensei.'
            *
            * You can write a single or double quote in a word or
            * sentence by escaping the character: txt Arona 'It\'s yummy.'
            * or use double quote as delimiter: txt Arona "It's yummy."
            * and vice versa.
            *
            * Source: txt 'Arona' "Arona" "I'm, Arona." 'Say, "Hello"' 'You\'re'
            * Result: ["txt", "'Arona'", "\"Arona\"", "\"I'm, Arona.\"", "'Say, "Hello"'", "'You\'re'"]
            */
            const string pattern = @"('[^'\\]*(?:\\.[^'\\]*)*')|(\""[^""\\]*(?:\\.[^""\\]*)*"")|(\S+)";
            String[] textSplit = Regex
                .Matches(textCommand, pattern)
                .Cast<Match>()
                .Select(m => m.Value)
                .ToArray();

            /*
            * Normalize splitted text by removing either single or
            * double quote at the start and end of the word.
            * Also Unescape escaped character.
            *
            * Using string.Trim() resulting in unexpected result, so
            * I reconstruct it using string.Substring().
            *
            * Source: txt 'Arona' "Arona" "I'm, Arona." 'Say, "Hello"' 'You\'re' // this is comment
            * Result: ["txt", "Arona", "Arona", "I'm, Arona.", "Say, "Hello"", "You're"]
            */
            var finalCommand = new List<string>();

            foreach (String str in textSplit)
            {
                String cmd = str;

                if (cmd.Trim() == "//")
                {
                    break;
                }

                if ((str.StartsWith("'") && str.EndsWith("'")) || (str.StartsWith("\"") && str.EndsWith("\"")))
                {
                    cmd = str.Substring(1, str.Length - 2);
                    cmd = Regex.Unescape(cmd);
                }

                finalCommand.Add(cmd);
            }

            return finalCommand.ToArray();
        }
    }
}
