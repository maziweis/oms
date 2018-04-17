namespace mdt
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.cklChinese = new System.Windows.Forms.CheckedListBox();
            this.cklMath = new System.Windows.Forms.CheckedListBox();
            this.cklEnglish = new System.Windows.Forms.CheckedListBox();
            this.chkChinese = new System.Windows.Forms.CheckBox();
            this.chkMath = new System.Windows.Forms.CheckBox();
            this.chkEnglish = new System.Windows.Forms.CheckBox();
            this.btnok = new System.Windows.Forms.Button();
            this.chkDec = new System.Windows.Forms.CheckBox();
            this.chkBook = new System.Windows.Forms.CheckBox();
            this.chkResource = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cklChinese
            // 
            this.cklChinese.CheckOnClick = true;
            this.cklChinese.ColumnWidth = 230;
            this.cklChinese.FormattingEnabled = true;
            this.cklChinese.Location = new System.Drawing.Point(156, 21);
            this.cklChinese.MultiColumn = true;
            this.cklChinese.Name = "cklChinese";
            this.cklChinese.Size = new System.Drawing.Size(570, 52);
            this.cklChinese.TabIndex = 4;
            // 
            // cklMath
            // 
            this.cklMath.CheckOnClick = true;
            this.cklMath.ColumnWidth = 230;
            this.cklMath.FormattingEnabled = true;
            this.cklMath.Location = new System.Drawing.Point(156, 89);
            this.cklMath.MultiColumn = true;
            this.cklMath.Name = "cklMath";
            this.cklMath.Size = new System.Drawing.Size(570, 52);
            this.cklMath.TabIndex = 5;
            // 
            // cklEnglish
            // 
            this.cklEnglish.CheckOnClick = true;
            this.cklEnglish.ColumnWidth = 230;
            this.cklEnglish.FormattingEnabled = true;
            this.cklEnglish.Location = new System.Drawing.Point(156, 157);
            this.cklEnglish.MultiColumn = true;
            this.cklEnglish.Name = "cklEnglish";
            this.cklEnglish.Size = new System.Drawing.Size(570, 212);
            this.cklEnglish.TabIndex = 6;
            // 
            // chkChinese
            // 
            this.chkChinese.AutoSize = true;
            this.chkChinese.Location = new System.Drawing.Point(25, 37);
            this.chkChinese.Name = "chkChinese";
            this.chkChinese.Size = new System.Drawing.Size(48, 16);
            this.chkChinese.TabIndex = 7;
            this.chkChinese.Text = "语文";
            this.chkChinese.UseVisualStyleBackColor = true;
            // 
            // chkMath
            // 
            this.chkMath.AutoSize = true;
            this.chkMath.Location = new System.Drawing.Point(25, 109);
            this.chkMath.Name = "chkMath";
            this.chkMath.Size = new System.Drawing.Size(48, 16);
            this.chkMath.TabIndex = 8;
            this.chkMath.Text = "数学";
            this.chkMath.UseVisualStyleBackColor = true;
            // 
            // chkEnglish
            // 
            this.chkEnglish.AutoSize = true;
            this.chkEnglish.Location = new System.Drawing.Point(25, 233);
            this.chkEnglish.Name = "chkEnglish";
            this.chkEnglish.Size = new System.Drawing.Size(48, 16);
            this.chkEnglish.TabIndex = 9;
            this.chkEnglish.Text = "英语";
            this.chkEnglish.UseVisualStyleBackColor = true;
            // 
            // btnok
            // 
            this.btnok.Location = new System.Drawing.Point(307, 455);
            this.btnok.Name = "btnok";
            this.btnok.Size = new System.Drawing.Size(135, 23);
            this.btnok.TabIndex = 10;
            this.btnok.Text = "导出数据";
            this.btnok.UseVisualStyleBackColor = true;
            this.btnok.Click += new System.EventHandler(this.btnok_Click);
            // 
            // chkDec
            // 
            this.chkDec.AutoSize = true;
            this.chkDec.Location = new System.Drawing.Point(25, 416);
            this.chkDec.Name = "chkDec";
            this.chkDec.Size = new System.Drawing.Size(96, 16);
            this.chkDec.TabIndex = 11;
            this.chkDec.Text = "生成加密文件";
            this.chkDec.UseVisualStyleBackColor = true;
            // 
            // chkBook
            // 
            this.chkBook.AutoSize = true;
            this.chkBook.Location = new System.Drawing.Point(169, 415);
            this.chkBook.Name = "chkBook";
            this.chkBook.Size = new System.Drawing.Size(60, 16);
            this.chkBook.TabIndex = 12;
            this.chkBook.Text = "导出书";
            this.chkBook.UseVisualStyleBackColor = true;
            // 
            // chkResource
            // 
            this.chkResource.AutoSize = true;
            this.chkResource.Location = new System.Drawing.Point(307, 416);
            this.chkResource.Name = "chkResource";
            this.chkResource.Size = new System.Drawing.Size(108, 16);
            this.chkResource.TabIndex = 13;
            this.chkResource.Text = "导出碎片化资源";
            this.chkResource.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(757, 506);
            this.Controls.Add(this.chkResource);
            this.Controls.Add(this.chkBook);
            this.Controls.Add(this.chkDec);
            this.Controls.Add(this.btnok);
            this.Controls.Add(this.chkEnglish);
            this.Controls.Add(this.chkMath);
            this.Controls.Add(this.chkChinese);
            this.Controls.Add(this.cklEnglish);
            this.Controls.Add(this.cklMath);
            this.Controls.Add(this.cklChinese);
            this.Name = "Form1";
            this.Text = "资源导出";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckedListBox cklChinese;
        private System.Windows.Forms.CheckedListBox cklMath;
        private System.Windows.Forms.CheckedListBox cklEnglish;
        private System.Windows.Forms.CheckBox chkChinese;
        private System.Windows.Forms.CheckBox chkMath;
        private System.Windows.Forms.CheckBox chkEnglish;
        private System.Windows.Forms.Button btnok;
        private System.Windows.Forms.CheckBox chkDec;
        private System.Windows.Forms.CheckBox chkBook;
        private System.Windows.Forms.CheckBox chkResource;
    }
}

