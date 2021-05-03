﻿namespace NTS_Reader_CS
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
            this.txtPass = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnPdf = new System.Windows.Forms.Button();
            this.txtPdf = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.grpButton = new System.Windows.Forms.GroupBox();
            this.btnEuckr = new System.Windows.Forms.Button();
            this.btnUtf8 = new System.Windows.Forms.Button();
            this.btnVerify = new System.Windows.Forms.Button();
            this.grpEuckr = new System.Windows.Forms.GroupBox();
            this.txtEuckr = new System.Windows.Forms.RichTextBox();
            this.grpUtf8 = new System.Windows.Forms.GroupBox();
            this.txtUtf8 = new System.Windows.Forms.RichTextBox();
            this.grpPdf.SuspendLayout();
            this.grpButton.SuspendLayout();
            this.grpEuckr.SuspendLayout();
            this.grpUtf8.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpPdf
            // 
            this.grpPdf.Controls.Add(this.txtPass);
            this.grpPdf.Controls.Add(this.label2);
            this.grpPdf.Controls.Add(this.btnPdf);
            this.grpPdf.Controls.Add(this.txtPdf);
            this.grpPdf.Controls.Add(this.label1);
            this.grpPdf.Location = new System.Drawing.Point(12, 12);
            this.grpPdf.Name = "grpPdf";
            this.grpPdf.Size = new System.Drawing.Size(818, 63);
            this.grpPdf.TabIndex = 0;
            this.grpPdf.TabStop = false;
            // 
            // txtPass
            // 
            this.txtPass.Location = new System.Drawing.Point(677, 21);
            this.txtPass.Name = "txtPass";
            this.txtPass.Size = new System.Drawing.Size(119, 21);
            this.txtPass.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(583, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "PDF 비밀번호 :";
            // 
            // btnPdf
            // 
            this.btnPdf.Location = new System.Drawing.Point(479, 20);
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
            this.txtPdf.Size = new System.Drawing.Size(373, 21);
            this.txtPdf.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "PDF 파일 : ";
            // 
            // grpButton
            // 
            this.grpButton.Controls.Add(this.btnEuckr);
            this.grpButton.Controls.Add(this.btnUtf8);
            this.grpButton.Controls.Add(this.btnVerify);
            this.grpButton.Location = new System.Drawing.Point(12, 81);
            this.grpButton.Name = "grpButton";
            this.grpButton.Size = new System.Drawing.Size(818, 63);
            this.grpButton.TabIndex = 1;
            this.grpButton.TabStop = false;
            // 
            // btnEuckr
            // 
            this.btnEuckr.Location = new System.Drawing.Point(516, 20);
            this.btnEuckr.Name = "btnEuckr";
            this.btnEuckr.Size = new System.Drawing.Size(138, 37);
            this.btnEuckr.TabIndex = 2;
            this.btnEuckr.Text = "XML 읽기 (EUC-KR)";
            this.btnEuckr.UseVisualStyleBackColor = true;
            this.btnEuckr.Click += new System.EventHandler(this.btnEuckr_Click);
            // 
            // btnUtf8
            // 
            this.btnUtf8.Location = new System.Drawing.Point(349, 20);
            this.btnUtf8.Name = "btnUtf8";
            this.btnUtf8.Size = new System.Drawing.Size(138, 37);
            this.btnUtf8.TabIndex = 1;
            this.btnUtf8.Text = "XML 읽기 (UTF-8)";
            this.btnUtf8.UseVisualStyleBackColor = true;
            this.btnUtf8.Click += new System.EventHandler(this.btnUtf8_Click);
            // 
            // btnVerify
            // 
            this.btnVerify.Location = new System.Drawing.Point(165, 20);
            this.btnVerify.Name = "btnVerify";
            this.btnVerify.Size = new System.Drawing.Size(138, 37);
            this.btnVerify.TabIndex = 0;
            this.btnVerify.Text = "원본검증";
            this.btnVerify.UseVisualStyleBackColor = true;
            this.btnVerify.Click += new System.EventHandler(this.btnVerify_Click);
            // 
            // grpEuckr
            // 
            this.grpEuckr.Controls.Add(this.txtEuckr);
            this.grpEuckr.Location = new System.Drawing.Point(424, 150);
            this.grpEuckr.Name = "grpEuckr";
            this.grpEuckr.Size = new System.Drawing.Size(406, 354);
            this.grpEuckr.TabIndex = 1;
            this.grpEuckr.TabStop = false;
            this.grpEuckr.Text = "XML (EUC-KR)";
            // 
            // txtEuckr
            // 
            this.txtEuckr.BackColor = System.Drawing.SystemColors.Window;
            this.txtEuckr.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtEuckr.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtEuckr.Location = new System.Drawing.Point(6, 20);
            this.txtEuckr.Name = "txtEuckr";
            this.txtEuckr.ReadOnly = true;
            this.txtEuckr.Size = new System.Drawing.Size(394, 328);
            this.txtEuckr.TabIndex = 0;
            this.txtEuckr.Text = "";
            // 
            // grpUtf8
            // 
            this.grpUtf8.Controls.Add(this.txtUtf8);
            this.grpUtf8.Location = new System.Drawing.Point(12, 150);
            this.grpUtf8.Name = "grpUtf8";
            this.grpUtf8.Size = new System.Drawing.Size(406, 354);
            this.grpUtf8.TabIndex = 1;
            this.grpUtf8.TabStop = false;
            this.grpUtf8.Text = "XML (UTF-8)";
            // 
            // txtUtf8
            // 
            this.txtUtf8.BackColor = System.Drawing.SystemColors.Window;
            this.txtUtf8.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtUtf8.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtUtf8.Location = new System.Drawing.Point(6, 20);
            this.txtUtf8.Name = "txtUtf8";
            this.txtUtf8.ReadOnly = true;
            this.txtUtf8.Size = new System.Drawing.Size(394, 328);
            this.txtUtf8.TabIndex = 0;
            this.txtUtf8.Text = "";
            // 
            // NTS_Reader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(842, 516);
            this.Controls.Add(this.grpButton);
            this.Controls.Add(this.grpEuckr);
            this.Controls.Add(this.grpUtf8);
            this.Controls.Add(this.grpPdf);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NTS_Reader";
            this.ShowIcon = false;
            this.Text = "NTS_Reader";
            this.grpPdf.ResumeLayout(false);
            this.grpPdf.PerformLayout();
            this.grpButton.ResumeLayout(false);
            this.grpEuckr.ResumeLayout(false);
            this.grpUtf8.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpPdf;
        private System.Windows.Forms.GroupBox grpButton;
        private System.Windows.Forms.GroupBox grpEuckr;
        private System.Windows.Forms.GroupBox grpUtf8;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnPdf;
        private System.Windows.Forms.TextBox txtPdf;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnEuckr;
        private System.Windows.Forms.Button btnUtf8;
        private System.Windows.Forms.Button btnVerify;
        private System.Windows.Forms.TextBox txtPass;
        private System.Windows.Forms.RichTextBox txtUtf8;
        private System.Windows.Forms.RichTextBox txtEuckr;
    }
}

