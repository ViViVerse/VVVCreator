using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace VVVCreator
{
    /// <summary>
    /// Enumerates the available macro types.
    /// Four special macros exist which are not enumerated here:
    /// GUID, CODE_GUID, DATE, YEAR. These represent non-referable one time values.
    /// The are substituted as follows:
    /// GUID: a new guid in the format {00000000-0000-0000-0000-000000000000}
    /// CODE_GUID: a new guid in the format 00000000, 0000, 0000, 0000, 000000000000
    /// DATE: the current date in the format yyyy-MM-dd
    /// YEAR: the current year in the format yyyy
    /// </summary>
    public enum MacroType
    {
        /// <summary>
        /// The current date in the format yyyy-MM-dd.
        /// </summary>
        SysDate,
        /// <summary>
        /// A new guid in the format {00000000-0000-0000-0000-000000000000}.
        /// </summary>
        SysGuid,
        /// <summary>
        // A string which has to be provided by the user.
        /// </summary>
        UserString
    } // enum MacroType


    class MacroDefinition
    {
        /// <summary>
        /// The name of the macro.
        /// </summary>
        public string Name;

        /// <summary>
        /// The type of the macro.
        /// </summary>
        public MacroType Type;

        /// <summary>
        /// 
        /// </summary>
        public string Description;

        /// <summary>
        /// The descrption of the macro.
        /// </summary>
        public string Value;


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the macro.</param>
        /// <param name="type">The type of the macro.</param>
        /// <param name="description">The descrption of the macro.</param>
        public MacroDefinition(string name, MacroType type, string description)
        {
            Name = name;
            Type = type;
            Value = "";
            Description = description;
        } // MacroDefinition


        /// <summary>
        /// Reads the macro definitions in the given xml file.
        /// </summary>
        /// <param name="xmlFileSpec">The absolute spec of the xml file.</param>
        /// <returns>The definitions read from the file.</returns>
        public static List<MacroDefinition> ReadFromXml(string xmlFileSpec)
        {
            // Load the xml file.
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFileSpec);

            // Look for the first node that really contains data.
            XmlNode dataNode = xmlDoc.FirstChild;
            while (!dataNode.HasChildNodes && dataNode.NextSibling != null)
                dataNode = dataNode.NextSibling;
            XmlNodeList macroNodes = dataNode.ChildNodes;

            // Loop through all nodes.
            List<MacroDefinition> macroDefinitions = new List<MacroDefinition>();
            foreach (XmlNode macroNode in macroNodes)
            {
                // Read the macro name, type and description. The value is not read.
                string macroName = macroNode["Name"].InnerText;
                MacroType macroType = (MacroType)Enum.Parse< MacroType>(macroNode["Type"].InnerText);
                string macroDescription = macroNode["Description"].InnerText;

                // Create a new macro data entry.
                MacroDefinition macroDefinition = new MacroDefinition(macroName, macroType, macroDescription);

                // Add the new macro data entry to our list.
                macroDefinitions.Add(macroDefinition);
            }

            return macroDefinitions;
        } // ReadFromXml


        /// <summary>
        /// Sets the value of a system macro to the value representing its type. User macros are not changed.
        /// </summary>
        public void SetSystemMacroValue()
        {
            switch(Type)
            {
                case MacroType.SysDate:
                    Value = System.DateTime.Now.ToString("yyyy-MM-dd");
                    break;
                case MacroType.SysGuid:
                    Value = System.Guid.NewGuid().ToString("B");
                    break;
            }
        } // SetSystemMacroValue
    } // class MacroDefinition
} // namespace VVVCreator
