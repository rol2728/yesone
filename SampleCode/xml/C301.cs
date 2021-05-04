using System;
using System.Collections.Generic;
using System.Text;
using NTS_Reader_CS.db;

namespace NTS_Reader_CS.xml
{
    class C301 : Dbconn
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
            public string dat_cd { get; set; }  //자료코드
            public string busnid { get; set; }  //사업자번호
            public string trade_nm { get; set; }  //상호
            public int sum { get; set; }  //납입금액계

            public C301M[] 월별;

        }

        public struct C301M
        {
            public int amt { get; set; }  //월별납입금액
            public string mm { get; set; }  //납입월
        }

        

        
    }
}
