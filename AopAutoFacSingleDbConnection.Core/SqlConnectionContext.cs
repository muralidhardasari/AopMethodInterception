using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace AopMethodInterception.Core
{
    public class SqlConnectionContext 
    {
        public const string ConnectionKey = "SqlConnection";
        public const string TransactionKey = "SqlTransaction";
        private static readonly object LockObject = new object();

        public SqlConnectionContext()
        {
            lock (LockObject)
            {
                if (WebCallContext.GetData(ConnectionKey) == null)
                {
                    WebCallContext.SetData(ConnectionKey, new SqlConnection(ConnectionString));
                }
                else
                {
                    throw new Exception("There is already a sql connection");
                }
            }
        }

        private static string ConnectionString
        {
            get
            {
                return WebCallContext.GetData("DBConnection") == null ? ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString : WebCallContext.GetData("DBConnection").ToString();
            }
        }

        public static SqlConnection GetDBConnection()
        {
            return SqlConnection(ConnectionKey);
        }

        public static SqlTransaction GetTransaction()
        {
            return Transaction(ConnectionKey, TransactionKey);
        }


        public void Dispose()
        {
            FreeTransaction();
        }

        public void Commit()
        {
            var transaction = (SqlTransaction)WebCallContext.GetData(TransactionKey);
            if (transaction != null)
            {
               transaction.Commit();
             }
        }

        public void Flush()
        {
        }

        

        private static SqlTransaction Transaction(string connectionKey, string transactionKey)
        {
            var transaction = WebCallContext.GetData(transactionKey);
            if (transaction == null)
            {
                OpenSqlConnection(connectionKey, transactionKey);
                transaction = WebCallContext.GetData(transactionKey);
            }

            return (SqlTransaction)transaction;
        }

        private static SqlConnection SqlConnection(string connectionKey)
        {
            var sqlConnection = WebCallContext.GetData(connectionKey);
            if (sqlConnection == null)
            {
                throw new Exception("No sql connection available");
            }

            var connection = (SqlConnection)sqlConnection;
            return connection;
        }

      private static SqlConnection OpenSqlConnection(string connectionKey, string transactionKey)
        {
            var connection = SqlConnection(connectionKey);
            if (connection.State != ConnectionState.Open)
            {
                try
                {
                    connection.Open();
                }
                catch (NullReferenceException exc)
                {
                    throw new Exception("OpenConnectionException", exc);
                }
                catch (InvalidOperationException exc)
                {
                    throw new Exception("OpenConnectionException", exc);
                }

                var transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);
                WebCallContext.SetData(transactionKey, transaction);
            }

            return connection;
        }

        public static SqlConnection GetOpenSqlConnection()
        {
            return OpenSqlConnection(ConnectionKey, TransactionKey);
        }

        private static void FreeTransaction()
        {
            WebCallContext.FreeNamedDataSlot(TransactionKey);
            WebCallContext.FreeNamedDataSlot(ConnectionKey);
        }
    }
}
