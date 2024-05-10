using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("<Eskill>k__BackingField", "<Itemid>k__BackingField", "<Keyid>k__BackingField", "<Islock>k__BackingField", "Eskill", "Itemid", "Keyid", "Islock")]
	public class ES3UserType_Talismandatabase : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_Talismandatabase() : base(typeof(Talismandatabase)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (Talismandatabase)obj;
			
			writer.WritePrivateField("<Eskill>k__BackingField", instance);
			writer.WritePrivateField("<Itemid>k__BackingField", instance);
			writer.WritePrivateField("<Keyid>k__BackingField", instance);
			writer.WritePrivateField("<Islock>k__BackingField", instance);
			writer.WriteProperty("Eskill", instance.Eskill, ES3Internal.ES3TypeMgr.GetES3Type(typeof(System.Collections.Generic.List<System.String>)));
			writer.WriteProperty("Itemid", instance.Itemid, ES3Type_string.Instance);
			writer.WriteProperty("Keyid", instance.Keyid, ES3Type_string.Instance);
			writer.WriteProperty("Islock", instance.Islock, ES3Type_bool.Instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (Talismandatabase)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "<Eskill>k__BackingField":
					reader.SetPrivateField("<Eskill>k__BackingField", reader.Read<System.Collections.Generic.List<System.String>>(), instance);
					break;
					case "<Itemid>k__BackingField":
					reader.SetPrivateField("<Itemid>k__BackingField", reader.Read<System.String>(), instance);
					break;
					case "<Keyid>k__BackingField":
					reader.SetPrivateField("<Keyid>k__BackingField", reader.Read<System.String>(), instance);
					break;
					case "<Islock>k__BackingField":
					reader.SetPrivateField("<Islock>k__BackingField", reader.Read<System.Boolean>(), instance);
					break;
					case "Eskill":
						instance.Eskill = reader.Read<System.Collections.Generic.List<System.String>>();
						break;
					case "Itemid":
						instance.Itemid = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "Keyid":
						instance.Keyid = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "Islock":
						instance.Islock = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new Talismandatabase();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_TalismandatabaseArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_TalismandatabaseArray() : base(typeof(Talismandatabase[]), ES3UserType_Talismandatabase.Instance)
		{
			Instance = this;
		}
	}
}