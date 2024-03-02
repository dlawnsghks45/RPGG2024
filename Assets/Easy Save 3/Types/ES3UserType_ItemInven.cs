using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("id", "howmany", "expiredate", "Id", "Howmany", "Expiredate")]
	public class ES3UserType_ItemInven : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_ItemInven() : base(typeof(ItemInven)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (ItemInven)obj;
			
			writer.WritePrivateField("id", instance);
			writer.WritePrivateField("howmany", instance);
			writer.WritePrivateField("expiredate", instance);
			writer.WriteProperty("Id", instance.Id, ES3Type_string.Instance);
			writer.WriteProperty("Howmany", instance.Howmany, ES3Type_int.Instance);
			writer.WriteProperty("Expiredate", instance.Expiredate, ES3Type_string.Instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (ItemInven)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "id":
					reader.SetPrivateField("id", reader.Read<System.String>(), instance);
					break;
					case "howmany":
					reader.SetPrivateField("howmany", reader.Read<System.Int32>(), instance);
					break;
					case "expiredate":
					reader.SetPrivateField("expiredate", reader.Read<System.String>(), instance);
					break;
					case "Id":
						instance.Id = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "Howmany":
						instance.Howmany = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "Expiredate":
						instance.Expiredate = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new ItemInven();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_ItemInvenArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_ItemInvenArray() : base(typeof(ItemInven[]), ES3UserType_ItemInven.Instance)
		{
			Instance = this;
		}
	}
}