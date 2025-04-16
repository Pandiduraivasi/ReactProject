using System.Data;

namespace WMSCORE.Common
{
	public enum Transaction
	{
		Begin = 0,
		Commit = 1,
		Rollback = 2,
		None = 3
	}
	public class DataAccess
	{
		DataBase _dbObj = new DataBase();
		DataSet _ds = new DataSet();
		static string _ScreenID = string.Empty;
		#region Handle Transaction

		public void HandleTransaction(Transaction Transaction)
		{
			if (Transaction == Transaction.Begin)
			{
				_dbObj.HandleTransactionType(TransactionType.Begin);
			}
			else if (Transaction == Transaction.Commit)
			{
				_dbObj.HandleTransactionType(TransactionType.Commit);
			}
			else if (Transaction == Transaction.Rollback)
			{
				_dbObj.HandleTransactionType(TransactionType.Rollback);
			}
		}

		public string ScreenID
		{
			get { return _ScreenID; }
			set { _ScreenID = value; }
		}

		#endregion Handle Transaction

		#region DataAccess
		public DataAccess()
		{
		}

		public DataAccess(string ScreenID)
		{

		}

		public DataAccess(Transaction Transaction)
		{
			HandleTransaction(Transaction);
		}
		#endregion DataAccess

		public void Close()
		{
			if (_dbObj != null) _dbObj.Close();
		}


		#region UsersLogin
		public DataSet LoadMenu(object[] _parameterValues)
		{
			try
			{
				string[] _parameterNames = { "V_TE_IP", "V_US_ID", "V_SC_TERTYPE", "V_SC_STATUS", "V_MO_STATUS" };
				_ds = _dbObj.GetDatasetProcTwoResultSet("LOADMENU", _parameterNames, _parameterValues);
				return _ds;
			}
			catch
			{
				throw;
			}
			finally
			{
				if (_ds != null) { _ds.Dispose(); }
			}
		}
		#endregion UsersLogin


		#region Privilege
		public string GetAccessRights(object[] _parameterValues)
		{
			try
			{
				string _result;
				string[] _parameterNames = { "V_US_ID", "V_SC_ID" };
				_result = _dbObj.GetStringProc("GETSCRPRIVILEGE", _parameterNames, _parameterValues);
				return _result;
			}
			catch
			{
				throw;
			}
		}

		#endregion

		#region Terminal

		public DataSet GetTerminal(object[] _parameterValues)
		{
			try
			{
				string[] _parameterNames = { "V_TE_ID", "V_MODE", "V_TE_IP", "V_TE_TYPE", "V_TE_VERSION" };
				_ds = _dbObj.GetDatasetProc("GETTERMINAL", _parameterNames, _parameterValues);
				return _ds;
			}
			catch
			{
				throw;
			}
			finally
			{
				if (_ds != null) { _ds.Dispose(); }
			}
		}

		#endregion

		#region Configuration

		public DataSet GetConfiguration()
		{
			try
			{
				_ds = _dbObj.GetDatasetProc("GETCONFIGURATION");
				return _ds;
			}
			catch
			{
				throw;
			}
			finally
			{
				if (_ds != null) { _ds.Dispose(); }
			}
		}

		#endregion

		#region Check Password

		public string CheckPassword(object[] parameterValues)
		{
			try
			{
				string[] parameterNames = { "V_US_ID", "V_US_NAME", "V_OLDPWD", "V_NEWPWD", "V_MODE" };
				return _dbObj.GetStringProc("CHECKPASSWORD", parameterNames, parameterValues);
			}
			catch
			{
				throw;
			}
		}

