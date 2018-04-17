namespace mdt
{
    partial class Form2
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
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.btnExport = new System.Windows.Forms.Button();
            this.chkDec = new System.Windows.Forms.CheckBox();
            this.chkBook = new System.Windows.Forms.CheckBox();
            this.chkResource = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.CheckBoxes = true;
            this.treeView1.Location = new System.Drawing.Point(55, 12);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(382, 474);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCheck);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(508, 413);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(136, 23);
            this.btnExport.TabIndex = 1;
            this.btnExport.Text = "导出数据";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // chkDec
            // 
            this.chkDec.AutoSize = true;
            this.chkDec.Location = new System.Drawing.Point(508, 89);
            this.chkDec.Name = "chkDec";
            this.chkDec.Size = new System.Drawing.Size(96, 16);
            this.chkDec.TabIndex = 2;
            this.chkDec.Text = "生成加密文件";
            this.chkDec.UseVisualStyleBackColor = true;
            // 
            // chkBook
            // 
            this.chkBook.AutoSize = true;
            this.chkBook.Location = new System.Drawing.Point(508, 169);
            this.chkBook.Name = "chkBook";
            this.chkBook.Size = new System.Drawing.Size(60, 16);
            this.chkBook.TabIndex = 3;
            this.chkBook.Text = "导出书";
            this.chkBook.UseVisualStyleBackColor = true;
            this.chkBook.Visible = false;
            // 
            // chkResource
            // 
            this.chkResource.AutoSize = true;
            this.chkResource.Location = new System.Drawing.Point(508, 261);
            this.chkResource.Name = "chkResource";
            this.chkResource.Size = new System.Drawing.Size(108, 16);
            this.chkResource.TabIndex = 4;
            this.chkResource.Text = "导出碎片化资源";
            this.chkResource.UseVisualStyleBackColor = true;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(739, 498);
            this.Controls.Add(this.chkResource);
            this.Controls.Add(this.chkBook);
            this.Controls.Add(this.chkDec);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.treeView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form2";
            this.Text = "资源导出工具";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.CheckBox chkDec;
        private System.Windows.Forms.CheckBox chkBook;
        private System.Windows.Forms.CheckBox chkResource;
    }
}