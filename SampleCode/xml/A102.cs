using System;
using System.Collections.Generic;
using System.Text;
using NTS_Reader_CS.db;

namespace NTS_Reader_CS.xml
{

    class A102 : Dbconn
    {
        public String form_cd {get;set;}// 서식코드 
        public 인별반복[] 인별;

        public struct 인별반복
        {

            public string resid { get; set; }//주민등록번호
            public string name { get; set; } //성명

            public 상품별반복[] 상품;            
        }

        public struct 상품별반복
        {
            public string dat_cd { get; set; }//자료코드
            public string busnid { get; set; } //사업자번호
            public string trade_nm { get; set; } //상호
            public string acc_no { get; set; } //증권번호            

            public string good_nm { get; set; } //보험종료
            public string insu1_resid { get; set; } //주민등록번호_주피보험자
            public string insu1_nm { get; set; } //성명_주피보험자
            public string insu2_resid_1 { get; set; } //주민등록번호_종피보험자_1
            public string insu2_nm_1 { get; set; } //성명_종피보험자_1
            public string insu2_resid_2 { get; set; } //주민등록번호_종피보험자_2
            public string insu2_nm_2 { get; set; } //성명_종피보험자_2
            public string insu2_resid_3 { get; set; }  //주민등록번호_종피보험자_3
            public string insu2_nm_3 { get; set; } //성명_종피보험자_3
            public int sum { get; set; } //납입금액계      


            public A102M[] 월별;

        }
        
        public struct A102M
        {
            public int amt { get; set; } //월별납입금액
            public string mm { get; set; } //납입월
            public string fix_cd { get; set; } //확정구분코드
        }

        public void Execute(A102 entity)
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

                int 보장성_개인별합계 = 0;
                int 보장성_전체합계 = 0;
                int 장애인보장성_개인별합계 = 0;
                int 장애인보장성_전체합계 = 0;


                foreach (var 인별 in entity.인별)
                {
                    보장성_개인별합계 = 0;
                    장애인보장성_개인별합계 = 0;

                    Dictionary<string, object> resultMap = ReadSql($"select * from QE023DT WHERE ycal_resi = fn_za010ms_03('{인별.resid}') and ycal_year = '{calYear}' and YCAL_RERA='0' ");
                    if (resultMap.Count > 0)
                    {
                        emp_no = resultMap["EMP_NO"].ToString(); //사번
                    }

                    foreach (var data in 인별.상품)
                    {
                        if (data.dat_cd == "G0001")
                        {
                            보장성_개인별합계 += data.sum;
                            executeSql($@"                                      
                                     UPDATE QE023DT
                                       SET YCAL_INSU_AMT = {보장성_개인별합계}
                                     WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear} and YCAL_RESI=fn_za010ms_03('{인별.resid}')               
                        ");

                        }
                        else if (data.dat_cd == "G0002")
                        {
                            장애인보장성_개인별합계 += data.sum;
                            executeSql($@"                                      
                                     UPDATE QE023DT
                                       SET YCAL_INSU_21_AMT = {장애인보장성_개인별합계}
                                     WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear} and YCAL_RESI=fn_za010ms_03('{인별.resid}')                                   
                        ");
                        }
                    }
                    보장성_전체합계 += 보장성_개인별합계;
                    장애인보장성_전체합계 += 장애인보장성_개인별합계;

                }

                //전체 합계금액 수정
                executeSql($@"                                      
                                     UPDATE QE020MS
                                       SET YCAL_SPCD_1_GINS_1_AMT = {보장성_전체합계}
                                     WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear}                                 
                        ");

                //전체 합계금액 수정
                executeSql($@"                                      
                                     UPDATE QE020MS
                                       SET YCAL_SPCD_1_GINS_OBS_AMT = {장애인보장성_전체합계}
                                     WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear}                                    
                        ");
            }
            catch(Exception ex)
            {
                throw new Exception("보장성보험[A102] 처리 중 오류가 발생하였습니다.");
            }
            
        }

    }
}
