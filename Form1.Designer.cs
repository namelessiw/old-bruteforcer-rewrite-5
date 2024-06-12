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
            LblStats = new Label();
            TxtStrat = new TextBox();
            BtnDoStrat = new Button();
            SuspendLayout();
            // 
            // BtnStep
            // 
            BtnStep.Location = new Point(532, 303);
            BtnStep.Name = "BtnStep";
            BtnStep.Size = new Size(75, 23);
            BtnStep.TabIndex = 0;
            BtnStep.Text = "Step";
            BtnStep.UseVisualStyleBackColor = true;
            BtnStep.Click += BtnStep_Click;
            // 
            // BtnToStable
            // 
            BtnToStable.Location = new Point(613, 303);
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
            ChkPress.Location = new Point(12, 12);
            ChkPress.Name = "ChkPress";
            ChkPress.Size = new Size(53, 19);
            ChkPress.TabIndex = 3;
            ChkPress.Text = "Press";
            ChkPress.UseVisualStyleBackColor = true;
            // 
            // ChkRelease
            // 
            ChkRelease.AutoSize = true;
            ChkRelease.Location = new Point(12, 37);
            ChkRelease.Name = "ChkRelease";
            ChkRelease.Size = new Size(65, 19);
            ChkRelease.TabIndex = 4;
            ChkRelease.Text = "Release";
            ChkRelease.UseVisualStyleBackColor = true;
            // 
            // LblStats
            // 
            LblStats.AutoSize = true;
            LblStats.Location = new Point(122, 16);
            LblStats.Name = "LblStats";
            LblStats.Size = new Size(35, 15);
            LblStats.TabIndex = 5;
            LblStats.Text = "Stats:";
            // 
            // TxtStrat
            // 
            TxtStrat.Location = new Point(12, 62);
            TxtStrat.Name = "TxtStrat";
            TxtStrat.Size = new Size(100, 23);
            TxtStrat.TabIndex = 6;
            // 
            // BtnDoStrat
            // 
            BtnDoStrat.Location = new Point(12, 91);
            BtnDoStrat.Name = "BtnDoStrat";
            BtnDoStrat.Size = new Size(75, 23);
            BtnDoStrat.TabIndex = 7;
            BtnDoStrat.Text = "Do Strat";
            BtnDoStrat.UseVisualStyleBackColor = true;
            BtnDoStrat.Click += BtnDoStrat_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(700, 338);
            Controls.Add(BtnDoStrat);
            Controls.Add(TxtStrat);
            Controls.Add(LblStats);
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
        private Label LblStats;
        private TextBox TxtStrat;
        private Button BtnDoStrat;
    }
}
