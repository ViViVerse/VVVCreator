using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace VVVCreator
{
    public partial class FormattedMessageBox : Form
    {
        /// <summary>
        /// The url of the message text which shall be display.
        /// </summary>
        private string Url;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        public FormattedMessageBox(string url)
        {
            Url = url;
            InitializeComponent();
        } // FormattedMessageBox


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormattedMessageBox_Load(object sender, EventArgs e)
        {
            webBrowser.Navigate(Url);
        } // FormattedMessageBox_Load
    } // FormattedMessageBox
} // namespace VVVCreator
