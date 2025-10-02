namespace Forager {
    partial class SettingsDialog {
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
            this.numShroomsNUD = new System.Windows.Forms.NumericUpDown();
            this.fieldSizeNUD = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numShroomsNUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldSizeNUD)).BeginInit();
            this.SuspendLayout();
            // 
            // numShroomsNUD
            // 
            this.numShroomsNUD.BackColor = System.Drawing.Color.White;
            this.numShroomsNUD.Location = new System.Drawing.Point(179, 23);
            this.numShroomsNUD.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numShroomsNUD.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numShroomsNUD.Name = "numShroomsNUD";
            this.numShroomsNUD.ReadOnly = true;
            this.numShroomsNUD.Size = new System.Drawing.Size(55, 22);
            this.numShroomsNUD.TabIndex = 10;
            this.numShroomsNUD.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // fieldSizeNUD
            // 
            this.fieldSizeNUD.BackColor = System.Drawing.Color.White;
            this.fieldSizeNUD.Location = new System.Drawing.Point(179, 64);
            this.fieldSizeNUD.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.fieldSizeNUD.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.fieldSizeNUD.Name = "fieldSizeNUD";
            this.fieldSizeNUD.ReadOnly = true;
            this.fieldSizeNUD.Size = new System.Drawing.Size(55, 22);
            this.fieldSizeNUD.TabIndex = 1;
            this.fieldSizeNUD.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(142, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Number of mushrooms";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Size of field";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(78, 99);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(159, 99);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // SettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(252, 134);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.fieldSizeNUD);
            this.Controls.Add(this.numShroomsNUD);
            this.Name = "SettingsDialog";
            ((System.ComponentModel.ISupportInitialize)(this.numShroomsNUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldSizeNUD)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numShroomsNUD;
        private System.Windows.Forms.NumericUpDown fieldSizeNUD;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}