		public DataSet CheckTerminalTrace(object[] ParamValues)
		{
			string[] _parameterNames = { "V_TT_TE", "V_TT_USID", "V_TT_TYPE", "V_MODE" };
			_ds = _dbObj.GetDatasetProc("CheckTerminalTrace", _parameterNames, ParamValues);
			return _ds;
		}
		public DataSet CheckTerminalTraceLogin(object[] ParamValues)
		{
			string[] _parameterNames = { "V_TT_TE", "V_TT_USCODE", "V_TT_TYPE", "V_MODE" };
			_ds = _dbObj.GetDatasetProc("CHECKTERMINALTRACELOGIN", _parameterNames, ParamValues);
			return _ds;
		}
		public string SAVETerminalTrace(object[] ParamValues)
		{
			try
			{
				string[] ParamNames = { "V_TT_TE", "V_TT_USID", "V_TT_SC", "V_TT_TYPE", "V_UPDATEDUSER", "V_MODE" };
				return _dbObj.GetStringProc("SAVETerminalTrace", ParamNames, ParamValues);
			}
			catch
			{
				throw;
			}
		}
		public DataSet GetTerminalID(object[] _parameterValues)
		{
			try
			{
				string[] _parameterNames = { "V_TE_IP" };
				_ds = _dbObj.GetDatasetProc("GetTerminalID", _parameterNames, _parameterValues);
				return _ds;
			}
			catch
			{
				throw;
			}
			finally
			{
				if (_ds != null) { _ds.Dispose(); }
			}
		}
		public DataSet GetTerminalIP(object[] _parameterValues)
		{
			try
			{
				string[] _parameterNames = { "V_TE_ID", "V_US_ID" };
				_ds = _dbObj.GetDatasetProc("GetTerminalIP", _parameterNames, _parameterValues);
				return _ds;
			}
			catch
			{
				throw;
			}
			finally
			{
				if (_ds != null) { _ds.Dispose(); }
			}
		}
		public DataSet GetTerminalDetails(object[] _parameterValues)
		{
			try
			{
				string[] _parameterNames = { "V_TE_IP", "V_TE_TYPE", "V_MODE" };
				_ds = _dbObj.GetDatasetProc("GETTERMINALDETAILS", _parameterNames, _parameterValues);
				return _ds;
			}
			catch
			{
				throw;
			}
			finally
			{
				if (_ds != null) { _ds.Dispose(); }
			}
		}

		public DataSet GetUser(object[] _parameterValues)
		{
			try
			{
				string[] _parameterNames = { "V_FromPage", "V_US_ID", "V_US_STATUS" };
				_ds = _dbObj.GetDatasetProc("GetUser", _parameterNames, _parameterValues);
				return _ds;
			}
			catch
			{
				throw;
			}
			finally
			{
				if (_ds != null) { _ds.Dispose(); }
			}
		}

		public string SaveUser(object[] _parameterValues)
		{
			try
			{
				string _Result;
				string[] _parameterNames = { "V_US_CODE", "V_US_NAME", "V_US_PASSWORD", "V_US_STATUS", "V_US_EFFFROM", "V_US_EFFTO", "V_US_INCHARGEID", "V_US_MAILID", "V_US_REMARKS", "V_US_DPTCODE", "V_UPDATEDUSER", "V_MODE", "V_MANPOWERVENDOR" };
				_Result = _dbObj.GetStringProc("SAVEUSER", _parameterNames, _parameterValues);
				return _Result;
			}
			catch
			{
				throw;
			}
		}
		#endregion

		#region Log
		public string SaveSystemErrorLog(object[] _parameterValues)
		{
			string _Result = string.Empty;
			try
			{
				string[] _parameterNames = { "V_SYL_MSG", "V_SYL_TE", "V_SYL_PRG", "V_SYL_STATUS", "V_SYL_CONT", "V_USER" };
				_Result = _dbObj.GetStringProc("SAVESYSTEMERRORLOG", _parameterNames, _parameterValues);
				return _Result;
			}
			catch
			{
				throw;
			}
			finally
			{
				if (_Result != null) _Result = string.Empty;
			}
		}

