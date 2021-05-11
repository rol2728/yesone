using System;
using System.Collections.Generic;
using System.Text;
using NTS_Reader_CS.db;

namespace NTS_Reader_CS.xml
{
    class Q201 : Dbconn //벤처기업투자신탁(전년도 납입분)
    {
        public String form_cd { get; set; } // 서식코드
        public 인별반복[] 인별;

        public struct 인별반복
        {
            public string resid { get; set; } //주민등록번호
            public string name { get; set; } //성명
            public 상품별반복[] 상품;
        }

        public struct 상품별반복
        {
            public string dat_cd { get; set; } //자료코드    
            public string busnid { get; set; } //사업자번호    
            public string trade_nm { get; set; } //투자기관    
            public string secu_no { get; set; } //계좌번호    

            public string fund_nm { get; set; } //투자신탁명
            public string reg_dt { get; set; } //납입연도
            public string vnt_asct_cl_cd { get; set; } //벤처조합구분코드
            public string com_cd { get; set; } //금융기관코드
            public int sum { get; set; } //연간합계액

            public Q201M[] 월별;
        }

        public struct Q201M
        {
            public int amt { get; set; } //월별납입금액
            public string mm { get; set; } //납입월         
        }

        public void Execute(Q201 entity)
        {
            // 1. 본인확인
            foreach (var 인별 in entity.인별)
            {
                if (인별.resid == "test")
                {
                    string tes = "Test";
                }


            }

            // 2.0 본인인 경우 
            // 2.1 본인이 아니며 초중고 EDU_TIP = 2,3,4 인경우            
            // 2.1 본인이 아니며 대학교 EDU_TIP = 5,6 인경우
            // 2.1 본인이 아니며 취학전 EDU_TIP = 1 인경우
            // 2.1 본인이 아니며 장애 EDU_TIP = J,K 인경우


            executeSql("update qe020ms set YCAL_FORG_SMPL_RATE_YN = 'Y' where emp_no = '10110007' and ycal_year = '2020'");
        }
    }
}
