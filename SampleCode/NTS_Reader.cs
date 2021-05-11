using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using System.Linq;
using NTS_Reader_CS.xml;
using NTS_Reader_CS.db;

namespace NTS_Reader_CS
{
    public partial class NTS_Reader : Form
    {

        /*---------------------  클래스 초기화 -----------------*/

        A102 a102 = new A102();
        B101 b101 = new B101();
        B201 b201 = new B201();
        C102 c102 = new C102();
        C202 c202 = new C202();
        C301 c301 = new C301();
        C401 c401 = new C401();
        D101 d101 = new D101();
        E102 e102 = new E102();
        F102 f102 = new F102();
        G108 g108 = new G108();
        G208 g208 = new G208();
        G308 g308 = new G308();
        G408 g408 = new G408();
        J101 j101 = new J101();
        J203 j203 = new J203();
        J301 j301 = new J301();
        J401 j401 = new J401();
        J501 j501 = new J501();
        K101 k101 = new K101();
        L102 l102 = new L102();
        N101 n101 = new N101();
        O101 o101 = new O101();
        P102 p102 = new P102();
        Q101 q101 = new Q101();
        Q201 q201 = new Q201();


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
            string password = "";
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

                    strBuf.Replace("\n", " ");
                    XDocument xdoc = XDocument.Parse(strBuf);

                    txtUtf8.Text = strBuf;
                    

                    XElement xElem = XElement.Parse(strBuf);
                    var result = from xe in xElem.Elements("doc")                             
                                 select xe;

                    var form = result.SingleOrDefault();
                    string att_year = form.Element("att_year").Value;

                    if(att_year != "2020")
                    {
                        MessageBox.Show("해당 정산년도가 아닙니다.");
                    }

                    

                    /*-------------------------------- XML 로딩 후 데이터 처리 --------------------------------------------*/

                    var forms = from xe in xElem.Elements("form")
                                 select xe;

