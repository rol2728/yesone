using System;
using System.Collections.Generic;
using System.Text;
using NTS_Reader_CS.db;

namespace NTS_Reader_CS.xml
{
    class D101 : Dbconn //개인연금저축
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
            public string trade_nm { get; set; } //기관명
            public string acc_no { get; set; } //계좌/증권번호            

            public string start_dt { get; set; } //계약시작일
            public string end_dt { get; set; } //계약종료일
            public string com_cd { get; set; } //금융기관코드
            public int sum { get; set; } //납입금액계

            public D101M[] 월별;

        }

        public struct D101M
        {
            public int amt { get; set; } //월별납입금액
            public string mm { get; set; } //납입월
            public string fix_cd { get; set; } //확정구분코드
        }

        public void Execute(D101 entity)
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
            int 전체합계 = 0;
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

                foreach (var 인별 in entity.인별)
            {                

                foreach (var data in 인별.상품)
                {
                    전체합계 += data.sum;

                    Dictionary<string, object> resultMap = ReadSql($@" SELECT NVL((SELECT MAX(SEQ_NO) +1 FROM QE024MS WHERE YCAL_YEAR = '{calYear}' AND EMP_NO = '{emp_no}'),1) AS SEQ_NO FROM DUAL");
                    시퀀스 = Convert.ToInt32(resultMap["SEQ_NO"].ToString()); //사번                     
                   

                    //테이블 입력 (QE024MS)
                    executeSql($@"                                      
                                    INSERT INTO QE024MS(YCAL_YEAR, EMP_NO, SEQ_NO,  ANNU_RENO, ANNU_CODE, ANNU_NAME, ANNU_ACCO, ANNU_AMT, U_EMP_NO, U_DATE, U_IP)
                                           VALUES('{calYear}', {emp_no},{시퀀스}, '21', '{data.com_cd}', '{data.trade_nm}', '{data.acc_no}', {data.sum}, '{emp_no}', sysdate, '{Util.getIP()}')
                               ");
                }
            }

            //전체합계
            executeSql($@"                                      
                                UPDATE QE020MS
                                SET YCAL_NTXD_1_AMT = nvl(YCAL_NTXD_1_AMT,0)+{전체합계}  
                                   , U_DATE =SYSDATE
                                WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear}
                        ");

        }
            catch (Exception ex)
            {
                throw new Exception("개인연금저축[D101] 처리 중 오류가 발생하였습니다.");
            }
        }
    }
}
