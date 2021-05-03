using System;
using System.Collections.Generic;
using System.Text;

namespace NTS_Reader_CS.xml
{
    class J301 : Dbconn //주택마련저축
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

            public string goods_nm { get; set; } //저축명
            public string saving_gubn { get; set; } //저축구분
            public string reg_dt { get; set; } //가입일자
            public string com_cd { get; set; } //금융기관코드
            public int sum { get; set; } //납입금액계

            public J301M[] 월별;

        }

        public struct J301M
        {
            public int amt { get; set; } //월별납입금액
            public string mm { get; set; } //납입월
            public string fix_cd { get; set; } //확정구분코드
        }

        
    }
}
