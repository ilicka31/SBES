using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CryptographyManager
{
    public class DESAlgorithm
    {
		public static byte[] EncryptData(byte[] data, string secretKey)
		{
			using (var DES = new DESCryptoServiceProvider())
			{
				DES.GenerateIV();
				DES.Key = Encoding.ASCII.GetBytes(secretKey);
				DES.Mode = CipherMode.CBC;
				DES.Padding = PaddingMode.PKCS7;

				using (var memStream = new MemoryStream())
				{
					using (CryptoStream cryptoStream = new CryptoStream(memStream, DES.CreateEncryptor(), CryptoStreamMode.Write))
					{
						cryptoStream.Write(data, 0, data.Length);
						cryptoStream.FlushFinalBlock();
					}
					return DES.IV.Concat(memStream.ToArray()).ToArray();
				}
				
			}
		}


		public static byte[] DecryptData(byte[] data, string secretKey)
		{
			byte[] decryptedBody = null;
			using (var DES = new DESCryptoServiceProvider())
			{
				DES.IV = data.Take(DES.BlockSize / 8).ToArray();
				DES.Key = Encoding.ASCII.GetBytes(secretKey);
				DES.Mode = CipherMode.CBC;
				DES.Padding = PaddingMode.PKCS7;

				ICryptoTransform desDecryptTransform = DES.CreateDecryptor();

				using (MemoryStream memoryStream = new MemoryStream(data.Skip(DES.BlockSize / 8).ToArray()))
				{
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, desDecryptTransform, CryptoStreamMode.Read))
					{
						decryptedBody = new byte[data.Length - DES.BlockSize / 8];     //decrypted data body - the same lenght as encrypted part
						cryptoStream.Read(decryptedBody, 0, decryptedBody.Length);
					}
				}
			}

			return decryptedBody;
		}
	}
}
