using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace TargetCreation
{
    /// <summary>
    /// Provides functionality for finding and handling conditional sections (ifs) in a text.<br />
    /// An if starts with a conditional macro without macro value: ##cond?##. The condition must be a macro found in the defined macros.<br/>
    /// It is true if it evaluates to 'true' and it can be negated: ##!cond?##. Every text in the section is either left in the text or
    /// removeed from the text depending on the condition.<br />
    /// A conditional section is ended (endif) by the ##}## macro.<br />
    /// LF or CR LF directly behind the if and endif macros are removed.<br />
    /// Conditional sections can be nested.
    /// </summary>
    public class If
    {
        /// <summary>
        /// The start index of the if macro.
        /// </summary>
        private int StartInd;

        /// <summary>
        /// The length of the if macro.
        /// </summary>
        private int MacroLength;

        /// <summary>
        /// The evaluated condition.
        /// </summary>
        private bool Condition;


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="startInd">The index into the text where the if starts.</param>
        /// <param name="macroLength">The length of the if macro.</param>
        /// <param name="condition">The evaluated value of the condition.</param>
        protected If(
                 int  startInd,
                 int  macroLength,
                 bool condition)
        {
            StartInd = startInd;
            MacroLength = macroLength;
            Condition = condition;
        } // If


        /// <summary>
        /// Searches for and handles ifs (conditional sections) in the given text.
        /// </summary>
        /// <param name="sourceText">The text to process.</param>
        /// <param name="macroDictionary">Used for condition evaluation.</param>
        /// <returns>The processed text.</returns>
        public static string ApplyIfs(string sourceText, Dictionary<string, string> macroDictionary)
        {
            // All top level ifs that can be found in the given text are handled. This may require recursion for nested ifs.
            StringBuilder destText = new StringBuilder(sourceText);
            If outerIf;
            while ((outerIf = searchForIf(destText, 0, macroDictionary, true)) != null)
                handleIf(ref destText, macroDictionary, outerIf);
            // Return the manipulated string.
            return destText.ToString();
        } // ApplyIfs


        /// <summary>
        /// Searches for a conditional section in the specified text and evaluates the condition.
        /// </summary>
        /// <param name="destText">The text to search in.</param>
        /// <param name="searchStart">The search start index into the text.</param>
        /// <param name="macroDictionary">Used for evaluating the conditions.</param>
        /// <returns>An object containing information about the conditional section.</returns>
        private static If searchForIf(StringBuilder destText, int searchStart, Dictionary<string, string> macroDictionary, bool outerCond)
        {
            Match match;
            int ifSearchInd = searchStart;
            while (true)
            {
                // Try to find an if macro entry in the string. If there is none, we're done.
                // The regex search also finds conditional macros which we have to skip.
                match = Regex.Match(destText.ToString(ifSearchInd, destText.Length - ifSearchInd), @"\#\#(.*?)\#\#");
                if (!match.Success || match.Groups.Count == 0/* || match.Groups[1].Value[match.Groups[1].Value.Length - 1] == '}'*/)
                    return null;
                if (match.Groups[1].Value[match.Groups[1].Value.Length - 1] == '?')
                    break;
                ifSearchInd += match.Index + match.Length;
            }

            // Unless the given outer condition is false, evaluate the local condition.
            bool condition = outerCond;
            bool negateCondition = false;
            if (outerCond)
            {
                string condMacroName = match.Groups[1].Value.Substring(0, match.Groups[1].Value.Length - 1);
                if (condMacroName == null || condMacroName.Length == 0)
                    throw new Exception("The condition macro " + condMacroName + " has zero length.");

                // Shall we negate the evaluated condition?
                negateCondition = false;
                if (condMacroName[0] == '!')
                {
                    negateCondition = true;
                    condMacroName = condMacroName.Remove(0, 1);
                }

                // Find the condition macro in the dictionary.
                string conditionMacroValue;
                if (!macroDictionary.TryGetValue(condMacroName, out conditionMacroValue))
                    throw new Exception("The condition macro " + condMacroName + " is unknown.");
                condition = conditionMacroValue.ToLower() == "true";
            }

            // Create and return a new if control object.
            return new If(ifSearchInd + match.Index, match.Length, negateCondition ? !condition : condition);
        } // searchForIf


        /// <summary>
        /// Recursively handles nested ifs by either inserting the conditional sections or removing them.
        /// </summary>
        /// <param name="destText">The text which contains the conditional sections.</param>
        /// <param name="macroDictionary">The macro dictionary for evaluating the conditions.</param>
        /// <param name="prevIf">The if which shall be handled.</param>
        protected static void handleIf(ref StringBuilder destText, Dictionary<string, string> macroDictionary, If prevIf)
        {
            // We first start to search behind the end of the previous if.
            int searchStart = prevIf.StartInd + prevIf.MacroLength;

            int nextEndIfInd;
            while (true)
            {
                // Search for the next if.
                If nextIf = searchForIf(destText, searchStart, macroDictionary, prevIf.Condition);
                // Search for the next endif. If there is none, throw.
                nextEndIfInd = destText.ToString().IndexOf("##}##", searchStart);
                if (nextEndIfInd == -1)
                    throw new Exception("Unterminated if.");

                // If the next if is closer than the next endif, we have a nested if and call this function recursively.
                if (nextIf != null && nextIf.StartInd < nextEndIfInd)
                    handleIf(ref destText, macroDictionary, nextIf);
                else
                    break;
            }

            // If the previous if has been closed by an endif.
            // If the condition is true, remove the endif macro and then the if macro.
            // CR LF or LF directly after the if and endif are removed as well.
            if (prevIf.Condition)
            {
                destText.Remove(nextEndIfInd, 5 /* the length of ##}## */);
                removeCRLF(ref destText, nextEndIfInd);
                destText.Remove(prevIf.StartInd, prevIf.MacroLength);
                removeCRLF(ref destText, prevIf.StartInd);
            }
            // If the condition is false, remove the entire section between the if and endif, including the macros themselves.
            // CR LF or LF directly after the endif are removed as well.
            else
            {
                destText.Remove(prevIf.StartInd, nextEndIfInd + 5 /* the length of ##}## */ - prevIf.StartInd);
                removeCRLF(ref destText, prevIf.StartInd);
            }
        } // handleIf


        /// <summary>
        /// Removes either an LF or a CR LF from the given text at the specified index.
        /// </summary>
        /// <param name="destText">The text to process.</param>
        /// <param name="startInd">The spot in the text where to look for the charaters to remove.</param>
        private static void removeCRLF(ref StringBuilder destText, int startInd)
        {
            if (startInd < destText.Length && destText[startInd] == '\n')
                destText.Remove(startInd, 1);
            else if (startInd < destText.Length - 1 && destText[startInd] == '\r' && destText[startInd + 1] == '\n')
                destText.Remove(startInd, 2);
        } // removeCRLF
    } // class If
} // namespace TargetCreation 
