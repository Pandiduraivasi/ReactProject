using System.Data;
using System.Reflection;
using WMSCORE.Common;
using static WMSCORE.Repositories.LoginRepository;

namespace WMSCORE.Repositories
{
	public class LoginRepository : ILoginRepository
	{
		public interface ILoginRepository
		{
			DataSet GetTerminalDetails(object[] _objParameters);
			DataSet GetLoginTerminalIP(object[] _objParameters);
			string GetSettings(object[] _objParameters);
			string SaveTerminalTrace(object[] _objParameters);
			DataSet CheckTerminalTraceLogin(object[] _objParameters);
			DataSet GetTerminal(object[] _objParameters);
			DataSet GetUserLogin(object[] _objParameters);
			DataSet GetConfiguration();
			DataSet GetArchieveConfigPrivilege(object[] _objParameters);
			DataSet GetTerminalID(object[] _objParameters);
			DataSet GetUser(object[] _objParameters);
			string SaveUser(object[] _objParameters);
		}
		DataAccess _daObj = new DataAccess();
		private DataSet _ds = new DataSet();
		public DataSet GetTerminalDetails(object[] _objParameters)
		{
			try
			{
				_ds = _daObj.GetTerminalDetails(_objParameters);
				return  _ds;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public DataSet GetLoginTerminalIP(object[] _objParameters)
		{
			try
			{
				_ds = _daObj.GetLoginTerminalIP(_objParameters);
				return  _ds;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public string GetSettings(object[] _objParameters)
		{
			try
			{
				string _Result = _daObj.GETSETTINGS(_objParameters);
				return  _Result;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public string SaveTerminalTrace(object[] _objParameters)
		{
			try
			{
				string _Result = _daObj.SAVETerminalTrace(_objParameters);
				return  _Result;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public DataSet CheckTerminalTraceLogin(object[] _objParameters)
		{
			try
			{
				_ds = _daObj.CheckTerminalTraceLogin(_objParameters);
				return  _ds;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public DataSet GetTerminal(object[] _objParameters)
		{
			try
			{
				_ds = _daObj.GetTerminal(_objParameters);
				return  _ds;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public DataSet GetUserLogin(object[] _objParameters)
		{
			try
			{
				_ds = _daObj.GetUserLogin(_objParameters);
				return  _ds;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public DataSet GetConfiguration()
		{
			try
			{
				_ds = _daObj.GetConfiguration();
				return _ds;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public DataSet GetArchieveConfigPrivilege(object[] _objParameters)
		{
			try
			{
				_ds = _daObj.GetArchieveConfigPrivilege(_objParameters);
				return  _ds;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public DataSet GetTerminalID(object[] _objParameters)
		{
			try
			{
				_ds = _daObj.GetTerminalID(_objParameters);
				return  _ds;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public DataSet GetUser(object[] _objParameters)
		{
			try
			{
				_ds = _daObj.GetUser(_objParameters);
				return  _ds;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public string SaveUser(object[] _objParameters)
		{
			try
			{
				string _Result = _daObj.SaveUser(_objParameters);
				return  _Result;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
