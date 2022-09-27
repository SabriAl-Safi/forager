namespace Forager.WinForms {
    partial class Forager {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.panel1 = new System.Windows.Forms.Panel();
            this.newGameButton = new System.Windows.Forms.Button();
            this.resetBoardButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.distanceLabel = new System.Windows.Forms.Label();
            this.goalLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.streakLabel = new System.Windows.Forms.Label();
            this.topStreakLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.difficultyButton = new System.Windows.Forms.Button();
            this.resetStreakButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.difficultyLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(16, 15);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(573, 541);
            this.panel1.TabIndex = 0;
            // 
            // newGameButton
            // 
            this.newGameButton.Location = new System.Drawing.Point(16, 560);
            this.newGameButton.Margin = new System.Windows.Forms.Padding(4);
            this.newGameButton.Name = "newGameButton";
            this.newGameButton.Size = new System.Drawing.Size(110, 28);
            this.newGameButton.TabIndex = 1;
            this.newGameButton.Text = "New game";
            this.newGameButton.UseVisualStyleBackColor = true;
            this.newGameButton.Click += new System.EventHandler(this.newGameButton_Click);
            // 
            // resetBoardButton
            // 
            this.resetBoardButton.Enabled = false;
            this.resetBoardButton.Location = new System.Drawing.Point(16, 596);
            this.resetBoardButton.Margin = new System.Windows.Forms.Padding(4);
            this.resetBoardButton.Name = "resetBoardButton";
            this.resetBoardButton.Size = new System.Drawing.Size(110, 28);
            this.resetBoardButton.TabIndex = 2;
            this.resetBoardButton.Text = "Reset Board";
            this.resetBoardButton.UseVisualStyleBackColor = true;
            this.resetBoardButton.Click += new System.EventHandler(this.ResetBoard);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(412, 563);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Distance:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(412, 586);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "Goal:";
            // 
            // distanceLabel
            // 
            this.distanceLabel.AutoSize = true;
            this.distanceLabel.Location = new System.Drawing.Point(472, 563);
            this.distanceLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.distanceLabel.Name = "distanceLabel";
            this.distanceLabel.Size = new System.Drawing.Size(14, 16);
            this.distanceLabel.TabIndex = 5;
            this.distanceLabel.Text = "0";
            // 
            // goalLabel
            // 
            this.goalLabel.AutoSize = true;
            this.goalLabel.Location = new System.Drawing.Point(472, 586);
            this.goalLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.goalLabel.Name = "goalLabel";
            this.goalLabel.Size = new System.Drawing.Size(14, 16);
            this.goalLabel.TabIndex = 6;
            this.goalLabel.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(502, 564);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 16);
            this.label3.TabIndex = 7;
            this.label3.Text = "Streak:";
            // 
            // streakLabel
            // 
            this.streakLabel.AutoSize = true;
            this.streakLabel.Location = new System.Drawing.Point(573, 564);
            this.streakLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.streakLabel.Name = "streakLabel";
            this.streakLabel.Size = new System.Drawing.Size(14, 16);
            this.streakLabel.TabIndex = 8;
            this.streakLabel.Text = "0";
            // 
            // topStreakLabel
            // 
            this.topStreakLabel.AutoSize = true;
            this.topStreakLabel.Location = new System.Drawing.Point(573, 586);
            this.topStreakLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.topStreakLabel.Name = "topStreakLabel";
            this.topStreakLabel.Size = new System.Drawing.Size(14, 16);
            this.topStreakLabel.TabIndex = 10;
            this.topStreakLabel.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(501, 586);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 16);
            this.label5.TabIndex = 9;
            this.label5.Text = "Top Streak:";
            // 
            // difficultyButton
            // 
            this.difficultyButton.Location = new System.Drawing.Point(140, 560);
            this.difficultyButton.Margin = new System.Windows.Forms.Padding(4);
            this.difficultyButton.Name = "difficultyButton";
            this.difficultyButton.Size = new System.Drawing.Size(110, 28);
            this.difficultyButton.TabIndex = 11;
            this.difficultyButton.Text = "Difficulty";
            this.difficultyButton.UseVisualStyleBackColor = true;
            this.difficultyButton.Click += new System.EventHandler(this.EditDifficulty);
            // 
            // resetStreakButton
            // 
            this.resetStreakButton.Enabled = false;
            this.resetStreakButton.Location = new System.Drawing.Point(140, 596);
            this.resetStreakButton.Margin = new System.Windows.Forms.Padding(4);
            this.resetStreakButton.Name = "resetStreakButton";
            this.resetStreakButton.Size = new System.Drawing.Size(110, 28);
            this.resetStreakButton.TabIndex = 12;
            this.resetStreakButton.Text = "Reset Streak";
            this.resetStreakButton.UseVisualStyleBackColor = true;
            this.resetStreakButton.Click += new System.EventHandler(this.ResetStreak);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(287, 563);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 16);
            this.label4.TabIndex = 13;
            this.label4.Text = "Difficulty:";
            // 
            // difficultyLabel
            // 
            this.difficultyLabel.AutoSize = true;
            this.difficultyLabel.Location = new System.Drawing.Point(347, 563);
            this.difficultyLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.difficultyLabel.Name = "difficultyLabel";
            this.difficultyLabel.Size = new System.Drawing.Size(55, 16);
            this.difficultyLabel.TabIndex = 14;
            this.difficultyLabel.Text = "Medium";
            // 
            // Forager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(608, 634);
            this.Controls.Add(this.difficultyLabel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.resetStreakButton);
            this.Controls.Add(this.difficultyButton);
            this.Controls.Add(this.topStreakLabel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.streakLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.goalLabel);
            this.Controls.Add(this.distanceLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.resetBoardButton);
            this.Controls.Add(this.newGameButton);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Forager";
            this.Text = "Forager";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button newGameButton;
        private System.Windows.Forms.Button resetBoardButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label distanceLabel;
        private System.Windows.Forms.Label goalLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label streakLabel;
        private System.Windows.Forms.Label topStreakLabel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button difficultyButton;
        private System.Windows.Forms.Button resetStreakButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label difficultyLabel;
    }
}

