namespace IF_IDF
{
    partial class FormLucene
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
            this.txt = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnResultFile = new System.Windows.Forms.Button();
            this.rstData = new System.Windows.Forms.DataGridView();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rstData)).BeginInit();
            this.SuspendLayout();
            // 
            // txt
            // 
            this.txt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt.Location = new System.Drawing.Point(495, 9);
            this.txt.Multiline = true;
            this.txt.Name = "txt";
            this.txt.Size = new System.Drawing.Size(205, 62);
            this.txt.TabIndex = 11;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(247, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(170, 59);
            this.button1.TabIndex = 10;
            this.button1.Text = "Get Precision";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(495, 77);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(477, 498);
            this.dataGridView1.TabIndex = 9;
            // 
            // btnResultFile
            // 
            this.btnResultFile.Location = new System.Drawing.Point(12, 12);
            this.btnResultFile.Name = "btnResultFile";
            this.btnResultFile.Size = new System.Drawing.Size(170, 59);
            this.btnResultFile.TabIndex = 8;
            this.btnResultFile.Text = "get File Result";
            this.btnResultFile.UseVisualStyleBackColor = true;
            this.btnResultFile.Click += new System.EventHandler(this.btnResultFile_Click_1);
            // 
            // rstData
            // 
            this.rstData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.rstData.Location = new System.Drawing.Point(12, 77);
            this.rstData.Name = "rstData";
            this.rstData.RowTemplate.Height = 24;
            this.rstData.Size = new System.Drawing.Size(477, 498);
            this.rstData.TabIndex = 7;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // FormLucene
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(978, 588);
            this.Controls.Add(this.txt);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnResultFile);
            this.Controls.Add(this.rstData);
            this.Name = "FormLucene";
            this.Text = "FormLucene";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rstData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnResultFile;
        private System.Windows.Forms.DataGridView rstData;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}