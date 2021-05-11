﻿using System;
using System.Collections.Generic;
using System.Text;
using NTS_Reader_CS.db;

namespace NTS_Reader_CS.xml
{
    class J101 : Dbconn //주택임차차입금
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

            public string goods_nm { get; set; } //상품명
            public string lend_dt { get; set; } //대출일
            public int sum { get; set; } //상환액계

            public J101M[] 월별;

        }

        public struct J101M
        {
            public int amt { get; set; } //월별납입금액
            public string mm { get; set; } //상환월
            public string fix_cd { get; set; } //확정구분코드
        }

        public void Execute(J101 entity)
        {
            // 1. 본인확인
            foreach (var 인별 in entity.인별)
            {
                if (인별.resid == "test")
                {
                    string tes = "Test";
                }


            }

            // 2.0 본인인 경우 
            // 2.1 본인이 아니며 초중고 EDU_TIP = 2,3,4 인경우            
            // 2.1 본인이 아니며 대학교 EDU_TIP = 5,6 인경우
            // 2.1 본인이 아니며 취학전 EDU_TIP = 1 인경우
            // 2.1 본인이 아니며 장애 EDU_TIP = J,K 인경우

        }
    }
}
