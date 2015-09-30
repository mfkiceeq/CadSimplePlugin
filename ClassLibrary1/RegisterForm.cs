using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZLPlugin.Commands;
using Autodesk.AutoCAD;

namespace ZLPlugin
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
            info.Text = "如有任何问题,请联系作者:\n邮箱:zhou-han-peng@163.com\nQQ:104979306";
        }

        private void RegisterForm_Load(object sender, EventArgs e)
        {
            this.mNumTxt.Text = KeyUtil.GetMNum();
        }

        private void confirm_Click(object sender, EventArgs e)
        {
            if (this.registerTxt.Text == KeyUtil.GetRegisterKey(KeyUtil.GetMNum()))
            {
                KeyUtil.writeRegisterKey();
                Autodesk.AutoCAD.ApplicationServices.Application.ShowAlertDialog("注册成功，请继续使用");
                this.Close();
            }
            else
            {
                Autodesk.AutoCAD.ApplicationServices.Application.ShowAlertDialog("注册失败，请重新输入");
                CadPlugin.logToEditor("注册失败，请重新输入");
            }
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mNumTxt_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

    }
}
