using System;
using System.Collections.Generic;
using System.Text;
using NTS_Reader_CS.db;

namespace NTS_Reader_CS.xml
{
    class Q201 : Dbconn //벤처기업투자신탁(전년도 납입분)
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
            public string trade_nm { get; set; } //투자기관    
            public string secu_no { get; set; } //계좌번호    

            public string fund_nm { get; set; } //투자신탁명
            public string reg_dt { get; set; } //납입연도
            public string vnt_asct_cl_cd { get; set; } //벤처조합구분코드
            public string com_cd { get; set; } //금융기관코드
            public int sum { get; set; } //연간합계액

            public Q201M[] 월별;
        }

        public struct Q201M
        {
            public int amt { get; set; } //월별납입금액
            public string mm { get; set; } //납입월         
        }

        public void Execute(Q201 entity)
        {
          try
           { 
            if(entity.인별 == null)
            {
                return;
            }

            int calYear = DateTime.Now.Year - 1; //연말정산 대상연도
            calYear = NTS_Reader.ycal_year; //테스트 년도

            string emp_no = ""; ;

            int 벤처기업투자신탁_전체합계 = 0;
            int 시퀀스 = 0;

           /* foreach (var 인별 in entity.인별)
            {
                Dictionary<string, object> resultMap = ReadSql($"select * from QE023DT WHERE ycal_resi = fn_za010ms_03('{인별.resid}') and ycal_year = '{calYear}' and YCAL_RERA='0' ");
                if (resultMap.Count > 0)
                {
                    emp_no = resultMap["EMP_NO"].ToString(); //사번
                }
            }*/
           emp_no = NTS_Reader.emp_no;

                //벤처기업투자신탁
                executeSql($@" DELETE FROM QE024MS WHERE EMP_NO='{emp_no}' and YCAL_YEAR={calYear-1} and ANNU_RENO ='61' ");

            //시퀀스 번호가져오기
            Dictionary<string, object> resultMap2 = ReadSql($"select MAX(NVL(SEQ_NO,0))+1 AS SEQ_NO from QE024MS WHERE emp_no = '{emp_no}' and ycal_year = '{calYear}' GROUP BY EMP_NO ");
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
                //   벤처기업투자신탁_전체합계 = 0;

                foreach (var data in 인별.상품)
                {
                    if (data.dat_cd == "G0035" && data.reg_dt ==Convert.ToString(calYear - 1))
                    {
                        벤처기업투자신탁_전체합계 += data.sum;

                        //연금저축 테이블 입력 (QE024MS)

                        executeSql($@"                                      
                                    INSERT INTO QE024MS(YCAL_YEAR, EMP_NO, SEQ_NO, ANNU_RENO, ANNU_CODE,
                                                        ANNU_NAME, ANNU_ACCO, ANNU_YEAR, ANNU_AMT, ANNU_DTRG_AMT, 
                                                        U_EMP_NO, U_DATE, U_IP,ANNU_INVE_YEAR, ANNU_INVE_GUBUN)
                                           VALUES('{calYear}', {emp_no},{시퀀스},'61', '{data.com_cd}',
                                                  '{data.trade_nm}','{data.secu_no}','', {data.sum}, 0, {emp_no}, sysdate, '{Util.getIP()}',{data.reg_dt},'3' )
                              ");
                        //  시퀀스 += 1;
                    }
                    시퀀스 += 1;
                }

            }

            //전체 합계금액 수정
            //3천만원이하
            if (벤처기업투자신탁_전체합계 <= 30000000)
            {
                executeSql($@"                                      
                                     UPDATE QE020MS
                                       SET YCAL_NTXD_3_15_3_AMT = nvl(YCAL_NTXD_3_15_3_AMT,0)+{벤처기업투자신탁_전체합계}
                                           , U_DATE =SYSDATE
                                           , U_IP ='{Util.getIP()}'
                                     WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear}                                 
                        ");
            }
            else if (벤처기업투자신탁_전체합계 <= 50000000)
            {
                executeSql($@"                                      
                                     UPDATE QE020MS
                                       SET YCAL_NTXD_3_15_2_AMT = nvl(YCAL_NTXD_3_15_2_AMT,0)+{벤처기업투자신탁_전체합계}
                                           , U_DATE =SYSDATE
                                           , U_IP ='{Util.getIP()}'
                                     WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear}                                 
                        ");
            }
            else if (벤처기업투자신탁_전체합계 > 50000000)
            {
                executeSql($@"                                      
                                     UPDATE QE020MS
                                       SET YCAL_NTXD_3_15_1_AMT = nvl(YCAL_NTXD_3_15_1_AMT,0)+{벤처기업투자신탁_전체합계}
                                           , U_DATE =SYSDATE
                                           , U_IP ='{Util.getIP()}'
                                     WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear}                                 
                        ");
            }
        }
     catch (Exception ex)
            {
                throw new Exception("전년도 벤처기업투자신탁[Q201]  처리 중 오류가 발생하였습니다.");
            }
      }
    }
}
