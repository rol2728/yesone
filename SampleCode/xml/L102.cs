using System;
using System.Collections.Generic;
using System.Text;
using NTS_Reader_CS.db;

namespace NTS_Reader_CS.xml
{
    class L102 : Dbconn //기부금
    {
        public String form_cd { get; set; } // 서식코드
        public 인별반복[] 인별;

        public struct 인별반복
        {
            public string resid { get; set; } //주민등록번호
            public string name { get; set; } //성명
            public 기관별반복[] 기관;
        }

        public struct 기관별반복
        {
            public string dat_cd { get; set; } //자료코드           
            public string busnid { get; set; } //사업자번호
            public string trade_nm { get; set; } //단체명
            public string donation_cd { get; set; } //기부유형

            public int sum { get; set; } //공제대상기부액
            public int sbdy_apln_sum { get; set; } //기부장려금신청금액
            public int conb_sum { get; set; } //기부금액합계


            public L102M[] 일별;

        }

        public struct L102M
        {
            public int amt { get; set; } //일별기부금액
            public string dd { get; set; } //기부일자
            public int apln { get; set; } //일별기부장려금신청금액
            public int sum { get; set; } //일별기부금액합계


        }

        public void Execute(L102 entity)
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


        }
    }
}
