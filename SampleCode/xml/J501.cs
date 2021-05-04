using System;
using System.Collections.Generic;
using System.Text;
using NTS_Reader_CS.db;

namespace NTS_Reader_CS.xml
{
    class J501 : Dbconn //월세액
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
            public string lsor_no { get; set; } //임대인번호
            public string lsor_nm { get; set; } //임대인명          

            public string start_dt { get; set; } //임대차시작일자
            public string end_dt { get; set; } //임대차종료일자
            public string adr { get; set; } //계약서상 주소지
            public string area { get; set; } //계약면적
            public string typeCd { get; set; } //유형코드

            public int sum { get; set; } //연간합계액

            public J501M[] 월별;

        }

        public struct J501M
        {
            public int amt { get; set; } //월별상환액
            public string mm { get; set; } //상환월
            public string fix_cd { get; set; } //확정구분코드
        }

        
    }
}
