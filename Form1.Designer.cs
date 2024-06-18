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
            BtnStep = new Button();
            BtnToStable = new Button();
            BtnNew = new Button();
            ChkPress = new CheckBox();
            ChkRelease = new CheckBox();
            LblInfo = new Label();
            TxtStrat = new TextBox();
            BtnDoStrat = new Button();
            TxtPlayerY = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            TxtFloorY = new TextBox();
            TxtCeiling = new TextBox();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            TxtYUpper = new TextBox();
            TxtYLower = new TextBox();
            BtnFloorCollision = new Button();
            BtnCeilingCollision = new Button();
            label7 = new Label();
            TxtVSpeed = new TextBox();
            LstResults = new ListBox();
            label8 = new Label();
            BtnSearchExact = new Button();
            BtnSearchRange = new Button();
            SuspendLayout();
            // 
            // BtnStep
            // 
            BtnStep.Location = new Point(12, 245);
            BtnStep.Name = "BtnStep";
            BtnStep.Size = new Size(75, 23);
            BtnStep.TabIndex = 0;
            BtnStep.Text = "Step";
            BtnStep.UseVisualStyleBackColor = true;
            BtnStep.Click += BtnStep_Click;
            // 
            // BtnToStable
            // 
            BtnToStable.Location = new Point(12, 274);
            BtnToStable.Name = "BtnToStable";
            BtnToStable.Size = new Size(75, 23);
            BtnToStable.TabIndex = 1;
            BtnToStable.Text = "To Stable";
            BtnToStable.UseVisualStyleBackColor = true;
            BtnToStable.Click += BtnToStable_Click;
            // 
            // BtnNew
            // 
            BtnNew.Location = new Point(12, 303);
            BtnNew.Name = "BtnNew";
            BtnNew.Size = new Size(75, 23);
            BtnNew.TabIndex = 2;
            BtnNew.Text = "New";
            BtnNew.UseVisualStyleBackColor = true;
            BtnNew.Click += BtnNew_Click;
            // 
            // ChkPress
            // 
            ChkPress.AutoSize = true;
            ChkPress.Location = new Point(12, 195);
            ChkPress.Name = "ChkPress";
            ChkPress.Size = new Size(53, 19);
            ChkPress.TabIndex = 3;
            ChkPress.Text = "Press";
            ChkPress.UseVisualStyleBackColor = true;
            // 
            // ChkRelease
            // 
            ChkRelease.AutoSize = true;
            ChkRelease.Location = new Point(12, 220);
            ChkRelease.Name = "ChkRelease";
            ChkRelease.Size = new Size(65, 19);
            ChkRelease.TabIndex = 4;
            ChkRelease.Text = "Release";
            ChkRelease.UseVisualStyleBackColor = true;
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
            // TxtStrat
            // 
            TxtStrat.Location = new Point(12, 115);
            TxtStrat.Name = "TxtStrat";
            TxtStrat.Size = new Size(100, 23);
            TxtStrat.TabIndex = 6;
            // 
            // BtnDoStrat
            // 
            BtnDoStrat.Location = new Point(12, 144);
            BtnDoStrat.Name = "BtnDoStrat";
            BtnDoStrat.Size = new Size(75, 23);
            BtnDoStrat.TabIndex = 7;
            BtnDoStrat.Text = "Do Strat";
            BtnDoStrat.UseVisualStyleBackColor = true;
            BtnDoStrat.Click += BtnDoStrat_Click;
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
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 97);
            label2.Name = "label2";
            label2.Size = new Size(31, 15);
            label2.TabIndex = 10;
            label2.Text = "Strat";
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
            TxtCeiling.Location = new Point(118, 71);
            TxtCeiling.Name = "TxtCeiling";
            TxtCeiling.Size = new Size(100, 23);
            TxtCeiling.TabIndex = 13;
            TxtCeiling.Text = "363";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(118, 53);
            label4.Name = "label4";
            label4.Size = new Size(44, 15);
            label4.TabIndex = 14;
            label4.Text = "Ceiling";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(118, 97);
            label5.Name = "label5";
            label5.Size = new Size(49, 15);
            label5.TabIndex = 15;
            label5.Text = "Y Upper";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(118, 141);
            label6.Name = "label6";
            label6.Size = new Size(49, 15);
            label6.TabIndex = 16;
            label6.Text = "Y Lower";
            // 
            // TxtYUpper
            // 
            TxtYUpper.Location = new Point(118, 115);
            TxtYUpper.Name = "TxtYUpper";
            TxtYUpper.Size = new Size(100, 23);
            TxtYUpper.TabIndex = 17;
            TxtYUpper.Text = "406.5";
            // 
            // TxtYLower
            // 
            TxtYLower.Location = new Point(118, 159);
            TxtYLower.Name = "TxtYLower";
            TxtYLower.Size = new Size(100, 23);
            TxtYLower.TabIndex = 18;
            TxtYLower.Text = "407.5";
            // 
            // BtnFloorCollision
            // 
            BtnFloorCollision.Location = new Point(118, 231);
            BtnFloorCollision.Name = "BtnFloorCollision";
            BtnFloorCollision.Size = new Size(100, 23);
            BtnFloorCollision.TabIndex = 19;
            BtnFloorCollision.Text = "Floor Collision";
            BtnFloorCollision.UseVisualStyleBackColor = true;
            BtnFloorCollision.Click += BtnFloorCollision_Click;
            // 
            // BtnCeilingCollision
            // 
            BtnCeilingCollision.Location = new Point(118, 259);
            BtnCeilingCollision.Name = "BtnCeilingCollision";
            BtnCeilingCollision.Size = new Size(100, 23);
            BtnCeilingCollision.TabIndex = 20;
            BtnCeilingCollision.Text = "Ceil Collision";
            BtnCeilingCollision.UseVisualStyleBackColor = true;
            BtnCeilingCollision.Click += BtnCeilingCollision_Click;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(118, 185);
            label7.Name = "label7";
            label7.Size = new Size(46, 15);
            label7.TabIndex = 21;
            label7.Text = "VSpeed";
            // 
            // TxtVSpeed
            // 
            TxtVSpeed.Location = new Point(118, 203);
            TxtVSpeed.Name = "TxtVSpeed";
            TxtVSpeed.Size = new Size(100, 23);
            TxtVSpeed.TabIndex = 22;
            TxtVSpeed.Text = "0";
            // 
            // LstResults
            // 
            LstResults.FormattingEnabled = true;
            LstResults.ItemHeight = 15;
            LstResults.Location = new Point(224, 70);
            LstResults.Name = "LstResults";
            LstResults.Size = new Size(464, 259);
            LstResults.TabIndex = 23;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(224, 53);
            label8.Name = "label8";
            label8.Size = new Size(44, 15);
            label8.TabIndex = 24;
            label8.Text = "Results";
            // 
            // BtnSearchExact
            // 
            BtnSearchExact.Location = new Point(504, 12);
            BtnSearchExact.Name = "BtnSearchExact";
            BtnSearchExact.Size = new Size(89, 23);
            BtnSearchExact.TabIndex = 25;
            BtnSearchExact.Text = "Search Exact";
            BtnSearchExact.UseVisualStyleBackColor = true;
            BtnSearchExact.Click += BtnSearchExact_Click;
            // 
            // BtnSearchRange
            // 
            BtnSearchRange.Location = new Point(599, 12);
            BtnSearchRange.Name = "BtnSearchRange";
            BtnSearchRange.Size = new Size(89, 23);
            BtnSearchRange.TabIndex = 26;
            BtnSearchRange.Text = "Search Range";
            BtnSearchRange.UseVisualStyleBackColor = true;
            BtnSearchRange.Click += BtnSearchRange_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(700, 338);
            Controls.Add(BtnSearchRange);
            Controls.Add(BtnSearchExact);
            Controls.Add(label8);
            Controls.Add(LstResults);
            Controls.Add(TxtVSpeed);
            Controls.Add(label7);
            Controls.Add(BtnCeilingCollision);
            Controls.Add(BtnFloorCollision);
            Controls.Add(TxtYLower);
            Controls.Add(TxtYUpper);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(TxtCeiling);
            Controls.Add(TxtFloorY);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(TxtPlayerY);
            Controls.Add(BtnDoStrat);
            Controls.Add(TxtStrat);
            Controls.Add(LblInfo);
            Controls.Add(ChkRelease);
            Controls.Add(ChkPress);
            Controls.Add(BtnNew);
            Controls.Add(BtnToStable);
            Controls.Add(BtnStep);
            Margin = new Padding(3, 2, 3, 2);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button BtnStep;
        private Button BtnToStable;
        private Button BtnNew;
        private CheckBox ChkPress;
        private CheckBox ChkRelease;
        private Label LblInfo;
        private TextBox TxtStrat;
        private Button BtnDoStrat;
        private TextBox TxtPlayerY;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox TxtFloorY;
        private TextBox TxtCeiling;
        private Label label4;
        private Label label5;
        private Label label6;
        private TextBox TxtYUpper;
        private TextBox TxtYLower;
        private Button BtnFloorCollision;
        private Button BtnCeilingCollision;
        private Label label7;
        private TextBox TxtVSpeed;
        private ListBox LstResults;
        private Label label8;
        private Button BtnSearchExact;
        private Button BtnSearchRange;
    }
}
