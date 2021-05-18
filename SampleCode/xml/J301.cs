using System;
using System.Collections.Generic;
using System.Text;
using NTS_Reader_CS.db;

namespace NTS_Reader_CS.xml
{
    class J301 : Dbconn //주택마련저축
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

            public string goods_nm { get; set; } //저축명
            public string saving_gubn { get; set; } //저축구분
            public string reg_dt { get; set; } //가입일자
            public string com_cd { get; set; } //금융기관코드
            public int sum { get; set; } //납입금액계

            public J301M[] 월별;

        }

        public struct J301M
        {
            public int amt { get; set; } //월별납입금액
            public string mm { get; set; } //납입월
            public string fix_cd { get; set; } //확정구분코드
        }

        public void Execute(J301 entity)
        {
            if (entity.인별 == null)
            {
                return;
            }

            int calYear = DateTime.Now.Year - 1; //연말정산 대상연도
            calYear = 2021; //테스트 년도

            string emp_no = ""; ;
             
            Dictionary<string, object> resultMap1 = ReadSql($@" SELECT NVL((SELECT MAX(SEQ_NO) +1 FROM QE024MS WHERE YCAL_YEAR = '{calYear}' AND EMP_NO = '{emp_no}'),0) AS SEQ_NO FROM DUAL");
            int 시퀀스 = Convert.ToInt32(resultMap1["SEQ_NO"].ToString()); //사번         

            foreach (var 인별 in entity.인별)
            {
                Dictionary<string, object> resultMap = ReadSql($"select * from QE023DT WHERE ycal_resi = fn_za010ms_03('{인별.resid}') and ycal_year = '{calYear}' and YCAL_RERA='0' ");
                if (resultMap.Count > 0)
                {
                    emp_no = resultMap["EMP_NO"].ToString(); //사번
                }
            }

            //전체합계컬럼 초기화
            executeSql($@"                                      
                                UPDATE QE020MS
                                SET YCAL_SPCD_4_1_1_AMT = 0        
                                   ,YCAL_SPCD_4_1_2_AMT = 0        
                                   ,YCAL_SPCD_4_1_3_AMT = 0                                                                           
                                WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear}
                        ");


            foreach (var 인별 in entity.인별)
            {

                foreach (var data in 인별.상품)
                {
                    

                    if (data.saving_gubn == "1")
                    {
                        //전체합계
                        executeSql($@"                                      
                                UPDATE QE020MS
                                SET YCAL_SPCD_4_1_2_AMT = YCAL_SPCD_4_1_2_AMT + {data.sum}                                                                                               
                                WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear}
                        ");


                        //테이블 입력 (QE024MS)
                        executeSql($@"                                      
                                    INSERT INTO QE024MS(YCAL_YEAR, EMP_NO, SEQ_NO,  ANNU_RENO, ANNU_CODE, ANNU_NAME, ANNU_ACCO, ANNU_AMT, U_EMP_NO, U_DATE, U_IP)
                                           VALUES('{calYear}', {emp_no},{시퀀스}, '31', '{data.com_cd}', '{data.trade_nm}', '{data.acc_no}', {data.sum}, '{emp_no}', sysdate, '10.10.11.104')
                               ");

                    }
                    else if (data.saving_gubn == "2")
                    {
                        //전체합계
                        executeSql($@"                                      
                                UPDATE QE020MS
                                SET YCAL_SPCD_4_1_2_AMT = YCAL_SPCD_4_1_1_AMT + {data.sum}                                                                                               
                                WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear}
                        ");

                        //테이블 입력 (QE024MS)
                        executeSql($@"                                      
                                    INSERT INTO QE024MS(YCAL_YEAR, EMP_NO, SEQ_NO,  ANNU_RENO, ANNU_CODE, ANNU_NAME, ANNU_ACCO, ANNU_AMT, U_EMP_NO, U_DATE, U_IP)
                                           VALUES('{calYear}', {emp_no},{시퀀스}, '32', '{data.com_cd}', '{data.trade_nm}', '{data.acc_no}', {data.sum}, '{emp_no}', sysdate, '10.10.11.104')
                               ");
                    }
                    else if (data.saving_gubn == "3")
                    {
                        //전체합계
                        executeSql($@"                                      
                                UPDATE QE020MS
                                SET YCAL_SPCD_4_1_2_AMT = YCAL_SPCD_4_1_3_AMT + {data.sum}                                                                                               
                                WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear}
                        ");

                        //테이블 입력 (QE024MS)
                        executeSql($@"                                      
                                    INSERT INTO QE024MS(YCAL_YEAR, EMP_NO, SEQ_NO,  ANNU_RENO, ANNU_CODE, ANNU_NAME, ANNU_ACCO, ANNU_AMT, U_EMP_NO, U_DATE, U_IP)
                                           VALUES('{calYear}', {emp_no},{시퀀스}, '34', '{data.com_cd}', '{data.trade_nm}', '{data.acc_no}', {data.sum}, '{emp_no}', sysdate, '10.10.11.104')
                               ");
                    }

                    시퀀스++;
                }
            }

        }
    }
}
