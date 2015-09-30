using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZLPlugin.Commands;

namespace ZLPlugin
{
    public partial class KeyGenForm : Form
    {
        public KeyGenForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.pwd.Text == "527014" 
                && this.mNum.Text != null
                && this.mNum.Text.Length > 0)
            {
                this.regKey.Text = KeyUtil.GetRegisterKey(this.mNum.Text);
            }
        }
    }
}
