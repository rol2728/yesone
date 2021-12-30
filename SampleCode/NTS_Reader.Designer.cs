namespace NTS_Reader_CS
{
    partial class NTS_Reader
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.grpPdf = new System.Windows.Forms.GroupBox();
            this.lb_결과 = new System.Windows.Forms.Label();
            this.tb_empNo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnUtf8 = new System.Windows.Forms.Button();
            this.btnPdf = new System.Windows.Forms.Button();
            this.txtPdf = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtUtf8 = new System.Windows.Forms.RichTextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_ycalyear = new System.Windows.Forms.TextBox();
            this.grpPdf.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // grpPdf
            // 
            this.grpPdf.Controls.Add(this.tb_ycalyear);
            this.grpPdf.Controls.Add(this.label3);
            this.grpPdf.Controls.Add(this.lb_결과);
            this.grpPdf.Controls.Add(this.tb_empNo);
            this.grpPdf.Controls.Add(this.label2);
            this.grpPdf.Controls.Add(this.btnUtf8);
            this.grpPdf.Controls.Add(this.btnPdf);
            this.grpPdf.Controls.Add(this.txtPdf);
            this.grpPdf.Controls.Add(this.label1);
            this.grpPdf.Location = new System.Drawing.Point(271, 6);
            this.grpPdf.Name = "grpPdf";
            this.grpPdf.Size = new System.Drawing.Size(559, 197);
            this.grpPdf.TabIndex = 0;
            this.grpPdf.TabStop = false;
            // 
            // lb_결과
            // 
            this.lb_결과.AutoSize = true;
            this.lb_결과.Location = new System.Drawing.Point(28, 125);
            this.lb_결과.Name = "lb_결과";
            this.lb_결과.Size = new System.Drawing.Size(53, 12);
            this.lb_결과.TabIndex = 6;
            this.lb_결과.Text = "결과문자";
            // 
            // tb_empNo
            // 
            this.tb_empNo.BackColor = System.Drawing.Color.White;
            this.tb_empNo.Location = new System.Drawing.Point(100, 76);
            this.tb_empNo.Name = "tb_empNo";
            this.tb_empNo.ReadOnly = true;
            this.tb_empNo.Size = new System.Drawing.Size(68, 21);
            this.tb_empNo.TabIndex = 4;
            this.tb_empNo.Text = "10110006";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "사     번";
            // 
            // btnUtf8
            // 
            this.btnUtf8.Image = global::NTS_Reader_CS.Properties.Resources.upload1;
            this.btnUtf8.Location = new System.Drawing.Point(435, 14);
            this.btnUtf8.Name = "btnUtf8";
            this.btnUtf8.Size = new System.Drawing.Size(101, 107);
            this.btnUtf8.TabIndex = 1;
            this.btnUtf8.UseVisualStyleBackColor = true;
            this.btnUtf8.Click += new System.EventHandler(this.btnUtf8_Click);
            // 
            // btnPdf
            // 
            this.btnPdf.Location = new System.Drawing.Point(305, 20);
            this.btnPdf.Name = "btnPdf";
            this.btnPdf.Size = new System.Drawing.Size(85, 23);
            this.btnPdf.TabIndex = 2;
            this.btnPdf.Text = "파일 찾기...";
            this.btnPdf.UseVisualStyleBackColor = true;
            this.btnPdf.Click += new System.EventHandler(this.btnPdf_Click);
            // 
            // txtPdf
            // 
            this.txtPdf.Location = new System.Drawing.Point(100, 21);
            this.txtPdf.Name = "txtPdf";
            this.txtPdf.Size = new System.Drawing.Size(199, 21);
            this.txtPdf.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Location = new System.Drawing.Point(26, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "PDF 파일 : ";
            // 
            // txtUtf8
            // 
            this.txtUtf8.BackColor = System.Drawing.SystemColors.Window;
            this.txtUtf8.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtUtf8.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtUtf8.Location = new System.Drawing.Point(12, 227);
            this.txtUtf8.Name = "txtUtf8";
            this.txtUtf8.ReadOnly = true;
            this.txtUtf8.Size = new System.Drawing.Size(818, 277);
            this.txtUtf8.TabIndex = 0;
            this.txtUtf8.Text = "";
            this.txtUtf8.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::NTS_Reader_CS.Properties.Resources.yeonmal;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(253, 191);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "정산연도";
            // 
            // tb_ycalyear
            // 
            this.tb_ycalyear.BackColor = System.Drawing.Color.White;
            this.tb_ycalyear.Location = new System.Drawing.Point(100, 47);
            this.tb_ycalyear.Name = "tb_ycalyear";
            this.tb_ycalyear.ReadOnly = true;
            this.tb_ycalyear.Size = new System.Drawing.Size(68, 21);
            this.tb_ycalyear.TabIndex = 8;
            this.tb_ycalyear.Text = "2021";
            // 
            // NTS_Reader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(842, 216);
            this.Controls.Add(this.txtUtf8);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.grpPdf);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NTS_Reader";
            this.ShowIcon = false;
            this.Text = "편리한 연말정산 Muju";
            this.grpPdf.ResumeLayout(false);
            this.grpPdf.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpPdf;
        private System.Windows.Forms.Button btnPdf;
        private System.Windows.Forms.TextBox txtPdf;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnUtf8;
        private System.Windows.Forms.TextBox tb_empNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.RichTextBox txtUtf8;
        private System.Windows.Forms.Label lb_결과;
        private System.Windows.Forms.TextBox tb_ycalyear;
        private System.Windows.Forms.Label label3;
    }
}

