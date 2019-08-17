using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace forgotten_construction_set
{
    public partial class Donate : Form
    {
        public Donate()
        {
            InitializeComponent();
        }

        private void SourceLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            NativeTranslte.ShellExecute(0, @"open", @"https://github.com/alexliyu7352/kenshi_editor", null, null, (int)NativeTranslte.ShowWindowCommands.SW_NORMAL);
            
        }
    }
}
