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
            try
            { 
            if (entity.인별 == null)
            {
                return;
            }

            int calYear = DateTime.Now.Year - 1; //연말정산 대상연도
            calYear = NTS_Reader.ycal_year;  //테스트 년도

                string emp_no = ""; 

            int 개인별합계 = 0;
            int 장애_개인별합계 = 0;
            int 전체합계1 = 0; //본인
            int 전체합계2 = 0; //취학전
            int 전체합계3 = 0; //초중고
            int 전체합계4 = 0; //대학교
            int 전체합계5 = 0; //장애


            emp_no = NTS_Reader.emp_no;


            foreach (var 인별 in entity.인별)
            {
                개인별합계 = 0;
                장애_개인별합계 = 0;

                foreach (var data in 인별.학교)
                {
                    

                    Dictionary<string, object> resultMap = ReadSql($"select * from QE023DT WHERE emp_no = '{emp_no}' and ycal_year = '{calYear}' and ycal_resi = fn_za010ms_03('{인별.resid}')");

                    //본인인 경우
                    if (resultMap["YCAL_RERA"].ToString() == "0")
                    {
                        개인별합계 += data.sum;
                        전체합계1 += data.sum;
                        executeSql($@"                                      
                                    UPDATE QE023DT
                                    SET YCAL_EDUC_AMT = NVL(YCAL_EDUC_AMT,0)+{개인별합계}
                                       ,YCAL_EDUC_GUBUN=1
                                    WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear} and YCAL_RESI=fn_za010ms_03('{인별.resid}')                                                                        
                         ");
                      
                    }
                    //취학전
                    else if (data.edu_tp == "1" || data.edu_tp == "F")
                    {
                        개인별합계 += data.sum;
                        전체합계2 += data.sum;                        
                        executeSql($@"                                      
                                    UPDATE QE023DT
                                    SET YCAL_EDUC_AMT =  NVL(YCAL_EDUC_AMT,0)+{개인별합계}
                                       ,YCAL_EDUC_GUBUN=2
                                    WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear} and YCAL_RESI=fn_za010ms_03('{인별.resid}')                                      
                         ");
                    }
                    //초중고
                    else if(data.edu_tp == "2" || data.edu_tp == "3" || data.edu_tp == "4" || data.edu_tp == "9")
                    {
                        개인별합계 += data.sum;
                        전체합계3 += data.sum;//개인별합계;                       
                            executeSql($@"                                      
                                    UPDATE QE023DT
                                    SET YCAL_EDUC_AMT =  NVL(YCAL_EDUC_AMT,0)+{개인별합계}
                                       ,YCAL_EDUC_GUBUN=3
                                    WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear} and YCAL_RESI=fn_za010ms_03('{인별.resid}')                                    
                         ");
                            
                        }
                    //대학교
                    else if (data.edu_tp == "5" || data.edu_tp == "6")
                    {
                        개인별합계 += data.sum;
                        전체합계4 += data.sum;                        
                        executeSql($@"                                      
                                    UPDATE QE023DT
                                    SET YCAL_EDUC_AMT =  NVL(YCAL_EDUC_AMT,0)+{개인별합계}
                                       ,YCAL_EDUC_GUBUN=4
                                    WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear} and YCAL_RESI=fn_za010ms_03('{인별.resid}')                                                                        
                         ");
                    }
                    //장애
                    else if (data.edu_tp == "J" || data.edu_tp == "K" || data.edu_tp == "H")
                    {
                           
                        장애_개인별합계 += data.sum;
                        전체합계5 += data.sum;
                        executeSql($@"                                      
                                    UPDATE QE023DT
                                    SET YCAL_EDUC3_AMT =  NVL(YCAL_EDUC3_AMT,0)+{장애_개인별합계}
                                       ,YCAL_EDUC_GUBUN=5
                                    WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear} and YCAL_RESI=fn_za010ms_03('{인별.resid}')                                                                       
                         ");
                    }
                }

            }

            //본인
            executeSql($@"                                      
                                    UPDATE QE020MS
                                    SET YCAL_SPCD_3_SELF_AMT = NVL(YCAL_SPCD_3_SELF_AMT,0)+ {전체합계1}
                                    WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear}                                    
                    ");
            //취학전
            executeSql($@"                                      
                                    UPDATE QE020MS
                                    SET YCAL_SPCD_3_BEFR_AMT = NVL(YCAL_SPCD_3_BEFR_AMT,0)+{전체합계2}
                                    WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear}                                    
                    ");
            //초중고
            executeSql($@"                                      
                                    UPDATE QE020MS
                                    SET YCAL_SPCD_3_SCH_AMT = NVL(YCAL_SPCD_3_SCH_AMT,0)+{전체합계3}
                                    WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear}                                    
                    ");
            //대학교
            executeSql($@"                                      
                                    UPDATE QE020MS
                                    SET YCAL_SPCD_3_UNIV_AMT = NVL(YCAL_SPCD_3_UNIV_AMT,0)+{전체합계4}
                                    WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear}                                    
                    ");


            //장애
            executeSql($@"                                      
                                    UPDATE QE020MS
                                    SET YCAL_SPCD_3_OBS_AMT = NVL(YCAL_SPCD_3_OBS_AMT,0)+{전체합계5}
                                    WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear}                                    
                    ");

        }
            catch (Exception ex)
            {
                throw new Exception("교육비[C102] 처리 중 오류가 발생하였습니다.");
            }

        }
    }
}
