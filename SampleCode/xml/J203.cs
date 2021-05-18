using System;
using System.Collections.Generic;
using System.Text;
using NTS_Reader_CS.db;

namespace NTS_Reader_CS.xml
{
    class J203 : Dbconn//주택임차차입금
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

            public string lend_kd { get; set; } //대출종류
            public string house_take_dt { get; set; } //주택취득일
            public string mort_setup_dt { get; set; } //저당권설정일
            public string start_dt { get; set; } //최초차입일
            public string end_dt { get; set; } //최종상환예정일
            public int repay_years { get; set; } //상환기간연수
            public string lend_goods_nm { get; set; } //상품명
            public int debt { get; set; } //차입금
            public int fixed_rate_debt { get; set; } //고정금리차입금
            public int not_defer_debt { get; set; } //비거치식상환차입금
            public int this_year_rede_amt { get; set; } //당해년 원금상환액
            public int sum { get; set; } //연간합계액
            public int ddct { get; set; } //소득공제대상액

            public J203M[] 월별;

        }

        public struct J203M
        {
            public int amt { get; set; } //월별납입금액
            public string mm { get; set; } //납입월
            public string fix_cd { get; set; } //확정구분코드
        }

        public void Execute(J203 entity)
        {
            int calYear = DateTime.Now.Year - 1; //연말정산 대상연도
            calYear = 2021; //테스트 년도

            string emp_no = ""; ;            

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
                                SET YCAL_SPCD_4_4_I_REFN_AMT = 0        
                                   ,YCAL_SPCD_4_3_I_REFN_AMT = 0        
                                   ,YCAL_SPCD_4_5_I_REFN_AMT = 0        
                                   ,YCAL_SPCD_4_15_1_I_REFN_AMT = 0
                                   ,YCAL_SPCD_4_15_2_I_REFN_AMT = 0
                                   ,YCAL_SPCD_4_15_3_I_REFN_AMT = 0
                                   ,YCAL_SPCD_4_15_4_I_REFN_AMT = 0
                                   ,YCAL_SPCD_4_6_I_REFN_AMT = 0
                                   ,YCAL_SPCD_4_7_I_REFN_AMT = 0                                   
                                WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear}
                        ");


            foreach (var 인별 in entity.인별)
            {

                foreach (var data in 인별.상품)
                {
                    //2011년 이전인 경우
                    if(data.start_dt.CompareTo("20111231") <= 0)
                    {
                        //상환기간
                        if (data.repay_years >= 10 && data.repay_years < 15)
                        {
                            executeSql($@"                                      
                                                UPDATE QE020MS
                                                SET YCAL_SPCD_4_4_I_REFN_AMT = YCAL_SPCD_4_4_I_REFN_AMT + {data.ddct}
                                                WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear}
                                 ");
                        }
                        else if (data.repay_years >= 15 && data.repay_years < 30)
                        {
                            executeSql($@"                                      
                                                UPDATE QE020MS
                                                SET YCAL_SPCD_4_3_I_REFN_AMT = YCAL_SPCD_4_3_I_REFN_AMT + {data.ddct}
                                                WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear}
                                 ");
                        }
                        else if (data.repay_years >= 30)
                        {
                            executeSql($@"                                      
                                                UPDATE QE020MS
                                                SET YCAL_SPCD_4_5_I_REFN_AMT = YCAL_SPCD_4_5_I_REFN_AMT + {data.ddct}
                                                WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear}
                                 ");
                        }
                    }
                    //2012년 ~ 2014년
                    else if (data.start_dt.CompareTo("20120101") >= 0 && data.start_dt.CompareTo("20141231") <= 0)
                    {
                        if (data.fixed_rate_debt > 0 || data.not_defer_debt > 0)
                        {
                            executeSql($@"                                      
                                                UPDATE QE020MS
                                                SET YCAL_SPCD_4_6_I_REFN_AMT = YCAL_SPCD_4_6_I_REFN_AMT + {data.ddct}
                                                WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear}
                                 ");
                        }
                        else
                        {
                            executeSql($@"                                      
                                                UPDATE QE020MS
                                                SET YCAL_SPCD_4_7_I_REFN_AMT = YCAL_SPCD_4_7_I_REFN_AMT + {data.ddct}
                                                WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear}
                                 ");
                        }
                    }
                    //2015년 이후인경우
                    else if (data.start_dt.CompareTo("20150101") >= 0)
                    {
                        //상환기간
                        if(data.repay_years >= 15)
                        {
                            if(data.fixed_rate_debt > 0 && data.not_defer_debt > 0)
                            {                               
                                executeSql($@"                                      
                                                UPDATE QE020MS
                                                SET YCAL_SPCD_4_15_1_I_REFN_AMT = YCAL_SPCD_4_15_1_I_REFN_AMT + {data.ddct}
                                                WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear}
                                 ");
                            }
                            else if(data.fixed_rate_debt > 0 || data.not_defer_debt > 0)
                            {                                
                                executeSql($@"                                      
                                                UPDATE QE020MS
                                                SET YCAL_SPCD_4_15_2_I_REFN_AMT = YCAL_SPCD_4_15_2_I_REFN_AMT + {data.ddct}
                                                WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear}
                                 ");
                            }
                            else
                            {                                
                                executeSql($@"                                      
                                                UPDATE QE020MS
                                                SET YCAL_SPCD_4_15_3_I_REFN_AMT = YCAL_SPCD_4_15_3_I_REFN_AMT + {data.ddct}
                                                WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear}
                                 ");
                            }
                        }
                        //상환기간이 10 ~ 15년인 경우
                        else if (data.repay_years >= 10 &&  data.repay_years < 15)
                        {
                            if (data.fixed_rate_debt > 0 && data.not_defer_debt > 0)
                            {
                                executeSql($@"                                      
                                                UPDATE QE020MS
                                                SET YCAL_SPCD_4_15_4_I_REFN_AMT = YCAL_SPCD_4_15_4_I_REFN_AMT + {data.ddct}
                                                WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear}
                                 ");
                            }
                        }


                    }
                }
            }


        }
    }
}
