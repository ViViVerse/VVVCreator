using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace TargetCreation
{
    /// <summary>
    /// Holds the data of user macros for package creation. Non-user macros are assigned values by the software on the fly and is not persisted.
    /// </summary>
    public class MacroData
    {
        /// <summary>
        /// The name of the macro as in the macro definition.
        /// </summary>
        public string Name;
        /// <summary>
        /// The value of the macro as suipplied by the user.
        /// </summary>
        public string Value;


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the macro.</param>
        /// <param name="value">The value of the macro.</param>
        public MacroData(string name, string value)
        {
            Name = name;
            Value = value;
        } // MacroData


        /// <summary>
        /// Reads a list of macro data from an xml file.
        /// </summary>
        /// <param name="xmlFileSpec">The absolute spec of the xml file.</param>
        /// <returns>the list of macro data</returns>
        public static List<MacroData> ReadFromXml(string xmlFileSpec)
        {
            // Load the xml file.
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFileSpec);

            // Look for the first node that really contains data
            XmlNode dataNode = xmlDoc.FirstChild;
            while (!dataNode.HasChildNodes && dataNode.NextSibling != null)
                dataNode = dataNode.NextSibling;
            XmlNodeList macroNodes = dataNode.ChildNodes;

            // Loop through all nodes.
            List<MacroData> macroDataList = new List<MacroData>();
            foreach (XmlNode macroNode in macroNodes)
            {
                // Read the macro name and value.
                string macroName = macroNode["Name"].InnerText;
                string macroValue = macroNode["Value"].InnerText;

                // Create a new macro data entry.
                MacroData macroData = new MacroData(macroName, macroValue);

                // Add the new macro data entry to our list.
                macroDataList.Add(macroData);
            }

            return macroDataList;
        } // ReadFromXml


        /// <summary>
        /// Writes the given list of user supplied macro data to an xml file.
        /// </summary>
        /// <param name="xmlFileSpec">The absolute spec of the xml file.</param>
        /// <param name="macroDataList">The list of macro data.</param>
        public static void WriteToXml(string xmlFileSpec, ref List<MacroData> macroDataList)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            XmlWriter writer = XmlWriter.Create(xmlFileSpec, settings);

            // Write the start tag for the root element.
            writer.WriteStartElement("MacroDataList");

            // Loop through all macro data entries.
            foreach (MacroData macroData in macroDataList)
            {
                // Write the start tag for the macro data entry.
                writer.WriteStartElement("MacroData");
                // Write the macro name and value.
                writer.WriteElementString("Name", macroData.Name);
                writer.WriteElementString("Value", macroData.Value);
                // Write the close tag for the macro data entry.
                writer.WriteEndElement();
            }

            // Write the close tag for the root element.
            writer.WriteEndElement();

            // Write the XML to file and close the writer.
            writer.Flush();
            writer.Close();
        } // WriteToXml
    } // class MacroData
} // namespace TargetCreation
