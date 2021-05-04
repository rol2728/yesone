using System;
using System.Collections.Generic;
using System.Text;
using NTS_Reader_CS.db;

namespace NTS_Reader_CS.xml
{
    class C102 : Dbconn
    {
        public String form_cd { get; set; } // 서식코드
        public 인별반복[] 인별;

        public struct 인별반복
        {
            public string resid; //주민등록번호
            public string name; //성명
            public 학교별반복[] 학교;
        }

        public struct 학교별반복
        {
            public string dat_cd { get; set; } //자료코드
            public string busnid { get; set; } //사업자번호
            public string trade_nm { get; set; } //학교명
            public string edu_tp { get; set; } //교육비종류
            public string edu_cl { get; set; } //교육비구분
            public int sum { get; set; } //납입금액계

            public C102M[] 월별;

        }

        public struct C102M
        {
            public int amt { get; set; } //월별납입금액
            public string mm { get; set; } //납입월
        }

        

        
    }
}
