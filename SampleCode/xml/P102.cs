using System;
using System.Collections.Generic;
using System.Text;
using NTS_Reader_CS.db;

namespace NTS_Reader_CS.xml
{
    class P102 : Dbconn //국민연금보험료
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

            public int sp_ntf { get; set; } //직장가입자소급고지금액합계
            public int spym { get; set; } //추납보험료납부금액
            public int jlc { get; set; } //실업크레딧납부금액
            public int ntf { get; set; } //직장가입자 고지금액 합계
            public int pmt { get; set; } //지역가입자 등 납부금액 합계

            public P102M[] 월별;

        }

        public struct P102M
        {     
            public string mm { get; set; } //납입월
            public int wrkp_ntf { get; set; } //직장가입자 고지금액
            public int rgn_pmt { get; set; } //지역가입자 등 납부금액       
        }

        public void Execute(P102 entity)
        {
          try
            { 
            if (entity.인별 == null)
            {
                return;
            }
        }
            catch (Exception ex)
            {
                throw new Exception("국민연금보험료[P102] 처리 중 오류가 발생하였습니다.");
            }
        }
    }
}
