using System.Data;
using System.Data.SqlClient;

namespace AopMethodInterception.Core
{
    public class DbCommand
    {
       
        public static SqlCommand GetCommand()
        {
            return new SqlCommand
            {
                Connection = SqlConnectionContext.GetOpenSqlConnection(),
                Transaction = SqlConnectionContext.GetTransaction()
            };
        }

        public static SqlCommand GetCommand(string storedProcedureName)
        {
            return new SqlCommand
            {
                Connection = SqlConnectionContext.GetOpenSqlConnection(),
                Transaction = SqlConnectionContext.GetTransaction(),
                CommandType = CommandType.StoredProcedure,
                CommandText = storedProcedureName
            };
        }
    }
}
