using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ArisStudio.Core
{
    /// <summary>
    /// Aris Studio command parser.
    /// </summary>
    public static class AsCommand
    {
        /// <summary>
        /// Parse a string of command.
        /// </summary>
        /// <param name="textCommand"></param>
        /// <returns>An array of string of those command.</returns>
        public static string[] Parse(string textCommand)
        {
            /*
            * Split command with space delimiter then return it as array.
            *
            * You can write a word without using single or double quote.
            * Example: txt Arona Hello
            *
            * But, you must use either single or double quote if
            * you write a sentence that have a 'Space' character.
            * Example: txt Arona 'Hello, Sensei.'
            *
            * You can write a single ( ' ) or double ( " ) quote character
            * in a word or a sentence by escaping the character...
            * Example: txt Arona 'It\'s yummy.'
            * or use the double quote as delimiter...
            * Example: txt Arona "It's yummy."
            * and vice versa.
            *
            * Source: txt 'Arona' "Arona" "I'm, Arona." 'Say, "Hello"' 'You\'re'
            * Result: txt | 'Arona' | "Arona" | "I'm, Arona." | 'Say, "Hello"' | 'You\'re'
            */

            const string pattern = @"('[^'\\]*(?:\\.[^'\\]*)*')|(\""[^""\\]*(?:\\.[^""\\]*)*"")|(\S+)";
            String[] splittedCommand = Regex
                .Matches(textCommand, pattern)
                .Cast<Match>()
                .Select(m => m.Value)
                .ToArray();

            /*
            * Normalize splitted text by removing either single or
            * double quote at the start and the end of the word.
            * Also Unescape escaped character.
            *
            * Using string.Trim() resulting in some unexpected result,
            * so we reconstruct it using string.Substring().
            *
            * Source: txt 'Arona' "Arona" "I'm, Arona." 'Say, "Hello"' 'You\'re'
            * Result: txt | Arona | Arona | I'm, Arona. | Say, "Hello" | You're
            */

            List<string> finalCommand = new List<string>();

            foreach (String item in splittedCommand)
            {
                String cmd = item; // Clone the text first, so we can do other operations

                // If the current text is "// (comment)", exit iteration.
                if (cmd.Trim() == "//")
                    break;

                if ((item.StartsWith("'") && item.EndsWith("'")) || (item.StartsWith("\"") && item.EndsWith("\"")))
                {
                    cmd = item.Substring(1, item.Length - 2); // Reconstruct the text
                    cmd = Regex.Unescape(cmd); // Unescape escaped character in text
                }

                finalCommand.Add(cmd);
            }

            return finalCommand.ToArray();
        }
    }
}
