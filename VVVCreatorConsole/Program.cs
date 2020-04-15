using System;
using System.Collections.Generic;
using System.IO;

using TargetCreation;


namespace VVVCreatorConsole
{
    /// <summary>
    /// The command line version of the VVVCreator tool.<br />
    /// The first argument must be the name of the folder which contains the template to use.<br />
    /// The second argument must be the absolute spec of the file which contains the values of the user macros.<br />
    /// The third argument must be the absolute spec of the folder where the template will be instantiated.<br />
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Read the meta data of the template given by the first argument.
                string templatePath = Path.Combine("Template", args[0]);
                TemplateMetaData template = TemplateMetaData.ReadFromXml(Path.Combine(templatePath, "Template.xml"));

                // Read the macro values from the file given by the second argument.
                List<MacroData> macroDataList = MacroData.ReadFromXml(args[1]);

                // Fill the value fields in the macro definitions with the macro values.
                foreach (MacroData macroData in macroDataList)
                {
                    foreach (MacroDefinition macroDefinition in template.MacroDefinitions)
                    {
                        if (macroDefinition.Type == MacroType.UserString && macroDefinition.Name == macroData.Name)
                            macroDefinition.Value = macroData.Value;
                    }
                }

                // Create the target in the folder given by the third argument.
                string templateContentPath = Path.Combine(templatePath, "Content");
                TargetCreation.TargetCreator.CreateTarget(templateContentPath, template.MacroDefinitions, args[2]);

                Console.WriteLine("Target successfully created!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        } // Main
    } // class Program
} // namespace VVVCreatorConsole
