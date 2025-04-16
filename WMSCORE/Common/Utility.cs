using Microsoft.VisualBasic;
using System.Data;

namespace WMSCORE.Common
{
	public class Utility
	{
		public  string ReplaceQuote(string _Desc)
		{
			return _Desc.Replace("'", "`");
		}
		public  string EncryptDecrypt(string input)
		{
			string result = "";
			for (int i = 0; i < input.Length; i++)
			{
				// Get the ASCII value of the current character
				byte originalByte = (byte)input[i];
				// Complement the byte
				byte complementedByte = (byte)~originalByte;
				// Add an offset (23 in this case)
				if (complementedByte != 255)
				{
					result += (char)(complementedByte + 23);
				}
			}
			return result;
		}
		public List<Dictionary<string, object>> ConvertDataTableToDictionary(DataTable table)
		{
			var result = new List<Dictionary<string, object>>();

			foreach (DataRow row in table.Rows)
			{
				var dict = new Dictionary<string, object>();
				foreach (DataColumn column in table.Columns)
				{
					dict[column.ColumnName] = row[column];
				}
				result.Add(dict);
			}

			return result;
		}
		//public string EncryptDecrypt(string str_pwd)
		//{
		//	String Str_Pass = "";
		//	int Lng_Pos;    // For Iterating through each byte
		//	Byte Byte_Data;  // For Reading Data
		//	Byte Byte_DataC; // For storing Complemented (or encrypted) Byte_Data
		//	Lng_Pos = 1;
		//	for (int i = 0; i <= (Strings.Len(str_pwd) - 1); i++)
		//	{
		//		Byte_Data = (byte)Strings.Asc(Strings.Mid(str_pwd, Lng_Pos, 1));
		//		Byte_DataC = (byte)(~Byte_Data);
		//		//Encryption of the Data 
		//		if (!(Byte_DataC == 255))
		//		{
		//			Str_Pass += Strings.Chr(Byte_DataC + 23);
		//			//Writing Encrypted Data 
		//		}
		//		Lng_Pos = Lng_Pos + 1;
		//	}
		//	// EncryptDecrypt = Str_Pass;
		//	return Str_Pass;
		//}

		//public string GetAccessRights(string UserID, string ScreenID)
		//{

		//	try
		//	{
		//		string Output = GlobalManager.Gethttpresult("api/Common/GetAccessRights/?UserID=" + GlobalManager.EncryptDecrypt(UserID) +
		//			"&ScreenID=" + GlobalManager.EncryptDecrypt(ScreenID), "GET", "text/html");
		//		Output = GlobalManager.EncryptDecrypt(JsonConvert.DeserializeObject<string>(Output));
		//		return Output;
		//	}
		//	catch (Exception ex)
		//	{
		//		throw ex;
		//	}

		//}

		public bool KeyValue(string _sPrivilege, string _Input, string _sOtherPriv = "")
		{
			bool IsEnableKey = false;
			string FunctionKey = string.Empty;
			try
			{
				if (_Input == string.Empty)      /* Function Key Not available (i.e.) Privilege not required */
					return true;

				if (_sOtherPriv != string.Empty) /* If other than function key Privilege */
					FunctionKey = _sOtherPriv;
				else                             /* Function key Privilege */
					FunctionKey =
					   _Input == "F1" ? "A" :
					   _Input == "F2" ? "B" :
					   _Input == "F3" ? "C" :
					   _Input == "F4" ? "D" :
					   _Input == "F5" ? "E" :
					   _Input == "F6" ? "F" :
					   _Input == "F7" ? "G" :
					   _Input == "F8" ? "H" :
					   _Input == "F9" ? "I" :
					   _Input == "F10" ? "J" :
					   _Input == "F11" ? "K" :
					   _Input == "F12" ? "L" : "-1";
				/* Check the Privilege */
				IsEnableKey = _sPrivilege.IndexOf(FunctionKey) == -1 ? false : true;
			}
			catch
			{

				throw;
			}

			return IsEnableKey; /*If Return value "True" --> enable the button, "False" --> Disable the button */
		}
	}
}
