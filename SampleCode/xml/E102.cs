﻿using System;
using System.Collections.Generic;
using System.Text;
using NTS_Reader_CS.db;

namespace NTS_Reader_CS.xml
{
    class E102 : Dbconn //연금저축
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
            public string trade_nm { get; set; } //상호
            public string acc_no { get; set; } //계좌번호

            public string com_cd { get; set; } //금융기관코드
            public int ann_tot_amt { get; set; } //당해연도납입금액
            public int tax_year_amt { get; set; } //당해연도인출금액
            public int ddct_bs_ass_amt { get; set; } //순납입급액
            public int sum { get; set; } //납입금액계


            public E102M[] 월별;

        }

        public struct E102M
        {
            public int amt { get; set; } //월별납입금액
            public string mm { get; set; } //납입월
            public string fix_cd { get; set; } //확정구분코드
        }

        
    }
}
