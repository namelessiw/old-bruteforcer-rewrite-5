namespace old_bruteforcer_rewrite_5
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            LblInfo = new Label();
            TxtPlayerY = new TextBox();
            label1 = new Label();
            label3 = new Label();
            TxtFloorY = new TextBox();
            TxtCeiling = new TextBox();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            TxtYUpper = new TextBox();
            TxtYLower = new TextBox();
            label7 = new Label();
            TxtVSpeed = new TextBox();
            LstResults = new ListBox();
            label8 = new Label();
            BtnSearchExact = new Button();
            BtnSearchRange = new Button();
            ChkSinglejump = new CheckBox();
            ChkDoublejump = new CheckBox();
            Chk1fConvention = new CheckBox();
            SuspendLayout();
            // 
            // LblInfo
            // 
            LblInfo.AutoSize = true;
            LblInfo.Location = new Point(224, 9);
            LblInfo.Name = "LblInfo";
            LblInfo.Size = new Size(31, 15);
            LblInfo.TabIndex = 5;
            LblInfo.Text = "Info:";
            // 
            // TxtPlayerY
            // 
            TxtPlayerY.Location = new Point(12, 27);
            TxtPlayerY.Name = "TxtPlayerY";
            TxtPlayerY.Size = new Size(100, 23);
            TxtPlayerY.TabIndex = 8;
            TxtPlayerY.Text = "407.4";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(14, 15);
            label1.TabIndex = 9;
            label1.Text = "Y";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 53);
            label3.Name = "label3";
            label3.Size = new Size(34, 15);
            label3.TabIndex = 11;
            label3.Text = "Floor";
            // 
            // TxtFloorY
            // 
            TxtFloorY.Location = new Point(12, 71);
            TxtFloorY.Name = "TxtFloorY";
            TxtFloorY.Size = new Size(100, 23);
            TxtFloorY.TabIndex = 12;
            TxtFloorY.Text = "408";
            // 
            // TxtCeiling
            // 
            TxtCeiling.Location = new Point(12, 115);
            TxtCeiling.Name = "TxtCeiling";
            TxtCeiling.Size = new Size(100, 23);
            TxtCeiling.TabIndex = 13;
            TxtCeiling.Text = "363";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 97);
            label4.Name = "label4";
            label4.Size = new Size(44, 15);
            label4.TabIndex = 14;
            label4.Text = "Ceiling";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(118, 9);
            label5.Name = "label5";
            label5.Size = new Size(49, 15);
            label5.TabIndex = 15;
            label5.Text = "Y Upper";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(118, 53);
            label6.Name = "label6";
            label6.Size = new Size(49, 15);
            label6.TabIndex = 16;
            label6.Text = "Y Lower";
            // 
            // TxtYUpper
            // 
            TxtYUpper.Location = new Point(118, 27);
            TxtYUpper.Name = "TxtYUpper";
            TxtYUpper.Size = new Size(100, 23);
            TxtYUpper.TabIndex = 17;
            TxtYUpper.Text = "406.5";
            // 
            // TxtYLower
            // 
            TxtYLower.Location = new Point(118, 71);
            TxtYLower.Name = "TxtYLower";
            TxtYLower.Size = new Size(100, 23);
            TxtYLower.TabIndex = 18;
            TxtYLower.Text = "407.5";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(118, 97);
            label7.Name = "label7";
            label7.Size = new Size(46, 15);
            label7.TabIndex = 21;
            label7.Text = "VSpeed";
            // 
            // TxtVSpeed
            // 
            TxtVSpeed.Location = new Point(118, 115);
            TxtVSpeed.Name = "TxtVSpeed";
            TxtVSpeed.Size = new Size(100, 23);
            TxtVSpeed.TabIndex = 22;
            TxtVSpeed.Text = "0";
            // 
            // LstResults
            // 
            LstResults.FormattingEnabled = true;
            LstResults.ItemHeight = 15;
            LstResults.Location = new Point(12, 160);
            LstResults.Name = "LstResults";
            LstResults.Size = new Size(676, 169);
            LstResults.TabIndex = 23;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(12, 141);
            label8.Name = "label8";
            label8.Size = new Size(44, 15);
            label8.TabIndex = 24;
            label8.Text = "Results";
            // 
            // BtnSearchExact
            // 
            BtnSearchExact.Location = new Point(224, 115);
            BtnSearchExact.Name = "BtnSearchExact";
            BtnSearchExact.Size = new Size(89, 23);
            BtnSearchExact.TabIndex = 25;
            BtnSearchExact.Text = "Search Exact";
            BtnSearchExact.UseVisualStyleBackColor = true;
            BtnSearchExact.Click += BtnSearchExact_Click;
            // 
            // BtnSearchRange
            // 
            BtnSearchRange.Location = new Point(319, 115);
            BtnSearchRange.Name = "BtnSearchRange";
            BtnSearchRange.Size = new Size(89, 23);
            BtnSearchRange.TabIndex = 26;
            BtnSearchRange.Text = "Search Range";
            BtnSearchRange.UseVisualStyleBackColor = true;
            BtnSearchRange.Click += BtnSearchRange_Click;
            // 
            // ChkSinglejump
            // 
            ChkSinglejump.AutoSize = true;
            ChkSinglejump.Checked = true;
            ChkSinglejump.CheckState = CheckState.Checked;
            ChkSinglejump.Location = new Point(224, 90);
            ChkSinglejump.Name = "ChkSinglejump";
            ChkSinglejump.Size = new Size(86, 19);
            ChkSinglejump.TabIndex = 27;
            ChkSinglejump.Text = "Singlejump";
            ChkSinglejump.UseVisualStyleBackColor = true;
            // 
            // ChkDoublejump
            // 
            ChkDoublejump.AutoSize = true;
            ChkDoublejump.Checked = true;
            ChkDoublejump.CheckState = CheckState.Checked;
            ChkDoublejump.Location = new Point(316, 90);
            ChkDoublejump.Name = "ChkDoublejump";
            ChkDoublejump.Size = new Size(92, 19);
            ChkDoublejump.TabIndex = 28;
            ChkDoublejump.Text = "Doublejump";
            ChkDoublejump.UseVisualStyleBackColor = true;
            // 
            // Chk1fConvention
            // 
            Chk1fConvention.AutoSize = true;
            Chk1fConvention.Location = new Point(224, 65);
            Chk1fConvention.Name = "Chk1fConvention";
            Chk1fConvention.Size = new Size(101, 19);
            Chk1fConvention.TabIndex = 29;
            Chk1fConvention.Text = "1f Convention";
            Chk1fConvention.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(700, 338);
            Controls.Add(Chk1fConvention);
            Controls.Add(ChkDoublejump);
            Controls.Add(ChkSinglejump);
            Controls.Add(BtnSearchRange);
            Controls.Add(BtnSearchExact);
            Controls.Add(label8);
            Controls.Add(LstResults);
            Controls.Add(TxtVSpeed);
            Controls.Add(label7);
            Controls.Add(TxtYLower);
            Controls.Add(TxtYUpper);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(TxtCeiling);
            Controls.Add(TxtFloorY);
            Controls.Add(label3);
            Controls.Add(label1);
            Controls.Add(TxtPlayerY);
            Controls.Add(LblInfo);
            Margin = new Padding(3, 2, 3, 2);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label LblInfo;
        private TextBox TxtPlayerY;
        private Label label1;
        private Label label3;
        private TextBox TxtFloorY;
        private TextBox TxtCeiling;
        private Label label4;
        private Label label5;
        private Label label6;
        private TextBox TxtYUpper;
        private TextBox TxtYLower;
        private Label label7;
        private TextBox TxtVSpeed;
        private ListBox LstResults;
        private Label label8;
        private Button BtnSearchExact;
        private Button BtnSearchRange;
        private CheckBox ChkSinglejump;
        private CheckBox ChkDoublejump;
        private CheckBox Chk1fConvention;
    }
}
