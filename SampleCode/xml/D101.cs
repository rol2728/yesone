﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NTS_Reader_CS.xml
{
    class D101 : Dbconn //개인연금저축
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
            public string trade_nm { get; set; } //기관명
            public string acc_no { get; set; } //계좌/증권번호            

            public string start_dt { get; set; } //계약시작일
            public string end_dt { get; set; } //계약종료일
            public string com_cd { get; set; } //금융기관코드
            public int sum { get; set; } //납입금액계

            public D101M[] 월별;

        }

        public struct D101M
        {
            public int amt { get; set; } //월별납입금액
            public string mm { get; set; } //납입월
            public string fix_cd { get; set; } //확정구분코드
        }

        
    }
}