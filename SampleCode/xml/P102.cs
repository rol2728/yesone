using System;
using System.Collections.Generic;
using System.Text;
using NTS_Reader_CS.db;

namespace NTS_Reader_CS.xml
{
    class P102 : Dbconn //국민연금보험료
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

            public int sp_ntf { get; set; } //직장가입자소급고지금액합계
            public int spym { get; set; } //추납보험료납부금액
            public int jlc { get; set; } //실업크레딧납부금액
            public int ntf { get; set; } //직장가입자 고지금액 합계
            public int pmt { get; set; } //지역가입자 등 납부금액 합계

            public P102M[] 월별;

        }

        public struct P102M
        {     
            public string mm { get; set; } //납입월
            public int wrkp_ntf { get; set; } //직장가입자 고지금액
            public int rgn_pmt { get; set; } //지역가입자 등 납부금액       
        }

        public void Execute(P102 entity)
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
