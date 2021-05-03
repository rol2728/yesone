using System;
using System.Collections.Generic;
using System.Text;

namespace NTS_Reader_CS.xml
{
    class K101 : Dbconn //소기업소상공인 공제부금
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
            public string acc_no { get; set; } //공제계약번호
            public string start_dt { get; set; } //공제가입일자
            public string end_dt { get; set; } //대상기간종료일
            public string rule_yn { get; set; } //개정규칙 적용신청여부
            public string pay_method { get; set; } //납입방법
            public int sum { get; set; } //납입금액계
            public int ddct { get; set; } //소득공제대상액

            public K101M[] 월별;

        }

        public struct K101M
        {
            public int amt { get; set; } //월별상환액
            public string mm { get; set; } //상환월
            public string date { get; set; } //납일일자
        }

        
    }
}
