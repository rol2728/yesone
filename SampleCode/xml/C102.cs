using System;
using System.Collections.Generic;
using System.Text;
using NTS_Reader_CS.db;

namespace NTS_Reader_CS.xml
{
    class C102 : Dbconn
    {
        public String form_cd { get; set; } // 서식코드
        public 인별반복[] 인별;

        public struct 인별반복
        {
            public string resid; //주민등록번호
            public string name; //성명
            public 학교별반복[] 학교;
        }

        public struct 학교별반복
        {
            public string dat_cd { get; set; } //자료코드
            public string busnid { get; set; } //사업자번호
            public string trade_nm { get; set; } //학교명
            public string edu_tp { get; set; } //교육비종류
            public string edu_cl { get; set; } //교육비구분
            public int sum { get; set; } //납입금액계

            public C102M[] 월별;

        }

        public struct C102M
        {
            public int amt { get; set; } //월별납입금액
            public string mm { get; set; } //납입월
        }

        public void Execute(C102 entity)
        {

            int calYear = DateTime.Now.Year - 1; //연말정산 대상연도
            calYear = 2021; //테스트 년도

            string emp_no = ""; ;

            int 개인별합계 = 0;
            int 전체합계1 = 0; //본인
            int 전체합계2 = 0; //취학전
            int 전체합계3 = 0; //초중고
            int 전체합계4 = 0; //대학교
            int 전체합계5 = 0; //장애


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

                foreach (var data in 인별.학교)
                {
                    개인별합계 += data.sum;

                    Dictionary<string, object> resultMap = ReadSql($"select * from QE023DT WHERE emp_no = '{emp_no}' and ycal_year = '{calYear}' and ycal_resi = fn_za010ms_03('{인별.resid}')");

                    //본인인 경우
                    if (resultMap["YCAL_RERA"].ToString() == "0")
                    {
                        전체합계1 += 개인별합계;
                    }
                    //취학전
                    else if (data.edu_tp == "1")
                    {
                        전체합계2 += 개인별합계;
                    }
                    //초중구
                    else if(data.edu_tp == "2" || data.edu_tp == "3" || data.edu_tp == "4" )
                    {
                        전체합계3 += 개인별합계;
                    }
                    //대학교
                    else if (data.edu_tp == "5" || data.edu_tp == "6")
                    {
                        전체합계4 += 개인별합계;
                    }
                    //장애
                    else if (data.edu_tp == "J" || data.edu_tp == "K")
                    {
                        전체합계5 += 개인별합계;
                    }
                }

            }

            //본인
            executeSql($@"                                      
                                    UPDATE QE020MS
                                    SET YCAL_SPCD_3_SELF_AMT = {전체합계1}
                                    WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear}                                    
                    ");
            //취학전
            executeSql($@"                                      
                                    UPDATE QE020MS
                                    SET YCAL_SPCD_3_BEFR_AMT = {전체합계2}
                                    WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear}                                    
                    ");
            //초중고
            executeSql($@"                                      
                                    UPDATE QE020MS
                                    SET YCAL_SPCD_3_SCH_AMT = {전체합계3}
                                    WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear}                                    
                    ");
            //대학교
            executeSql($@"                                      
                                    UPDATE QE020MS
                                    SET YCAL_SPCD_3_UNIV_AMT = {전체합계4}
                                    WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear}                                    
                    ");


            //장애
            executeSql($@"                                      
                                    UPDATE QE020MS
                                    SET YCAL_SPCD_3_OBS_AMT = {전체합계5}
                                    WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear}                                    
                    ");

        }



    }
}
