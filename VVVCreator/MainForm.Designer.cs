namespace VVVCreator
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.ToolStripMenuItem menuItemFile;
        private System.Windows.Forms.ToolStripMenuItem menuItemLoad;
        private System.Windows.Forms.ToolStripMenuItem menuItemSave;
        private System.Windows.Forms.ToolStripMenuItem menuItemSaveAs;
        private System.Windows.Forms.ToolStripMenuItem menuItemExit;
        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.ComboBox comboBoxTemplates;
        private System.Windows.Forms.ListView listViewMacros;
        private System.Windows.Forms.ColumnHeader colhdrMacroName;
        private System.Windows.Forms.ColumnHeader colhdrMacroValue;
        private System.Windows.Forms.Label labelMacroDescription;
        private System.Windows.Forms.Button buttonEditMacro;
        private System.Windows.Forms.Button buttonCreateTarget;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuItemFile = new System.Windows.Forms.ToolStripMenuItem("File");
            this.menuItemLoad = new System.Windows.Forms.ToolStripMenuItem("Load...", null, new System.EventHandler(menuFileLoad_Click));
            this.menuItemSave = new System.Windows.Forms.ToolStripMenuItem("Save", null, new System.EventHandler(menuFileSave_Click));
            this.menuItemSaveAs = new System.Windows.Forms.ToolStripMenuItem("Save As...", null, new System.EventHandler(menuFileSaveAs_Click));
            this.menuItemExit = new System.Windows.Forms.ToolStripMenuItem("Exit", null, new System.EventHandler(menuFileExit_Click));
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.comboBoxTemplates = new System.Windows.Forms.ComboBox();
            this.listViewMacros = new System.Windows.Forms.ListView();
            this.colhdrMacroName = new System.Windows.Forms.ColumnHeader();
            this.colhdrMacroValue = new System.Windows.Forms.ColumnHeader();
            this.labelMacroDescription = new System.Windows.Forms.Label();
            this.buttonEditMacro = new System.Windows.Forms.Button();
            this.buttonCreateTarget = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // menu
            //
            this.menuItemFile.DropDownItems.Add(menuItemLoad);
            this.menuItemFile.DropDownItems.Add(menuItemSave);
            this.menuItemFile.DropDownItems.Add(menuItemSave);
            this.menuItemFile.DropDownItems.Add(menuItemSaveAs);
            this.menuItemFile.DropDownItems.Add(menuItemExit);
            this.menuStripMain.Items.Add(menuItemFile);
            this.menuStripMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.MainMenuStrip = this.menuStripMain;
            //
            // comboBoxTemplates
            //
            this.comboBoxTemplates.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxTemplates.Location = new System.Drawing.Point(13, 30);
            this.comboBoxTemplates.Name = "comboBoxTemplates";
            this.comboBoxTemplates.Size = new System.Drawing.Size(775, 50);
            this.comboBoxTemplates.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTemplates.SelectedIndexChanged += new System.EventHandler(this.comboBoxTemplates_SelectedIndexChanged);
            // 
            // listViewMacros
            // 
            this.listViewMacros.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewMacros.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colhdrMacroName,
            this.colhdrMacroValue});
            this.listViewMacros.FullRowSelect = true;
            this.listViewMacros.GridLines = true;
            this.listViewMacros.HideSelection = false;
            this.listViewMacros.Location = new System.Drawing.Point(13, 60);
            this.listViewMacros.Name = "listViewMacros";
            this.listViewMacros.Size = new System.Drawing.Size(776, 406);
            this.listViewMacros.TabIndex = 0;
            this.listViewMacros.UseCompatibleStateImageBehavior = false;
            this.listViewMacros.View = System.Windows.Forms.View.Details;
            this.listViewMacros.SelectedIndexChanged += new System.EventHandler(this.listViewMacros_SelectedIndexChanged);
            this.listViewMacros.DoubleClick += new System.EventHandler(this.listViewMacros_DoubleClick);
            // 
            // colhdrName
            // 
            this.colhdrMacroName.Name = "Name";
            this.colhdrMacroName.Text = "Name";
            // 
            // colhdrValue
            // 
            this.colhdrMacroValue.Name = "Value";
            this.colhdrMacroValue.Text = "Value";
            // 
            // labelMacroDescription
            // 
            this.labelMacroDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelMacroDescription.Location = new System.Drawing.Point(13, 469);
            this.labelMacroDescription.Name = "labelMacroDescription";
            this.labelMacroDescription.Size = new System.Drawing.Size(775, 63);
            this.labelMacroDescription.TabIndex = 1;
            this.labelMacroDescription.Text = "Macro description";
            // 
            // buttonEditMacro
            // 
            this.buttonEditMacro.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonEditMacro.Location = new System.Drawing.Point(12, 539);
            this.buttonEditMacro.Name = "buttonEditMacro";
            this.buttonEditMacro.Size = new System.Drawing.Size(105, 23);
            this.buttonEditMacro.TabIndex = 2;
            this.buttonEditMacro.Text = "Edit Macro...";
            this.buttonEditMacro.UseVisualStyleBackColor = true;
            this.buttonEditMacro.Click += new System.EventHandler(this.buttonEditMacro_Click);
            // 
            // buttonCreateTarget
            // 
            this.buttonCreateTarget.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCreateTarget.Location = new System.Drawing.Point(638, 539);
            this.buttonCreateTarget.Name = "buttonCreateTarget";
            this.buttonCreateTarget.Size = new System.Drawing.Size(150, 23);
            this.buttonCreateTarget.TabIndex = 3;
            this.buttonCreateTarget.Text = "Create Target...";
            this.buttonCreateTarget.UseVisualStyleBackColor = true;
            this.buttonCreateTarget.Click += new System.EventHandler(this.buttonCreateTarget_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(800, 578);
            this.Controls.Add(this.buttonCreateTarget);
            this.Controls.Add(this.buttonEditMacro);
            this.Controls.Add(this.labelMacroDescription);
            this.Controls.Add(this.listViewMacros);
            this.Controls.Add(this.comboBoxTemplates);
            this.Name = "MainForm";
            this.Text = "VVV Creator";
            this.Load += new System.EventHandler(this.mainForm_Load);

            // The main menu must be added last
            this.Controls.Add(this.menuStripMain);

            this.ResumeLayout(false);

        }

        #endregion
    } // class MainForm
} // namespace VVVCreator

