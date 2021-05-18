using System;
using System.Collections.Generic;
using System.Text;
using NTS_Reader_CS.db;

namespace NTS_Reader_CS.xml
{
    class J501 : Dbconn //월세액
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
            public string lsor_no { get; set; } //임대인번호
            public string lsor_nm { get; set; } //임대인명          

            public string start_dt { get; set; } //임대차시작일자
            public string end_dt { get; set; } //임대차종료일자
            public string adr { get; set; } //계약서상 주소지
            public string area { get; set; } //계약면적
            public string typeCd { get; set; } //유형코드

            public int sum { get; set; } //연간합계액

            public J501M[] 월별;

        }

        public struct J501M
        {
            public int amt { get; set; } //월별상환액
            public string mm { get; set; } //상환월
            public string fix_cd { get; set; } //확정구분코드
        }

        public void Execute(J501 entity)
        {
            if (entity.인별 == null)
            {
                return;
            }

            int calYear = DateTime.Now.Year - 1; //연말정산 대상연도
            calYear = 2021; //테스트 년도

            string emp_no = ""; ;

            int 월세_전체합계 = 0;
            int 시퀀스 = 0;

            foreach (var 인별 in entity.인별)
            {
                Dictionary<string, object> resultMap = ReadSql($"select * from QE023DT WHERE ycal_resi = fn_za010ms_03('{인별.resid}') and ycal_year = '{calYear}' and YCAL_RERA='0' ");
                if (resultMap.Count > 0)
                {
                    emp_no = resultMap["EMP_NO"].ToString(); //사번
                }
            }

            //벤처기업투자신탁
            executeSql($@" DELETE FROM QE027MS WHERE EMP_NO='{emp_no}' and YCAL_YEAR={calYear} ");

            //시퀀스 번호가져오기
            Dictionary<string, object> resultMap2 = ReadSql($"select MAX(NVL(SEQ_NO,0))+1 AS SEQ_NO from QE027MS WHERE emp_no = '{emp_no}' and ycal_year = '{calYear}' GROUP BY EMP_NO ");
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
                //   월세_전체합계 = 0;

                foreach (var data in 인별.상품)
                {
                    if (data.dat_cd == "G0037" )
                    {
                        월세_전체합계 += data.sum;

                        //월세 테이블 입력 (QE027MS)
                        executeSql($@"                                      
                                    INSERT INTO QE024MS(YCAL_YEAR, EMP_NO, SEQ_NO, MONT_NAME, MONT_RESI_NO,
                                                        MONT_ADDRESS, MONT_FROM_DATE, MONT_TO_DATE, MONT_AMT,MONT_DAMT
                                                        U_EMP_NO, U_DATE, U_IP, HOUSE_TYPE, HOUSE_SPACE)
                                           VALUES('{calYear}', {emp_no},{시퀀스}, {data.lsor_nm},{data.lsor_no},{data.adr}
                                                  {data.start_dt},{data.end_dt},{data.sum}, {data.sum}, 0, '국세청', sysdate, '10.10.11.104',{data.typeCd},{data.area}
                              ");
                        //  시퀀스 += 1;
                    }
                    시퀀스 += 1;
                }

            }

            //전체 합계금액 수정
                executeSql($@"                                      
                                     UPDATE QE020MS
                                       SET YCAL_SPCD_4_6_MONT_AMT = {월세_전체합계}
                                           , U_DATE =SYSDATE
                                     WHERE EMP_NO = '{emp_no}' and YCAL_YEAR={calYear}                                 
                        ");

        }
    }
}
