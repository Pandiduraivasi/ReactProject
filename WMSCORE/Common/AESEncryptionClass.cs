using System.Security.Cryptography;
using System.Text;

namespace WMSCORE.Common
{
	public class AESEncryptionClass
	{
		public string Key = "5369657272613233"; // CryptoJS.enc.Utf8.parse("Sierra23")
		public string IVK = "5369657272613233"; // CryptoJS.enc.Utf8.parse("Sierra23")

		public AESEncryptionClass()
		{
			string IVKKey = string.Empty;
		}
		public string EncryptStringAES(string plaintext)
		{
			if (string.IsNullOrEmpty(plaintext))
				return string.Empty;

			var keybytes = Encoding.UTF8.GetBytes(Key);
			var iv = Encoding.UTF8.GetBytes(IVK);
			var encrypted = Convert.ToBase64String(EncryptStringToBytes(plaintext, keybytes, iv));
			return encrypted;
		}

		public string DecryptStringAES(string cipherText)
		{
			if (string.IsNullOrEmpty(cipherText))
				return string.Empty;

			var keybytes = Encoding.UTF8.GetBytes(Key);
			var iv = Encoding.UTF8.GetBytes(IVK);
			var encrypted = Convert.FromBase64String(cipherText);
			var decrypted = DecryptStringFromBytes(encrypted, keybytes, iv);
			return decrypted;
		}
		private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
		{
			// Check arguments.
			if (cipherText == null || cipherText.Length <= 0)
				throw new ArgumentNullException("cipherText");
			if (key == null || key.Length <= 0)
				throw new ArgumentNullException("key");
			if (iv == null || iv.Length <= 0)
				throw new ArgumentNullException("key");

			// Declare the string used to hold
			// the decrypted text.
			string plaintext = null;

			// Create an RijndaelManaged object
			// with the specified key and IV.
			using (var rijAlg = new RijndaelManaged())
			{
				// Settings
				rijAlg.Mode = CipherMode.CBC;
				rijAlg.Padding = PaddingMode.PKCS7;
				rijAlg.FeedbackSize = 128;

				rijAlg.Key = key;
				rijAlg.IV = iv;

				// Create a decrytor to perform the stream transform.
				var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);
				try
				{
					// Create the streams used for decryption.
					using (var msDecrypt = new MemoryStream(cipherText))
					{
						using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
						{
							using (var srDecrypt = new StreamReader(csDecrypt))
							{
								// Read the decrypted bytes from the decrypting stream
								// and place them in a string.

								plaintext = srDecrypt.ReadToEnd();
							}
						}
					}
				}
				catch
				{
					plaintext = "";
				}
			}

			return plaintext;
		}
		private static byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
		{
			using (var rijAlg = Aes.Create())
			{
				rijAlg.Key = key;
				rijAlg.IV = iv;

				using (var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV))
				using (var msEncrypt = new MemoryStream())
				using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
				using (var swEncrypt = new StreamWriter(csEncrypt))
				{
					swEncrypt.Write(plainText);
					swEncrypt.Flush();
					csEncrypt.FlushFinalBlock();
					return msEncrypt.ToArray();
				}
			}
		}
	}
}
