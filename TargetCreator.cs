using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace VVVCreator
{
    class TargetCreator
    {
        /// <summary>
        /// Creates the target with the contents of the given template folder and the specified macro definitions.
        /// </summary>
        /// <param name="templateFolder">The folder which contains the template folders and files. It itself will not be copied</param>
        /// <param name="macroDefinitions">The macro dictionary used for substitution.</param>
        /// <param name="targetFolder">The target folder to which the contents of the template folder will be copied.</param>
        public static void CreateTarget(string templateFolder, ref List<MacroDefinition> macroDefinitions, string targetFolder)
        {
            // Create a macro dictionary from the definitions
            Dictionary<string, string> macroDictionary = new Dictionary<string, string>();
            foreach (MacroDefinition macroDef in macroDefinitions)
                macroDictionary.Add(macroDef.Name, macroDef.Value);

            // Copy the entire template folder to the target folder.
            // While doing this, substitute the macros in folder and file specs and the file contents.
            copyAndProcessFilesRecursively(new DirectoryInfo(templateFolder), new DirectoryInfo(targetFolder), ref macroDictionary);
        } // CreateTarget


        /// <summary>
        /// Recursively copies a directory tree. Taken from https://stackoverflow.com/questions/58744/copy-the-entire-contents-of-a-directory-in-c-sharp.
        /// While copying, macros in folder and file names as well as in file contents are processed.
        /// </summary>
        /// <param name="sourceFolder">The source folder. On the initial call from CreateTarget, this is the template folder.</param>
        /// <param name="targetFolder">The target folder. On the initial call from CreateTarget, this is the folder to which
        /// the contents of the template folder will be copied.</param>
        private static void copyAndProcessFilesRecursively(DirectoryInfo sourceFolder, DirectoryInfo targetFolder, ref Dictionary<string, string> macroDictionary)
        {
            // Recursively handle subfolders. Since folder names may have macros within them, substitute these macros before folder creation
            foreach (DirectoryInfo dir in sourceFolder.GetDirectories())
            {
                copyAndProcessFilesRecursively(
                  dir,
                  targetFolder.CreateSubdirectory(substituteMacros(dir.Name, ref macroDictionary)),
                  ref macroDictionary);
            }

            // Process files: copy the files and substitute macros in their names and contents.
            // We have a special case: do not copy the 'MacroDefinitions.xml' and WhatToDoNext.txt' files from the template folder.
            foreach (FileInfo file in sourceFolder.GetFiles())
            {
                if (file.Name != "MacroDefinitions.xml" && file.Name != "WhatToDoNext.txt")
                {
                    string targetFileSpec = substituteMacros(Path.Combine(targetFolder.FullName, file.Name), ref macroDictionary);
                    System.IO.File.WriteAllText(targetFileSpec, substituteMacros(System.IO.File.ReadAllText(file.FullName), ref macroDictionary));
                }
            }
        } // copyAndProcessFilesRecursively


        /// <summary>
        /// Substitutes all macros (given by ##Macro name##) in the given string with their values from the specified dictionary. If an unknown macro
        /// is found an exception is thrown.
        /// </summary>
        /// <param name="sourceText">The input string containing macros.</param>
        /// <param name="macroDictionary">The macro distionary used for substitution.</param>
        /// <returns>The input string with all macros substituted.</returns>
        private static string substituteMacros(string sourceText, ref Dictionary<string, string> macroDictionary)
        {
            StringBuilder destText = new StringBuilder(sourceText);

            // Loop through the text using regex
            int searchStart = 0;
            while (true)
            {
                // Try to find a macro in the string. If there is none, we're done.
                Match match = Regex.Match(destText.ToString(searchStart, destText.Length - searchStart), @"\#\#(.*?)\#\#");
                if (!match.Success || match.Groups.Count == 0)
                    break;

                // Get the macro name from the search result.
                string macroName = match.Groups[1].Value;
                // Find the value for the macro name. We do have standard hardcoded macros and custom macros from the dictionary.
                string macroValue;
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

                // Replace the macro with its value.
                destText = destText.Replace(match.Value, macroValue, searchStart + match.Index, match.Value.Length);
                // Adjust the start value for the next search: start it behind the macro value we have just inserted.
                searchStart += match.Index + macroValue.Length;
            } // while (true)

            return destText.ToString();
        } // substituteMacros
    } // class TargetCreator
}
