using System.Data;
using System.Reflection;
using static WMSCORE.Repositories.LoginRepository;
using static WMSCORE.Services.LoginService;

namespace WMSCORE.Services
{
	public class LoginService : ILoginService
	{
		public interface ILoginService
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
		private readonly ILoginRepository _Loginrepository;

		public LoginService(ILoginRepository Loginrepository)
		{
			_Loginrepository = Loginrepository;
		}
		public DataSet GetTerminalDetails(object[] _objParameters)
		{
			return _Loginrepository.GetTerminalDetails(_objParameters);
		}
		public DataSet GetLoginTerminalIP(object[] _objParameters)
		{
			return _Loginrepository.GetLoginTerminalIP(_objParameters);
		}
		public string GetSettings(object[] _objParameters)
		{
			return _Loginrepository.GetSettings(_objParameters);
		}
		public string SaveTerminalTrace(object[] _objParameters)
		{
			return _Loginrepository.SaveTerminalTrace(_objParameters);
		}
		public DataSet CheckTerminalTraceLogin(object[] _objParameters)
		{
			return _Loginrepository.CheckTerminalTraceLogin(_objParameters);
		}
		public DataSet GetTerminal(object[] _objParameters)
		{
			return _Loginrepository.GetTerminal(_objParameters);
		}
		public DataSet GetUserLogin(object[] _objParameters)
		{
			return _Loginrepository.GetUserLogin(_objParameters);
		}
		public DataSet GetConfiguration()
		{
			return _Loginrepository.GetConfiguration();
		}
		public DataSet GetArchieveConfigPrivilege(object[] _objParameters)
		{
			return _Loginrepository.GetArchieveConfigPrivilege(_objParameters);
		}
		public DataSet GetTerminalID(object[] _objParameters)
		{
			return _Loginrepository.GetTerminalID(_objParameters);
		}

		public DataSet GetUser(object[] _objParameters)
		{
			return _Loginrepository.GetUser(_objParameters);
		}
		public string SaveUser(object[] _objParameters)
		{
			return _Loginrepository.SaveUser(_objParameters);
		}
	}
}
