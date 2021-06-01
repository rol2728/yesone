using System;
using System.Collections.Generic;
using System.Text;
using NTS_Reader_CS.db;

namespace NTS_Reader_CS.xml
{
    class L102 : Dbconn //기부금
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
            public string trade_nm { get; set; } //단체명
            public string donation_cd { get; set; } //기부유형

            public int sum { get; set; } //공제대상기부액
            public int sbdy_apln_sum { get; set; } //기부장려금신청금액
            public int conb_sum { get; set; } //기부금액합계


            public L102M[] 일별;

        }

        public struct L102M
        {
            public int amt { get; set; } //일별기부금액
            public string dd { get; set; } //기부일자
            public int apln { get; set; } //일별기부장려금신청금액
            public int sum { get; set; } //일별기부금액합계


        }

        public void Execute(L102 entity)
        {  
          try
            { 
            if (entity.인별 == null)
            {
                return;
            }

            int calYear = DateTime.Now.Year - 1; //연말정산 대상연도
            calYear = 2021; //테스트 년도

            string emp_no = "";
            string fam_rera = "";//가족관계
            int 개인별합계 = 0;
            int 법정합계 = 0; //법정기부금
            int 종교합계 = 0; //종교단체기부금
            int 지정합계 = 0; //종교단체외지정기부금
            int 정치합계 = 0; //정치자금기부금
            
            int 시퀀스 = 0;

            foreach (var 인별 in entity.인별)
            {
                Dictionary<string, object> resultMap = ReadSql($"select * from QE023DT WHERE ycal_resi = fn_za010ms_03('{인별.resid}') and ycal_year = '{calYear}' and YCAL_RERA='0' ");
                if (resultMap.Count > 0)
                {
                    emp_no = resultMap["EMP_NO"].ToString(); //사번
                }
            }

            //기부금 지우기
            executeSql($@" DELETE FROM QE023MS WHERE EMP_NO='{emp_no}' and YCAL_YEAR={calYear} and u_emp_no ='국세청' ");

            //시퀀스 번호가져오기
            Dictionary<string, object> resultMap2 = ReadSql($"select MAX(NVL(SEQ_NO,0))+1 AS SEQ_NO from QE023MS WHERE emp_no = '{emp_no}' and ycal_year = '{calYear}' GROUP BY EMP_NO ");
            if (resultMap2.Count > 0)
            {
                시퀀스 = Convert.ToInt32(resultMap2["SEQ_NO"].ToString()); //시퀀스
            }
            else
            {
                시퀀스 = 1; //시퀀스
            }
            foreach (var 인별 in entity.인별)
            {
                개인별합계 = 0;

                foreach (var data in 인별.기관)
                {
                    Dictionary<string, object> resultMap = ReadSql($"select * from QE023DT WHERE emp_no = '{emp_no}' and ycal_year = '{calYear}' and ycal_resi = fn_za010ms_03('{인별.resid}')");
                    //가족관계
                    if (resultMap["YCAL_RERA"].ToString() == "0") //본인
                    {
                        fam_rera = "1";
                     }
                    else if (resultMap["YCAL_RERA"].ToString() == "3")//배우자
                    {
                        fam_rera = "2";
                     }
                    else if (resultMap["YCAL_RERA"].ToString() == "4" || resultMap["YCAL_RERA"].ToString() == "5") //자녀
                    {
                        fam_rera = "3";
                     }
                    else if (resultMap["YCAL_RERA"].ToString() == "2"|| resultMap["YCAL_RERA"].ToString() == "3") //부모
                    {
                        fam_rera = "4";
                    }
                    else if (resultMap["YCAL_RERA"].ToString() == "6")  //형제자매
                    {
                        fam_rera = "5";
                    }
                    else { fam_rera = "6"; }

                    //법정기부금
                    if (data.donation_cd == "10")
                    {
                        개인별합계 += data.conb_sum;
                        법정합계 += data.conb_sum;
                        executeSql($@"                                      
                                    UPDATE QE023DT
                                    SET YCAL_GIBU_AMT = {개인별합계}
                                    WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear} and YCAL_RESI=fn_za010ms_03('{인별.resid}')                                                                        
                         ");
                        //기부금테이블
                        executeSql($@"                                      
                                    INSERT INTO QE023MS(YCAL_YEAR, EMP_NO, SEQ_NO, PROV_RENO, PROV_NAME, PROV_RERA,
                                                        PROV_COUN, PROV_AMT, PROV_IO_TYPE, PROV_RERA_NAME, PROV_RESI_NO,PROV_RERA_CODE,
                                                        PROV_DEDU_AMT, PROV_TRAN_AMT, PROV_DEDU2_AMT, PROV_TRAN2_AMT, 
                                                        PROV_YEAR, PROV_TOT_AMT, PROV_DIS_AMT, D_SEQ_NO, PROV_GUBUN, PROV_APP_AMT,
                                                        U_EMP_NO, U_DATE, U_IP)
                                           VALUES('{calYear}', '{emp_no}',{시퀀스}, '{data.busnid}','{data.trade_nm}','10','1',
                                                  {data.conb_sum},'1','{인별.name}',  fn_za010ms_03('{인별.resid}'),'{fam_rera}', 0,{data.sum},0,0,'{calYear}',
                                                  {data.conb_sum},0,1,1,{data.sbdy_apln_sum},
                                                  '국세청', sysdate, '10.10.11.104')
                              ");
                        }
                    //종교
                    else if (data.donation_cd == "41")
                    {
                        개인별합계 += data.conb_sum;
                        종교합계 += data.conb_sum;
                        executeSql($@"                                      
                                    UPDATE QE023DT
                                    SET YCAL_GIBU_AMT =  {개인별합계}
                                    WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear} and YCAL_RESI=fn_za010ms_03('{인별.resid}')                                      
                         ");
                        //기부금테이블
                        executeSql($@"                                      
                                    INSERT INTO QE023MS(YCAL_YEAR, EMP_NO, SEQ_NO, PROV_RENO, PROV_NAME, PROV_RERA,
                                                        PROV_COUN, PROV_AMT, PROV_IO_TYPE, PROV_RERA_NAME, PROV_RESI_NO,PROV_RERA_CODE,
                                                        PROV_DEDU_AMT, PROV_TRAN_AMT, PROV_DEDU2_AMT, PROV_TRAN2_AMT, 
                                                        PROV_YEAR, PROV_TOT_AMT, PROV_DIS_AMT, D_SEQ_NO, PROV_GUBUN, PROV_APP_AMT,
                                                        U_EMP_NO, U_DATE, U_IP)
                                           VALUES('{calYear}', '{emp_no}',{시퀀스}, '{data.busnid}','{data.trade_nm}','41','1',
                                                  {data.conb_sum},'1','{인별.name}',  fn_za010ms_03('{인별.resid}'),'{fam_rera}', 0,{data.sum},0,0,'{calYear}',
                                                  {data.conb_sum},0,1,1,{data.sbdy_apln_sum},
                                                  '국세청', sysdate, '10.10.11.104')
                              ");
                    }
                    //지정
                    else if (data.donation_cd == "40")
                    {
                        개인별합계 += data.conb_sum;
                        지정합계 += data.conb_sum;//개인별합계;                       
                        executeSql($@"                                      
                                    UPDATE QE023DT
                                    SET YCAL_GIBU_AMT = {개인별합계}
                                    WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear} and YCAL_RESI=fn_za010ms_03('{인별.resid}')                                    
                         ");
                        //기부금테이블
                        executeSql($@"                                      
                                    INSERT INTO QE023MS(YCAL_YEAR, EMP_NO, SEQ_NO, PROV_RENO, PROV_NAME, PROV_RERA,
                                                        PROV_COUN, PROV_AMT, PROV_IO_TYPE, PROV_RERA_NAME, PROV_RESI_NO,PROV_RERA_CODE,
                                                        PROV_DEDU_AMT, PROV_TRAN_AMT, PROV_DEDU2_AMT, PROV_TRAN2_AMT, 
                                                        PROV_YEAR, PROV_TOT_AMT, PROV_DIS_AMT, D_SEQ_NO, PROV_GUBUN, PROV_APP_AMT,
                                                        U_EMP_NO, U_DATE, U_IP)
                                           VALUES('{calYear}', '{emp_no}',{시퀀스}, '{data.busnid}','{data.trade_nm}','40','1',
                                                  {data.conb_sum},'1','{인별.name}',  fn_za010ms_03('{인별.resid}'),'{fam_rera}', 0,{data.sum},0,0,'{calYear}',
                                                  {data.conb_sum},0,1,1,{data.sbdy_apln_sum},
                                                  '국세청', sysdate, '10.10.11.104')
                              ");
                    }
                    //정치자금
                    else if (data.donation_cd == "20")
                    {
                        개인별합계 += data.conb_sum;
                        정치합계 += data.conb_sum;
                        executeSql($@"                                      
                                    UPDATE QE023DT
                                    SET YCAL_GIBU_AMT = {개인별합계}
                                    WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear} and YCAL_RESI=fn_za010ms_03('{인별.resid}')                                                                        
                         ");
                        //기부금테이블
                        executeSql($@"                                      
                                    INSERT INTO QE023MS(YCAL_YEAR, EMP_NO, SEQ_NO, PROV_RENO, PROV_NAME, PROV_RERA,
                                                        PROV_COUN, PROV_AMT, PROV_IO_TYPE, PROV_RERA_NAME, PROV_RESI_NO,PROV_RERA_CODE,
                                                        PROV_DEDU_AMT, PROV_TRAN_AMT, PROV_DEDU2_AMT, PROV_TRAN2_AMT, 
                                                        PROV_YEAR, PROV_TOT_AMT, PROV_DIS_AMT, D_SEQ_NO, PROV_GUBUN, PROV_APP_AMT,
                                                        U_EMP_NO, U_DATE, U_IP)
                                           VALUES('{calYear}', '{emp_no}',{시퀀스}, '{data.busnid}','{data.trade_nm}','20','1',
                                                  {data.conb_sum},'1','{인별.name}', fn_za010ms_03('{인별.resid}'), '{fam_rera}',0,{data.sum},0,0,'{calYear}',
                                                  {data.conb_sum},0,1,1,{data.sbdy_apln_sum},
                                                  '국세청', sysdate, '10.10.11.104')
                              ");
                    }
                    시퀀스 += 1;
                }

            }

            //전체합계
            executeSql($@"                                      
                                    UPDATE QE020MS
                                    SET YCAL_SPCD_5_ALL_AMT = {법정합계},
                                        YCAL_SPCD_5_KNOW_2_1_AMT = {종교합계},
                                        YCAL_SPCD_5_KNOW_2_AMT = {지정합계},
                                        YCAL_SPCD_5_POLI_AMT = {정치합계}
                                    WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear}                                    
                    ");

        }
     catch (Exception ex)
            {
                throw new Exception("B201 처리 중 오류가 발생하였습니다.");
            }
     }
    }
}
