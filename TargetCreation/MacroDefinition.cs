using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace TargetCreation
{
    /// <summary>
    /// Enumerates the available macro types.<br />
    /// Four special macros exist which are not enumerated here:<br />
    /// GUID, CODE_GUID, DATE, YEAR. These represent non-referable one time values.<br />
    /// They are substituted as follows:<br />
    /// GUID: a new guid in the format {00000000-0000-0000-0000-000000000000}<br />
    /// CODE_GUID: a new guid in the format 00000000, 0000, 0000, 0000, 000000000000<br />
    /// DATE: the current date in the format yyyy-MM-dd<br />
    /// YEAR: the current year in the format yyyy<br />
    /// </summary>
    public enum MacroType
    {
        /// <summary>The current date in the format yyyy-MM-dd.</summary>
        SysDate,
        /// <summary>A new guid in the format {00000000-0000-0000-0000-000000000000}.</summary>
        SysGuid,
        /// <summary>A new guid in the format 00000000, 0000, 0000, 0000, 000000000000.</summary>
        SysCodeGuid,
        /// <summary>A string which has to be provided by the user.</summary>
        UserString
    } // enum MacroType


    public class MacroDefinition
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
        /// A descriptive text which shall make it easier for the user to provide the correct value for the macro.
        /// </summary>
        public string Description;

        /// <summary>
        /// The value of the macro. The macro entry in the template will be replaced by this text.
        /// </summary>
        public string Value;


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the macro.</param>
        /// <param name="type">The type of the macro.</param>
        /// <param name="description">The description of the macro.</param>
        /// <param name="defaultValue">An optional default value.</param>
        public MacroDefinition(string name, MacroType type, string description, string defaultValue)
        {
            Name = name;
            Type = type;
            Description = description;
            Value = defaultValue == null ? "" : defaultValue;
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
                XmlElement defaultValueElement = macroNode["DefaultValue"];
                string macroDefaultValue = defaultValueElement == null ? null : defaultValueElement.InnerText;

                // Create a new macro data entry.
                MacroDefinition macroDefinition = new MacroDefinition(macroName, macroType, macroDescription, macroDefaultValue);

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
                    Value = DateTime.Now.ToString("yyyy-MM-dd");
                    break;
                case MacroType.SysGuid:
                    Value = Guid.NewGuid().ToString("B");
                    break;
                case MacroType.SysCodeGuid:
                    Value = Guid.NewGuid().ToString("D").Replace("-", ", ").ToUpper();
                    break;
            }
        } // SetSystemMacroValue
    } // class MacroDefinition
} // namespace TargetCreation
