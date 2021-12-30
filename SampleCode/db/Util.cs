using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace NTS_Reader_CS.db
{
    class Util
    {
        public static string getIP()
        {
            IPHostEntry host = Dns.GetHostByName(Dns.GetHostName());
            string myip = host.AddressList[0].ToString();
            return myip;
        }
    }
}
