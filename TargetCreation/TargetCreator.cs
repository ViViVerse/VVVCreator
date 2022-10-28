using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace TargetCreation
{
    public class TargetCreator
    {
        /// <summary>
        /// The modifications which can be applied to macro values.
        /// </summary>
        private enum ModifierType
        {
            /// <summary>No modifier.</summary> 
            None,
            /// <summary>Make the entire string uppercase.</summary>
            UpperCase,
            /// <summary>Make the entire string lowercase.</summary>
            LowerCase,
            /// <summary>Make the entire string lowercase and put an underscore in front of each uppercase letter,
            /// except at the beginning and after other uppercase letters.</summary>
            Underscore,
            /// <summary>Make the first character upperscore, replace any underscore and make the following character upperscore.</summary>
            Deunderscore
        } // enum ModifierType


        /// <summary>
        /// Creates the target with the contents of the given template folder and the specified macro definitions.
        /// </summary>
        /// <param name="templateFolder">The folder which contains the template folders and files. It itself will not be copied</param>
        /// <param name="macroDefinitions">The macro dictionary used for substitution.</param>
        /// <param name="targetFolder">The target folder to which the contents of the template folder will be copied.</param>
        public static void CreateTarget(string templateFolder, List<MacroDefinition> macroDefinitions, string targetFolder)
        {
            // Create a macro dictionary from the definitions
            Dictionary<string, string> macroDictionary = new Dictionary<string, string>();
            foreach (MacroDefinition macroDef in macroDefinitions)
                macroDictionary.Add(macroDef.Name, macroDef.Value);

            //string t = "abc\r\n##INS?##\r\ndef\r\n##INS?##123##}##ghi\r\n##}##\r\n##INS?KLM####INS?##XYZ##}##\r\n";
            //string st = If.ApplyIfs(t, macroDictionary);

            // Copy the entire template folder to the target folder.
            // While doing this, substitute the macros in folder and file specs and the file contents.
            copyAndProcessFilesRecursively(new DirectoryInfo(templateFolder), new DirectoryInfo(targetFolder), macroDictionary);
        } // CreateTarget


        /// <summary>
        /// Recursively copies a directory tree. Taken from https://stackoverflow.com/questions/58744/copy-the-entire-contents-of-a-directory-in-c-sharp.
        /// While copying, macros in folder and file names as well as in file contents are processed.
        /// </summary>
        /// <param name="sourceFolder">The source folder. On the initial call from CreateTarget, this is the 'Content' folder in the template folder.</param>
        /// <param name="targetFolder">The target folder. On the initial call from CreateTarget, this is the folder to which
        /// the contents of the template folder will be copied.</param>
        /// <param name="macroDictionary">Contains macro name/macro value pairs. Used for macro substitution.</param>
        private static void copyAndProcessFilesRecursively(DirectoryInfo sourceFolder, DirectoryInfo targetFolder, Dictionary<string, string> macroDictionary)
        {
            // Recursively handle subfolders. Since folder names may have macros within them, substitute these macros before folder creation
            foreach (DirectoryInfo dir in sourceFolder.GetDirectories())
            {
                // Folder names may have macro entries which tell whether to create the folder or not or may change the names.
                string folderName;
                if ((folderName = shallWeCreateFileOrFolder(dir.Name, macroDictionary)).Length > 0 &&
                    (folderName = substituteMacros(folderName, macroDictionary)).Length > 0)
                {
                    copyAndProcessFilesRecursively(dir, targetFolder.CreateSubdirectory(folderName), macroDictionary);
                }
            } // foreach (DirectoryInfo dir

            // Process files: copy the files and substitute macros in their names and contents. A condition macro can tell whether to create the file.
            // In file contents, conditional sections (ifs) are handled before macro substitution.
            foreach (FileInfo file in sourceFolder.GetFiles())
            {
                try
                {
                    string targetFileName;
                    if ((targetFileName = shallWeCreateFileOrFolder(file.Name, macroDictionary)).Length > 0 &&
                        (targetFileName = substituteMacros(targetFileName, macroDictionary)).Length > 0)
                    {
                        System.IO.File.WriteAllText(
                                         Path.Combine(targetFolder.FullName, targetFileName),
                                         substituteMacros(If.ApplyIfs(System.IO.File.ReadAllText(file.FullName), macroDictionary), macroDictionary));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(file.Name + ": ", ex);
                }
            } // foreach (FileInfo file
        } // copyAndProcessFilesRecursively


        /// <summary>
        /// Searches for a conditional macro in the given file or folder name and evaluates it.<br />
        /// If it evaluates to false, an empty string is returned. If it evaluates to true, the given file or folder name is returned with the
        /// conditional macro removed. The function searches only for the first macro in the string, so the condition must come first.<br />
        /// To differentiate it from a normal conditional macro, it must not have a macro name in it: ##USE_FILE¿##<br />
        /// The ¿ character can be obtained by using ALT + 168 on the number block. A '!' character at the beginning of the condition negates the
        /// evaluated value.
        /// </summary>
        /// <param name="fileOrFolderName">A file or folder name which may contain a conditional macro.</param>
        /// <param name="macroDictionary">Contains macro name/macro value pairs. Used for condition macro evaluation.</param>
        /// <returns>An empty string if a conditional macro without macro name is found and evaluated to false.</returns>
        private static string shallWeCreateFileOrFolder(string fileOrFolderName, Dictionary<string, string> macroDictionary)
        {
            // Search for a macro entry. If none is found, return the file or folder name without changes.
            Match match = Regex.Match(fileOrFolderName, @"\#\#(.*?)\#\#");
            if (!match.Success || match.Groups.Count == 0)
                return fileOrFolderName;

            // Get the macro. If it is not conditional or has a name, return the file or folder name without changes.
            string macroName = match.Groups[1].Value;

            // Check if the given macro name contains a condition. This is indicated by the '?' or '¿' characters in the macro name.
            // Everything in front of the character is regarded as the name of another macro.
            int condEnd = macroName.IndexOf('¿');
            if (condEnd == -1)
                return fileOrFolderName;
            string condMacroName = macroName.Substring(0, condEnd);
            if (condMacroName == null || condMacroName.Length == 0)
                throw new Exception("The condition macro " + condMacroName + " has zero length.");

            // Shall we negate the evaluated condition?
            bool negateCondition = false;
            if (condMacroName[0] == '!')
            {
                negateCondition = true;
                condMacroName = condMacroName.Remove(0, 1);
            }

            // Find the condition macro in the dictionary.
            string conditionMacroValue;
            if (!macroDictionary.TryGetValue(condMacroName, out conditionMacroValue))
                throw new Exception("The condition macro " + condMacroName + " is unknown.");

            // Evaluate the condition. We only return true here if the lowercase value of the macro is 'true'.
            // If wanted negate the condition.
            bool condition = conditionMacroValue.ToLower() == "true";
            if (negateCondition)
                condition = !condition;

            // Return the file or folder name with the condition removed.
            return condition ? fileOrFolderName.Remove(match.Index, match.Length) : "";
        } // shallWeCreateFileOrFolder


        /// <summary>
        /// Substitutes all macro entries (given by ##Macro name##) in the given string with their values from the specified dictionary.<br />
        /// If an unknown macro is found an exception is thrown.
        /// </summary>
        /// <param name="sourceText">The input string containing macro entries.</param>
        /// <param name="macroDictionary">The macro distionary used for substitution.</param>
        /// <returns>The input string with all macros substituted.</returns>
        private static string substituteMacros(string sourceText, Dictionary<string, string> macroDictionary)
        {
            StringBuilder destText = new StringBuilder(sourceText);

            // Loop through the text using regex.
            int searchStart = 0;
            while (true)
            {
                // Try to find a macro entry in the string. If there is none, we're done.
                Match match = Regex.Match(destText.ToString(searchStart, destText.Length - searchStart), @"\#\#(.*?)\#\#");
                if (!match.Success || match.Groups.Count == 0)
                    break;

                // Get the macro name from the search result. The macro name may contain a condition or a modifier code which will be removed below.
                string macroName = match.Groups[1].Value;

                // Determine whether the macro shall be used. If there is a condition and it evaluates to true, it is removed in this step.
                bool useMacro = findAndRemoveCondition(ref macroName, macroDictionary);

                string macroValue;
                if (useMacro)
                {
                    // Determine whether the macro shall be modified.
                    ModifierType modType = findAndRemoveModifier(ref macroName);

                    // Find the value for the macro name. We do have standard hardcoded macros and custom macros from the dictionary.
                    if (macroName == "GUID") // {00000000-0000-0000-0000-000000000000}
                    {
                        macroValue = Guid.NewGuid().ToString("B");
                    }
                    else if (macroName == "CODE_GUID") // 00000000, 0000, 0000, 0000, 000000000000
                    {
                        macroValue = Guid.NewGuid().ToString("D").Replace("-", ", ").ToUpper();
                    }
                    else if (macroName == "DATE")
                    {
                        macroValue = System.DateTime.Now.ToString("yyyy-MM-dd");
                    }
                    else if (macroName == "YEAR")
                    {
                        macroValue = System.DateTime.Now.ToString("yyyy");
                    }
                    else
                    {
                        if (!macroDictionary.TryGetValue(macroName, out macroValue))
                            throw new Exception("The macro " + macroName + " is unknown.");
                    }

                    // Apply the detected modifier to the macro value.
                    macroValue = applyModifier(macroValue, modType);
                }
                // The macro shall not be used. Set it to an empty string which will replace the macro entry.
                else
                {
                    macroValue = "";
                }

                // Replace the macro entry with its value.
                destText = destText.Replace(match.Value, macroValue, searchStart + match.Index, match.Value.Length);
                // Adjust the start value for the next search: start it behind the macro value we have just inserted.
                searchStart += match.Index + macroValue.Length;
            } // while (true)

            return destText.ToString();
        } // substituteMacros


        /// <summary>
        /// Checks whether the given macro name contains a condition.<br />
        /// A condition is indicated by a '?' character. Everything in front of this character is interpreted as the name of another macro whose
        /// value must either be 'true' or 'false' (case insensitve).<br />
        /// The condition must be at the beginning of a macro entry. The condition value can be negated by a '!' in front of the condition
        /// macro name. Because '?' is not allowed in file specs, '¿' (ALT + 168 on the number block) can be used alternatively.<br />
        /// If the condition macro cannot be found in the dictionary an exception is thrown.<br />
        /// Examples:<br />
        ///           ##USE_NAME?NAME##<br />
        ///           ##!DO_NOT_USE_NAME?NAME##<br />
        /// </summary>
        /// <param name="macroName">On entry this contains the macro name and maybe a condition. On exit, the condition will have been removed.</param>
        /// <param name="macroDictionary">The macro dictionary. It is used to find the value of the condition macro.</param>
        /// <returns>True if the given macro name either does not contain a condition or if the condition evaluates to true.</returns>
        private static bool findAndRemoveCondition(ref string macroName, Dictionary<string, string> macroDictionary)
        {
            // Check if the given macro name contains a condition. This is indicated by the '?' or '¿' characters in the macro name.
            // Everything in front of the character is regarded as the name of another macro.
            int condEnd = macroName.IndexOfAny( new char[]{'?', '¿'});
            if (condEnd == -1)
                return true;
            string condMacroName = macroName.Substring(0, condEnd);
            if (condMacroName == null || condMacroName.Length == 0)
                throw new Exception("The condition macro " + condMacroName + " has zero length.");

            // Shall we negate the evaluated condition?
            bool negateCondition = false;
            if (condMacroName[0] == '!')
            {
                negateCondition = true;
                condMacroName = condMacroName.Remove(0, 1);
            }

            // Remove the condition from the macro entry.
            macroName = macroName.Substring(condEnd + 1, macroName.Length - condEnd - 1);

            // Find the condition macro in the dictionary.
            string conditionMacroValue;
            if (!macroDictionary.TryGetValue(condMacroName, out conditionMacroValue))
                throw new Exception("The condition macro " + condMacroName + " is unknown.");

            // Evaluate the condition. We only return true here if the lowercase value of the macro is 'true'.
            // If wanted negate the condition.
            bool condition = conditionMacroValue.ToLower() == "true";
            return negateCondition ? !condition : condition;
        } // findAndRemoveCondition


        /// <summary>
        /// If the given macro name contains a modifier, indicated by a '(' (like the opening parenthesis of a function),<br />
        /// the modfier is removed from the macro name. If the modifier is unknown an exception is thrown.
        /// </summary>
        /// <param name="macroName">On entry, this contains the macro name and maybe a modifier. On exit, the modifier will have been removed.</param>
        /// <returns>The modifier type.</returns>
        private static ModifierType findAndRemoveModifier(ref string macroName)
        {
            // Check if the given macro name contains a modifier. This is indicated by the '(' character in the macro name.
            // Everything in front of the modifier is regarded as modifier.
            int modEnd = macroName.IndexOf('(');
            if (modEnd == -1)
                return ModifierType.None;
            string modifier = macroName.Substring(0, modEnd);
            macroName = macroName.Substring(modEnd + 1, macroName.Length - modEnd - 1);

            // Determine the modifier type and apply the modifier.
            ModifierType modType;
            // Uppercase.
            if (modifier == "uc")
                modType = ModifierType.UpperCase;
            // Lowercase.
            else if (modifier == "lc")
                modType = ModifierType.LowerCase;
            // Underscores and lowercase instead of uppercase.
            else if (modifier == "us")
                modType = ModifierType.Underscore;
            // Underscores are removed and the following lowercase character is replaced by uppercase.
            else if (modifier == "dus")
                modType = ModifierType.Deunderscore;
            // Unknown modifier.
            else
                throw new Exception("The modifier " + modifier + " is unknown.");

            return modType;
        } // handleModifier


        /// <summary>
        /// Applies the specified modifier to the given macro value.
        /// </summary>
        /// <param name="macroValue">The macro value.</param>
        /// <param name="modType">The modifier.</param>
        /// <returns>The resulting string.</returns>
        private static string applyModifier(string macroValue, ModifierType modType)
        {
            switch (modType)
            {
                case ModifierType.None:
                    return macroValue;
                case ModifierType.UpperCase:
                    return macroValue.ToUpper();
                case ModifierType.LowerCase:
                    return macroValue.ToLower();
                case ModifierType.Underscore:
                    return toUnderscore(macroValue);
                case ModifierType.Deunderscore:
                    return toDeunderscore(macroValue);
            }
            throw new Exception("The modifier " + modType.ToString() + " is unknown.");
        } // applyModifier


        /// <summary>
        /// Translates all uppercase characters in the given string into a leading underscore and the respective lowercase version.<br />
        /// Contiguous blocks of uppercase characters are converted to lowercase, but only the first and last characters get an underscore put in front.<br />
        /// If the string starts with an uppercase character, it is converted to lowercase without prepending an underscore.<br />
        /// Digits are converted into an underscore and the digit. For consecutive digits, only one underscore is put in front of the first digit.
        /// </summary>
        /// <param name="InText">The input text.</param>
        /// <returns>The manipulated text.</returns>
        private static string toUnderscore(string InText)
        {
            // If the given string is empty, return it
            if (InText.Length == 0)
                return InText;

            // Make sure the first character of the given string is lower case (to avoid putting an underscore in front of it).
            StringBuilder outText = new StringBuilder(InText);
            int ind = 0;
            int prevUppercaseInd = -1;
            int prevDigitInd = -1;
            int upperCaseGroupSize = 0;
            while (ind < outText.Length && Char.IsUpper(outText[ind]))
            {
                outText[ind] = Char.ToLower(outText[ind]);
                prevUppercaseInd = ind;
                ind++;
                upperCaseGroupSize++;
            }

            // Loop through the rest of the text.
            while (ind < outText.Length)
            {
                // If the current character is an uppercase character.
                if (Char.IsUpper(outText[ind]))
                {
                    outText[ind] = Char.ToLower(outText[ind]);
                    if (ind != prevUppercaseInd + 1)
                        outText.Insert(ind++, '_');
                    prevUppercaseInd = ind;
                    upperCaseGroupSize++;
                    ++ind;
                }
                // If the current character is a digit.
                else if (Char.IsDigit(outText[ind]))
                {
                    // Insert an underscore in front of the first digit.
                    if (ind != prevDigitInd + 1)
                        outText.Insert(ind++, '_');
                    prevDigitInd = ind;
                    ++ind;
                }
                // If the current character is a lowercase character.
                else
                {
                    // If this is the first lowercase character after an uppercase characters.
                    if (upperCaseGroupSize > 0)
                    {
                        // If this is the first lowercase character after a group of uppercase characters,
                        // insert an underscore in front of the last uppercase character in the group.
                        if (upperCaseGroupSize > 1)
                            outText.Insert(ind++ - 1, '_');
                        // Reset the group size.
                        upperCaseGroupSize = 0;
                    }
                    ++ind;
                }
            } // while (ind < outText.Length

            return outText.ToString();
        } // toUnderscore

        /// <summary>
        /// Translates the first character of the string to uppercase. Then removes all underscores from the string and translates the character following
        /// the underscore to uppercase.
        /// </summary>
        /// <param name="InText">The input text.</param>
        /// <returns>The manipulated text.</returns>
        private static string toDeunderscore(string InText)
        {
            // If the given string is empty, return it
            if (InText.Length == 0)
                return InText;

            // Translate the first character to uppercase.
            StringBuilder outText = new StringBuilder(InText);
            if (Char.IsLetter(outText[0]))
                outText[0] = Char.ToUpper(outText[0]);

            // Loop through the rest of the text.
            int ind = 0;
            while ((ind = outText.ToString().IndexOf('_', ind)) != -1)
            {
                // Remove the underscore.
                outText.Remove(ind, 1);
                // If there is a following letter, make it uppercase.
                if (ind != outText.Length && Char.IsLetter(outText[ind]))
                    outText[ind] = Char.ToUpper(outText[ind]);
            } // ((ind = outText

            return outText.ToString();
        } // toDeunderscore
    } // class TargetCreation
}
