using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("data", "ItemImage", "ItemCraftRare", "SmeltSlots")]
	public class ES3UserType_EquipmentItemData : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_EquipmentItemData() : base(typeof(EquipmentItemData)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (EquipmentItemData)obj;
			
			writer.WritePrivateField("data", instance);
			writer.WritePropertyByRef("ItemImage", instance.ItemImage);
			writer.WriteProperty("ItemCraftRare", instance.ItemCraftRare, ES3Type_GameObjectArray.Instance);
			writer.WriteProperty("SmeltSlots", instance.SmeltSlots, ES3Internal.ES3TypeMgr.GetES3Type(typeof(SmeltSlot[])));
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (EquipmentItemData)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "data":
					reader.SetPrivateField("data", reader.Read<EquipDatabase>(), instance);
					break;
					case "ItemImage":
						instance.ItemImage = reader.Read<UnityEngine.UI.Image>(ES3Type_Image.Instance);
						break;
					case "ItemCraftRare":
						instance.ItemCraftRare = reader.Read<UnityEngine.GameObject[]>(ES3Type_GameObjectArray.Instance);
						break;
					case "SmeltSlots":
						instance.SmeltSlots = reader.Read<SmeltSlot[]>();
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_EquipmentItemDataArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_EquipmentItemDataArray() : base(typeof(EquipmentItemData[]), ES3UserType_EquipmentItemData.Instance)
		{
			Instance = this;
		}
	}
}