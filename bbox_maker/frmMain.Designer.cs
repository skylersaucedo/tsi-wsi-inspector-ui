namespace inspectionUI
{
    partial class frmMain
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.txbxInputImageFolder = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lblCurrentPosition = new System.Windows.Forms.Label();
            this.lblTotalCount = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txbxInspector = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tbarZoom = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.lblZoomVal = new System.Windows.Forms.Label();
            this.btnDefectDetector = new System.Windows.Forms.Button();
            this.btnMakeReport = new System.Windows.Forms.Button();
            this.tbarImage = new System.Windows.Forms.TrackBar();
            this.txbxPipeID = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.cxbxSnycScrollbars = new System.Windows.Forms.CheckBox();
            this.cxbxInvertImages = new System.Windows.Forms.CheckBox();
            this.cxbxPINorBOX = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cxbxPassNumber = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbarZoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbarImage)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(20, 6);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1787, 466);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.SizeChanged += new System.EventHandler(this.pictureBox1_SizeChanged);
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // btnPrevious
            // 
            this.btnPrevious.Enabled = false;
            this.btnPrevious.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrevious.Location = new System.Drawing.Point(1034, 88);
            this.btnPrevious.Margin = new System.Windows.Forms.Padding(6);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(308, 150);
            this.btnPrevious.TabIndex = 6;
            this.btnPrevious.Text = "previous";
            this.btnPrevious.UseVisualStyleBackColor = true;
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // btnNext
            // 
            this.btnNext.Enabled = false;
            this.btnNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNext.Location = new System.Drawing.Point(2578, 88);
            this.btnNext.Margin = new System.Windows.Forms.Padding(6);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(175, 150);
            this.btnNext.TabIndex = 7;
            this.btnNext.Text = "next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // txbxInputImageFolder
            // 
            this.txbxInputImageFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txbxInputImageFolder.Location = new System.Drawing.Point(284, 25);
            this.txbxInputImageFolder.Margin = new System.Windows.Forms.Padding(6);
            this.txbxInputImageFolder.Name = "txbxInputImageFolder";
            this.txbxInputImageFolder.Size = new System.Drawing.Size(1058, 44);
            this.txbxInputImageFolder.TabIndex = 25;
            this.txbxInputImageFolder.TextChanged += new System.EventHandler(this.txbxInputImageFolder_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(26, 28);
            this.label6.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(225, 37);
            this.label6.TabIndex = 26;
            this.label6.Text = "Project Folder:";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(769, 2220);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dataGridView1.Name = "dataGridView1";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(3230, 490);
            this.dataGridView1.TabIndex = 28;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(762, 2178);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 37);
            this.label2.TabIndex = 29;
            this.label2.Text = "Defects:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(2864, 107);
            this.label11.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(233, 37);
            this.label11.TabIndex = 21;
            this.label11.Text = "image position:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(2824, 164);
            this.label12.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(280, 37);
            this.label12.TabIndex = 22;
            this.label12.Text = "total  image count:";
            // 
            // lblCurrentPosition
            // 
            this.lblCurrentPosition.AutoSize = true;
            this.lblCurrentPosition.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentPosition.Location = new System.Drawing.Point(3116, 105);
            this.lblCurrentPosition.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblCurrentPosition.Name = "lblCurrentPosition";
            this.lblCurrentPosition.Size = new System.Drawing.Size(35, 37);
            this.lblCurrentPosition.TabIndex = 23;
            this.lblCurrentPosition.Text = "n";
            // 
            // lblTotalCount
            // 
            this.lblTotalCount.AutoSize = true;
            this.lblTotalCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalCount.Location = new System.Drawing.Point(3116, 164);
            this.lblTotalCount.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblTotalCount.Name = "lblTotalCount";
            this.lblTotalCount.Size = new System.Drawing.Size(35, 37);
            this.lblTotalCount.TabIndex = 24;
            this.lblTotalCount.Text = "n";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(35, 2414);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(166, 37);
            this.label3.TabIndex = 30;
            this.label3.Text = "Inspector: ";
            // 
            // txbxInspector
            // 
            this.txbxInspector.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txbxInspector.Location = new System.Drawing.Point(219, 2407);
            this.txbxInspector.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txbxInspector.Name = "txbxInspector";
            this.txbxInspector.Size = new System.Drawing.Size(442, 49);
            this.txbxInspector.TabIndex = 31;
            this.txbxInspector.Text = "M. Salamanca";
            this.txbxInspector.TextChanged += new System.EventHandler(this.txbxInspector_TextChanged);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Location = new System.Drawing.Point(1498, 273);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1450, 1900);
            this.panel1.TabIndex = 32;
            this.panel1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.panel1_Scroll);
            // 
            // tbarZoom
            // 
            this.tbarZoom.Location = new System.Drawing.Point(3958, 121);
            this.tbarZoom.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbarZoom.Maximum = 100;
            this.tbarZoom.Name = "tbarZoom";
            this.tbarZoom.Size = new System.Drawing.Size(314, 90);
            this.tbarZoom.TabIndex = 2;
            this.tbarZoom.Value = 50;
            this.tbarZoom.Scroll += new System.EventHandler(this.tbarZoom_Scroll);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3749, 135);
            this.label4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(216, 37);
            this.label4.TabIndex = 33;
            this.label4.Text = "magnification:";
            // 
            // lblZoomVal
            // 
            this.lblZoomVal.AutoSize = true;
            this.lblZoomVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblZoomVal.Location = new System.Drawing.Point(4282, 135);
            this.lblZoomVal.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblZoomVal.Name = "lblZoomVal";
            this.lblZoomVal.Size = new System.Drawing.Size(33, 37);
            this.lblZoomVal.TabIndex = 35;
            this.lblZoomVal.Text = "1";
            this.lblZoomVal.Click += new System.EventHandler(this.lblZoomVal_Click);
            // 
            // btnDefectDetector
            // 
            this.btnDefectDetector.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDefectDetector.Location = new System.Drawing.Point(4039, 2558);
            this.btnDefectDetector.Margin = new System.Windows.Forms.Padding(6);
            this.btnDefectDetector.Name = "btnDefectDetector";
            this.btnDefectDetector.Size = new System.Drawing.Size(447, 59);
            this.btnDefectDetector.TabIndex = 36;
            this.btnDefectDetector.Text = "Use Defect Detector";
            this.btnDefectDetector.UseVisualStyleBackColor = true;
            this.btnDefectDetector.Click += new System.EventHandler(this.btnDefectDetector_Click);
            // 
            // btnMakeReport
            // 
            this.btnMakeReport.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMakeReport.Location = new System.Drawing.Point(4039, 2651);
            this.btnMakeReport.Margin = new System.Windows.Forms.Padding(6);
            this.btnMakeReport.Name = "btnMakeReport";
            this.btnMakeReport.Size = new System.Drawing.Size(447, 59);
            this.btnMakeReport.TabIndex = 37;
            this.btnMakeReport.Text = "Generate Inspection Report";
            this.btnMakeReport.UseVisualStyleBackColor = true;
            this.btnMakeReport.Click += new System.EventHandler(this.btnMakeReport_Click);
            // 
            // tbarImage
            // 
            this.tbarImage.Location = new System.Drawing.Point(1389, 148);
            this.tbarImage.Name = "tbarImage";
            this.tbarImage.Size = new System.Drawing.Size(1166, 90);
            this.tbarImage.TabIndex = 38;
            this.tbarImage.Scroll += new System.EventHandler(this.tbarImage_Scroll);
            // 
            // txbxPipeID
            // 
            this.txbxPipeID.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txbxPipeID.Location = new System.Drawing.Point(219, 2486);
            this.txbxPipeID.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txbxPipeID.Name = "txbxPipeID";
            this.txbxPipeID.Size = new System.Drawing.Size(442, 49);
            this.txbxPipeID.TabIndex = 39;
            this.txbxPipeID.Text = "GT690-1001122";
            this.txbxPipeID.TextChanged += new System.EventHandler(this.txbxPipeID_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(35, 2494);
            this.label5.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(129, 37);
            this.label5.TabIndex = 40;
            this.label5.Text = "Pipe ID:";
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.Controls.Add(this.pictureBox2);
            this.panel2.Location = new System.Drawing.Point(13, 273);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1450, 1900);
            this.panel2.TabIndex = 41;
            this.panel2.Scroll += new System.Windows.Forms.ScrollEventHandler(this.panel2_Scroll);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(20, 6);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(6);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(1787, 466);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox2_Paint);
            this.pictureBox2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox2_MouseDown);
            this.pictureBox2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox2_MouseMove);
            this.pictureBox2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox2_MouseUp);
            // 
            // panel3
            // 
            this.panel3.AutoScroll = true;
            this.panel3.Controls.Add(this.pictureBox3);
            this.panel3.Location = new System.Drawing.Point(2972, 273);
            this.panel3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1450, 1900);
            this.panel3.TabIndex = 42;
            this.panel3.Scroll += new System.Windows.Forms.ScrollEventHandler(this.panel3_Scroll);
            // 
            // pictureBox3
            // 
            this.pictureBox3.Location = new System.Drawing.Point(20, 6);
            this.pictureBox3.Margin = new System.Windows.Forms.Padding(6);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(1787, 466);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox3.TabIndex = 1;
            this.pictureBox3.TabStop = false;
            this.pictureBox3.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox3_Paint);
            this.pictureBox3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox3_MouseDown);
            this.pictureBox3.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox3_MouseMove);
            this.pictureBox3.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox3_MouseUp);
            // 
            // cxbxSnycScrollbars
            // 
            this.cxbxSnycScrollbars.AutoSize = true;
            this.cxbxSnycScrollbars.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cxbxSnycScrollbars.Location = new System.Drawing.Point(4039, 2191);
            this.cxbxSnycScrollbars.Name = "cxbxSnycScrollbars";
            this.cxbxSnycScrollbars.Size = new System.Drawing.Size(383, 41);
            this.cxbxSnycScrollbars.TabIndex = 43;
            this.cxbxSnycScrollbars.Text = "synchronize scrollbars?";
            this.cxbxSnycScrollbars.UseVisualStyleBackColor = true;
            this.cxbxSnycScrollbars.CheckedChanged += new System.EventHandler(this.cxbxSnycScrollbars_CheckedChanged);
            // 
            // cxbxInvertImages
            // 
            this.cxbxInvertImages.AutoSize = true;
            this.cxbxInvertImages.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cxbxInvertImages.Location = new System.Drawing.Point(4039, 2247);
            this.cxbxInvertImages.Name = "cxbxInvertImages";
            this.cxbxInvertImages.Size = new System.Drawing.Size(257, 41);
            this.cxbxInvertImages.TabIndex = 44;
            this.cxbxInvertImages.Text = "Invert Images?";
            this.cxbxInvertImages.UseVisualStyleBackColor = true;
            this.cxbxInvertImages.CheckedChanged += new System.EventHandler(this.cxbxInvertImages_CheckedChanged);
            // 
            // cxbxPINorBOX
            // 
            this.cxbxPINorBOX.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cxbxPINorBOX.FormattingEnabled = true;
            this.cxbxPINorBOX.Location = new System.Drawing.Point(284, 99);
            this.cxbxPINorBOX.Name = "cxbxPINorBOX";
            this.cxbxPINorBOX.Size = new System.Drawing.Size(181, 45);
            this.cxbxPINorBOX.TabIndex = 45;
            this.cxbxPINorBOX.SelectedIndexChanged += new System.EventHandler(this.cxbxPINorBOX_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(26, 99);
            this.label7.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(202, 37);
            this.label7.TabIndex = 46;
            this.label7.Text = "PIN or BOX: ";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(26, 165);
            this.label8.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(221, 37);
            this.label8.TabIndex = 47;
            this.label8.Text = "Pass Number:";
            // 
            // cxbxPassNumber
            // 
            this.cxbxPassNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cxbxPassNumber.FormattingEnabled = true;
            this.cxbxPassNumber.Location = new System.Drawing.Point(284, 165);
            this.cxbxPassNumber.Name = "cxbxPassNumber";
            this.cxbxPassNumber.Size = new System.Drawing.Size(181, 45);
            this.cxbxPassNumber.TabIndex = 48;
            this.cxbxPassNumber.SelectedIndexChanged += new System.EventHandler(this.cxbxPassNumber_SelectedIndexChanged);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(4504, 2763);
            this.Controls.Add(this.cxbxPassNumber);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cxbxPINorBOX);
            this.Controls.Add(this.cxbxInvertImages);
            this.Controls.Add(this.cxbxSnycScrollbars);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txbxPipeID);
            this.Controls.Add(this.tbarImage);
            this.Controls.Add(this.btnMakeReport);
            this.Controls.Add(this.btnDefectDetector);
            this.Controls.Add(this.lblZoomVal);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbarZoom);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.txbxInspector);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txbxInputImageFolder);
            this.Controls.Add(this.lblTotalCount);
            this.Controls.Add(this.lblCurrentPosition);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrevious);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "frmMain";
            this.Text = "TSI - Inspector View";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbarZoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbarImage)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.PictureBox pictureBox1;
        public System.Windows.Forms.Button btnPrevious;
        public System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.TextBox txbxInputImageFolder;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        public System.Windows.Forms.Label lblCurrentPosition;
        public System.Windows.Forms.Label lblTotalCount;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox txbxInspector;
        private System.Windows.Forms.TrackBar tbarZoom;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblZoomVal;
        public System.Windows.Forms.Button btnDefectDetector;
        public System.Windows.Forms.Button btnMakeReport;
        public System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.TrackBar tbarImage;
        public System.Windows.Forms.TextBox txbxPipeID;
        public System.Windows.Forms.Label label5;
        public System.Windows.Forms.Panel panel2;
        public System.Windows.Forms.PictureBox pictureBox2;
        public System.Windows.Forms.Panel panel3;
        public System.Windows.Forms.PictureBox pictureBox3;
        public System.Windows.Forms.CheckBox cxbxSnycScrollbars;
        public System.Windows.Forms.CheckBox cxbxInvertImages;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        public System.Windows.Forms.ComboBox cxbxPINorBOX;
        public System.Windows.Forms.ComboBox cxbxPassNumber;
    }
}

