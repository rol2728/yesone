using System;
using System.Collections.Generic;
using System.Text;
using NTS_Reader_CS.db;

namespace NTS_Reader_CS.xml
{
    class J401 : Dbconn //목돈안드는전세이자상환액
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

            public int lend_loan_amt { get; set; } //대출원금
            public string lend_dt { get; set; } //대출일자 
            public int sum { get; set; } //연간합계액

            public J301M[] 월별;

        }

        public struct J301M
        {
            public int amt { get; set; } //월별상환액
            public string mm { get; set; } //상환월
            public string fix_cd { get; set; } //확정구분코드
        }

        public void Execute(J401 entity)
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
            int 전체합계 = 0;


              /*foreach (var 인별 in entity.인별)
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

                foreach (var data in 인별.상품)
                {
                    전체합계 += data.sum;             
                }
            }

            //전체합계
            executeSql($@"                                      
                                UPDATE QE020MS
                                SET YCAL_NTXD_9_AMT = {전체합계} 
                                  , U_DATE =SYSDATE
                                  , U_IP ='{Util.getIP()}'
                                  , U_EMP_N0='{emp_no}'
                                WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear}
                        ");


        }
      catch (Exception ex)
            {
                throw new Exception("목돈 안드는 전세 이자상환액[401] 처리 중 오류가 발생하였습니다.");
         }

      }
    }
}
