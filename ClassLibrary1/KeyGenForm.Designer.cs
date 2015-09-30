namespace ZLPlugin
{
    partial class KeyGenForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mNum = new System.Windows.Forms.TextBox();
            this.regKey = new System.Windows.Forms.TextBox();
            this.pwd = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // mNum
            // 
            this.mNum.Location = new System.Drawing.Point(40, 41);
            this.mNum.Name = "mNum";
            this.mNum.Size = new System.Drawing.Size(256, 21);
            this.mNum.TabIndex = 0;
            // 
            // regKey
            // 
            this.regKey.Location = new System.Drawing.Point(40, 96);
            this.regKey.Name = "regKey";
            this.regKey.Size = new System.Drawing.Size(256, 21);
            this.regKey.TabIndex = 1;
            // 
            // pwd
            // 
            this.pwd.Location = new System.Drawing.Point(40, 142);
            this.pwd.Name = "pwd";
            this.pwd.Size = new System.Drawing.Size(133, 21);
            this.pwd.TabIndex = 2;
            this.pwd.UseSystemPasswordChar = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(139, 214);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "生成";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // KeyGenForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(353, 277);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pwd);
            this.Controls.Add(this.regKey);
            this.Controls.Add(this.mNum);
            this.Name = "KeyGenForm";
            this.Text = "KeyGenForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox mNum;
        private System.Windows.Forms.TextBox regKey;
        private System.Windows.Forms.TextBox pwd;
        private System.Windows.Forms.Button button1;
    }
}