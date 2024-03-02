using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("equipdata")]
	public class ES3UserType_EquiptmentData : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_EquiptmentData() : base(typeof(EquiptmentData)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (EquiptmentData)obj;
			
			writer.WritePrivateField("equipdata", instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (EquiptmentData)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "equipdata":
					reader.SetPrivateField("equipdata", reader.Read<EquipDatabase[]>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new EquiptmentData();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_EquiptmentDataArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_EquiptmentDataArray() : base(typeof(EquiptmentData[]), ES3UserType_EquiptmentData.Instance)
		{
			Instance = this;
		}
	}
}