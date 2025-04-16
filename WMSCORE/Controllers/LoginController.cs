using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.IO;
using System.Net;
using System.Reflection;
using System.Xml.Linq;
using WMSCORE.Common;
using WMSCORE.Services;
using static WMSCORE.Services.LoginService;

namespace WMSCORE.Controllers
{
	[Route("api/Login")]
	[ApiController]
	public class LoginController : ControllerBase
	{
		object[] _parameters = null;
		bool wmsstatus = false;
		string _strWmsStatus = string.Empty;
		private readonly ILoginService _LoginService;
		Utility _utiObj = new Utility();
		AESEncryptionClass _AEC = new AESEncryptionClass();
		string US_ID, Us_CODE, Us_Password = string.Empty, IpAddress = string.Empty;
		string TermID = string.Empty, TermArea = string.Empty, TermName = string.Empty, TermUser = string.Empty, TermStnCode = string.Empty;
		string LoginTerminalID = string.Empty;
		string US_PasswordEncry = string.Empty;
		string Us_PasswordAES = string.Empty;
		public LoginController(ILoginService LoginService)
		{
			_LoginService = LoginService;
		}

		[HttpPost("Login")]
		public IActionResult Login([FromBody] LoginRequest request)
		{
			try
			{
				string terminalID = GetTerminalDetails();
				if (string.IsNullOrEmpty(terminalID))
				{
					return Ok(new { success = false, message = "This terminal does not have privilege to access the application." });
				}

				var terminalTraceResult = CheckTerminalTrace(terminalID, "C");
				if (terminalTraceResult.Tables[0].Rows.Count > 0 && terminalTraceResult.Tables[0].Rows[0]["UID"].ToString() != "0")
				{
					var terminalTraceInfo = CheckTerminalTrace(terminalID, "I");
					if (terminalTraceInfo.Tables[0].Rows.Count > 0)
					{
						string termID = terminalTraceInfo.Tables[0].Rows[0]["TT_TE"].ToString();
						string termArea = terminalTraceInfo.Tables[0].Rows[0]["TE_AR"].ToString();
						string termName = terminalTraceInfo.Tables[0].Rows[0]["TE_NAME"].ToString();
						string termUser = terminalTraceInfo.Tables[0].Rows[0]["US_NAME"].ToString();
						string termStnCode = terminalTraceInfo.Tables[0].Rows[0]["TE_STN_CODE"].ToString();

						return Ok(new { success = false, message = $"User {termUser} is already logged in at Area: {termArea} on Terminal: {termID} ({termName})" });
					}
				}
				US_PasswordEncry = _AEC.EncryptStringAES(request.Password);
				Us_PasswordAES = _AEC.DecryptStringAES(US_PasswordEncry);
				Us_Password = _utiObj.EncryptDecrypt(_utiObj.ReplaceQuote(Us_PasswordAES));
				var loginResult = CheckUserLogin(request.Username, Us_Password, terminalID, US_PasswordEncry);
				if (!loginResult.Success)
				{
					return Ok(new { success = false, message = loginResult.Message });
				}

				return Ok(new { success = true });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { success = false, message = ex.Message });
			}
		}
		[HttpGet("GetTerminalDetails")]
		public IActionResult GetTerminalDetails([FromHeader(Name = "X-Session-Id")] string? sessionId = null)
		{
			try
			{
				var ipAddress = GetComputerIP();
				_parameters = new object[3];
				_parameters[0] = ipAddress;
				_parameters[1] = "G";
				_parameters[2] = string.Empty;
				var terminalDetails = _LoginService.GetTerminalDetails(_parameters);
				if (terminalDetails.Tables.Count > 0 && terminalDetails.Tables[0].Rows.Count > 0)
				{
					var row = terminalDetails.Tables[0].Rows[0];
					return Ok(new
					{
						SMTP = row["TE_ID"],
						FORGOTACTION = row["TE_AR"],
						LOGINBASED = row["ENVIRONMENT_NAME"],
						OTPREQUIRED = row["INFO_MSG"],
						success = true,
					}
					);
				}

				return Ok(new { success = false });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { success = false, message = ex.Message });
			}
		}
		[HttpGet("navItems")]
		public IActionResult navItems()
		{
			try
			{
				var routes = new List<object>
				{
				new {Name  = "Forms",Icon  ="ChevronDownIcon", SubItems = new List<object> { new { Name = "Form Elements", Path = "/form-elements", Pro = false} } },
				new {Name  = "Tables",Icon  ="HorizontaLDots", SubItems = new List<object> { new { Name = "Basic Tables", Path = "/basic-tables", Pro = false } } },
				new {Name  = "Pages",Icon  ="HorizontaLDots", SubItems = new List<object> { new { Name = "Blank Page", Path = "/blank", Pro = false } } },
				};

				return Ok(routes);
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { success = false, message = ex.Message });
			}
		}
		private string GetTerminalDetails()
		{
			var ipAddress = GetComputerIP();
			_parameters = new object[3];
			_parameters[0] = ipAddress;
			_parameters[1] = "G";
			_parameters[2] = string.Empty;
			var terminalDetails = _LoginService.GetTerminalDetails(_parameters);
			if (terminalDetails.Tables.Count > 0 && terminalDetails.Tables[0].Rows.Count > 0)
			{
				return terminalDetails.Tables[0].Rows[0]["TE_ID"].ToString();
			}
			return string.Empty;
		}
		private string GetComputerIP()
		{
			try
			{
				string ipAddress = Request.Headers["X-Forwarded-For"].FirstOrDefault();

				if (string.IsNullOrEmpty(ipAddress))
				{
					ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
				}

				// Handle localhost scenario
				if (ipAddress == "::1" || ipAddress == "127.0.0.1")
				{
					ipAddress = GetLocalIpAddress();
				}
				return ipAddress;
			}
			catch
			{
				throw;
			}
		}
		private string GetLocalIpAddress()
		{
			string hostName = Dns.GetHostName();
			var hostEntry = Dns.GetHostEntry(hostName);
			foreach (var ip in hostEntry.AddressList)
			{
				if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
				{
					return ip.ToString();
				}
			}
			return "Unknown";
		}
		private DataSet CheckTerminalTrace(string terminalID, string mode)
		{
			_parameters = new object[4];
			_parameters[0] = terminalID;
			_parameters[1] = string.Empty;
			_parameters[2] = "G";
			_parameters[3] = mode;
			return _LoginService.CheckTerminalTraceLogin(_parameters);
		}

		private (bool Success, string Message) CheckUserLogin(string username, string password, string terminalID,string orgpassword)
		{
			var userDetails = GetLoginUserDetails("G", username,"1");
			if (userDetails.Tables.Count == 0 || userDetails.Tables[0].Rows.Count == 0)
			{
				return (false, "Invalid User ID or Password");
			}

			var userRow = userDetails.Tables[0].Rows[0];
			if (password != userRow["US_PASSWORD"].ToString())
			{
				return (false, "Invalid User ID or Password");
			}

			if (userRow["US_EFFFROM"] != DBNull.Value && Convert.ToDateTime(userRow["US_EFFFROM"]) > DateTime.Now)
			{
				return (false, "Please note that your account is not activated. Please contact your administrator.");
			}
			string PasswordVal = _AEC.DecryptStringAES(orgpassword);
			string passwordPolicyMessage = CheckPasswordPolicy(_utiObj.ReplaceQuote(PasswordVal));
			if (wmsstatus)
			{
				return (false, "The current system status is OFFLINE. Please make status to ONLINE");
			}
			if (!string.IsNullOrEmpty(passwordPolicyMessage))
			{
				return (false, "Your current password does not match the current password policy.");
			}

			SAVETERMINALTRACE(terminalID, userRow["US_ID"].ToString());
			return (true, string.Empty);
		}

		private DataSet GetLoginUserDetails(string mode, string username, string status)
		{
			_parameters = new object[3];
			_parameters[0] = mode;
			_parameters[1] = username;
			_parameters[2] = status;
			return _LoginService.GetUserLogin(_parameters);
		}

		private string CheckPasswordPolicy(string password)
		{
			var config = _LoginService.GetConfiguration();
			if (config.Tables.Count > 0 && config.Tables[0].Rows.Count > 0)
			{
				var configRow = config.Tables[0].Rows[0];
				int minLen = Convert.ToInt32(configRow["CF_PASLENGTHMIN"]);
				int maxLen = Convert.ToInt32(configRow["CF_PASLENGTHMAX"]);
				int numericLen = Convert.ToInt32(configRow["CF_NUMERICCHAR"]);
				int upperLen = Convert.ToInt32(configRow["CF_UPPERCHAR"]);
				string specialChar = configRow["CF_EXCEPTIONALCHAR"].ToString();
				_strWmsStatus = configRow["CF_WMSSTATUS"].ToString();//To Check WMS Online or Offline
				if (_strWmsStatus == "0")//WMS is Offline
				{
					wmsstatus = true;//WMS is Offline
				}
				int upperCount = 0;
				int numericCount = 0;
				if (password == null)
					throw new ArgumentNullException("password");
				foreach (char c in password)
				{
					if (char.IsUpper(c)) { ++upperCount; }
					else if (char.IsDigit(c)) { ++numericCount; }
				}

				bool ch = password.IndexOfAny(specialChar.ToCharArray()) != -1;

				if (password.Length < minLen || password.Length > maxLen || upperCount < upperLen || numericCount < numericLen || ch)
				{
					return $"Minimum Length: {minLen}, Maximum Length: {maxLen}, Minimum Upper Characters: {upperLen}, Minimum Numeric Characters: {numericLen}, Special Characters Not Allowed: {specialChar}";
				}
			}
			return string.Empty;
		}

		private void SAVETERMINALTRACE(string terminalID, string userID)
		{
			_parameters = new object[6];
			_parameters[0] = terminalID;
			_parameters[1] = userID;
			_parameters[2] = string.Empty;
			_parameters[3] = "G";
			_parameters[4] = userID;
			_parameters[5] = "L";
			_LoginService.SaveTerminalTrace(_parameters);
		}
	}

	public class LoginRequest
	{
		public string Username { get; set; }
		public string Password { get; set; }
	}
}

