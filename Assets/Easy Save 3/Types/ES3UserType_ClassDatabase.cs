using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("isown", "ClassId", "Lv", "Skills", "equippedavarta", "ispasstive", "ClassId1", "Lv1", "Skills1", "Equippedavarta", "Isown", "Ispasstive")]
	public class ES3UserType_ClassDatabase : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_ClassDatabase() : base(typeof(ClassDatabase)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (ClassDatabase)obj;
			
			writer.WritePrivateField("isown", instance);
			writer.WritePrivateField("ClassId", instance);
			writer.WritePrivateField("Lv", instance);
			writer.WritePrivateField("Skills", instance);
			writer.WritePrivateField("equippedavarta", instance);
			writer.WritePrivateField("ispasstive", instance);
			writer.WriteProperty("ClassId1", instance.ClassId1, ES3Type_string.Instance);
			writer.WriteProperty("Lv1", instance.Lv1, ES3Type_int.Instance);
			writer.WriteProperty("Skills1", instance.Skills1, ES3Type_StringArray.Instance);
			writer.WriteProperty("Equippedavarta", instance.Equippedavarta, ES3Type_string.Instance);
			writer.WriteProperty("Isown", instance.Isown, ES3Type_bool.Instance);
			writer.WriteProperty("Ispasstive", instance.Ispasstive, ES3Type_bool.Instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (ClassDatabase)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "isown":
					reader.SetPrivateField("isown", reader.Read<System.Boolean>(), instance);
					break;
					case "ClassId":
					reader.SetPrivateField("ClassId", reader.Read<System.String>(), instance);
					break;
					case "Lv":
					reader.SetPrivateField("Lv", reader.Read<System.Int32>(), instance);
					break;
					case "Skills":
					reader.SetPrivateField("Skills", reader.Read<System.String[]>(), instance);
					break;
					case "equippedavarta":
					reader.SetPrivateField("equippedavarta", reader.Read<System.String>(), instance);
					break;
					case "ispasstive":
					reader.SetPrivateField("ispasstive", reader.Read<System.Boolean>(), instance);
					break;
					case "ClassId1":
						instance.ClassId1 = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "Lv1":
						instance.Lv1 = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "Skills1":
						instance.Skills1 = reader.Read<System.String[]>(ES3Type_StringArray.Instance);
						break;
					case "Equippedavarta":
						instance.Equippedavarta = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "Isown":
						instance.Isown = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "Ispasstive":
						instance.Ispasstive = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new ClassDatabase();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_ClassDatabaseArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_ClassDatabaseArray() : base(typeof(ClassDatabase[]), ES3UserType_ClassDatabase.Instance)
		{
			Instance = this;
		}
	}
}