using System;
using System.Collections.Generic;
using System.Text;
using NTS_Reader_CS.db;

namespace NTS_Reader_CS.xml
{
    class O101 : Dbconn //건강보험료
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

            public int sum { get; set; } //총합계
            public int hi_yrs { get; set; } //건강보험연말정산금액
            public int ltrm_yrs { get; set; } //장기요양연말정산금액
            public int hi_ntf { get; set; } //건강보험(보수월액)고지금액합계
            public int ltrm_ntf { get; set; } //장기요양(보수월액)고지금액합계
            public int hi_pmt { get; set; } //건강보험(소득월액)납부금액합계
            public int ltrm_pmt { get; set; } //장기요양(소득월액)납부금액합계


            public O101M[] 월별;

        }

        public struct O101M
        {     
            public string mm { get; set; } //납입월
            public int hi_ntf { get; set; } //건강보험료(보수월액)고지금액
            public int ltrm_ntf { get; set; } //장기요양보험료(보수월액)고지금액
            public int hi_pmt { get; set; } //건강보험료(소득월액)납부금액
            public int ltrm_pmt { get; set; } //장기요양보험료(소득월액)납부금액
        }

        public void Execute(O101 entity)
        {
          try { 
            if (entity.인별 == null)
            {
                return;
            }
        }
            catch (Exception ex)
            {
                throw new Exception("건강보험료[O101] 처리 중 오류가 발생하였습니다.");
            }
        }
    }
}
