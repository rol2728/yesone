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
            try
            {
                if (entity.인별 == null)
                {
                    return;
                }

                int calYear = DateTime.Now.Year - 1; //연말정산 대상연도
                calYear = NTS_Reader.ycal_year; //테스트 년도

                string emp_no = ""; ;

                int 개인별합계 = 0;
                int 전체합계1 = 0; //(본인, 65세, 장애인)
                int 전체합계2 = 0; //난임
                int 전체합계3 = 0; //그밖대상자
                int 시퀀스 = 0;


                foreach (var 인별 in entity.인별)
                {
                    Dictionary<string, object> resultMap = ReadSql($"select * from QE023DT WHERE ycal_resi = fn_za010ms_03('{인별.resid}') and ycal_year = '{calYear}' and YCAL_RERA='0' ");
                    if (resultMap.Count > 0)
                    {
                        emp_no = resultMap["EMP_NO"].ToString(); //사번
                    }
                }

                emp_no = NTS_Reader.emp_no;

                //의료비 상세내역 삭제 및 초기화
                executeSql($@" DELETE FROM QE021MS WHERE EMP_NO='{emp_no}' and YCAL_YEAR={calYear} and PROV_MEDI_CODE = '1' ");


                foreach (var 인별 in entity.인별)
                {
                    개인별합계 = 0;

                    Dictionary<string, object> resultMap = ReadSql($"select YCAL_RERA,YCAL_OBST,TRUNC(MONTHS_BETWEEN(TRUNC(TO_DATE('{calYear}'||'1231'))," +
                                                                   $" To_date(decode(substr(FN_ZA010MS_04(YCAL_RESI), 7, 1),   " +
                                                                   $"                                    '1', '19'," +
                                                                   $"                                    '2', '19'," +
                                                                   $"                                    '5', '19'," +
                                                                   $"                                    '6', '19'," +
                                                                   $"                                    '3', '20'," +
                                                                   $"                                    '4', '20'," +
                                                                   $"                                    '7', '20'," +
                                                                   $"                                    '8', '20' )" +
                                                                   $"|| substr(FN_ZA010MS_04(YCAL_RESI), 1, 6))) / 12) AGE from QE023DT WHERE emp_no = '{emp_no}' and ycal_year = '{calYear}' and ycal_resi = fn_za010ms_03('{인별.resid}')");

                    string ycal_rera = "";
                    string ycal_obst = "";
                    string ycal_old_yn = "";
                    string stan_yn = "";//장애경로구분
                    if (resultMap.Count > 0)
                    {
                        ycal_rera = resultMap["YCAL_RERA"].ToString(); //인적구분
                        ycal_obst = resultMap["YCAL_OBST"].ToString() == "1" ? "A" : ""; //장애인공제
                        if (Convert.ToInt32(resultMap["AGE"].ToString())>= 65) { 
                            ycal_old_yn = "B";
                        }
                        if (ycal_obst =="A")
                        {
                            stan_yn = "A";
                        }
                        else if (ycal_old_yn == "B")
                          { stan_yn = "B"; }

                    }
                 
                    else if (resultMap.Count==0)
                    {
                        continue;
                    }
  
                    foreach (var data in 인별.기관)
                    {
                        시퀀스 += 1;
                        개인별합계 += data.sum;
                       // string 난임여부 = data.dat_cd == "G0034" ? "Y" : "N";

                        //의료비 테이블 입력 (QE021MS)

                        executeSql($@"                                      
                                    INSERT INTO QE021MS(YCAL_YEAR, EMP_NO, SEQ_NO,  PROV_RENO, PROV_NAME, PROV_MEDI_CODE, PROV_PAYM, PROV_COUN, FAMI_RERA, FAMI_RESI, HAND_OLD, PROV_BABY_YN, U_EMP_NO, U_DATE, U_IP)
                                           VALUES('{calYear}', {emp_no},{시퀀스}, '{data.busnid}', '{data.trade_nm}', '1', {data.sum}, 1, '{ycal_rera}', fn_za010ms_03('{인별.resid}'), '{stan_yn}', 'N', '{emp_no}', sysdate, '{Util.getIP()}')
                              ");
                    }


                    if (resultMap["YCAL_RERA"].ToString() == "0")
                    {
                        전체합계1 += 개인별합계;

                        executeSql($@"                                      
                                    UPDATE QE023DT
                                    SET YCAL_MEDI_AMT = {개인별합계}
                                    WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear} and YCAL_RESI=fn_za010ms_03('{인별.resid}')                  
                         ");
                        executeSql($@"                                      
                                    UPDATE QE020MS
                                    SET YCAL_SPCD_2_OLD_AMT = {전체합계1}
                                    WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear}                  
                         ");
                    }
                    // else if (resultMap["YCAL_OBST"].ToString() == "1" || resultMap["YCAL_OLD_YN"].ToString() == "1")
                    else if (resultMap["YCAL_OBST"].ToString() == "1" || Convert.ToInt32(resultMap["AGE"].ToString()) >= 65)
                    {
                        전체합계1 += 개인별합계;
                        executeSql($@"                                      
                                    UPDATE QE023DT
                                    SET YCAL_MEDI_OBST_AMT = {개인별합계}
                                    WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear} and YCAL_RESI=fn_za010ms_03('{인별.resid}')                  
                         ");
                        executeSql($@"                                      
                                    UPDATE QE020MS
                                    SET YCAL_SPCD_2_OLD_AMT = {전체합계1}
                                    WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear}                  
                         ");
                    }
                    else
                    {
                        전체합계3 += 개인별합계;
                        executeSql($@"                                      
                                    UPDATE QE023DT
                                    SET YCAL_MEDI_AMT = {개인별합계}
                                    WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear} and YCAL_RESI=fn_za010ms_03('{인별.resid}')                  
                         ");
                        executeSql($@"                                      
                                    UPDATE QE020MS
                                    SET YCAL_SPCD_2_NORM_AMT = {전체합계3}
                                    WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear}                  
                         ");
                    }

                }
            }
            catch(Exception ex)
            {
                throw new Exception("의료비[B101] 처리 중 오류가 발생하였습니다.");
            }

        }



    }
}
