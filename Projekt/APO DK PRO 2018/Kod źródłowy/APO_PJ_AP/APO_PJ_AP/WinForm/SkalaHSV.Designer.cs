using System.ComponentModel;
using System.Windows.Forms;

namespace APO_PJ_AP
{
    partial class SkalaHSV
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SkalaHSV));
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.labelB = new System.Windows.Forms.Label();
            this.labelG = new System.Windows.Forms.Label();
            this.labelR = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.labelV = new System.Windows.Forms.Label();
            this.labelS = new System.Windows.Forms.Label();
            this.labelH = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pnlDarkColour2 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox5
            // 
            this.pictureBox5.ErrorImage = ((System.Drawing.Image)(resources.GetObject("pictureBox5.ErrorImage")));
            this.pictureBox5.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox5.Image")));
            this.pictureBox5.Location = new System.Drawing.Point(142, 192);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(431, 166);
            this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox5.TabIndex = 23;
            this.pictureBox5.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.pnlDarkColour2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(561, 174);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Skala Kolorów";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.labelB);
            this.groupBox3.Controls.Add(this.labelG);
            this.groupBox3.Controls.Add(this.labelR);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Location = new System.Drawing.Point(164, 23);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(181, 125);
            this.groupBox3.TabIndex = 29;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "RGB";
            // 
            // labelB
            // 
            this.labelB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelB.Location = new System.Drawing.Point(58, 84);
            this.labelB.Name = "labelB";
            this.labelB.Size = new System.Drawing.Size(61, 16);
            this.labelB.TabIndex = 30;
            // 
            // labelG
            // 
            this.labelG.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelG.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelG.Location = new System.Drawing.Point(59, 55);
            this.labelG.Name = "labelG";
            this.labelG.Size = new System.Drawing.Size(60, 16);
            this.labelG.TabIndex = 29;
            // 
            // labelR
            // 
            this.labelR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelR.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelR.Location = new System.Drawing.Point(59, 29);
            this.labelR.Name = "labelR";
            this.labelR.Size = new System.Drawing.Size(60, 16);
            this.labelR.TabIndex = 28;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label8.Location = new System.Drawing.Point(18, 28);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 16);
            this.label8.TabIndex = 25;
            this.label8.Text = "R = ";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label9.Location = new System.Drawing.Point(18, 78);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(34, 16);
            this.label9.TabIndex = 27;
            this.label9.Text = "B = ";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label10.Location = new System.Drawing.Point(18, 53);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(35, 16);
            this.label10.TabIndex = 26;
            this.label10.Text = "G = ";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.labelV);
            this.groupBox2.Controls.Add(this.labelS);
            this.groupBox2.Controls.Add(this.labelH);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(360, 23);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(181, 125);
            this.groupBox2.TabIndex = 28;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "HSV";
            // 
            // labelV
            // 
            this.labelV.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelV.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelV.Location = new System.Drawing.Point(58, 84);
            this.labelV.Name = "labelV";
            this.labelV.Size = new System.Drawing.Size(61, 16);
            this.labelV.TabIndex = 30;
            // 
            // labelS
            // 
            this.labelS.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelS.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelS.Location = new System.Drawing.Point(59, 55);
            this.labelS.Name = "labelS";
            this.labelS.Size = new System.Drawing.Size(60, 16);
            this.labelS.TabIndex = 29;
            // 
            // labelH
            // 
            this.labelH.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelH.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelH.Location = new System.Drawing.Point(59, 29);
            this.labelH.Name = "labelH";
            this.labelH.Size = new System.Drawing.Size(60, 16);
            this.labelH.TabIndex = 28;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(18, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 16);
            this.label2.TabIndex = 25;
            this.label2.Text = "H = ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label4.Location = new System.Drawing.Point(18, 78);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 16);
            this.label4.TabIndex = 27;
            this.label4.Text = "V = ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label3.Location = new System.Drawing.Point(18, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 16);
            this.label3.TabIndex = 26;
            this.label3.Text = "S = ";
            // 
            // pnlDarkColour2
            // 
            this.pnlDarkColour2.BackColor = System.Drawing.Color.RoyalBlue;
            this.pnlDarkColour2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDarkColour2.Location = new System.Drawing.Point(20, 80);
            this.pnlDarkColour2.Name = "pnlDarkColour2";
            this.pnlDarkColour2.Size = new System.Drawing.Size(112, 33);
            this.pnlDarkColour2.TabIndex = 23;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(20, 28);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 33);
            this.button1.TabIndex = 0;
            this.button1.Text = "RGB => HSV";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // SkalaHSV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(585, 371);
            this.Controls.Add(this.pictureBox5);
            this.Controls.Add(this.groupBox1);
            this.Name = "SkalaHSV";
            this.Text = "SkalaHSV";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private PictureBox pictureBox5;
        private GroupBox groupBox1;
        private GroupBox groupBox3;
        private Label labelB;
        private Label labelG;
        private Label labelR;
        private Label label8;
        private Label label9;
        private Label label10;
        private GroupBox groupBox2;
        private Label labelV;
        private Label labelS;
        private Label labelH;
        private Label label2;
        private Label label4;
        private Label label3;
        private Panel pnlDarkColour2;
        private Button button1;
    }
}