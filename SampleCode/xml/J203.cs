﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NTS_Reader_CS.xml
{
    class J203 : Dbconn//주택임차차입금
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

            public string lend_kd { get; set; } //대출종류
            public string house_take_dt { get; set; } //주택취득일
            public string mort_setup_dt { get; set; } //저당권설정일
            public string start_dt { get; set; } //최초차입일
            public string end_dt { get; set; } //최종상환예정일
            public string repay_years { get; set; } //상환기간연수
            public string lend_goods_nm { get; set; } //상품명
            public int debt { get; set; } //차입금
            public int fixed_rate_debt { get; set; } //고정금리차입금
            public int not_defer_debt { get; set; } //비거치식상환차입금
            public int this_year_rede_amt { get; set; } //당해년 원금상환액
            public int sum { get; set; } //연간합계액
            public int ddct { get; set; } //소득공제대상액

            public J203M[] 월별;

        }

        public struct J203M
        {
            public int amt { get; set; } //월별납입금액
            public string mm { get; set; } //납입월
            public string fix_cd { get; set; } //확정구분코드
        }

        
    }
}