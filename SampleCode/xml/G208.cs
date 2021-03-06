using System;
using System.Collections.Generic;
using System.Text;
using NTS_Reader_CS.db;

namespace NTS_Reader_CS.xml
{
    class G208 : Dbconn //현금영수증
    {
        public String form_cd { get; set; } // 서식코드
        public 인별반복[] 인별;

        public struct 인별반복
        {
            public string resid { get; set; } //주민등록번호
            public string name { get; set; } //성명

            public int gnrl_sum { get; set; }		//2020년 일반합계
            public int tdmr_sum { get; set; }      //2020년 전통시장합계
            public int trp_sum { get; set; }	      //2020년 대중교통합계
            public int isld_sum { get; set; }		//2020년 도서공연등합계
            public int tot_sum { get; set; }		//2020년 총합계
            public int gnrl_mar_sum { get; set; }		//3월분 일반합계
            public int tdmr_mar_sum { get; set; }		//3월분 전통시장합계
            public int trp_mar_sum { get; set; }		//3월분 대중교통합계
            public int isld_mar_sum { get; set; }		//3월분 도서공연등합계
            public int tot_mar_sum { get; set; }		//3월분 총합계
            public int gnrl_aprl_sum { get; set; }		//4~7월 일반합계
            public int tdmr_aprl_sum { get; set; }     //4~7월 전통시장합계
            public int trp_aprl_sum { get; set; }      //4~7월 대중교통합계
            public int isld_aprl_sum { get; set; }     //4~7월 도서공연등합계
            public int tot_aprl_sum { get; set; }      //4~7월 총합계
            public int gnrl_jan_sum { get; set; }     //그 외 일반합계
            public int tdmr_jan_sum { get; set; } //그 외 전통시장합계
            public int trp_jan_sum { get; set; }		//그 외 대중교통합계
            public int isld_jan_sum { get; set; } //그 외 도서공연등합계
            public int tot_jan_sum { get; set; }		//그 외 총합계

            public 상품별반복[] 상품;


        }

        public struct 상품별반복
        {
            public string dat_cd { get; set; } //자료코드  
            public string use_place_cd { get; set; } //종류
            public int sum { get; set; } //공제대상금액합계

            public G208M[] 월별;

        }

        public struct G208M
        {
            public int amt { get; set; } //월별공제대상금액
            public string mm { get; set; } //공제대상월  
        }

        public void Execute(G208 entity)
        {
            try
            { 
            if (entity.인별 == null)
            {
                return;
            }

            int calYear = DateTime.Now.Year - 1; //연말정산 대상연도
            calYear = NTS_Reader.ycal_year; //테스트 년도

            string emp_no = ""; 

            /*foreach (var 인별 in entity.인별)
            {
                Dictionary<string, object> resultMap = ReadSql($"select * from QE023DT WHERE ycal_resi = fn_za010ms_03('{인별.resid}') and ycal_year = '{calYear}' and YCAL_RERA='0' ");
                if (resultMap.Count > 0)
                {
                    emp_no = resultMap["EMP_NO"].ToString(); //사번
                }
            }*/

            emp_no = NTS_Reader.emp_no;
                //전체합계컬럼 초기화
                //  executeSql($@"                                      
                //                      UPDATE QE020MS
                //                      SET YCAL_NTXD_4_4_AMT = 0
                //                         ,YCAL_NTXD_4_15_AMT = 0
                //                        ,YCAL_NTXD_4_16_AMT = 0
                //                         ,YCAL_NTXD_4_10_AMT = 0
                //                         ,YCAL_NTXD_4_6_AMT = 0
                //                         ,YCAL_NTXD_4_7_AMT = 0                                   
                //                      WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear}
                //              ");

                //개인별컬럼 초기화
              executeSql($@"                                      
                                UPDATE QE023DT
                                SET YCAL_CASH_3M_AMT = 0
                                   ,YCAL_CASH_7M_AMT = 0
                                   ,YCAL_CASH_AMT = 0 
                                WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear}
               ");


            foreach (var 인별 in entity.인별)
            {
                    Dictionary<string, object> resultMap = ReadSql($"select * from QE023DT WHERE emp_no = '{emp_no}' and ycal_year = '{calYear}' and ycal_resi = fn_za010ms_03('{인별.resid}')");
                    if (resultMap.Count > 0)
                    {
                        emp_no = resultMap["EMP_NO"].ToString(); //사번
                    }
                    else
                    {
                        continue;
                    }
                    //전체합계
                    executeSql($@"                                      
                                    UPDATE QE020MS
                                    SET YCAL_NTXD_4_4_AMT = YCAL_NTXD_4_4_AMT + {인별.gnrl_sum}
                                       ,YCAL_NTXD_4_15_AMT = YCAL_NTXD_4_15_AMT + {인별.isld_mar_sum}
                                       ,YCAL_NTXD_4_16_AMT = YCAL_NTXD_4_16_AMT + {인별.isld_aprl_sum}
                                       ,YCAL_NTXD_4_10_AMT = YCAL_NTXD_4_10_AMT + {인별.isld_jan_sum}
                                       ,YCAL_NTXD_4_6_AMT = YCAL_NTXD_4_6_AMT + {인별.tdmr_sum}
                                       ,YCAL_NTXD_4_7_AMT = YCAL_NTXD_4_7_AMT + {인별.trp_sum}             
                                    WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear}
                        ");


                //개인별
                executeSql($@"                                      
                                    UPDATE QE023DT
                                    SET YCAL_CASH_3M_AMT = YCAL_CASH_3M_AMT + {인별.gnrl_mar_sum}
                                       ,YCAL_CASH_7M_AMT = YCAL_CASH_7M_AMT  + {인별.gnrl_aprl_sum}
                                       ,YCAL_CASH_AMT = YCAL_CASH_AMT + {인별.gnrl_jan_sum} 
                                       ,YCAL_MART_3M_AMT = YCAL_MART_3M_AMT + {인별.tdmr_mar_sum}
                                       ,YCAL_MART_7M_AMT = YCAL_MART_7M_AMT + {인별.tdmr_aprl_sum}
                                       ,YCAL_MART_AMT = YCAL_MART_AMT + {인별.tdmr_jan_sum}                                   
                                       ,YCAL_TRAN_3M_AMT = YCAL_TRAN_3M_AMT + {인별.trp_mar_sum}       
                                       ,YCAL_TRAN_7M_AMT = YCAL_TRAN_7M_AMT + {인별.trp_aprl_sum}       
                                       ,YCAL_TRAN_AMT = YCAL_TRAN_AMT + {인별.trp_jan_sum}       
                                       ,YCAL_BOOK_3M_AMT = YCAL_BOOK_3M_AMT + {인별.isld_mar_sum}       
                                       ,YCAL_BOOK_7M_AMT = YCAL_BOOK_7M_AMT + {인별.isld_aprl_sum}       
                                       ,YCAL_BOOK_AMT = YCAL_BOOK_AMT + {인별.isld_jan_sum}       
                                    WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear} and YCAL_RESI=fn_za010ms_03('{인별.resid}')                                 
                    ");
            }

        }
         catch (Exception ex)
            {
                throw new Exception("현금영수증[G208] 처리 중 오류가 발생하였습니다.");
    }

}
    }
}
