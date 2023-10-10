namespace inspectionUI
{
    partial class frmAddDefect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddDefect));
            this.btnAddDefect = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cmbxLabels = new System.Windows.Forms.ComboBox();
            this.txbxAddNewDefect = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cxbxAddNotes = new System.Windows.Forms.CheckBox();
            this.txbxNotes = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnAddDefect
            // 
            this.btnAddDefect.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddDefect.Location = new System.Drawing.Point(314, 541);
            this.btnAddDefect.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnAddDefect.Name = "btnAddDefect";
            this.btnAddDefect.Size = new System.Drawing.Size(248, 89);
            this.btnAddDefect.TabIndex = 0;
            this.btnAddDefect.Text = "Add";
            this.btnAddDefect.UseVisualStyleBackColor = true;
            this.btnAddDefect.Click += new System.EventHandler(this.btnAddDefect_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(603, 541);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(190, 89);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cmbxLabels
            // 
            this.cmbxLabels.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbxLabels.FormattingEnabled = true;
            this.cmbxLabels.Location = new System.Drawing.Point(378, 38);
            this.cmbxLabels.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbxLabels.Name = "cmbxLabels";
            this.cmbxLabels.Size = new System.Drawing.Size(414, 50);
            this.cmbxLabels.TabIndex = 2;
            // 
            // txbxAddNewDefect
            // 
            this.txbxAddNewDefect.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txbxAddNewDefect.Location = new System.Drawing.Point(378, 144);
            this.txbxAddNewDefect.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txbxAddNewDefect.Name = "txbxAddNewDefect";
            this.txbxAddNewDefect.Size = new System.Drawing.Size(414, 49);
            this.txbxAddNewDefect.TabIndex = 3;
            this.txbxAddNewDefect.TextChanged += new System.EventHandler(this.txbxAddNewDefect_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(34, 42);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(338, 42);
            this.label1.TabIndex = 4;
            this.label1.Text = "Use Existing Label:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(93, 152);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(276, 42);
            this.label2.TabIndex = 5;
            this.label2.Text = "Add new Label:";
            // 
            // cxbxAddNotes
            // 
            this.cxbxAddNotes.AutoSize = true;
            this.cxbxAddNotes.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cxbxAddNotes.Location = new System.Drawing.Point(126, 267);
            this.cxbxAddNotes.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cxbxAddNotes.Name = "cxbxAddNotes";
            this.cxbxAddNotes.Size = new System.Drawing.Size(225, 46);
            this.cxbxAddNotes.TabIndex = 6;
            this.cxbxAddNotes.Text = "Add Notes";
            this.cxbxAddNotes.UseVisualStyleBackColor = true;
            this.cxbxAddNotes.CheckedChanged += new System.EventHandler(this.cxbxAddNotes_CheckedChanged);
            // 
            // txbxNotes
            // 
            this.txbxNotes.Enabled = false;
            this.txbxNotes.Location = new System.Drawing.Point(378, 267);
            this.txbxNotes.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txbxNotes.Multiline = true;
            this.txbxNotes.Name = "txbxNotes";
            this.txbxNotes.Size = new System.Drawing.Size(414, 192);
            this.txbxNotes.TabIndex = 7;
            // 
            // frmAddDefect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(854, 686);
            this.Controls.Add(this.txbxNotes);
            this.Controls.Add(this.cxbxAddNotes);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txbxAddNewDefect);
            this.Controls.Add(this.cmbxLabels);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAddDefect);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAddDefect";
            this.Text = "Add a defect";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button btnAddDefect;
        public System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txbxAddNewDefect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.ComboBox cmbxLabels;
        public System.Windows.Forms.CheckBox cxbxAddNotes;
        public System.Windows.Forms.TextBox txbxNotes;
    }
}