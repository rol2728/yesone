﻿using System;
using System.Collections.Generic;
using System.Text;
using NTS_Reader_CS.db;

namespace NTS_Reader_CS.xml
{
    class J401 : Dbconn //목돈안드는전세이자상환액
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
            public string trade_nm { get; set; } //취급기관
            public string acc_no { get; set; } //계좌번호

            public int lend_loan_amt { get; set; } //대출원금
            public string lend_dt { get; set; } //대출일자 
            public int sum { get; set; } //연간합계액

            public J301M[] 월별;

        }

        public struct J301M
        {
            public int amt { get; set; } //월별상환액
            public string mm { get; set; } //상환월
            public string fix_cd { get; set; } //확정구분코드
        }

        
    }
}
