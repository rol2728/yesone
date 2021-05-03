using System;
using System.Collections.Generic;
using System.Text;

namespace NTS_Reader_CS.xml
{
    class B201 : Dbconn
    {
        public  String form_cd { get; set; } // 서식코드
        public 인별반복[] 인별;

        public struct 인별반복
        {
            public string resid { get; set; } //수익자 주민등록번호
            public string name { get; set; } //수익자 성명
            public 상품별반복[] 상품;
        }

        public struct 상품별반복
        {
            public string dat_cd { get; set; } //자료코드
            public string busnid { get; set; } //사업자번호
            public string trade_nm { get; set; } //상호
            public string acc_no { get; set; } //증권번호

            public string goods_nm { get; set; } //보험종료
            public string insu_resid { get; set; } //피보험자 주민등록번호
            public string insu_nm { get; set; } //피보험자 성명
            public int sum { get; set; } //수령금액계

            public B201M[] 월별;

        }

        public struct B201M
        {
            public int amt { get; set; } //월별수령금액
            public string mm { get; set; } //수령월
        }

        

      
    }
}
