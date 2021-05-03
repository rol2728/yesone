﻿using System;
using System.Collections.Generic;
using System.Text;
using NTS_Reader_CS.db;

namespace NTS_Reader_CS.xml
{

    class A102 : Dbconn
    {
        public String form_cd {get;set;}// 서식코드 
        public 인별반복[] 인별;

        public struct 인별반복
        {

            public string resid { get; set; }//주민등록번호
            public string name { get; set; } //성명

            public 상품별반복[] 상품;            
        }

        public struct 상품별반복
        {
            public string dat_cd { get; set; }//자료코드
            public string busnid { get; set; } //사업자번호
            public string trade_nm { get; set; } //상호
            public string acc_no { get; set; } //증권번호            

            public string good_nm { get; set; } //보험종료
            public string insu1_resid { get; set; } //주민등록번호_주피보험자
            public string insu1_nm { get; set; } //성명_주피보험자
            public string insu2_resid_1 { get; set; } //주민등록번호_종피보험자_1
            public string insu2_nm_1 { get; set; } //성명_종피보험자_1
            public string insu2_resid_2 { get; set; } //주민등록번호_종피보험자_2
            public string insu2_nm_2 { get; set; } //성명_종피보험자_2
            public string insu2_resid_3 { get; set; }  //주민등록번호_종피보험자_3
            public string insu2_nm_3 { get; set; } //성명_종피보험자_3
            public int sum { get; set; } //납입금액계      


            public A102M[] 월별;

        }
        
        public struct A102M
        {
            public int amt { get; set; } //월별납입금액
            public string mm { get; set; } //납입월
            public string fix_cd { get; set; } //확정구분코드
        }

        public void Execute(A102 entity)
        {
            // 1. 본인확인
            foreach (var 인별 in entity.인별)
            {
                if (인별.resid == "test")
                {
                    string tes = "Test";
                }

                foreach (var data in 인별.상품)
                {
                    if (data.acc_no == "")
                    {
                        string tes1 = "Test";
                    }
                }
            }

            // 2.0 본인인 경우 
            // 2.1 본인이 아니며 초중고 EDU_TIP = 2,3,4 인경우            
            // 2.1 본인이 아니며 대학교 EDU_TIP = 5,6 인경우
            // 2.1 본인이 아니며 취학전 EDU_TIP = 1 인경우
            // 2.1 본인이 아니며 장애 EDU_TIP = J,K 인경우


            executeSql("update qe020ms set YCAL_FORG_SMPL_RATE_YN = 'Y' where emp_no = '10110007' and ycal_year = '2020'");
        }


    }
}