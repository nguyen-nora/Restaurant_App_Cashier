namespace app_quan_ly
{
    partial class ChuyenBan
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
            this.cbChuyenA = new System.Windows.Forms.ComboBox();
            this.cbChuyenB = new System.Windows.Forms.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cbChuyenA
            // 
            this.cbChuyenA.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.cbChuyenA.FormattingEnabled = true;
            this.cbChuyenA.Location = new System.Drawing.Point(34, 23);
            this.cbChuyenA.Name = "cbChuyenA";
            this.cbChuyenA.Size = new System.Drawing.Size(363, 30);
            this.cbChuyenA.TabIndex = 170;
            this.cbChuyenA.Text = "Bàn muốn chuyển";
            // 
            // cbChuyenB
            // 
            this.cbChuyenB.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.cbChuyenB.FormattingEnabled = true;
            this.cbChuyenB.Location = new System.Drawing.Point(34, 72);
            this.cbChuyenB.Name = "cbChuyenB";
            this.cbChuyenB.Size = new System.Drawing.Size(363, 30);
            this.cbChuyenB.TabIndex = 171;
            this.cbChuyenB.Text = "Bàn chuyển sang";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(322, 142);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 172;
            this.btnSave.Text = "Chuyển";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // ChuyenBan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 207);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.cbChuyenB);
            this.Controls.Add(this.cbChuyenA);
            this.Name = "ChuyenBan";
            this.Text = "ChuyenBan";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cbChuyenA;
        private System.Windows.Forms.ComboBox cbChuyenB;
        private System.Windows.Forms.Button btnSave;
    }
}