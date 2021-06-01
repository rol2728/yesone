using System;
using System.Collections.Generic;
using System.Text;
using NTS_Reader_CS.db;

namespace NTS_Reader_CS.xml
{
    class C401 : Dbconn //교육비(학자금대출)
    {
        public String form_cd { get; set; } // 서식코드
        public 인별반복[] 인별;

        public struct 인별반복
        {
            public string resid { get; set; } //주민등록번호
            public string name { get; set; } //성명
            public 기관별반복[] 기관;
        }

        public struct 기관별반복
        {
            public string dat_cd { get; set; } //자료코드
            public string busnid { get; set; } //사업자번호
            public string trade_nm { get; set; } //기관명
            public int sum { get; set; } //납입금액계

            public C401M[] 월별;

        }

        public struct C401M
        {
            public int amt { get; set; } //월별납입금액
            public string mm { get; set; } //납입월
        }

        public void Execute(C401 entity)
        {
            try
            {
                
                if (entity.인별 == null)
            {
                return;
            }

            int calYear = DateTime.Now.Year - 1; //연말정산 대상연도
            calYear = 2021; //테스트 년도

            string emp_no = ""; ;

            int 개인별합계 = 0;
            int 본인_전체합계 = 0;            
            int 본인외_전체합계 = 0;

                /* foreach (var 인별 in entity.인별)
                 {
                     Dictionary<string, object> resultMap = ReadSql($"select * from QE023DT WHERE ycal_resi = fn_za010ms_03('{인별.resid}') and ycal_year = '{calYear}' and YCAL_RERA='0' ");
                     if (resultMap.Count > 0)
                     {
                         emp_no = resultMap["EMP_NO"].ToString(); //사번
                     }
                 }*/
            emp_no = NTS_Reader.emp_no;

                foreach (var 인별 in entity.인별)
            {
                Dictionary<string, object> resultMap = ReadSql($"select * from QE023DT WHERE emp_no = '{emp_no}' and ycal_year = '{calYear}' and ycal_resi = fn_za010ms_03('{인별.resid}')");


                if (resultMap["YCAL_RERA"].ToString() == "0")  //본인인 경우
                {
                    foreach (var data in 인별.기관)
                    {
                        개인별합계 += data.sum;
                        본인_전체합계 += data.sum;
                    }

                    //개인별합계
                    executeSql($@"                                      
                                    UPDATE QE023DT
                                    SET YCAL_EDUC_AMT = {개인별합계}
                                       ,YCAL_EDUC_GUBUN = '1'
                                    WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear} and YCAL_RESI=fn_za010ms_03('{인별.resid}')                                 
                         ");
                }
                else //본인 외
                {
                    foreach (var data in 인별.기관)
                    {
                        개인별합계 += data.sum;
                        본인외_전체합계 += data.sum;
                    }

                    //개인별합계
                    executeSql($@"                                      
                                    UPDATE QE023DT
                                    SET YCAL_EDUC_AMT = {개인별합계}
                                       ,YCAL_EDUC_GUBUN = '4'
                                    WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear} and YCAL_RESI=fn_za010ms_03('{인별.resid}')                                 
                         ");
                }
            }

            //본인_전체합계
            executeSql($@"                                      
                                    UPDATE QE020MS
                                    SET YCAL_SPCD_3_SELF_AMT = {본인_전체합계}                                       
                                    WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear}
                         ");

            //본인외_전체합계
            executeSql($@"                                      
                                    UPDATE QE020MS
                                    SET YCAL_SPCD_3_UNIV_AMT = {본인외_전체합계}                                       
                                    WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear}
                         ");

        }
            catch (Exception ex)
            {
                throw new Exception("학자금대출상환액[C401] 처리 중 오류가 발생하였습니다.");
            }

        }
    }
}
