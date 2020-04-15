using System;
using System.Collections.Generic;
using System.Xml;

namespace TargetCreation
{
    /// <summary>
    /// Contains template meta data.
    /// </summary>
    public class TemplateMetaData
    {
        /// <summary>
        /// A descriptive text which tells the user what content the template has.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gives the user a hint which folder to choose as target folder.
        /// </summary>
        public string TargetFolderHint { get; }

        /// <summary>
        /// The macro definitions.
        /// </summary>
        public List<MacroDefinition> MacroDefinitions { get; }


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="desc">The template description.</param>
        /// <param name="hint">The target folder hint.</param>
        protected TemplateMetaData(string desc, string hint)
        {
            Description = desc;
            TargetFolderHint = hint;
            MacroDefinitions = new List<MacroDefinition>();
        }


        /// <summary>
        /// Reads the template from the given xml file.
        /// </summary>
        /// <param name="xmlFileSpec">The absolute spec of the xml file.</param>
        /// <returns>The definitions read from the file.</returns>
        public static TemplateMetaData ReadFromXml(string xmlFileSpec)
        {
            // Load the xml file.
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFileSpec);

            // Look for the first node that really contains data.
            XmlNode dataNode = xmlDoc.FirstChild;
            while (!dataNode.HasChildNodes && dataNode.NextSibling != null)
                dataNode = dataNode.NextSibling;
            XmlNodeList templateNodes = dataNode.ChildNodes;

            // Get the template description and target folder hint. Also find the macro definitions.
            if (templateNodes.Item(0).Name != "Description")
                throw new Exception("XML entry 'Description' missing");
            if (templateNodes.Item(1).Name != "TargetFolderHint")
                throw new Exception("XML entry 'TargetFolderHint' missing");
            if (templateNodes.Item(2).Name != "MacroDefinitions" || !templateNodes.Item(2).HasChildNodes)
                throw new Exception("XML entry 'MacroDefinitions' missing");
            // Create the new template.
            TemplateMetaData template = new TemplateMetaData(templateNodes.Item(0).InnerText, templateNodes.Item(1).InnerText);

            // Loop through all macro definition nodes and add them to the template.
            XmlNodeList macroNodes = templateNodes.Item(2).ChildNodes;
            foreach (XmlNode macroNode in macroNodes)
            {
                // Read the macro name, type, description and default. The value is not read.
                string macroName = macroNode["Name"].InnerText;
                MacroType macroType = (MacroType)Enum.Parse<MacroType>(macroNode["Type"].InnerText);
                string macroDescription = macroNode["Description"].InnerText;
                XmlElement defaultValueElement = macroNode["DefaultValue"];
                string macroDefaultValue = defaultValueElement == null ? null : defaultValueElement.InnerText;

                // Create a new macro definition entry and add it to the list.
                MacroDefinition macroDefinition = new MacroDefinition(macroName, macroType, macroDescription, macroDefaultValue);
                template.MacroDefinitions.Add(macroDefinition);
            }

            return template;
        } // ReadFromXml
    } // class Template
} // namespace TargetCreation
