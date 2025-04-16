using Microsoft.VisualBasic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Net;
using System.Text;

namespace WMSCORE.Common
{
	public class GlobalManager
	{
		public static string DecryptText(string EncryptedText)
		{
			try
			{
				if (EncryptedText != string.Empty && EncryptedText != null)
					return Encoding.UTF8.GetString(Convert.FromBase64String(EncryptedText));
				else
					return "";
			}
			catch
			{
				throw;
			}
		}
		public static string EncryptText(string Text)
		{
			try
			{
				return Convert.ToBase64String(Encoding.UTF8.GetBytes(Text));
			}
			catch
			{
				throw;
			}
		}

		//Encryption and Decryption
		public static string EncryptDecrypt(string str)
		{
			try
			{
				String Res = "";
				int Lng_Pos;    // For Iterating through each byte
				Byte Byte_Data;  // For Reading Data
				Byte Byte_DataC; // For storing Complemented (or encrypted) Byte_Data
				Lng_Pos = 1;
				for (int i = 0; i <= (Strings.Len(str) - 1); i++)
				{
					Byte_Data = (byte)Strings.Asc(Strings.Mid(str, Lng_Pos, 1));
					Byte_DataC = (byte)(~Byte_Data);
					//Encryption of the Data 
					if (!(Byte_DataC == 255))
					{
						Res += Strings.Chr(Byte_DataC + 23);
					}
					Lng_Pos = Lng_Pos + 1;
				}
				return Res;
			}
			catch
			{
				throw;
			}
		}


		public string rightCase(string Str)
		{
			if (Str == null)
				throw new ArgumentNullException("Str Null: " + Str);

			TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
			string _Str = Str.ToLower();
			return myTI.ToTitleCase(_Str);
		}

	}
}
