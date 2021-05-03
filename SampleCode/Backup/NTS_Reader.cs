using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace NTS_Reader_CS
{
    public partial class NTS_Reader : Form
    {
        public NTS_Reader()
        {
            InitializeComponent();
        }

        private void btnPdf_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDlg = new OpenFileDialog();

            fileDlg.Filter = "PDF Files(*.PDF)|*.PDF";
            fileDlg.Multiselect = false;

            if (fileDlg.ShowDialog() == DialogResult.OK)
            {
                txtPdf.Text = fileDlg.FileName;
                txtPass.Text = string.Empty;
                txtEuckr.Text = string.Empty;
                txtUtf8.Text = string.Empty;
            }
        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            long result = 0;
            string filePath = txtPdf.Text;        
            string strMsg = string.Empty;
            
            byte[] baGenTime = new byte[1024];
            byte[] baHashAlg = new byte[1024];
            byte[] baHashVal = new byte[1024];
            byte[] baCertDN = new byte[1024];

            result = TstUtil.DSTSPdfSigVerifyF(filePath, baGenTime, baHashAlg, baHashVal, baCertDN);


            String sGenTimeTemp = Encoding.Unicode.GetString(baGenTime);
            String sHashAlgTemp = Encoding.Unicode.GetString(baHashAlg);
            String sHashValTemp = Encoding.Unicode.GetString(baHashVal);
            String sCertDNTemp = Encoding.Unicode.GetString(baCertDN);

            String sGenTime = sGenTimeTemp.Replace('\0', ' ').Trim();
            String sHashAlg = sHashAlgTemp.Replace('\0', ' ').Trim();
            String sHashVal = sHashValTemp.Replace('\0', ' ').Trim();
            String sCertDN = sCertDNTemp.Replace('\0', ' ').Trim();

            switch (result)
            {
                case 0:
                    strMsg = String.Format("원본 파일입니다. \n\nTS시각: {0} \n해쉬알고리즘: {1} \n해쉬값: {2} \nTSA인증서: {3}", sGenTime, sHashAlg, sHashVal, sCertDN);
                    break;
                case 2101:
                    strMsg = String.Format("문서가 변조되었습니다.");
                    break;
                case 2102:
                    strMsg = String.Format("TSA 서명 검증에 실패하였습니다.");
                    break;
                case 2103:
                    strMsg = String.Format("지원하지 않는 해쉬알고리즘 입니다.");
                    break;
                case 2104:
                    strMsg = String.Format("해당 파일을 읽을 수 없습니다.");
                    break;
                case 2105:
                    strMsg = String.Format("서명검증을 위한 API 처리 오류입니다.");
                    break;
                case 2106:
                    strMsg = String.Format("타임스탬프 토큰 데이터 파싱 오류입니다.");
                    break;
                case 2107:
                    strMsg = String.Format("TSA 인증서 처리 오류입니다.");
                    break;
                case 2108:
                    strMsg = String.Format("타임스탬프가 적용되지 않은 파일입니다.");
                    break;
                case 2109:
                    strMsg = String.Format("인증서 검증에 실패 하였습니다.");
                    break;
                default:
                    strMsg = String.Format("에러코드 미정의 error - [%d]", result);
                    break;
            } 
            

            MessageBox.Show(strMsg);
        }

        /// <summary>
        /// UTF-8 버튼 클릭 이벤트 : PDF 파일 내용을 UTF-8로 변환
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUtf8_Click(object sender, EventArgs e)
        {
            string filePath = txtPdf.Text;
            string password = txtPass.Text;
            string strXML = "XML";

            if(string.IsNullOrEmpty(filePath))
            {
                return;
            }

            int fileSize = EXPFile.NTS_GetFileSize(filePath,  password, strXML, 0);

            if (fileSize > 0)
	        {
                byte[] buf = new byte[fileSize];
		        fileSize = EXPFile.NTS_GetFileBuf( filePath,  password, strXML, buf, 0);
		        if (fileSize > 0)
		        {
                    string strBuf = Encoding.UTF8.GetString(buf);
                    strBuf.Replace("\n", "\r\n");
                    txtUtf8.Text = strBuf;
		        }
            }

            if (fileSize == -10)
            {
                MessageBox.Show("파일이 없거나 손상된 PDF 파일입니다.");
            }
            else if (fileSize == -11)
            {
                MessageBox.Show("국세청에서 발급된 전자문서가 아닙니다.");
            }
            else if (fileSize == -13)
            {
                MessageBox.Show("추출용 버퍼가 유효하지 않습니다.");
            }
            else if (fileSize == -200)
            {
                MessageBox.Show("비밀번호가 틀립니다.");
            }
        }

        /// <summary>
        /// EUC-KR 버튼 클릭 이벤트 : PDF 파일 내용을 EUC-KR로 변환
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEuckr_Click(object sender, EventArgs e)
        {
            int euckr = 51949;                  // EUC-KR 인코딩 값
            string filePath = txtPdf.Text;
            string password = txtPass.Text;
            string strXML = "XML";

            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            try
            {
                int fileSize = EXPFile.NTS_GetFileSize(filePath, password, strXML, 1);
                if (fileSize > 0)
                {
                    byte[] buf = new byte[fileSize];
                    fileSize = EXPFile.NTS_GetFileBuf(filePath, password, strXML, buf, 1);
                    if (fileSize > 0)
                    {
                        string strBuf = Encoding.GetEncoding(euckr).GetString(buf);
                        strBuf.Replace("\n", "\r\n");
                        txtEuckr.Text = strBuf;
                    }
                }

                if (fileSize == -10)
                {
                    MessageBox.Show("파일이 없거나 손상된 PDF 파일입니다.");
                }
                else if (fileSize == -11)
                {
                    MessageBox.Show("국세청에서 발급된 전자문서가 아닙니다.");
                }
                else if (fileSize == -13)
                {
                    MessageBox.Show("추출용 버퍼가 유효하지 않습니다.");
                }
                else if (fileSize == -200)
                {
                    MessageBox.Show("비밀번호가 틀립니다.");
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
