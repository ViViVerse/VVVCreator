using System;
using System.Collections.Generic;
using System.IO;

using TargetCreation;


namespace VVVCreatorConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Read the macro definitions of the template given by the first argument.
                string templatePath = Path.Combine("Template", args[0]);
                List<MacroDefinition> macroDefinitions = MacroDefinition.ReadFromXml(Path.Combine(templatePath, "MacroDefinitions.xml"));

                // Read the macro values from the file given by the second argument.
                List<MacroData> macroDataList = MacroData.ReadFromXml(args[1]);

                // Fill empty value fields in the macro definitions with the macro values.
                foreach (MacroData macroData in macroDataList)
                {
                    foreach (MacroDefinition macroDefinition in macroDefinitions)
                    {
                        if (macroDefinition.Type == MacroType.UserString && macroDefinition.Name == macroData.Name && macroDefinition.Value == "")
                            macroDefinition.Value = macroData.Value;
                    }
                }

                // Create the target in the folder given by the third argument.
                string templateContentPath = Path.Combine(templatePath, "Content");
                TargetCreation.TargetCreator.CreateTarget(templateContentPath, ref macroDefinitions, args[2]);

                Console.WriteLine("Target successfully created!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        } // Main
    } // class Program
} // namespace VVVCreatorConsole
