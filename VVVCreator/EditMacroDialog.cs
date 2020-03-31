using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace VVVCreator
{
    public partial class EditMacroDialog : Form
    {
        /// <summary>
        /// The name of the macro. It is displayed in the title of the dialog.
        /// </summary>
        private string MacroName;

        /// <summary>
        /// The value of the macro.
        /// </summary>
        public string MacroValue;


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="macroName">The name of the macro.</param>
        /// <param name="macroValue">The value of the macro.</param>
        public EditMacroDialog(string macroName, string macroValue)
        {
            MacroName = macroName;
            MacroValue = macroValue;
            InitializeComponent();

            textBoxValue.Text = MacroValue;
        } // EditMacroDialog


        /// <summary>
        /// Called when the user clicks the Ok button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOk_Click(object sender, EventArgs e)
        {
            // Get the macro value.
            MacroValue = textBoxValue.Text;
            DialogResult = DialogResult.OK;
            Close();
        } // buttonOk_Click
    } // class EditMacroDialog
} // namespace VVVCreator
