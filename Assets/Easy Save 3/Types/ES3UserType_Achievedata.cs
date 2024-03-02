using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("id", "coreid", "curcount", "maxcount", "achievetype", "isfinish")]
	public class ES3UserType_Achievedata : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_Achievedata() : base(typeof(Achievedata)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (Achievedata)obj;
			
			writer.WritePrivateField("id", instance);
			writer.WritePrivateField("coreid", instance);
			writer.WritePrivateField("curcount", instance);
			writer.WritePrivateField("maxcount", instance);
			writer.WritePrivateField("achievetype", instance);
			writer.WritePrivateField("isfinish", instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (Achievedata)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "id":
					reader.SetPrivateField("id", reader.Read<System.String>(), instance);
					break;
					case "coreid":
					reader.SetPrivateField("coreid", reader.Read<System.String>(), instance);
					break;
					case "curcount":
					reader.SetPrivateField("curcount", reader.Read<System.Int32>(), instance);
					break;
					case "maxcount":
					reader.SetPrivateField("maxcount", reader.Read<System.Int32>(), instance);
					break;
					case "achievetype":
					reader.SetPrivateField("achievetype", reader.Read<System.String>(), instance);
					break;
					case "isfinish":
					reader.SetPrivateField("isfinish", reader.Read<System.Boolean>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new Achievedata();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_AchievedataArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_AchievedataArray() : base(typeof(Achievedata[]), ES3UserType_Achievedata.Instance)
		{
			Instance = this;
		}
	}
}