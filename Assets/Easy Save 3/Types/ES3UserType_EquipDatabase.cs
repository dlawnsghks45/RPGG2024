using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("isEquip", "itemid", "KeyId", "CraftRare", "itemrare", "PrefixId", "MaxStoneCount", "StoneCount", "SmeltStatCount", "SmeltSuccCount", "SmeltFailCount", "EnchantNum", "EnchantFail", "islock", "ishaveEquipSkill", "EquipSkill", "Itemid", "KeyId1", "CraftRare1", "Itemrare", "PrefixId1", "MaxStoneCount1", "StoneCount1", "SmeltStatCount1", "IsEquip", "IsLock", "SmeltSuccCount1", "SmeltFailCount1", "EnchantNum1", "EnchantFail1", "IshaveEquipSkill", "EquipSkill1")]
	public class ES3UserType_EquipDatabase : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_EquipDatabase() : base(typeof(EquipDatabase)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (EquipDatabase)obj;
			
			writer.WritePrivateField("isEquip", instance);
			writer.WritePrivateField("itemid", instance);
			writer.WritePrivateField("KeyId", instance);
			writer.WritePrivateField("CraftRare", instance);
			writer.WritePrivateField("itemrare", instance);
			writer.WritePrivateField("PrefixId", instance);
			writer.WritePrivateField("MaxStoneCount", instance);
			writer.WritePrivateField("StoneCount", instance);
			writer.WritePrivateField("SmeltStatCount", instance);
			writer.WritePrivateField("SmeltSuccCount", instance);
			writer.WritePrivateField("SmeltFailCount", instance);
			writer.WritePrivateField("EnchantNum", instance);
			writer.WritePrivateField("EnchantFail", instance);
			writer.WritePrivateField("islock", instance);
			writer.WritePrivateField("ishaveEquipSkill", instance);
			writer.WritePrivateField("EquipSkill", instance);
			writer.WriteProperty("Itemid", instance.Itemid, ES3Type_string.Instance);
			writer.WriteProperty("KeyId1", instance.KeyId1, ES3Type_string.Instance);
			writer.WriteProperty("CraftRare1", instance.CraftRare1, ES3Type_int.Instance);
			writer.WriteProperty("Itemrare", instance.Itemrare, ES3Type_string.Instance);
			writer.WriteProperty("PrefixId1", instance.PrefixId1, ES3Type_string.Instance);
			writer.WriteProperty("MaxStoneCount1", instance.MaxStoneCount1, ES3Type_int.Instance);
			writer.WriteProperty("StoneCount1", instance.StoneCount1, ES3Type_int.Instance);
			writer.WriteProperty("SmeltStatCount1", instance.SmeltStatCount1, ES3Type_intArray.Instance);
			writer.WriteProperty("IsEquip", instance.IsEquip, ES3Type_bool.Instance);
			writer.WriteProperty("IsLock", instance.IsLock, ES3Type_bool.Instance);
			writer.WriteProperty("SmeltSuccCount1", instance.SmeltSuccCount1, ES3Type_int.Instance);
			writer.WriteProperty("SmeltFailCount1", instance.SmeltFailCount1, ES3Type_int.Instance);
			writer.WriteProperty("EnchantNum1", instance.EnchantNum1, ES3Type_int.Instance);
			writer.WriteProperty("EnchantFail1", instance.EnchantFail1, ES3Type_int.Instance);
			writer.WriteProperty("IshaveEquipSkill", instance.IshaveEquipSkill, ES3Type_bool.Instance);
			writer.WriteProperty("EquipSkill1", instance.EquipSkill1, ES3Internal.ES3TypeMgr.GetES3Type(typeof(System.Collections.Generic.List<System.String>)));
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (EquipDatabase)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "isEquip":
					reader.SetPrivateField("isEquip", reader.Read<System.Boolean>(), instance);
					break;
					case "itemid":
					reader.SetPrivateField("itemid", reader.Read<System.String>(), instance);
					break;
					case "KeyId":
					reader.SetPrivateField("KeyId", reader.Read<System.String>(), instance);
					break;
					case "CraftRare":
					reader.SetPrivateField("CraftRare", reader.Read<System.Int32>(), instance);
					break;
					case "itemrare":
					reader.SetPrivateField("itemrare", reader.Read<System.String>(), instance);
					break;
					case "PrefixId":
					reader.SetPrivateField("PrefixId", reader.Read<System.String>(), instance);
					break;
					case "MaxStoneCount":
					reader.SetPrivateField("MaxStoneCount", reader.Read<System.Int32>(), instance);
					break;
					case "StoneCount":
					reader.SetPrivateField("StoneCount", reader.Read<System.Int32>(), instance);
					break;
					case "SmeltStatCount":
					reader.SetPrivateField("SmeltStatCount", reader.Read<System.Int32[]>(), instance);
					break;
					case "SmeltSuccCount":
					reader.SetPrivateField("SmeltSuccCount", reader.Read<System.Int32>(), instance);
					break;
					case "SmeltFailCount":
					reader.SetPrivateField("SmeltFailCount", reader.Read<System.Int32>(), instance);
					break;
					case "EnchantNum":
					reader.SetPrivateField("EnchantNum", reader.Read<System.Int32>(), instance);
					break;
					case "EnchantFail":
					reader.SetPrivateField("EnchantFail", reader.Read<System.Int32>(), instance);
					break;
					case "islock":
					reader.SetPrivateField("islock", reader.Read<System.Boolean>(), instance);
					break;
					case "ishaveEquipSkill":
					reader.SetPrivateField("ishaveEquipSkill", reader.Read<System.Boolean>(), instance);
					break;
					case "EquipSkill":
					reader.SetPrivateField("EquipSkill", reader.Read<System.Collections.Generic.List<System.String>>(), instance);
					break;
					case "Itemid":
						instance.Itemid = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "KeyId1":
						instance.KeyId1 = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "CraftRare1":
						instance.CraftRare1 = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "Itemrare":
						instance.Itemrare = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "PrefixId1":
						instance.PrefixId1 = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "MaxStoneCount1":
						instance.MaxStoneCount1 = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "StoneCount1":
						instance.StoneCount1 = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "SmeltStatCount1":
						instance.SmeltStatCount1 = reader.Read<System.Int32[]>(ES3Type_intArray.Instance);
						break;
					case "IsEquip":
						instance.IsEquip = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "IsLock":
						instance.IsLock = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "SmeltSuccCount1":
						instance.SmeltSuccCount1 = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "SmeltFailCount1":
						instance.SmeltFailCount1 = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "EnchantNum1":
						instance.EnchantNum1 = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "EnchantFail1":
						instance.EnchantFail1 = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "IshaveEquipSkill":
						instance.IshaveEquipSkill = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "EquipSkill1":
						instance.EquipSkill1 = reader.Read<System.Collections.Generic.List<System.String>>();
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new EquipDatabase();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_EquipDatabaseArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_EquipDatabaseArray() : base(typeof(EquipDatabase[]), ES3UserType_EquipDatabase.Instance)
		{
			Instance = this;
		}
	}
}