		public void SaveOperationLog(object[] _parameterValues)
		{
			try
			{
				string[] _parameterNames = { "V_OPL_SC", "V_OPL_TE", "V_OPL_CONT", "V_OPL_RESULT", "V_USER" };
				_dbObj.GetStringProc("SAVEOPERATIONLOG", _parameterNames, _parameterValues);
			}
			catch
			{
				throw;
			}
		}
		#endregion

		#region ListValue

		public DataSet FillOrderType(object[] _parameterValues)
		{
			try
			{
				string[] _parameterNames = { "V_LV_CODE", "V_MODE" };
				_ds = _dbObj.GetDatasetProc("FillOrderType", _parameterNames, _parameterValues);
				return _ds;
			}
			catch
			{
				throw;
			}
			finally
			{
				if (_ds != null) { _ds.Dispose(); }
			}
		}
		public DataSet FILLDropDowns(object[] _parameterValues)
		{
			try
			{
				string[] _parameterNames = { "V_LV_CODE", "V_SELVAL", "V_AR_ID" };
				_ds = _dbObj.GetDatasetProc("FILLDropDowns", _parameterNames, _parameterValues);
				return _ds;
			}
			catch
			{
				throw;
			}
			finally
			{
				if (_ds != null) { _ds.Dispose(); }
			}
		}
		#endregion
		#region GetSettings
		// Modified by Sebastian.R on 06/01/2015.
		public string GETSETTINGS(object[] _parameterValues)
		{
			try
			{
				string[] _parameterNames = { "V_MODE" };
				return _dbObj.GetStringProc("GETSETTINGS", _parameterNames, _parameterValues);
			}
			catch
			{
				throw;
			}
		}
		#endregion

		public DataSet GetArchieveConfigPrivilege(object[] ParamValues)
		{
			try
			{
				string[] _parameterNames = { "V_MODE", "V_US_ID", "V_TE_ID" };
				_ds = _dbObj.GetDatasetProc("GetArchieveConfigPrivilege", _parameterNames, ParamValues);
				return _ds;
			}

			catch
			{
				throw;
			}
		}

		public DataSet GetUserLogin(object[] _parameterValues)
		{
			try
			{
				string[] _parameterNames = { "V_FromPage", "V_US_CODE", "V_US_STATUS" };
				_ds = _dbObj.GetDatasetProc("GetUserLogin", _parameterNames, _parameterValues);
				return _ds;
			}
			catch
			{
				throw;
			}
			finally
			{
				if (_ds != null) { _ds.Dispose(); }
			}
		}

		public DataSet GetLoginTerminalIP(object[] _parameterValues)
		{
			try
			{
				string[] _parameterNames = { "V_TE_ID", "V_US_CODE" };
				_ds = _dbObj.GetDatasetProc("GetLoginTerminalIP", _parameterNames, _parameterValues);
				return _ds;
			}
			catch
			{
				throw;
			}
			finally
			{
				if (_ds != null) { _ds.Dispose(); }
			}
		}

		#region "Printer Type Master"
		public string SavePrinterType(object[] _parameterValues)
		{
			try
			{
				string _result;
				string[] _parameterNames = { "V_PRT_CODE", "V_PRT_DESC", "V_PRT_LABELCOUNT", "V_PRT_STATUS", "V_USER", "V_MODE" };
				_result = _dbObj.GetStringProc("SAVEPRINTERTYPE", _parameterNames, _parameterValues);
				return _result;
			}
			catch
			{
				throw;
			}
		}
		public DataSet GetPrinterType(object[] _parameterValues)
		{
			try
			{
				string[] _parameterNames = { "V_PRT_CODE", "V_MODE" };
				_ds = _dbObj.GetDatasetProc("GETPRINTERTYPE", _parameterNames, _parameterValues);
				return _ds;
			}
			catch
			{
				throw;
			}
			finally
			{
				if (_ds != null) { _ds.Dispose(); }
			}
		}
		#endregion
	}
}
