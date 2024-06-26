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
            label3 = new Label();
            TxtFloorY = new TextBox();
            TxtCeilingY = new TextBox();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            TxtYUpper = new TextBox();
            TxtYLower = new TextBox();
            label7 = new Label();
            TxtVSpeed = new TextBox();
            LstResults = new ListBox();
            label8 = new Label();
            BtnSearch = new Button();
            ChkSinglejump = new CheckBox();
            ChkDoublejump = new CheckBox();
            Chk1fConvention = new CheckBox();
            CmbSolutionCondition = new ComboBox();
            label2 = new Label();
            TxtSolutionYUpper = new TextBox();
            label10 = new Label();
            TxtSolutionYLower = new TextBox();
            label11 = new Label();
            ChkPlayerYRange = new CheckBox();
            ChkSolutionYRange = new CheckBox();
            ChkAllowCactus = new CheckBox();
            ChkAllowWindowTrick = new CheckBox();
            SuspendLayout();
            // 
            // LblInfo
            // 
            LblInfo.AutoSize = true;
            LblInfo.Location = new Point(345, 53);
            LblInfo.Name = "LblInfo";
            LblInfo.Size = new Size(31, 15);
            LblInfo.TabIndex = 5;
            LblInfo.Text = "Info:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(113, 97);
            label3.Name = "label3";
            label3.Size = new Size(34, 15);
            label3.TabIndex = 11;
            label3.Text = "Floor";
            // 
            // TxtFloorY
            // 
            TxtFloorY.Location = new Point(113, 115);
            TxtFloorY.Name = "TxtFloorY";
            TxtFloorY.Size = new Size(100, 23);
            TxtFloorY.TabIndex = 12;
            TxtFloorY.Text = "408";
            // 
            // TxtCeilingY
            // 
            TxtCeilingY.Location = new Point(7, 115);
            TxtCeilingY.Name = "TxtCeilingY";
            TxtCeilingY.Size = new Size(100, 23);
            TxtCeilingY.TabIndex = 13;
            TxtCeilingY.Text = "363";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(7, 97);
            label4.Name = "label4";
            label4.Size = new Size(44, 15);
            label4.TabIndex = 14;
            label4.Text = "Ceiling";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(7, 9);
            label5.Name = "label5";
            label5.Size = new Size(49, 15);
            label5.TabIndex = 15;
            label5.Text = "Y Upper";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(113, 9);
            label6.Name = "label6";
            label6.Size = new Size(49, 15);
            label6.TabIndex = 16;
            label6.Text = "Y Lower";
            // 
            // TxtYUpper
            // 
            TxtYUpper.Location = new Point(7, 27);
            TxtYUpper.Name = "TxtYUpper";
            TxtYUpper.Size = new Size(100, 23);
            TxtYUpper.TabIndex = 17;
            TxtYUpper.Text = "406.5";
            // 
            // TxtYLower
            // 
            TxtYLower.Location = new Point(113, 27);
            TxtYLower.Name = "TxtYLower";
            TxtYLower.Size = new Size(100, 23);
            TxtYLower.TabIndex = 18;
            TxtYLower.Text = "407.5";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(219, 97);
            label7.Name = "label7";
            label7.Size = new Size(46, 15);
            label7.TabIndex = 21;
            label7.Text = "VSpeed";
            // 
            // TxtVSpeed
            // 
            TxtVSpeed.Location = new Point(219, 115);
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
            // BtnSearch
            // 
            BtnSearch.Location = new Point(613, 119);
            BtnSearch.Name = "BtnSearch";
            BtnSearch.Size = new Size(75, 23);
            BtnSearch.TabIndex = 26;
            BtnSearch.Text = "Search";
            BtnSearch.UseVisualStyleBackColor = true;
            BtnSearch.Click += BtnSearch_Click;
            // 
            // ChkSinglejump
            // 
            ChkSinglejump.AutoSize = true;
            ChkSinglejump.Checked = true;
            ChkSinglejump.CheckState = CheckState.Checked;
            ChkSinglejump.Location = new Point(341, 4);
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
            ChkDoublejump.Location = new Point(341, 29);
            ChkDoublejump.Name = "ChkDoublejump";
            ChkDoublejump.Size = new Size(92, 19);
            ChkDoublejump.TabIndex = 28;
            ChkDoublejump.Text = "Doublejump";
            ChkDoublejump.UseVisualStyleBackColor = true;
            // 
            // Chk1fConvention
            // 
            Chk1fConvention.AutoSize = true;
            Chk1fConvention.Location = new Point(587, 94);
            Chk1fConvention.Name = "Chk1fConvention";
            Chk1fConvention.Size = new Size(101, 19);
            Chk1fConvention.TabIndex = 29;
            Chk1fConvention.Text = "1f Convention";
            Chk1fConvention.UseVisualStyleBackColor = true;
            // 
            // CmbSolutionCondition
            // 
            CmbSolutionCondition.DropDownStyle = ComboBoxStyle.DropDownList;
            CmbSolutionCondition.FormattingEnabled = true;
            CmbSolutionCondition.Items.AddRange(new object[] { "Can Rejump", "Landing", "Stable", "Y position" });
            CmbSolutionCondition.Location = new Point(219, 71);
            CmbSolutionCondition.Name = "CmbSolutionCondition";
            CmbSolutionCondition.Size = new Size(121, 23);
            CmbSolutionCondition.TabIndex = 30;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(219, 53);
            label2.Name = "label2";
            label2.Size = new Size(107, 15);
            label2.TabIndex = 31;
            label2.Text = "Solution Condition";
            // 
            // TxtSolutionYUpper
            // 
            TxtSolutionYUpper.Location = new Point(7, 71);
            TxtSolutionYUpper.Name = "TxtSolutionYUpper";
            TxtSolutionYUpper.Size = new Size(100, 23);
            TxtSolutionYUpper.TabIndex = 35;
            TxtSolutionYUpper.Text = "406.5";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(7, 53);
            label10.Name = "label10";
            label10.Size = new Size(96, 15);
            label10.TabIndex = 34;
            label10.Text = "Solution Y Upper";
            // 
            // TxtSolutionYLower
            // 
            TxtSolutionYLower.Location = new Point(113, 71);
            TxtSolutionYLower.Name = "TxtSolutionYLower";
            TxtSolutionYLower.Size = new Size(100, 23);
            TxtSolutionYLower.TabIndex = 37;
            TxtSolutionYLower.Text = "407.5";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(113, 53);
            label11.Name = "label11";
            label11.Size = new Size(96, 15);
            label11.TabIndex = 36;
            label11.Text = "Solution Y Lower";
            // 
            // ChkPlayerYRange
            // 
            ChkPlayerYRange.AutoSize = true;
            ChkPlayerYRange.Checked = true;
            ChkPlayerYRange.CheckState = CheckState.Checked;
            ChkPlayerYRange.Location = new Point(219, 6);
            ChkPlayerYRange.Name = "ChkPlayerYRange";
            ChkPlayerYRange.Size = new Size(104, 19);
            ChkPlayerYRange.TabIndex = 38;
            ChkPlayerYRange.Text = "Player Y Range";
            ChkPlayerYRange.UseVisualStyleBackColor = true;
            ChkPlayerYRange.CheckedChanged += ChkPlayerYRange_CheckedChanged;
            // 
            // ChkSolutionYRange
            // 
            ChkSolutionYRange.AutoSize = true;
            ChkSolutionYRange.Checked = true;
            ChkSolutionYRange.CheckState = CheckState.Checked;
            ChkSolutionYRange.Location = new Point(219, 31);
            ChkSolutionYRange.Name = "ChkSolutionYRange";
            ChkSolutionYRange.Size = new Size(116, 19);
            ChkSolutionYRange.TabIndex = 39;
            ChkSolutionYRange.Text = "Solution Y Range";
            ChkSolutionYRange.UseVisualStyleBackColor = true;
            ChkSolutionYRange.CheckedChanged += ChkSolutionYRange_CheckedChanged;
            // 
            // ChkAllowCactus
            // 
            ChkAllowCactus.AutoSize = true;
            ChkAllowCactus.Checked = true;
            ChkAllowCactus.CheckState = CheckState.Checked;
            ChkAllowCactus.Location = new Point(439, 6);
            ChkAllowCactus.Name = "ChkAllowCactus";
            ChkAllowCactus.Size = new Size(112, 19);
            ChkAllowCactus.TabIndex = 40;
            ChkAllowCactus.Text = "Allow Cactusing";
            ChkAllowCactus.UseVisualStyleBackColor = true;
            // 
            // ChkAllowWindowTrick
            // 
            ChkAllowWindowTrick.AutoSize = true;
            ChkAllowWindowTrick.Checked = true;
            ChkAllowWindowTrick.CheckState = CheckState.Checked;
            ChkAllowWindowTrick.Location = new Point(439, 31);
            ChkAllowWindowTrick.Name = "ChkAllowWindowTrick";
            ChkAllowWindowTrick.Size = new Size(130, 19);
            ChkAllowWindowTrick.TabIndex = 41;
            ChkAllowWindowTrick.Text = "Allow Window Trick";
            ChkAllowWindowTrick.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(697, 338);
            Controls.Add(ChkAllowWindowTrick);
            Controls.Add(ChkAllowCactus);
            Controls.Add(ChkSolutionYRange);
            Controls.Add(ChkPlayerYRange);
            Controls.Add(TxtSolutionYLower);
            Controls.Add(label11);
            Controls.Add(TxtSolutionYUpper);
            Controls.Add(label10);
            Controls.Add(label2);
            Controls.Add(CmbSolutionCondition);
            Controls.Add(Chk1fConvention);
            Controls.Add(ChkDoublejump);
            Controls.Add(ChkSinglejump);
            Controls.Add(BtnSearch);
            Controls.Add(label8);
            Controls.Add(LstResults);
            Controls.Add(TxtVSpeed);
            Controls.Add(label7);
            Controls.Add(TxtYLower);
            Controls.Add(TxtYUpper);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(TxtCeilingY);
            Controls.Add(TxtFloorY);
            Controls.Add(label3);
            Controls.Add(LblInfo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(3, 2, 3, 2);
            MaximizeBox = false;
            Name = "Form1";
            Text = "old bruteforcer rewrite 5";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label LblInfo;
        private Label label3;
        private TextBox TxtFloorY;
        private TextBox TxtCeilingY;
        private Label label4;
        private Label label5;
        private Label label6;
        private TextBox TxtYUpper;
        private TextBox TxtYLower;
        private Label label7;
        private TextBox TxtVSpeed;
        private ListBox LstResults;
        private Label label8;
        private Button BtnSearch;
        private CheckBox ChkSinglejump;
        private CheckBox ChkDoublejump;
        private CheckBox Chk1fConvention;
        private ComboBox CmbSolutionCondition;
        private Label label2;
        private TextBox TxtSolutionYUpper;
        private Label label10;
        private TextBox TxtSolutionYLower;
        private Label label11;
        private CheckBox ChkPlayerYRange;
        private CheckBox ChkSolutionYRange;
        private CheckBox ChkAllowCactus;
        private CheckBox ChkAllowWindowTrick;
    }
}
