using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;


using TargetCreation;


namespace VVVCreator
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// The name of the selected template.
        /// </summary>
        private string TemplateName;

        /// <summary>
        /// The meta data of the selecteed template.
        /// </summary>
        private TemplateMetaData Template;

        /// <summary>
        /// The name of the file in which the user defined macro data for the target creation is stored.
        /// </summary>
        private string MacroDataFileSpec;


        /// <summary>
        /// Constructor.
        /// </summary>        
        public MainForm()
        {
            InitializeComponent();
        } // MainForm


        /// <summary>
        /// Called when the form is loaded for the first time.
        /// First, it finds the available templates.
        /// Then it loads the macro definitions and fills the macro list view.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mainForm_Load(object sender, System.EventArgs e)
        {
            // Fill the templates combo box with the available templates: these are taken from the names of the folders in the Template folder.
            DirectoryInfo templateFolder = new DirectoryInfo("Template");
            foreach (DirectoryInfo dir in templateFolder.GetDirectories())
                comboBoxTemplates.Items.Add(dir.Name);
            if (comboBoxTemplates.Items.Count == 0)
                return;
            // Select the first template and construct the absolute spec of the meta data file in its folder.
            TemplateName = templateFolder.GetDirectories()[0].Name;
            comboBoxTemplates.SelectedIndex = 0;
            string templateMetaDataFileSpec = Path.Combine(templateFolder.GetDirectories()[0].FullName, "Template.xml");

            try
            {
                // Read the template meta data from the xml file.
                Template = TemplateMetaData.ReadFromXml(templateMetaDataFileSpec);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                System.Windows.Forms.Application.Exit();
                return;
            }
        } // mainForm_Load


        /// <summary>
        /// Called when the user clicks the File/Load menu item. Displays aa file open dialog and loads the user macro data from the chosen
        /// xml file. The string MacroDataFileSpec is set to the spec. The data is then displayed in the macro list view, overwriting data
        /// that is already there.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuFileLoad_Click(object sender, EventArgs e)
        {
            // Since the loaded values will overwrite the current values, warn the user.
            if (MessageBox.Show(
                             "Loaded user macro values will overwrite the current values. Continue?",
                             "Warning",
                             MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            OpenFileDialog dlg = new OpenFileDialog();

            dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dlg.Filter = "xml files (*.xml)|*.xml";
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            // Load the user supplied macro data from the xml file.
            MacroDataFileSpec = dlg.FileName;
            List<MacroData> macroDataList = null;
            try
            {
                macroDataList = MacroData.ReadFromXml(MacroDataFileSpec);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            // Fill value fields in the macro definitions. This overwrites the current values.
            foreach (MacroData macroData in macroDataList)
            {
                foreach (MacroDefinition macroDefinition in Template.MacroDefinitions)
                {
                    if (macroDefinition.Type == MacroType.UserString && macroDefinition.Name == macroData.Name)
                        macroDefinition.Value = macroData.Value;
                }
            }

            // Display the value data in the macro list view.
            for (int ind = 0; ind < Template.MacroDefinitions.Count; ind++)
            {
                if (Template.MacroDefinitions[ind].Type == MacroType.UserString)
                    listViewMacros.Items[ind].SubItems[1].Text = Template.MacroDefinitions[ind].Value;
            }
        } // menuFileLoad_Click


        /// <summary>
        /// Called when the user clicks the File/Save menu item. If no spec for the xml file has been set (either by Load or Save As),
        /// the Save As dialog is displayed and MacroDataFileSpec is set to the chosen spec. Then the user macro data is saved to the
        /// xml file with the spec MacroDataFileSpec.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuFileSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (MacroDataFileSpec == null)
                    saveUserMacroDataAs();
                else
                    saveUserMacroData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        } // menuFileSave_Click


        /// <summary>
        /// Called when the user clicks the File/Save As menu item. The Save As dialog is displayed and MacroDataFileSpec is set to the chosen spec.
        /// Then the user macro data is saved to the xml file with the spec MacroDataFileSpec.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuFileSaveAs_Click(object sender, EventArgs e)
        {
            try
            {
                saveUserMacroDataAs();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        } // menuFileSaveAs_Click


        /// <summary>
        /// Exits the application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuFileExit_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        } // menuFileExit_Click


        /// <summary>
        /// Called whe the user selects an item in the templates combo box. The template meta data is read and the macro list
        /// view is filled with the macro names and values.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxTemplates_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Empty the macros list view.
            listViewMacros.Items.Clear();

            // Get the name of the selected template.
            DirectoryInfo templateFolder = new DirectoryInfo("Template");
            TemplateName = templateFolder.GetDirectories()[comboBoxTemplates.SelectedIndex].Name;

            try
            {
                // Read the template meta data of the selected template.
                string templateMetaDataFileSpec = Path.Combine(templateFolder.GetDirectories()[comboBoxTemplates.SelectedIndex].FullName, "Template.xml");
                Template = TemplateMetaData.ReadFromXml(templateMetaDataFileSpec);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                System.Windows.Forms.Application.Exit();
                return;
            }

            // Display the template description.
            labelTemplateDescription.Text = Template.Description;
            // Fill the macros list view with the macros of the selected template.
            fillMacrosListView();
        } // listViewMacros_SelectedIndexChanged


        /// <summary>
        /// Called whe the user selects an item in the macros list view. The description of the item is displayed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewMacros_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Display the description of the selected macro.
            if (listViewMacros.SelectedIndices.Count == 0)
            {
                labelMacroDescription.Text = "";
            }
            else
            {
                int selItemInd = listViewMacros.SelectedIndices[0];
                labelMacroDescription.Text = Template.MacroDefinitions[selItemInd].Description;
            }
        } // listViewMacros_SelectedIndexChanged


        /// <summary>
        /// Called whe the user double-clicks an item in the macros list view. Displays the macro edit dialog.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewMacros_DoubleClick(object sender, EventArgs e)
        {
            editMacro();
        } // listViewMacros_DoubleClick


        /// <summary>
        /// Called when the user clicks the 'Edit Macro' button. Displays the macro edit dialog. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonEditMacro_Click(object sender, EventArgs e)
        {
            editMacro();
        } // buttonEditMacro_Click


        /// <summary>
        /// Called when the user clicks the 'Create Target' button. Instantiates the template in the selected folder.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCreateTarget_Click(object sender, EventArgs e)
        {
            // Check whether all user macros do have values. If one does not, warn the user.
            bool confirmed = false;
            foreach (MacroDefinition macroDefinition in Template.MacroDefinitions)
            {
                if (macroDefinition.Value == "" && !confirmed)
                {
                    if (MessageBox.Show(
                                     "At least one of the user macros is empty. Continue?",
                                     "Warning",
                                     MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return;
                    }
                    confirmed = true;
                }
            }

            // Give th user a hint which target folder to use.
            MessageBox.Show(Template.TargetFolderHint, "Hint");

            // Let the user choose the target folder.
            FolderBrowserDialog dlg = new FolderBrowserDialog();

            dlg.ShowNewFolderButton = true;
            dlg.RootFolder = Environment.SpecialFolder.Personal;

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            // Create the target.
            string templatePath = Path.Combine("Template", TemplateName);
            string templateContentPath = Path.Combine(templatePath, "Content");
            try
            {
                TargetCreation.TargetCreator.CreateTarget(templateContentPath, Template.MacroDefinitions, dlg.SelectedPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            // If one of them exists, display the contents of the WhatToDoNext.html or WhatToDoNext.txt file in the template folder.
            // Otherwise, display a general success message.
            FileInfo htmlMsgFileInfo = new FileInfo(Path.Combine(templatePath, "WhatToDoNext.html"));
            string txtMsgFileSpec = Path.Combine(templatePath, "WhatToDoNext.txt");
            if (File.Exists(htmlMsgFileInfo.FullName))
            {
                FormattedMessageBox msgBox = new FormattedMessageBox(htmlMsgFileInfo.FullName);
                msgBox.Show();
            }
            else if (File.Exists(txtMsgFileSpec))
            {
                MessageBox.Show(System.IO.File.ReadAllText(txtMsgFileSpec));
            }
            else
            {
                MessageBox.Show("Target successfully created!", "Congratulation!");
            }
        } // buttonCreateTarget_Click


        /// <summary>
        /// Fill the macros list view with the macro definitions in Template.MacroDefinitions.
        /// </summary>
        private void fillMacrosListView()
        {
            // Fill the macros list view with the macro definitions. System macro values are set by the software.
            // Non-user suppliable macros are grayed out.
            foreach (MacroDefinition macroDefinition in Template.MacroDefinitions)
            {
                ListViewItem item;
                item = new ListViewItem(macroDefinition.Name);
                macroDefinition.SetSystemMacroValue();
                item.SubItems.Add(macroDefinition.Value);
                if (macroDefinition.Type != MacroType.UserString)
                    item.BackColor = Color.LightGray;
                listViewMacros.Items.Add(item);
            }

            // Set the widths of the list view columns. The first column is fitted to the macro names, the second column covers the rest of the line.
            colhdrMacroName.Width = -1;
            colhdrMacroValue.Width = -2;

            // If there is at least one macro definition, select the first.
            if (listViewMacros.Items.Count > 0)
                listViewMacros.SelectedIndices.Add(0);
        } // fillMacrosListView


        /// <summary>
        /// Displays a dialog in which the user can choose a location and name for the user macro data file. MacroDataFileSpec is set to the
        /// chosen spec, then the data is saved to the file.
        /// </summary>
        private void saveUserMacroDataAs()
        {
            // Display the Save As dialog.
            SaveFileDialog dlg = new SaveFileDialog();

            dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dlg.Filter = "xml files (*.xml)|*.xml";

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            // Save the data as xml file.
            MacroDataFileSpec = dlg.FileName;
            saveUserMacroData();
        } // saveUserMacroDataAs


        /// <summary>
        /// Saves the user macro data to an xml file with the file spec MacroDataFileSpec.
        /// </summary>
        private void saveUserMacroData()
        {
            // Extract the user supplied macro data.
            List<MacroData> macroDataList = new List<MacroData>();
            foreach (MacroDefinition macroDefinition in Template.MacroDefinitions)
            {
                if (macroDefinition.Type == MacroType.UserString)
                {
                    MacroData macroData = new MacroData(macroDefinition.Name, macroDefinition.Value);
                    macroDataList.Add(macroData);
                }
            }

            // Write the user macros data.
            MacroData.WriteToXml(MacroDataFileSpec, ref macroDataList);
        } // saveUserMacroData


        /// <summary>
        /// Displays a dialog in which the user can edit the value of the macro which the user has selected in the macro list view.
        /// </summary>
        private void editMacro()
        {
            // Only user macros must be edited.
            if (listViewMacros.SelectedIndices.Count == 0)
                return;
            int selItemInd = listViewMacros.SelectedIndices[0];
            if (Template.MacroDefinitions[selItemInd].Type != MacroType.UserString)
                return;

            // Display the edit macro dialog.
            EditMacroDialog dlg = new EditMacroDialog(Template.MacroDefinitions[selItemInd].Name, Template.MacroDefinitions[selItemInd].Value);
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            // Set the new macro value and display it in the list view.
            Template.MacroDefinitions[selItemInd].Value = dlg.MacroValue;
            listViewMacros.Items[selItemInd].SubItems[1].Text = dlg.MacroValue;
        } // editMacro
    }  //  class MainForm
}  //  namespace VVVCreator
