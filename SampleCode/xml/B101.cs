using System;
using System.Collections.Generic;
using System.Text;
using NTS_Reader_CS.db;

namespace NTS_Reader_CS.xml
{
    class B101 : Dbconn
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
            public string trade_nm { get; set; } //상호

            public int sum { get; set; } //납입금액계
            public B101D[] 일별;
        }

        public struct B101D
        {
            public int amt { get; set; } //납입금액
            public string dd { get; set; } //납입일자
        }


        public void Execute(B101 entity)
        {
            int calYear = DateTime.Now.Year - 1; //연말정산 대상연도
            calYear = 2021; //테스트 년도

            string emp_no = ""; ;

            int 개인별합계 = 0;
            int 전체합계 = 0;
            int 시퀀스 = 0;


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
                시퀀스 += 1;

                Dictionary<string, object> resultMap = ReadSql($"select * from QE023DT WHERE emp_no = '{emp_no}' and ycal_year = '{calYear}' and ycal_resi = fn_za010ms_03('{인별.resid}')");


                string ycal_rera = "";
                string ycal_obst = "";
                string ycal_old_yn = "";

                if (resultMap.Count > 0)
                {
                    ycal_rera = resultMap["YCAL_RERA"].ToString(); //인적구분
                    ycal_obst = resultMap["YCAL_OBST"].ToString() == "1" ? "A" : ""; //장애인공제
                    ycal_old_yn = resultMap["YCAL_OLD_YN"].ToString() == "1" ? "B" : ""; //경로우대
                }
         

                foreach (var data in 인별.기관)
                {
                    개인별합계 += data.sum;
                    string 난임여부 = data.dat_cd == "G0034" ? "Y" : "N";

                    //의료비 테이블 입력 (QE021MS)

                    executeSql($@"                                      
                                    INSERT INTO QE021MS(YCAL_YEAR, EMP_NO, SEQ_NO,  PROV_RENO, PROV_NAME, PROV_MEDI_CODE, PROV_PAYM, PROV_COUN, FAMI_RERA, FAMI_RESI, HAND_OLD, PROV_BABY_YN, U_EMP_NO, U_DATE, U_IP)
                                           VALUES('{calYear}', {emp_no},{시퀀스}, '{data.busnid}', '{data.trade_nm}', '1', {data.sum}, 1, '{ycal_rera}', fn_za010ms_03('{인별.resid}'), '{ycal_obst}', '{난임여부}', '{emp_no}', sysdate, '10.10.11.104')
                              ");
                }

                전체합계 += 개인별합계;


                if (resultMap["YCAL_RERA"].ToString() == "0")
                {


                    executeSql($@"                                      
                                    UPDATE QE023DT
                                    SET YCAL_MEDI_AMT = {개인별합계}
                                    WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear} and YCAL_RESI=fn_za010ms_03('{인별.resid}')                  
                         ");
                    executeSql($@"                                      
                                    UPDATE QE020MS
                                    SET YCAL_SPCD_2_OLD_AMT = {전체합계}
                                    WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear}                  
                         ");
                }
                else if (resultMap["YCAL_OBST"].ToString() == "1" || resultMap["YCAL_OLD_YN"].ToString() == "1")
                {
                    executeSql($@"                                      
                                    UPDATE QE023DT
                                    SET YCAL_MEDI_OBST_AMT = {개인별합계}
                                    WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear} and YCAL_RESI=fn_za010ms_03('{인별.resid}')                  
                         ");
                    executeSql($@"                                      
                                    UPDATE QE020MS
                                    SET YCAL_SPCD_2_OLD_AMT = {전체합계}
                                    WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear}                  
                         ");
                }
                else
                {
                    executeSql($@"                                      
                                    UPDATE QE023DT
                                    SET YCAL_MEDI_AMT = {개인별합계}
                                    WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear} and YCAL_RESI=fn_za010ms_03('{인별.resid}')                  
                         ");
                    executeSql($@"                                      
                                    UPDATE QE020MS
                                    SET YCAL_SPCD_2_NORM_AMT = {전체합계}
                                    WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear}                  
                         ");
                }

            }

        }



    }
}
