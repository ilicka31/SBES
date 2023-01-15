using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace CryptographyManager
{
	public static class ByteArraySerializer<T>
	{
		public static Byte[] Serialize(T m)
		{
			var ms = new MemoryStream();
			try
			{
				ms.Seek(0, SeekOrigin.Begin);
				ms.Position = 0;
				var formatter = new BinaryFormatter();
				formatter.Serialize(ms, m);
				ms.Flush();

				return ms.ToArray();
			}
			finally
			{
				ms.Close();
			}
		}

		public static T Deserialize(Byte[] byteArray)
		{
			var ms = new MemoryStream(byteArray);
			try
			{
				ms.Seek(0, SeekOrigin.Begin);
				ms.Position = 0;
				var formatter = new BinaryFormatter();
				return (T)formatter.Deserialize(ms);
			}
			finally
			{
				ms.Close();
			}
		}
	}
}
