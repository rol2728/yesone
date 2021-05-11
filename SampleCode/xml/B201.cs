using System;
using System.Collections.Generic;
using System.Text;
using NTS_Reader_CS.db;

namespace NTS_Reader_CS.xml
{
    class B201 : Dbconn
    {
        public  String form_cd { get; set; } // 서식코드
        public 인별반복[] 인별;

        public struct 인별반복
        {
            public string resid { get; set; } //수익자 주민등록번호
            public string name { get; set; } //수익자 성명
            public 상품별반복[] 상품;
        }

        public struct 상품별반복
        {
            public string dat_cd { get; set; } //자료코드
            public string busnid { get; set; } //사업자번호
            public string trade_nm { get; set; } //상호
            public string acc_no { get; set; } //증권번호

            public string goods_nm { get; set; } //보험종료
            public string insu_resid { get; set; } //피보험자 주민등록번호
            public string insu_nm { get; set; } //피보험자 성명
            public int sum { get; set; } //수령금액계

            public B201M[] 월별;

        }

        public struct B201M
        {
            public int amt { get; set; } //월별수령금액
            public string mm { get; set; } //수령월
        }

        public void Execute(B201 entity)
        {
            int calYear = DateTime.Now.Year - 1; //연말정산 대상연도
            calYear = 2021; //테스트 년도

            string emp_no = ""; ;

            int 개인별합계 = 0;
            int 전체합계 = 0;

            foreach (var 인별 in entity.인별)
            {
                개인별합계 = 0;                

                Dictionary<string, object> resultMap = ReadSql($"select * from QE023DT WHERE ycal_resi = fn_za010ms_03('{인별.resid}') and ycal_year = '{calYear}' and YCAL_RERA='0' ");
                if (resultMap.Count > 0)
                {
                    emp_no = resultMap["EMP_NO"].ToString(); //사번
                }

                foreach (var data in 인별.상품)
                {
                    개인별합계 += data.sum;
                    
                    executeSql($@"                                      
                                    UPDATE QE023DT
                                    SET YCAL_MEDI_INSU_AMT = {개인별합계}
                                    WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear}                                 
                    ");
                  
                }
                전체합계 += 개인별합계;              

            }

            //전체 합계금액 수정
            executeSql($@"                                      
                                    UPDATE QE020MS
                                    SET YCAL_SPCD_6_MEDI_INSU_AMT = {전체합계}
                                    WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear}                                    
                    ");
            
        }


    }
}
