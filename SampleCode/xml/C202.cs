using System;
using System.Collections.Generic;
using System.Text;
using NTS_Reader_CS.db;

namespace NTS_Reader_CS.xml
{
    class C202 : Dbconn
    {
        public String form_cd { get; set; } // 서식코드
        public 인별반복[] 인별;

        public struct 인별반복
        {
            public string resid { get; set; } //주민등록번호
            public string name { get; set; } //성명
            public 과정별반복[] 과정;
        }

        public struct 과정별반복
        {
            public string dat_cd { get; set; } //자료코드
            public string busnid { get; set; } //사업자번호
            public string trade_nm { get; set; } //교유기관명

            public string course_cd { get; set; } //과정코드
            public string subject_nm { get; set; } //과정명
            public int sum { get; set; } //납입금액계

            public C202M[] 월별;

        }

        public struct C202M
        {
            public int amt { get; set; } //월별납입금액
            public string mm { get; set; } //납입월
        }

        public void Execute(C202 entity)
        {

            int calYear = DateTime.Now.Year - 1; //연말정산 대상연도
            calYear = 2021; //테스트 년도

            string emp_no = "";

            int 개인별합계 = 0;
            int 전체합계 = 0;

            foreach (var 인별 in entity.인별)
            {
                Dictionary<string, object> resultMap = ReadSql($"select * from QE023DT WHERE ycal_resi = fn_za010ms_03('{인별.resid}') and ycal_year = '{calYear}' and YCAL_RERA='0' ");
                if (resultMap.Count > 0)
                {
                    emp_no = resultMap["EMP_NO"].ToString(); //사번
                }
            }


            foreach (var 인별 in entity.인별)
            {
                개인별합계 = 0;

                foreach (var data in 인별.과정)
                {
                    개인별합계 += data.sum;
                    전체합계 += data.sum;
                }


                //개인별 합계
                executeSql($@"                                      
                                    UPDATE QE023DT
                                    SET YCAL_EDUC_AMT = {개인별합계}
                                       ,YCAL_EDUC_GUBUN = '1'
                                    WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear} and YCAL_RESI=fn_za010ms_03('{인별.resid}')                                 
                ");
            }

            //전체 합계
            executeSql($@"                                      
                                    UPDATE QE020MS
                                    SET YCAL_SPCD_3_SCH_AMT = {전체합계}
                                    WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear}                                    
            ");
        }
    }
}
