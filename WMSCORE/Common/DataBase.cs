using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace WMSCORE.Common
{
	public enum TransactionType
	{
		Begin = 0,
		Commit = 1,
		Rollback = 2,
		NoTrans = 3
	}
	public enum ProviderType
	{
		OracleClient = 0
	}
	public class DataBase
	{
		private Int16 Num_Tries = 0;
		private static int intConnection;
		private Oracle.ManagedDataAccess.Client.OracleConnection OracleConn = null;
		private Oracle.ManagedDataAccess.Client.OracleTransaction OracleTrans = null;
		private DataSet Idataset = new DataSet();

		private static Type[] _connectionTypes = { typeof(Oracle.ManagedDataAccess.Client.OracleConnection) };
		private static Type[] _commandTypes = { typeof(Oracle.ManagedDataAccess.Client.OracleCommand) };
		private static Type[] _dataAdapterTypes = { typeof(Oracle.ManagedDataAccess.Client.OracleDataAdapter) };
		//private static Type[] _dataReaderTypes = { typeof(System.Data.OracleClient.OracleDataReader) };
		private static Type[] _dataParameterTypes = { typeof(Oracle.ManagedDataAccess.Client.OracleParameter) };
		//private static Type[] _transactionTypes = { typeof(Oracle.ManagedDataAccess.Client.OracleTransaction) };

		private ProviderType _provider = ProviderType.OracleClient;
		private Oracle.ManagedDataAccess.Client.OracleCommand ICommand = null;
		private Oracle.ManagedDataAccess.Client.OracleDataAdapter IDataAdapter = null;
		private Oracle.ManagedDataAccess.Client.OracleParameter IDataParameter = null;
		//private OracleDataReader IDataReader = null;

		public string[] ParamNames { get; private set; }
		public object[] Parameters { get; private set; }
		public DataTable _dtDestination { get; private set; }
		private Oracle.ManagedDataAccess.Client.OracleConnection OracleDAConn = null;
		private Oracle.ManagedDataAccess.Client.OracleTransaction OracleDATrans = null;
		private static Type[] _connectionTypesOracleDA = { typeof(Oracle.ManagedDataAccess.Client.OracleConnection) };
		private Int16 Num_TriesDAConn = 0;
		private static int intConnectionDAConn;
		public static string _Key = "$ierra@Her0Wm$";
		// public DataBase()
		// {
		//// OracleConnection OracleConn = new OracleConnection(ConfigurationManager.ConnectionStrings["Connectionstring"].ConnectionString);
		//ConnectionStringSettingsCollection settings = ConfigurationManager.ConnectionStrings;
		//OracleConn = CreateConnection(Decryption(settings[settings.Count - 1].ToString(), Global._Key));
		//OracleDAConn = CreateConnectionOracleDA(Decryption(settings[settings.Count - 1].ToString(), Global._Key));
		//}

		private static IConfigurationRoot _configuration;

		private readonly string _connectionString;

		public DataBase()
		{
			if (_configuration == null)
			{
				// Access configuration from wherever it's set up in your application
				_configuration = new ConfigurationBuilder()
					.AddJsonFile("appsettings.json")
					.Build();
			}

			_connectionString = _configuration.GetConnectionString("COREConnection");
			_connectionString = Decryption(_connectionString, _Key);
		}

		public static string Decryption(string CypherText, string key)
		{
			try
			{
				byte[] b = Convert.FromBase64String(CypherText);
				TripleDES des = CreateDES(key);
				ICryptoTransform ct = des.CreateDecryptor();
				byte[] output = ct.TransformFinalBlock(b, 0, b.Length);
				return Encoding.Unicode.GetString(output);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		static TripleDES CreateDES(string key)
		{
			try
			{
				MD5 md5 = new MD5CryptoServiceProvider();
				TripleDES des = new TripleDESCryptoServiceProvider();
				des.Key = md5.ComputeHash(Encoding.Unicode.GetBytes(key));
				des.IV = new byte[des.BlockSize / 8];
				return des;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public DataSet GetDatasetProc(string ProcName)
		{
			try
			{
				Idataset = new DataSet();

				Connect();
				ICommand = CreateCommand(ProcName, OracleConn, OracleTrans);
				ICommand.CommandType = CommandType.StoredProcedure;

				IDataParameter = new Oracle.ManagedDataAccess.Client.OracleParameter();
				IDataParameter.ParameterName = "RESULTDATA";
				IDataParameter.Direction = ParameterDirection.Output;
				IDataParameter.OracleDbType = OracleDbType.RefCursor;
				ICommand.Parameters.Add(IDataParameter);

				IDataAdapter = CreateDataAdapter(ICommand);
				Idataset.Clear();
				IDataAdapter.Fill(Idataset);
				OracleConn.Dispose();
				OracleConn.Close();
				return Idataset;
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}
		public string GetStringProc(string ProcName, string[] ParamNames1, object[] Parameters1)
		{
			ParamNames = ParamNames1;
			Parameters = Parameters1;

			Idataset = new DataSet();
			int ctr = 0;

			Connect();
			ICommand = CreateCommand(ProcName, OracleConn, OracleTrans);
			ICommand.CommandType = CommandType.StoredProcedure;
			for (ctr = 0; ctr < ParamNames.Length; ctr++)
			{
				IDataParameter = CreateDataParameter(ParamNames[ctr], Parameters[ctr]);
				ICommand.Parameters.Add(IDataParameter);
			}
			//Add OUT Parameter
			IDataParameter = new Oracle.ManagedDataAccess.Client.OracleParameter();
			IDataParameter.ParameterName = "RETMESSAGE";
			IDataParameter.Direction = ParameterDirection.Output;
			IDataParameter.OracleDbType = OracleDbType.Varchar2;
			IDataParameter.Size = 500;
			ICommand.Parameters.Add(IDataParameter);

			ICommand.ExecuteNonQuery();
			return ICommand.Parameters["RETMESSAGE"].Value.ToString();
		}

		public string GetStringProc(string ProcName)
		{
			Idataset = new DataSet();
			Connect();
			ICommand = CreateCommand(ProcName, OracleConn, OracleTrans);
			ICommand.CommandType = CommandType.StoredProcedure;

			//Add OUT Parameter
			IDataParameter = new Oracle.ManagedDataAccess.Client.OracleParameter();
			IDataParameter.ParameterName = "RETMESSAGE";
			IDataParameter.Direction = ParameterDirection.Output;
			IDataParameter.OracleDbType = OracleDbType.Varchar2;
			IDataParameter.Size = 500;
			ICommand.Parameters.Add(IDataParameter);

			ICommand.ExecuteNonQuery();
			return ICommand.Parameters["RETMESSAGE"].Value.ToString();
		}

		public DataSet GetDatasetProc(string ProcName, string[] ParamNames1, object[] Parameters1)
		{
			try
			{
				ParamNames = ParamNames1;
				Parameters = Parameters1;

				Idataset = new DataSet();
				int ctr = 0;

				Connect();
				ICommand = CreateCommand(ProcName, OracleConn, OracleTrans);
				ICommand.CommandType = CommandType.StoredProcedure;

				for (ctr = 0; ctr < ParamNames.Length; ctr++)
				{
					IDataParameter = CreateDataParameter(ParamNames[ctr], Parameters[ctr]);
					ICommand.Parameters.Add(IDataParameter);
				}

				IDataParameter = new Oracle.ManagedDataAccess.Client.OracleParameter();
				IDataParameter.ParameterName = "RESULTDATA";
				IDataParameter.Direction = ParameterDirection.Output;
				IDataParameter.OracleDbType = OracleDbType.RefCursor;
				ICommand.Parameters.Add(IDataParameter);

				IDataAdapter = CreateDataAdapter(ICommand);
				Idataset.Clear();
				if (OracleConn.State != ConnectionState.Open)
				{
					OracleConn.Open();
				}
				IDataAdapter.Fill(Idataset);
				OracleConn.Dispose();
				OracleConn.Close();
				return Idataset;
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}
		public DataSet GetDatasetProcTwoResultSet(string ProcName, string[] ParamNames1, object[] Parameters1)
		{
			ParamNames = ParamNames1;
			Parameters = Parameters1;

			Idataset = new DataSet();
			int ctr = 0;

			Connect();
			ICommand = CreateCommand(ProcName, OracleConn, OracleTrans);
			ICommand.CommandType = CommandType.StoredProcedure;
			for (ctr = 0; ctr < ParamNames.Length; ctr++)
			{
				IDataParameter = CreateDataParameter(ParamNames[ctr], Parameters[ctr]);
				ICommand.Parameters.Add(IDataParameter);
			}
			//--Return Message 1
			IDataParameter = new Oracle.ManagedDataAccess.Client.OracleParameter();
			IDataParameter.ParameterName = "RESULTDATA";
			IDataParameter.Direction = ParameterDirection.Output;
			IDataParameter.OracleDbType = OracleDbType.RefCursor;
			ICommand.Parameters.Add(IDataParameter);

			//--Return Message 2
			IDataParameter = new Oracle.ManagedDataAccess.Client.OracleParameter();
			IDataParameter.ParameterName = "RESULTDATA1";
			IDataParameter.Direction = ParameterDirection.Output;
			IDataParameter.OracleDbType = OracleDbType.RefCursor;
			ICommand.Parameters.Add(IDataParameter);

			IDataAdapter = CreateDataAdapter(ICommand);
			Idataset.Clear();
			IDataAdapter.Fill(Idataset);
			return Idataset;
		}

		public void OracleBulkCopyOperation(string strTableName, DataTable dtWMSData)
		{
			Oracle.ManagedDataAccess.Client.OracleBulkCopy bulkCopy = null;

			try
			{
				string strConnection = _configuration.GetConnectionString("COREConnection");
				strConnection = Decryption(strConnection, _Key);

				using (bulkCopy = new Oracle.ManagedDataAccess.Client.OracleBulkCopy(strConnection, OracleBulkCopyOptions.Default))
				{
					bulkCopy.DestinationTableName = strTableName;
					bulkCopy.WriteToServer(dtWMSData);
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Error during bulk copy operation", ex);
			}
			finally
			{
				if (bulkCopy != null)
				{
					bulkCopy.Close();
				}
			}
		}

		private Oracle.ManagedDataAccess.Client.OracleConnection CreateConnectionOracleDA(string connectionString)
		{
			Oracle.ManagedDataAccess.Client.OracleConnection conn = null;
			object[] args = { connectionString };
			try
			{
				conn = (Oracle.ManagedDataAccess.Client.OracleConnection)(Activator.CreateInstance(_connectionTypesOracleDA[System.Convert.ToInt32(_provider)], args));
			}
			catch (TargetInvocationException e)
			{
				throw new SystemException(e.InnerException.Message, e.InnerException);
			}
			return conn;
		}

		public void Connect()
		{
			try
			{
				// Create a new OracleConnection object with the connection string
				OracleConn = new OracleConnection(_connectionString);

				// Open the connection
				OracleConn.Open();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private Oracle.ManagedDataAccess.Client.OracleCommand CreateCommand(string cmdText, IDbConnection connection, IDbTransaction transaction)
		{
			try
			{
				Oracle.ManagedDataAccess.Client.OracleCommand cmd = new Oracle.ManagedDataAccess.Client.OracleCommand(cmdText);
				cmd.Connection = (OracleConnection)connection;
				cmd.Transaction = (OracleTransaction)transaction;
				return cmd;
			}
			catch (Exception ex)
			{
				throw new SystemException("Error creating OracleCommand: " + ex.Message, ex);
			}
		}

		private Oracle.ManagedDataAccess.Client.OracleParameter CreateDataParameter(string parameterName, object value)
		{
			Oracle.ManagedDataAccess.Client.OracleParameter param = null;
			object[] args = { parameterName, value };

			try
			{
				param = (Oracle.ManagedDataAccess.Client.OracleParameter)(Activator.CreateInstance(_dataParameterTypes[System.Convert.ToInt32(_provider)], args));
			}
			catch (TargetInvocationException e)
			{
				throw new SystemException(e.InnerException.Message, e.InnerException);
			}

			return param;
		}

		private Oracle.ManagedDataAccess.Client.OracleDataAdapter CreateDataAdapter(IDbCommand selectCommand)
		{
			try
			{
				Oracle.ManagedDataAccess.Client.OracleDataAdapter da = new Oracle.ManagedDataAccess.Client.OracleDataAdapter((Oracle.ManagedDataAccess.Client.OracleCommand)selectCommand);
				return da;
			}
			catch (Exception ex)
			{
				throw new SystemException("Error creating OracleDataAdapter: " + ex.Message, ex);
			}
		}

		private Oracle.ManagedDataAccess.Client.OracleConnection CreateConnection(string connectionString)
		{
			Oracle.ManagedDataAccess.Client.OracleConnection conn = null;
			object[] args = { connectionString };
			try
			{
				conn = (Oracle.ManagedDataAccess.Client.OracleConnection)(Activator.CreateInstance(_connectionTypes[System.Convert.ToInt32(_provider)], args));
			}
			catch (TargetInvocationException e)
			{
				throw new SystemException(e.InnerException.Message, e.InnerException);
			}
			return conn;
		}
		public void Close()
		{
			try
			{
				if (OracleConn.State == ConnectionState.Open)
				{
					OracleConn.Close();
					intConnection -= 1;
				}
			}
			catch (TargetInvocationException e)
			{
				throw new SystemException(e.InnerException.Message, e.InnerException);
			}
		}

		#region Handle Transaction Type
		public void HandleTransactionType(TransactionType Transactiontype)
		{
			Connect();
			try
			{
				if (Transactiontype == TransactionType.Begin)
				{
					OracleTrans = OracleConn.BeginTransaction();
				}
				else if (Transactiontype == TransactionType.Commit)
				{
					OracleTrans.Commit();
				}
				else if (Transactiontype == TransactionType.Rollback)
				{
					OracleTrans.Rollback();
				}
			}
			catch (TargetInvocationException e)
			{
				throw new SystemException(e.InnerException.Message, e.InnerException);
			}
		}
		#endregion Handle Transaction Type

		#region Handle Transaction Type - Bulk Copy
		public void HandleTransactionTypeOracleDA(TransactionType Transactiontype)
		{
			ConnectOracleDA();
			try
			{
				if (Transactiontype == TransactionType.Begin)
				{
					OracleDATrans = OracleDAConn.BeginTransaction();
				}
				else if (Transactiontype == TransactionType.Commit)
				{
					OracleDATrans.Commit();
				}
				else if (Transactiontype == TransactionType.Rollback)
				{
					OracleDATrans.Rollback();
				}
			}
			catch (TargetInvocationException e)
			{
				throw new SystemException(e.InnerException.Message, e.InnerException);
			}
		}

		private void ConnectOracleDA()
		{
			try
			{
				if (OracleDAConn.State == ConnectionState.Closed)
				{
					//OracleDAConn = CreateConnectionOracleDA(ConfigurationManager.ConnectionStrings["connectionString"].ToString()); // It is added here for contrlling the instance created in constructor.
					OracleDAConn.Open();
					intConnectionDAConn += 1;
					if (intConnectionDAConn > 1)
					{
						intConnectionDAConn = intConnectionDAConn - 1;
					}
				}
			}
			catch (TargetInvocationException e)
			{
				if (Num_TriesDAConn < 15)
				{
					Num_TriesDAConn += 5;
					ConnectOracleDA();
				}
				throw new SystemException(e.InnerException.Message, e.InnerException);
			}
			Num_TriesDAConn = 0;
		}

		public void CloseOracleDA()
		{
			try
			{
				if (OracleDAConn.State == ConnectionState.Open)
				{
					OracleDAConn.Close();
					intConnectionDAConn -= 1;
				}
			}
			catch (TargetInvocationException e)
			{
				throw new SystemException(e.InnerException.Message, e.InnerException);
			}
		}
		#endregion Handle Transaction Type


	}
}
