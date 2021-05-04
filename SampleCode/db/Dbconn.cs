using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OracleClient;
using System.Data;

namespace NTS_Reader_CS.db
{

    class Dbconn
    {
        public static OracleConnection conn;

        // DB 연결
        public bool ConnectionDB(string dbIp, string dbName, string dbId, string dbPw)
        {
            try
            {
                String connString = $"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={dbIp})(PORT=1521)))(CONNECT_DATA = (SID = {dbName})));User ID={dbId};Password={dbPw};";
                conn = new OracleConnection(connString);
            }catch(Exception ex)
            {

            }
            

            return true;
        }


        public Dictionary<string, object> ReadSql(string sql)
        {

            Dictionary<string, object> resultMap = new Dictionary<string, object>();
            OracleDataReader dr = null;

            //재시작 문자 발송
            try
            {
                conn.Open();

                using (OracleCommand cmd = new OracleCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;

                    dr = cmd.ExecuteReader();                    
                }

                while (dr.Read())
                {
                    for(int lp = 0; lp < dr.FieldCount; lp++)
                    {
                        resultMap.Add(dr.GetName(lp), dr.GetValue(lp));
                    }
                    
                }
                    
            }
            catch (Exception ex) // catches any other error
            {

            }
            finally
            {
                // Don't need conn.Close() because the using takes care of it.
                conn.Close();
            }
            return resultMap;
        }

        //쿼리 실행
        public void executeSql(String sql)
        {          
            try
            {
                conn.Open();

                using (OracleCommand cmd = new OracleCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex) 
            {

            }
            finally
            {                
                conn.Close();
            }
        }

        public virtual void Execute()
        {

        }


    }
}
