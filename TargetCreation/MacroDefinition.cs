using System;

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
        public string Name { get; }

        /// <summary>
        /// The type of the macro.
        /// </summary>
        public MacroType Type { get; }

        /// <summary>
        /// A descriptive text which shall make it easier for the user to provide the correct value for the macro.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// The value of the macro. The macro entry in the template will be replaced by this text.
        /// </summary>
        public string Value { get; set; }


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
