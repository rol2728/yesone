using System;
using System.Collections.Generic;
using System.Text;

namespace NTS_Reader_CS.xml
{
    class C202 : Dbconn
    {
        public String form_cd { get; set; } // 서식코드
        public 인별반복[] 인별;

        public struct 인별반복
        {
            public string resid { get; set; } //주민등록번호
            public string name { get; set; } //성명
            public 과정별반복[] 과정;
        }

        public struct 과정별반복
        {
            public string dat_cd { get; set; } //자료코드
            public string busnid { get; set; } //사업자번호
            public string trade_nm { get; set; } //교유기관명

            public string course_cd { get; set; } //과정코드
            public string subject_nm { get; set; } //과정명
            public int sum { get; set; } //납입금액계

            public C202M[] 월별;

        }

        public struct C202M
        {
            public int amt { get; set; } //월별납입금액
            public string mm { get; set; } //납입월
        }

        

        
    }
}
