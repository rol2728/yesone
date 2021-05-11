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
            // 1. 본인확인
            foreach (var 인별 in entity.인별)
            {
                if (인별.resid == "test")
                {
                    string tes = "Test";
                }


            }

            // 2.0 본인인 경우 
            // 2.1 본인이 아니며 초중고 EDU_TIP = 2,3,4 인경우            
            // 2.1 본인이 아니며 대학교 EDU_TIP = 5,6 인경우
            // 2.1 본인이 아니며 취학전 EDU_TIP = 1 인경우
            // 2.1 본인이 아니며 장애 EDU_TIP = J,K 인경우

        }
    }
}
