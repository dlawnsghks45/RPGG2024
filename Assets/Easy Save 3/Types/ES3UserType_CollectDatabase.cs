using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("setid", "itemid", "curcount", "maxcount", "isfinish", "isfinishall", "SetID", "ItemID", "Curcount", "Maxcount", "Isfinish", "Isfinishall")]
	public class ES3UserType_CollectDatabase : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_CollectDatabase() : base(typeof(CollectDatabase)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (CollectDatabase)obj;
			
			writer.WritePrivateField("setid", instance);
			writer.WritePrivateField("itemid", instance);
			writer.WritePrivateField("curcount", instance);
			writer.WritePrivateField("maxcount", instance);
			writer.WritePrivateField("isfinish", instance);
			writer.WritePrivateField("isfinishall", instance);
			writer.WriteProperty("SetID", instance.SetID, ES3Type_string.Instance);
			writer.WriteProperty("ItemID", instance.ItemID, ES3Type_StringArray.Instance);
			writer.WriteProperty("Curcount", instance.Curcount, ES3Type_intArray.Instance);
			writer.WriteProperty("Maxcount", instance.Maxcount, ES3Type_intArray.Instance);
			writer.WriteProperty("Isfinish", instance.Isfinish, ES3Type_boolArray.Instance);
			writer.WriteProperty("Isfinishall", instance.Isfinishall, ES3Type_bool.Instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (CollectDatabase)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "setid":
					reader.SetPrivateField("setid", reader.Read<System.String>(), instance);
					break;
					case "itemid":
					reader.SetPrivateField("itemid", reader.Read<System.String[]>(), instance);
					break;
					case "curcount":
					reader.SetPrivateField("curcount", reader.Read<System.Int32[]>(), instance);
					break;
					case "maxcount":
					reader.SetPrivateField("maxcount", reader.Read<System.Int32[]>(), instance);
					break;
					case "isfinish":
					reader.SetPrivateField("isfinish", reader.Read<System.Boolean[]>(), instance);
					break;
					case "isfinishall":
					reader.SetPrivateField("isfinishall", reader.Read<System.Boolean>(), instance);
					break;
					case "SetID":
						instance.SetID = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "ItemID":
						instance.ItemID = reader.Read<System.String[]>(ES3Type_StringArray.Instance);
						break;
					case "Curcount":
						instance.Curcount = reader.Read<System.Int32[]>(ES3Type_intArray.Instance);
						break;
					case "Maxcount":
						instance.Maxcount = reader.Read<System.Int32[]>(ES3Type_intArray.Instance);
						break;
					case "Isfinish":
						instance.Isfinish = reader.Read<System.Boolean[]>(ES3Type_boolArray.Instance);
						break;
					case "Isfinishall":
						instance.Isfinishall = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new CollectDatabase();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_CollectDatabaseArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_CollectDatabaseArray() : base(typeof(CollectDatabase[]), ES3UserType_CollectDatabase.Instance)
		{
			Instance = this;
		}
	}
}