                    foreach (var frm in forms)
                    {
                        string form_cd = frm.Attribute("form_cd").Value;

                        
                        /* -------------------- 보험료 ------------------*/
                        if (form_cd.Contains("A102"))  
                        {   
                            a102.form_cd = form_cd;

                            // 인별반복
                            var mans = from man in frm.Elements("man")
                                        select man;

                            a102.인별 = new A102.인별반복[mans.Count()];

                            int ix = 0;
                            foreach (var man in mans)
                            {                                

                                a102.인별[ix].resid = man.Attribute("resid").Value;
                                a102.인별[ix].name = man.Attribute("name").Value;

                                //상품별반복
                                var datas = from data in man.Elements("data")
                                           select data;

                               
                                a102.인별[ix].상품 = new A102.상품별반복[datas.Count()];

                                int j = 0;
                                foreach (var data in datas)
                                {
                                    a102.인별[ix].상품[j].dat_cd = data.Attribute("dat_cd").Value;
                                    a102.인별[ix].상품[j].busnid = data.Attribute("busnid").Value;
                                    a102.인별[ix].상품[j].trade_nm = data.Attribute("trade_nm").Value;
                                    a102.인별[ix].상품[j].acc_no = data.Attribute("acc_no").Value;

                                    a102.인별[ix].상품[j].good_nm = data.Element("goods_nm").Value;
                                    a102.인별[ix].상품[j].insu1_resid = data.Element("insu1_resid").Value;
                                    a102.인별[ix].상품[j].insu1_nm = data.Element("insu1_nm").Value;
                                    a102.인별[ix].상품[j].insu2_resid_1 = data.Element("insu2_resid_1").Value;
                                    a102.인별[ix].상품[j].insu2_nm_1 = data.Element("insu2_nm_1").Value;
                                    a102.인별[ix].상품[j].insu2_resid_2 = data.Element("insu2_resid_2").Value;
                                    a102.인별[ix].상품[j].insu2_nm_2 = data.Element("insu2_nm_2").Value;
                                    a102.인별[ix].상품[j].insu2_resid_3 = data.Element("insu2_resid_3").Value;
                                    a102.인별[ix].상품[j].insu2_nm_3 = data.Element("insu2_nm_3").Value;
                                    a102.인별[ix].상품[j].sum = Int32.Parse(data.Element("sum").Value);
                                    j++;
                                }
                                ix++;
                            }


                        }
                        /* -------------------- 의료비 ------------------*/
                        else if (form_cd.Contains("B101"))
                        {

                            b101.form_cd = form_cd;

                            // 인별반복
                            var mans = from man in frm.Elements("man")
                                       select man;

                            b101.인별 = new B101.인별반복[mans.Count()];

                            int ix = 0;
                            foreach (var man in mans)
                            {

                                b101.인별[ix].resid = man.Attribute("resid").Value;
                                b101.인별[ix].name = man.Attribute("name").Value;

                                //상품별반복
                                var datas = from data in man.Elements("data")
                                            select data;


                                b101.인별[ix].기관 = new B101.기관별반복[datas.Count()];

                                int j = 0;
                                foreach (var data in datas)
                                {
                                    b101.인별[ix].기관[j].dat_cd = data.Attribute("dat_cd").Value;
                                    b101.인별[ix].기관[j].busnid = data.Attribute("busnid").Value;
                                    b101.인별[ix].기관[j].trade_nm = data.Attribute("trade_nm").Value;

                                    b101.인별[ix].기관[j].sum = Int32.Parse(data.Element("sum").Value);
                                    j++;
                                }
                                ix++;
                            }

                        }
                        /* -------------------- 실손의료보험금 ------------------*/
                        else if (form_cd.Contains("B201"))  
                        {

                            b201.form_cd = form_cd;

                            // 인별반복
                            var mans = from man in frm.Elements("man")
                                       select man;

                            b201.인별 = new B201.인별반복[mans.Count()];

                            int ix = 0;
                            foreach (var man in mans)
                            {

                                b201.인별[ix].resid = man.Attribute("resid").Value;
                                b201.인별[ix].name = man.Attribute("name").Value;

                                //상품별반복
                                var datas = from data in man.Elements("data")
                                            select data;


                                b201.인별[ix].상품 = new B201.상품별반복[datas.Count()];

                                int j = 0;
                                foreach (var data in datas)
                                {
                                    b201.인별[ix].상품[j].dat_cd = data.Attribute("dat_cd").Value;
                                    b201.인별[ix].상품[j].busnid = data.Attribute("busnid").Value;
                                    b201.인별[ix].상품[j].trade_nm = data.Attribute("trade_nm").Value;
                                    b201.인별[ix].상품[j].acc_no = data.Attribute("acc_no").Value;

                                    b201.인별[ix].상품[j].goods_nm = data.Element("goods_nm").Value;
                                    b201.인별[ix].상품[j].insu_resid = data.Element("insu_resid").Value;
                                    b201.인별[ix].상품[j].insu_nm = data.Element("insu_nm").Value;
                                    b201.인별[ix].상품[j].sum = Int32.Parse(data.Element("sum").Value);
                                    j++;
                                }
                                ix++;
                            }

                        }
                        /* -------------------- 교육비(유초중고,대학,기타) ------------------*/
                        else if (form_cd.Contains("C102"))
                        {
                            c102.form_cd = form_cd;

                            // 인별반복
                            var mans = from man in frm.Elements("man")
                                       select man;

                            c102.인별 = new C102.인별반복[mans.Count()];

                            int ix = 0;
                            foreach (var man in mans)
                            {

                                c102.인별[ix].resid = man.Attribute("resid").Value;
                                c102.인별[ix].name = man.Attribute("name").Value;

                                //상품별반복
                                var datas = from data in man.Elements("data")
                                            select data;


                                c102.인별[ix].학교 = new C102.학교별반복[datas.Count()];

                                int j = 0;
                                foreach (var data in datas)
                                {
                                    c102.인별[ix].학교[j].dat_cd = data.Attribute("dat_cd").Value;
                                    c102.인별[ix].학교[j].busnid = data.Attribute("busnid").Value;
                                    c102.인별[ix].학교[j].trade_nm = data.Attribute("trade_nm").Value;
                                    c102.인별[ix].학교[j].edu_tp = data.Attribute("edu_tp").Value;
                                    c102.인별[ix].학교[j].edu_cl = data.Attribute("edu_cl").Value;

                                    c102.인별[ix].학교[j].sum = Int32.Parse(data.Element("sum").Value);
                                    j++;
                                }
                                ix++;
                            }
                        }
                        /* -------------------- 교육비(직업훈련비) ------------------*/
                        else if (form_cd.Contains("C202"))
                        {
                            c202.form_cd = form_cd;

                            // 인별반복
                            var mans = from man in frm.Elements("man")
                                       select man;

                            c202.인별 = new C202.인별반복[mans.Count()];

                            int ix = 0;
                            foreach (var man in mans)
                            {

                                c202.인별[ix].resid = man.Attribute("resid").Value;
                                c202.인별[ix].name = man.Attribute("name").Value;

                                //상품별반복
                                var datas = from data in man.Elements("data")
                                            select data;


                                c202.인별[ix].과정 = new C202.과정별반복[datas.Count()];

                                int j = 0;
                                foreach (var data in datas)
                                {
                                    c202.인별[ix].과정[j].dat_cd = data.Attribute("dat_cd").Value;
                                    c202.인별[ix].과정[j].busnid = data.Attribute("busnid").Value;

                                    c202.인별[ix].과정[j].course_cd = data.Element("course_cd").Value;
                                    c202.인별[ix].과정[j].subject_nm = data.Element("subject_nm").Value;  
                                    c202.인별[ix].과정[j].sum = Int32.Parse(data.Element("sum").Value);
                                    j++;
                                }
                                ix++;
                            }
                        }
                        /* -------------------- 교육비(교복구입비) ------------------*/
                        else if (form_cd.Contains("C301"))
                        {

                            c301.form_cd = form_cd;

                            // 인별반복
                            var mans = from man in frm.Elements("man")
                                       select man;

                            c301.인별 = new C301.인별반복[mans.Count()];

                            int ix = 0;
                            foreach (var man in mans)
                            {

                                c301.인별[ix].resid = man.Attribute("resid").Value;
                                c301.인별[ix].name = man.Attribute("name").Value;

                                //상품별반복
                                var datas = from data in man.Elements("data")
                                            select data;


                                c301.인별[ix].기관 = new C301.기관별반복[datas.Count()];

                                int j = 0;
                                foreach (var data in datas)
                                {
                                    c301.인별[ix].기관[j].dat_cd = data.Attribute("dat_cd").Value;
                                    c301.인별[ix].기관[j].busnid = data.Attribute("busnid").Value;
                                    c301.인별[ix].기관[j].trade_nm = data.Attribute("trade_nm").Value;                                    

                                    c301.인별[ix].기관[j].sum = Int32.Parse(data.Element("sum").Value);
                                    j++;
                                }
                                ix++;
                            }

                        }
                        /* -------------------- 교육비(학자급대출) ------------------*/
                        else if (form_cd.Contains("C401"))
                        {
                            c401.form_cd = form_cd;

                            // 인별반복
                            var mans = from man in frm.Elements("man")
                                       select man;

                            c401.인별 = new C401.인별반복[mans.Count()];

                            int ix = 0;
                            foreach (var man in mans)
                            {

                                c401.인별[ix].resid = man.Attribute("resid").Value;
                                c401.인별[ix].name = man.Attribute("name").Value;

                                //상품별반복
                                var datas = from data in man.Elements("data")
                                            select data;


                                c401.인별[ix].기관 = new C401.기관별반복[datas.Count()];

                                int j = 0;
                                foreach (var data in datas)
                                {
                                    c401.인별[ix].기관[j].dat_cd = data.Attribute("dat_cd").Value;
                                    c401.인별[ix].기관[j].busnid = data.Attribute("busnid").Value;
                                    c401.인별[ix].기관[j].trade_nm = data.Attribute("trade_nm").Value;

                                    c401.인별[ix].기관[j].sum = Int32.Parse(data.Element("sum").Value);
                                    j++;
                                }
                                ix++;
                            }
                        }
                        /* -------------------- 개인연금대출 ------------------*/
                        else if (form_cd.Contains("D101"))
                        {
                            d101.form_cd = form_cd;

                            // 인별반복
                            var mans = from man in frm.Elements("man")
                                       select man;

                            d101.인별 = new D101.인별반복[mans.Count()];

                            int ix = 0;
                            foreach (var man in mans)
                            {

                                d101.인별[ix].resid = man.Attribute("resid").Value;
                                d101.인별[ix].name = man.Attribute("name").Value;

                                //상품별반복
                                var datas = from data in man.Elements("data")
                                            select data;


                                d101.인별[ix].상품 = new D101.상품별반복[datas.Count()];

                                int j = 0;
                                foreach (var data in datas)
                                {
                                    d101.인별[ix].상품[j].dat_cd = data.Attribute("dat_cd").Value;
                                    d101.인별[ix].상품[j].busnid = data.Attribute("busnid").Value;
                                    d101.인별[ix].상품[j].trade_nm = data.Attribute("trade_nm").Value;
                                    d101.인별[ix].상품[j].acc_no = data.Attribute("acc_no").Value;

                                    d101.인별[ix].상품[j].start_dt = data.Element("start_dt").Value;
                                    d101.인별[ix].상품[j].end_dt = data.Element("end_dt").Value;
                                    d101.인별[ix].상품[j].com_cd = data.Element("com_cd").Value;
                                    d101.인별[ix].상품[j].sum = Int32.Parse(data.Element("sum").Value);
                                    j++;
                                }
                                ix++;
                            }
                        }
                        /* -------------------- 연금저축 ------------------*/
                        else if (form_cd.Contains("E102"))
                        {

                            e102.form_cd = form_cd;

                            // 인별반복
                            var mans = from man in frm.Elements("man")
                                       select man;

                            e102.인별 = new E102.인별반복[mans.Count()];

                            int ix = 0;
                            foreach (var man in mans)
                            {

                                e102.인별[ix].resid = man.Attribute("resid").Value;
                                e102.인별[ix].name = man.Attribute("name").Value;

                                //상품별반복
                                var datas = from data in man.Elements("data")
                                            select data;


                                e102.인별[ix].상품 = new E102.상품별반복[datas.Count()];

                                int j = 0;
                                foreach (var data in datas)
                                {
                                    e102.인별[ix].상품[j].dat_cd = data.Attribute("dat_cd").Value;
                                    e102.인별[ix].상품[j].busnid = data.Attribute("busnid").Value;
                                    e102.인별[ix].상품[j].trade_nm = data.Attribute("trade_nm").Value;
                                    e102.인별[ix].상품[j].acc_no = data.Attribute("acc_no").Value;
                                                                        
                                    e102.인별[ix].상품[j].com_cd = data.Element("com_cd").Value;
                                    e102.인별[ix].상품[j].ann_tot_amt = Int32.Parse(data.Element("ann_tot_amt").Value);
                                    e102.인별[ix].상품[j].tax_year_amt = Int32.Parse(data.Element("tax_year_amt").Value);
                                    e102.인별[ix].상품[j].ddct_bs_ass_amt = Int32.Parse(data.Element("ddct_bs_ass_amt").Value);                                    
                                    
                                    j++;
                                }
                                ix++;
                            }

                        }
                        /* -------------------- 퇴직연금 ------------------*/
                        else if (form_cd.Contains("F102"))
                        {
                            f102.form_cd = form_cd;

                            // 인별반복
                            var mans = from man in frm.Elements("man")
                                       select man;

                            f102.인별 = new F102.인별반복[mans.Count()];

                            int ix = 0;
                            foreach (var man in mans)
                            {

                                f102.인별[ix].resid = man.Attribute("resid").Value;
                                f102.인별[ix].name = man.Attribute("name").Value;

                                //상품별반복
                                var datas = from data in man.Elements("data")
                                            select data;


                                f102.인별[ix].상품 = new F102.상품별반복[datas.Count()];

                                int j = 0;
                                foreach (var data in datas)
                                {
                                    f102.인별[ix].상품[j].dat_cd = data.Attribute("dat_cd").Value;
                                    f102.인별[ix].상품[j].busnid = data.Attribute("busnid").Value;
                                    f102.인별[ix].상품[j].trade_nm = data.Attribute("trade_nm").Value;
                                    f102.인별[ix].상품[j].acc_no = data.Attribute("acc_no").Value;

                                    f102.인별[ix].상품[j].com_cd = data.Element("com_cd").Value;
                                    f102.인별[ix].상품[j].pension_cd = data.Element("pension_cd").Value;
                                    f102.인별[ix].상품[j].ann_tot_amt = Int32.Parse(data.Element("ann_tot_amt").Value);
                                    f102.인별[ix].상품[j].tax_year_amt = Int32.Parse(data.Element("tax_year_amt").Value);
                                    f102.인별[ix].상품[j].ddct_bs_ass_amt = Int32.Parse(data.Element("ddct_bs_ass_amt").Value);                                    
                                    j++;
                                }
                                ix++;
                            }
                        }
                        /* -------------------- 신용카드(2020년 귀속) ------------------*/
                        else if (form_cd.Contains("G108"))
                        {
                            g108.form_cd = form_cd;

                            // 인별반복
                            var mans = from man in frm.Elements("man")
                                       select man;

                            g108.인별 = new G108.인별반복[mans.Count()];

                            int ix = 0;
                            foreach (var man in mans)
                            {

                                g108.인별[ix].resid = man.Attribute("resid").Value;
                                g108.인별[ix].name = man.Attribute("name").Value;

                                //합계 sum_data

                                var sum_data = man.Element("sum_data");
                                g108.인별[ix].gnrl_sum = Int32.Parse(sum_data.Element("gnrl_sum").Value);
                                g108.인별[ix].tdmr_sum = Int32.Parse(sum_data.Element("tdmr_sum").Value);
                                g108.인별[ix].trp_sum = Int32.Parse(sum_data.Element("trp_sum").Value);
                                g108.인별[ix].isld_sum = Int32.Parse(sum_data.Element("isld_sum").Value);
                                g108.인별[ix].tot_sum = Int32.Parse(sum_data.Element("tot_sum").Value);
                                g108.인별[ix].gnrl_mar_sum = Int32.Parse(sum_data.Element("gnrl_mar_sum").Value);
                                g108.인별[ix].tdmr_mar_sum = Int32.Parse(sum_data.Element("tdmr_mar_sum").Value);
                                g108.인별[ix].trp_mar_sum = Int32.Parse(sum_data.Element("trp_mar_sum").Value);
                                g108.인별[ix].isld_mar_sum = Int32.Parse(sum_data.Element("isld_mar_sum").Value);
                                g108.인별[ix].tot_mar_sum = Int32.Parse(sum_data.Element("tot_mar_sum").Value);
                                g108.인별[ix].gnrl_aprl_sum = Int32.Parse(sum_data.Element("gnrl_aprl_sum").Value);
                                g108.인별[ix].tdmr_aprl_sum = Int32.Parse(sum_data.Element("tdmr_aprl_sum").Value);
                                g108.인별[ix].trp_aprl_sum = Int32.Parse(sum_data.Element("trp_aprl_sum").Value);
                                g108.인별[ix].isld_aprl_sum = Int32.Parse(sum_data.Element("isld_aprl_sum").Value);
                                g108.인별[ix].tot_aprl_sum = Int32.Parse(sum_data.Element("tot_aprl_sum").Value);
                                g108.인별[ix].gnrl_jan_sum = Int32.Parse(sum_data.Element("gnrl_jan_sum").Value);
                                g108.인별[ix].tdmr_jan_sum = Int32.Parse(sum_data.Element("tdmr_jan_sum").Value);
                                g108.인별[ix].trp_jan_sum = Int32.Parse(sum_data.Element("trp_jan_sum").Value);
                                g108.인별[ix].isld_jan_sum = Int32.Parse(sum_data.Element("isld_jan_sum").Value);
                                g108.인별[ix].tot_jan_sum = Int32.Parse(sum_data.Element("tot_jan_sum").Value);


                                //상품별반복
                                var datas = from data in man.Elements("data")
                                            select data;

                                g108.인별[ix].상품 = new G108.상품별반복[datas.Count()];                               

                                int j = 0;
                                foreach (var data in datas)
                                {
                                    g108.인별[ix].상품[j].dat_cd = data.Attribute("dat_cd").Value;
                                    g108.인별[ix].상품[j].busnid = data.Attribute("busnid").Value;
                                    g108.인별[ix].상품[j].trade_nm = data.Attribute("trade_nm").Value;
                                    g108.인별[ix].상품[j].use_place_cd = data.Attribute("use_place_cd").Value;

                                    g108.인별[ix].상품[j].sum = Int32.Parse(data.Element("sum").Value);
                                    
                                    j++;
                                }
                                ix++;
                            }


                        }
                        /* -------------------- 현금영수증(2020년 귀속) ------------------*/
                        else if (form_cd.Contains("G208"))
                        {
                            g208.form_cd = form_cd;

                            // 인별반복
                            var mans = from man in frm.Elements("man")
                                       select man;

                            g208.인별 = new G208.인별반복[mans.Count()];

                            int ix = 0;
                            foreach (var man in mans)
                            {

                                g208.인별[ix].resid = man.Attribute("resid").Value;
                                g208.인별[ix].name = man.Attribute("name").Value;

                                //합계 sum_data

                                var sum_data = man.Element("sum_data");
                                g208.인별[ix].gnrl_sum = Int32.Parse(sum_data.Element("gnrl_sum").Value);
                                g208.인별[ix].tdmr_sum = Int32.Parse(sum_data.Element("tdmr_sum").Value);
                                g208.인별[ix].trp_sum = Int32.Parse(sum_data.Element("trp_sum").Value);
                                g208.인별[ix].isld_sum = Int32.Parse(sum_data.Element("isld_sum").Value);
                                g208.인별[ix].tot_sum = Int32.Parse(sum_data.Element("tot_sum").Value);
                                g208.인별[ix].gnrl_mar_sum = Int32.Parse(sum_data.Element("gnrl_mar_sum").Value);
                                g208.인별[ix].tdmr_mar_sum = Int32.Parse(sum_data.Element("tdmr_mar_sum").Value);
                                g208.인별[ix].trp_mar_sum = Int32.Parse(sum_data.Element("trp_mar_sum").Value);
                                g208.인별[ix].isld_mar_sum = Int32.Parse(sum_data.Element("isld_mar_sum").Value);
                                g208.인별[ix].tot_mar_sum = Int32.Parse(sum_data.Element("tot_mar_sum").Value);
                                g208.인별[ix].gnrl_aprl_sum = Int32.Parse(sum_data.Element("gnrl_aprl_sum").Value);
                                g208.인별[ix].tdmr_aprl_sum = Int32.Parse(sum_data.Element("tdmr_aprl_sum").Value);
                                g208.인별[ix].trp_aprl_sum = Int32.Parse(sum_data.Element("trp_aprl_sum").Value);
                                g208.인별[ix].isld_aprl_sum = Int32.Parse(sum_data.Element("isld_aprl_sum").Value);
                                g208.인별[ix].tot_aprl_sum = Int32.Parse(sum_data.Element("tot_aprl_sum").Value);
                                g208.인별[ix].gnrl_jan_sum = Int32.Parse(sum_data.Element("gnrl_jan_sum").Value);
                                g208.인별[ix].tdmr_jan_sum = Int32.Parse(sum_data.Element("tdmr_jan_sum").Value);
                                g208.인별[ix].trp_jan_sum = Int32.Parse(sum_data.Element("trp_jan_sum").Value);
                                g208.인별[ix].isld_jan_sum = Int32.Parse(sum_data.Element("isld_jan_sum").Value);
                                g208.인별[ix].tot_jan_sum = Int32.Parse(sum_data.Element("tot_jan_sum").Value);


                                //상품별반복
                                var datas = from data in man.Elements("data")
                                            select data;

                                g208.인별[ix].상품 = new G208.상품별반복[datas.Count()];

                                int j = 0;
                                foreach (var data in datas)
                                {
                                    g208.인별[ix].상품[j].dat_cd = data.Attribute("dat_cd").Value;                                    
                                    g208.인별[ix].상품[j].use_place_cd = data.Attribute("use_place_cd").Value;

                                    g208.인별[ix].상품[j].sum = Int32.Parse(data.Element("sum").Value);

                                    j++;
                                }
                                ix++;
                            }
                        }
                        /* -------------------- 직불카드(2020년 귀속) ------------------*/
                        else if (form_cd.Contains("G308"))
                        {
                            g308.form_cd = form_cd;

                            // 인별반복
                            var mans = from man in frm.Elements("man")
                                       select man;

                            g308.인별 = new G308.인별반복[mans.Count()];

                            int ix = 0;
                            foreach (var man in mans)
                            {

                                g308.인별[ix].resid = man.Attribute("resid").Value;
                                g308.인별[ix].name = man.Attribute("name").Value;

                                //합계 sum_data

                                var sum_data = man.Element("sum_data");
                                g308.인별[ix].gnrl_sum = Int32.Parse(sum_data.Element("gnrl_sum").Value);
                                g308.인별[ix].tdmr_sum = Int32.Parse(sum_data.Element("tdmr_sum").Value);
                                g308.인별[ix].trp_sum = Int32.Parse(sum_data.Element("trp_sum").Value);
                                g308.인별[ix].isld_sum = Int32.Parse(sum_data.Element("isld_sum").Value);
                                g308.인별[ix].tot_sum = Int32.Parse(sum_data.Element("tot_sum").Value);
                                g308.인별[ix].gnrl_mar_sum = Int32.Parse(sum_data.Element("gnrl_mar_sum").Value);
                                g308.인별[ix].tdmr_mar_sum = Int32.Parse(sum_data.Element("tdmr_mar_sum").Value);
                                g308.인별[ix].trp_mar_sum = Int32.Parse(sum_data.Element("trp_mar_sum").Value);
                                g308.인별[ix].isld_mar_sum = Int32.Parse(sum_data.Element("isld_mar_sum").Value);
                                g308.인별[ix].tot_mar_sum = Int32.Parse(sum_data.Element("tot_mar_sum").Value);
                                g308.인별[ix].gnrl_aprl_sum = Int32.Parse(sum_data.Element("gnrl_aprl_sum").Value);
                                g308.인별[ix].tdmr_aprl_sum = Int32.Parse(sum_data.Element("tdmr_aprl_sum").Value);
                                g308.인별[ix].trp_aprl_sum = Int32.Parse(sum_data.Element("trp_aprl_sum").Value);
                                g308.인별[ix].isld_aprl_sum = Int32.Parse(sum_data.Element("isld_aprl_sum").Value);
                                g308.인별[ix].tot_aprl_sum = Int32.Parse(sum_data.Element("tot_aprl_sum").Value);
                                g308.인별[ix].gnrl_jan_sum = Int32.Parse(sum_data.Element("gnrl_jan_sum").Value);
                                g308.인별[ix].tdmr_jan_sum = Int32.Parse(sum_data.Element("tdmr_jan_sum").Value);
                                g308.인별[ix].trp_jan_sum = Int32.Parse(sum_data.Element("trp_jan_sum").Value);
                                g308.인별[ix].isld_jan_sum = Int32.Parse(sum_data.Element("isld_jan_sum").Value);
                                g308.인별[ix].tot_jan_sum = Int32.Parse(sum_data.Element("tot_jan_sum").Value);


                                //상품별반복
                                var datas = from data in man.Elements("data")
                                            select data;

                                g308.인별[ix].상품 = new G308.상품별반복[datas.Count()];

                                int j = 0;
                                foreach (var data in datas)
                                {
                                    g308.인별[ix].상품[j].dat_cd = data.Attribute("dat_cd").Value;
                                    g308.인별[ix].상품[j].busnid = data.Attribute("busnid").Value;
                                    g308.인별[ix].상품[j].trade_nm = data.Attribute("trade_nm").Value;
                                    g308.인별[ix].상품[j].use_place_cd = data.Attribute("use_place_cd").Value;

                                    g308.인별[ix].상품[j].sum = Int32.Parse(data.Element("sum").Value);

                                    j++;
                                }
                                ix++;
                            }
                        }
                        /* -------------------- 제로페이(2020년 귀속) ------------------*/
                        else if (form_cd.Contains("G408"))
                        {
                            g408.form_cd = form_cd;

                            // 인별반복
                            var mans = from man in frm.Elements("man")
                                       select man;

                            g408.인별 = new G408.인별반복[mans.Count()];

                            int ix = 0;
                            foreach (var man in mans)
                            {

                                g408.인별[ix].resid = man.Attribute("resid").Value;
                                g408.인별[ix].name = man.Attribute("name").Value;

                                //합계 sum_data

                                var sum_data = man.Element("sum_data");
                                g408.인별[ix].gnrl_sum = Int32.Parse(sum_data.Element("gnrl_sum").Value);
                                g408.인별[ix].tdmr_sum = Int32.Parse(sum_data.Element("tdmr_sum").Value);
                                g408.인별[ix].trp_sum = Int32.Parse(sum_data.Element("trp_sum").Value);
                                g408.인별[ix].isld_sum = Int32.Parse(sum_data.Element("isld_sum").Value);
                                g408.인별[ix].tot_sum = Int32.Parse(sum_data.Element("tot_sum").Value);
                                g408.인별[ix].gnrl_mar_sum = Int32.Parse(sum_data.Element("gnrl_mar_sum").Value);
                                g408.인별[ix].tdmr_mar_sum = Int32.Parse(sum_data.Element("tdmr_mar_sum").Value);
                                g408.인별[ix].trp_mar_sum = Int32.Parse(sum_data.Element("trp_mar_sum").Value);
                                g408.인별[ix].isld_mar_sum = Int32.Parse(sum_data.Element("isld_mar_sum").Value);
                                g408.인별[ix].tot_mar_sum = Int32.Parse(sum_data.Element("tot_mar_sum").Value);
                                g408.인별[ix].gnrl_aprl_sum = Int32.Parse(sum_data.Element("gnrl_aprl_sum").Value);
                                g408.인별[ix].tdmr_aprl_sum = Int32.Parse(sum_data.Element("tdmr_aprl_sum").Value);
                                g408.인별[ix].trp_aprl_sum = Int32.Parse(sum_data.Element("trp_aprl_sum").Value);
                                g408.인별[ix].isld_aprl_sum = Int32.Parse(sum_data.Element("isld_aprl_sum").Value);
                                g408.인별[ix].tot_aprl_sum = Int32.Parse(sum_data.Element("tot_aprl_sum").Value);
                                g408.인별[ix].gnrl_jan_sum = Int32.Parse(sum_data.Element("gnrl_jan_sum").Value);
                                g408.인별[ix].tdmr_jan_sum = Int32.Parse(sum_data.Element("tdmr_jan_sum").Value);
                                g408.인별[ix].trp_jan_sum = Int32.Parse(sum_data.Element("trp_jan_sum").Value);
                                g408.인별[ix].isld_jan_sum = Int32.Parse(sum_data.Element("isld_jan_sum").Value);
                                g408.인별[ix].tot_jan_sum = Int32.Parse(sum_data.Element("tot_jan_sum").Value);


                                //상품별반복
                                var datas = from data in man.Elements("data")
                                            select data;

                                g408.인별[ix].상품 = new G408.상품별반복[datas.Count()];

                                int j = 0;
                                foreach (var data in datas)
                                {
                                    g408.인별[ix].상품[j].dat_cd = data.Attribute("dat_cd").Value;
                                    g408.인별[ix].상품[j].busnid = data.Attribute("busnid").Value;
                                    g408.인별[ix].상품[j].trade_nm = data.Attribute("trade_nm").Value;
                                    g408.인별[ix].상품[j].use_place_cd = data.Attribute("use_place_cd").Value;

                                    g408.인별[ix].상품[j].sum = Int32.Parse(data.Element("sum").Value);

                                    j++;
                                }
                                ix++;
                            }
                        }
                        /* -------------------- 주택임차차입금 원리금상환액 ------------------*/
                        else if (form_cd.Contains("J101"))
                        {
                            j101.form_cd = form_cd;

                            // 인별반복
                            var mans = from man in frm.Elements("man")
                                       select man;

                            j101.인별 = new J101.인별반복[mans.Count()];

                            int ix = 0;
                            foreach (var man in mans)
                            {

                                j101.인별[ix].resid = man.Attribute("resid").Value;
                                j101.인별[ix].name = man.Attribute("name").Value;

                                //상품별반복
                                var datas = from data in man.Elements("data")
                                            select data;


                                j101.인별[ix].상품 = new J101.상품별반복[datas.Count()];

                                int j = 0;
                                foreach (var data in datas)
                                {
                                    j101.인별[ix].상품[j].dat_cd = data.Attribute("dat_cd").Value;
                                    j101.인별[ix].상품[j].busnid = data.Attribute("busnid").Value;
                                    j101.인별[ix].상품[j].trade_nm = data.Attribute("trade_nm").Value;
                                    j101.인별[ix].상품[j].acc_no = data.Attribute("acc_no").Value;

                                    j101.인별[ix].상품[j].goods_nm = data.Element("goods_nm").Value;
                                    j101.인별[ix].상품[j].lend_dt = data.Element("lend_dt").Value;
                                    j101.인별[ix].상품[j].sum = Int32.Parse(data.Element("sum").Value);                                
                                    j++;
                                }
                                ix++;
                            }
                        }
                        /* -------------------- 장기주택저당차입금 이자상환액 ------------------*/
                        else if (form_cd.Contains("J203"))
                        {
                            j203.form_cd = form_cd;

                            // 인별반복
                            var mans = from man in frm.Elements("man")
                                       select man;

                            j203.인별 = new J203.인별반복[mans.Count()];

                            int ix = 0;
                            foreach (var man in mans)
                            {

                                j203.인별[ix].resid = man.Attribute("resid").Value;
                                j203.인별[ix].name = man.Attribute("name").Value;

                                //상품별반복
                                var datas = from data in man.Elements("data")
                                            select data;


                                j203.인별[ix].상품 = new J203.상품별반복[datas.Count()];

                                int j = 0;
                                foreach (var data in datas)
                                {
                                    j203.인별[ix].상품[j].dat_cd = data.Attribute("dat_cd").Value;
                                    j203.인별[ix].상품[j].busnid = data.Attribute("busnid").Value;
                                    j203.인별[ix].상품[j].trade_nm = data.Attribute("trade_nm").Value;
                                    j203.인별[ix].상품[j].acc_no = data.Attribute("acc_no").Value;

                                    j203.인별[ix].상품[j].lend_kd = data.Element("lend_kd").Value;
                                    j203.인별[ix].상품[j].house_take_dt = data.Element("house_take_dt").Value;
                                    j203.인별[ix].상품[j].mort_setup_dt = data.Element("mort_setup_dt").Value;
                                    j203.인별[ix].상품[j].start_dt = data.Element("start_dt").Value;
                                    j203.인별[ix].상품[j].end_dt = data.Element("end_dt").Value;
                                    j203.인별[ix].상품[j].repay_years = data.Element("repay_years").Value;
                                    j203.인별[ix].상품[j].lend_goods_nm = data.Element("lend_goods_nm").Value;
                                    j203.인별[ix].상품[j].debt = Int32.Parse(data.Element("debt").Value);
                                    j203.인별[ix].상품[j].fixed_rate_debt = Int32.Parse(data.Element("fixed_rate_debt").Value);
                                    j203.인별[ix].상품[j].not_defer_debt = Int32.Parse(data.Element("not_defer_debt").Value);
                                    j203.인별[ix].상품[j].this_year_rede_amt = Int32.Parse(data.Element("this_year_rede_amt").Value);

                                    j203.인별[ix].상품[j].ddct = Int32.Parse(data.Element("sum").Attribute("ddct").Value);
                                    j203.인별[ix].상품[j].sum = Int32.Parse(data.Element("sum").Value);
                                    j++;
                                }
                                ix++;
                            }

                        }
                        /* -------------------- 주택마련저축 ------------------*/
                        else if (form_cd.Contains("J301"))
                        {
                            j301.form_cd = form_cd;

                            // 인별반복
                            var mans = from man in frm.Elements("man")
                                       select man;

                            j301.인별 = new J301.인별반복[mans.Count()];

                            int ix = 0;
                            foreach (var man in mans)
                            {

                                j301.인별[ix].resid = man.Attribute("resid").Value;
                                j301.인별[ix].name = man.Attribute("name").Value;

                                //상품별반복
                                var datas = from data in man.Elements("data")
                                            select data;


                                j301.인별[ix].상품 = new J301.상품별반복[datas.Count()];

                                int j = 0;
                                foreach (var data in datas)
                                {
                                    j301.인별[ix].상품[j].dat_cd = data.Attribute("dat_cd").Value;
                                    j301.인별[ix].상품[j].busnid = data.Attribute("busnid").Value;
                                    j301.인별[ix].상품[j].trade_nm = data.Attribute("trade_nm").Value;
                                    j301.인별[ix].상품[j].acc_no = data.Attribute("acc_no").Value;

                                    j301.인별[ix].상품[j].goods_nm = data.Element("goods_nm").Value;
                                    j301.인별[ix].상품[j].saving_gubn = data.Element("saving_gubn").Value;
                                    j301.인별[ix].상품[j].reg_dt = data.Element("reg_dt").Value;
                                    j301.인별[ix].상품[j].com_cd = data.Element("com_cd").Value;
                                    j301.인별[ix].상품[j].sum = Int32.Parse(data.Element("sum").Value);
                                    j++;
                                }
                                ix++;
                            }
                        }
                        /* -------------------- 목돈 안드는 전세 이자상환액(2016년 귀속까지) ------------------*/
                        else if (form_cd.Contains("J401"))
                        {
                            j401.form_cd = form_cd;

                            // 인별반복
                            var mans = from man in frm.Elements("man")
                                       select man;

                            j401.인별 = new J401.인별반복[mans.Count()];

                            int ix = 0;
                            foreach (var man in mans)
                            {

                                j401.인별[ix].resid = man.Attribute("resid").Value;
                                j401.인별[ix].name = man.Attribute("name").Value;

                                //상품별반복
                                var datas = from data in man.Elements("data")
                                            select data;


                                j401.인별[ix].상품 = new J401.상품별반복[datas.Count()];

                                int j = 0;
                                foreach (var data in datas)
                                {
                                    j401.인별[ix].상품[j].dat_cd = data.Attribute("dat_cd").Value;
                                    j401.인별[ix].상품[j].busnid = data.Attribute("busnid").Value;
                                    j401.인별[ix].상품[j].trade_nm = data.Attribute("trade_nm").Value;
                                    j401.인별[ix].상품[j].acc_no = data.Attribute("acc_no").Value;

                                    j401.인별[ix].상품[j].lend_loan_amt = Int32.Parse(data.Element("lend_loan_amt").Value);
                                    j401.인별[ix].상품[j].lend_dt = data.Element("lend_dt").Value;                                    
                                    j401.인별[ix].상품[j].sum = Int32.Parse(data.Element("sum").Value);
                                    j++;
                                }
                                ix++;
                            }
                        }
                        /* -------------------- 월세액(2020귀속부터) ------------------*/
                        else if (form_cd.Contains("J501"))
                        {
                            j501.form_cd = form_cd;

                            // 인별반복
                            var mans = from man in frm.Elements("man")
                                       select man;

                            j501.인별 = new J501.인별반복[mans.Count()];

                            int ix = 0;
                            foreach (var man in mans)
                            {

                                j501.인별[ix].resid = man.Attribute("resid").Value;
                                j501.인별[ix].name = man.Attribute("name").Value;

                                //상품별반복
                                var datas = from data in man.Elements("data")
                                            select data;


                                j501.인별[ix].상품 = new J501.상품별반복[datas.Count()];

                                int j = 0;
                                foreach (var data in datas)
                                {
                                    j501.인별[ix].상품[j].dat_cd = data.Attribute("dat_cd").Value;
                                    j501.인별[ix].상품[j].lsor_no = data.Attribute("lsor_no").Value;
                                    j501.인별[ix].상품[j].lsor_nm = data.Attribute("lsor_nm").Value;  
                                    
                                    j501.인별[ix].상품[j].start_dt = data.Element("start_dt").Value;
                                    j501.인별[ix].상품[j].end_dt = data.Element("end_dt").Value;
                                    j501.인별[ix].상품[j].adr = data.Element("adr").Value;
                                    j501.인별[ix].상품[j].area = data.Element("area").Value;
                                    j501.인별[ix].상품[j].typeCd = data.Element("typeCd").Value;

                                    j501.인별[ix].상품[j].sum = Int32.Parse(data.Element("sum").Value);
                                    j++;
                                }
                                ix++;
                            }
                        }
                        /* -------------------- 소기업소상공인 공제부금 ------------------*/
                        else if (form_cd.Contains("K101"))
                        {
                            k101.form_cd = form_cd;

                            // 인별반복
                            var mans = from man in frm.Elements("man")
                                       select man;

                            k101.인별 = new K101.인별반복[mans.Count()];

                            int ix = 0;
                            foreach (var man in mans)
                            {

                                k101.인별[ix].resid = man.Attribute("resid").Value;
                                k101.인별[ix].name = man.Attribute("name").Value;

                                //상품별반복
                                var datas = from data in man.Elements("data")
                                            select data;


                                k101.인별[ix].상품 = new K101.상품별반복[datas.Count()];

                                int j = 0;
                                foreach (var data in datas)
                                {
                                    k101.인별[ix].상품[j].dat_cd = data.Attribute("dat_cd").Value;
                                    k101.인별[ix].상품[j].acc_no = data.Attribute("acc_no").Value;                                    

                                    k101.인별[ix].상품[j].start_dt = data.Element("start_dt").Value;
                                    k101.인별[ix].상품[j].end_dt = data.Element("end_dt").Value;
                                    k101.인별[ix].상품[j].rule_yn = data.Element("rule_yn").Value;
                                    k101.인별[ix].상품[j].pay_method = data.Element("pay_method").Value;

                                    k101.인별[ix].상품[j].ddct = Int32.Parse(data.Element("sum").Attribute("ddct").Value);
                                    k101.인별[ix].상품[j].sum = Int32.Parse(data.Element("sum").Value);
                                    j++;
                                }
                                ix++;
                            }
                        }
                        /* -------------------- 기부금 ------------------*/
                        else if (form_cd.Contains("L102"))
                        {
                            l102.form_cd = form_cd;

                            // 인별반복
                            var mans = from man in frm.Elements("man")
                                       select man;

                            l102.인별 = new L102.인별반복[mans.Count()];

                            int ix = 0;
                            foreach (var man in mans)
                            {

                                l102.인별[ix].resid = man.Attribute("resid").Value;
                                l102.인별[ix].name = man.Attribute("name").Value;

                                //상품별반복
                                var datas = from data in man.Elements("data")
                                            select data;


                                l102.인별[ix].기관 = new L102.기관별반복[datas.Count()];

                                int j = 0;
                                foreach (var data in datas)
                                {
                                    l102.인별[ix].기관[j].dat_cd = data.Attribute("dat_cd").Value;
                                    l102.인별[ix].기관[j].busnid = data.Attribute("busnid").Value;
                                    l102.인별[ix].기관[j].trade_nm = data.Attribute("trade_nm").Value;
                                    l102.인별[ix].기관[j].donation_cd = data.Attribute("donation_cd").Value;

                                    l102.인별[ix].기관[j].sum = Int32.Parse(data.Element("sum").Value);
                                    l102.인별[ix].기관[j].sbdy_apln_sum = Int32.Parse(data.Element("sbdy_apln_sum").Value);
                                    l102.인별[ix].기관[j].conb_sum = Int32.Parse(data.Element("conb_sum").Value);                                                                        
                                    j++;
                                }
                                ix++;
                            }
                        }
                        /* -------------------- 장기집합투자증권저축 ------------------*/
                        else if (form_cd.Contains("N101"))
                        {
                            n101.form_cd = form_cd;

                            // 인별반복
                            var mans = from man in frm.Elements("man")
                                       select man;

                            n101.인별 = new N101.인별반복[mans.Count()];

                            int ix = 0;
                            foreach (var man in mans)
                            {

                                n101.인별[ix].resid = man.Attribute("resid").Value;
                                n101.인별[ix].name = man.Attribute("name").Value;

                                //상품별반복
                                var datas = from data in man.Elements("data")
                                            select data;


                                n101.인별[ix].상품 = new N101.상품별반복[datas.Count()];

                                int j = 0;
                                foreach (var data in datas)
                                {
                                    n101.인별[ix].상품[j].dat_cd = data.Attribute("dat_cd").Value;
                                    n101.인별[ix].상품[j].busnid = data.Attribute("busnid").Value;
                                    n101.인별[ix].상품[j].trade_nm = data.Attribute("trade_nm").Value;
                                    n101.인별[ix].상품[j].secu_no = data.Attribute("secu_no").Value;

                                    n101.인별[ix].상품[j].fund_nm = data.Element("fund_nm").Value;
                                    n101.인별[ix].상품[j].reg_dt = data.Element("reg_dt").Value;
                                    n101.인별[ix].상품[j].com_cd = data.Element("com_cd").Value;
                                    n101.인별[ix].상품[j].sum = Int32.Parse(data.Element("sum").Value);
                                    n101.인별[ix].상품[j].ddct_bs_ass_amt = Int32.Parse(data.Element("ddct_bs_ass_amt").Value);
                                    j++;
                                }
                                ix++;
                            }
                        }
                        /* -------------------- 건강보험료 ------------------*/
                        else if (form_cd.Contains("O101"))
                        {
                            o101.form_cd = form_cd;

                            // 인별반복
                            var mans = from man in frm.Elements("man")
                                       select man;

                            o101.인별 = new O101.인별반복[mans.Count()];

                            int ix = 0;
                            foreach (var man in mans)
                            {

                                o101.인별[ix].resid = man.Attribute("resid").Value;
                                o101.인별[ix].name = man.Attribute("name").Value;

                                //상품별반복
                                var datas = from data in man.Elements("data")
                                            select data;


                                o101.인별[ix].상품 = new O101.상품별반복[datas.Count()];

                                int j = 0;
                                foreach (var data in datas)
                                {
                                    o101.인별[ix].상품[j].dat_cd = data.Attribute("dat_cd").Value;

                                    var sum = data.Element("sum");
                                    o101.인별[ix].상품[j].hi_yrs = Int32.Parse(sum.Attribute("hi_yrs").Value);
                                    o101.인별[ix].상품[j].ltrm_yrs = Int32.Parse(sum.Attribute("ltrm_yrs").Value);
                                    o101.인별[ix].상품[j].hi_ntf = Int32.Parse(sum.Attribute("hi_ntf").Value);
                                    o101.인별[ix].상품[j].ltrm_ntf = Int32.Parse(sum.Attribute("ltrm_ntf").Value);
                                    o101.인별[ix].상품[j].hi_pmt = Int32.Parse(sum.Attribute("hi_pmt").Value);
                                    o101.인별[ix].상품[j].ltrm_pmt = Int32.Parse(sum.Attribute("ltrm_pmt").Value);

                                    j++;
                                }
                                ix++;
                            }
                        }
                        /* -------------------- 국민연금보혐료 ------------------*/
                        else if (form_cd.Contains("P102"))
                        {
                            p102.form_cd = form_cd;

                            // 인별반복
                            var mans = from man in frm.Elements("man")
                                       select man;

                            p102.인별 = new P102.인별반복[mans.Count()];

                            int ix = 0;
                            foreach (var man in mans)
                            {

                                p102.인별[ix].resid = man.Attribute("resid").Value;
                                p102.인별[ix].name = man.Attribute("name").Value;

                                //상품별반복
                                var datas = from data in man.Elements("data")
                                            select data;


                                p102.인별[ix].상품 = new P102.상품별반복[datas.Count()];

                                int j = 0;
                                foreach (var data in datas)
                                {
                                    p102.인별[ix].상품[j].dat_cd = data.Attribute("dat_cd").Value;

                                    var sum = data.Element("sum");
                                    p102.인별[ix].상품[j].sp_ntf = Int32.Parse(sum.Attribute("sp_ntf").Value);
                                    p102.인별[ix].상품[j].spym = Int32.Parse(sum.Attribute("spym").Value);
                                    p102.인별[ix].상품[j].jlc = Int32.Parse(sum.Attribute("jlc").Value);
                                    p102.인별[ix].상품[j].ntf = Int32.Parse(sum.Attribute("ntf").Value);
                                    p102.인별[ix].상품[j].pmt = Int32.Parse(sum.Attribute("pmt").Value);                                    

                                    j++;
                                }
                                ix++;
                            }
                        }
                        /* -------------------- 벤처기업투자신탁 ------------------*/
                        else if (form_cd.Contains("Q101"))
                        {
                            q101.form_cd = form_cd;

                            // 인별반복
                            var mans = from man in frm.Elements("man")
                                       select man;

                            q101.인별 = new Q101.인별반복[mans.Count()];

                            int ix = 0;
                            foreach (var man in mans)
                            {

                                q101.인별[ix].resid = man.Attribute("resid").Value;
                                q101.인별[ix].name = man.Attribute("name").Value;

                                //상품별반복
                                var datas = from data in man.Elements("data")
                                            select data;


                                q101.인별[ix].상품 = new Q101.상품별반복[datas.Count()];

                                int j = 0;
                                foreach (var data in datas)
                                {
                                    q101.인별[ix].상품[j].dat_cd = data.Attribute("dat_cd").Value;
                                    q101.인별[ix].상품[j].busnid = data.Attribute("busnid").Value;
                                    q101.인별[ix].상품[j].trade_nm = data.Attribute("trade_nm").Value;
                                    q101.인별[ix].상품[j].secu_no = data.Attribute("secu_no").Value;

                                    q101.인별[ix].상품[j].fund_nm = data.Element("fund_nm").Value;
                                    q101.인별[ix].상품[j].reg_dt = data.Element("reg_dt").Value;
                                    q101.인별[ix].상품[j].vnt_asct_cl_cd = data.Element("vnt_asct_cl_cd").Value;
                                    q101.인별[ix].상품[j].com_cd = data.Element("com_cd").Value;
                                    q101.인별[ix].상품[j].sum = Int32.Parse(data.Element("sum").Value);
                                    j++;
                                }
                                ix++;
                            }
                        }
                        /* -------------------- 벤처기업투자신탁(전년도 납입분) ------------------*/
                        else if (form_cd.Contains("Q201"))
                        {
                            q201.form_cd = form_cd;

                            // 인별반복
                            var mans = from man in frm.Elements("man")
                                       select man;

                            q201.인별 = new Q201.인별반복[mans.Count()];

                            int ix = 0;
                            foreach (var man in mans)
                            {

                                q201.인별[ix].resid = man.Attribute("resid").Value;
                                q201.인별[ix].name = man.Attribute("name").Value;

                                //상품별반복
                                var datas = from data in man.Elements("data")
                                            select data;


                                q201.인별[ix].상품 = new Q201.상품별반복[datas.Count()];

                                int j = 0;
                                foreach (var data in datas)
                                {
                                    q201.인별[ix].상품[j].dat_cd = data.Attribute("dat_cd").Value;
                                    q201.인별[ix].상품[j].busnid = data.Attribute("busnid").Value;
                                    q201.인별[ix].상품[j].trade_nm = data.Attribute("trade_nm").Value;
                                    q201.인별[ix].상품[j].secu_no = data.Attribute("secu_no").Value;

                                    q201.인별[ix].상품[j].fund_nm = data.Element("fund_nm").Value;
                                    q201.인별[ix].상품[j].reg_dt = data.Element("reg_dt").Value;
                                    q201.인별[ix].상품[j].vnt_asct_cl_cd = data.Element("vnt_asct_cl_cd").Value;
                                    q201.인별[ix].상품[j].com_cd = data.Element("com_cd").Value;
                                    q201.인별[ix].상품[j].sum = Int32.Parse(data.Element("sum").Value);
                                    j++;
                                }
                                ix++;
                            }
                        }
                        /* -------------------- 기타 ------------------*/
                        else
                        {

                        }


                    }

                    /* -----------------------------  반복문 폼 로딩 처리 끝 ----------------------------------------*/

                }


                /* ------------------------------- 데이터베이스 입력처리 -----------------------------*/

                Dbconn conn = new Dbconn();
                conn.ConnectionDB("61.81.162.13", "ORAMJ", "MJUSER", "MJUSER");
                
                a102.Execute(a102);
                b101.Execute(b101);
                b201.Execute(b201); 
                c102.Execute(c102);
                c202.Execute(c202);
                c301.Execute(c301);
                c401.Execute(c401);
                d101.Execute(d101);
                e102.Execute(e102);
                f102.Execute(f102);
                g108.Execute(g108);
                g208.Execute(g208);
                g308.Execute(g308);
                g408.Execute(g408);
                j101.Execute(j101);
                j203.Execute(j203);
                j301.Execute(j301);
                j401.Execute(j401);
                j501.Execute(j501);
                k101.Execute(k101);
                l102.Execute(l102);
                n101.Execute(n101);
                o101.Execute(o101);
                p102.Execute(p102);
                q101.Execute(q101);
                q201.Execute(q201);


            }


            /* -------------------------------- 예외처리 -----------------------------------------*/
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
    }
}
