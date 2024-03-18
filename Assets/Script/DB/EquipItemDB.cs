// This code automatically generated by TableCodeGen
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquipItemDB : MonoBehaviour
{
    
    public TextAsset ItemDatabase;

   
    public void Awake()
    {
        ItemDatabase = Resources.Load("CSV/EquipItem") as TextAsset;
        Load(ItemDatabase);
    }

    //싱글톤만들기. 변경
    private static EquipItemDB _instance = null;
    public static EquipItemDB Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(EquipItemDB)) as EquipItemDB;

                if (_instance == null)
                {
                }
            }
            return _instance;
        }
    }


	public class Row
	{
        		public string DESC;
		public string id;
		public string Name;
		public string Type;
		public string SubType;
		public string Rangetype;
		public string Sprite;
		public string EquipSprite;
		public string ArrowSprite;
		public string ArrowSpeed;
		public string ArrowSize;
		public string ArrowRotation;
		public string HitEffect;
		public string HitSound;
		public string HitCount;
		public string Tier;
		public string BattlePoint;
		public string Rare;
		public string RarePercent;
		public string RareCoung;
		public string RanSmeltCount;
		public string SmeltUpStat;
		public string craftrare;
		public string craftrarelist;
		public string material;
		public string attacktype;
		public string minatk;
		public string maxatk;
		public string atkspeed;
		public string castspeed;
		public string matk;
		public string def;
		public string mdef;
		public string Str;
		public string Dex;
		public string Int;
		public string Wis;
		public string AllStat;
		public string AdvanStat;
		public string Hit;
		public string Exp;
		public string Gold;
		public string Hp;
		public string Mp;
		public string Crit;
		public string CritDmg;
		public string MinAllDmg;
		public string MaxAllDmg;
		public string RanStatId;
		public string SetID;
		public string SpeMehod;
		public string SpeMehodP;
		public string SuccSpe;
		public string istwohand;
		public string smeltid;
		public string smeltcount;
		public string maxdotcount;
		public string dotdmgup;
		public string AdvanEquipID;
		public string AdvanNeedItem;
		public string AdvanNeedItemHowmany;
		public string AdvanGold;
		public string AdvanPercent;
		public string ChangeId;
		public string UpgradeNeedID;

	}

	List<Row> rowList = new List<Row>();
	bool isLoaded = false;

	public bool IsLoaded()
	{
		return isLoaded;
	}

	public List<Row> GetRowList()
	{
		return rowList;
	}

	public void Load(TextAsset csv)
	{
		rowList.Clear();
		string[][] grid = CsvParser2.Parse(csv.text);
		for(int i = 1 ; i < grid.Length ; i++)
		{
			Row row = new Row();
			row.DESC = grid[i][0];
			row.id = grid[i][1];
			row.Name = grid[i][2];
			row.Type = grid[i][3];
			row.SubType = grid[i][4];
			row.Rangetype = grid[i][5];
			row.Sprite = grid[i][6];
			row.EquipSprite = grid[i][7];
			row.ArrowSprite = grid[i][8];
			row.ArrowSpeed = grid[i][9];
			row.ArrowSize = grid[i][10];
			row.ArrowRotation = grid[i][11];
			row.HitEffect = grid[i][12];
			row.HitSound = grid[i][13];
			row.HitCount = grid[i][14];
			row.Tier = grid[i][15];
			row.BattlePoint = grid[i][16];
			row.Rare = grid[i][17];
			row.RarePercent = grid[i][18];
			row.RareCoung = grid[i][19];
			row.RanSmeltCount = grid[i][20];
			row.SmeltUpStat = grid[i][21];
			row.craftrare = grid[i][22];
			row.craftrarelist = grid[i][23];
			row.material = grid[i][24];
			row.attacktype = grid[i][25];
			row.minatk = grid[i][26];
			row.maxatk = grid[i][27];
			row.atkspeed = grid[i][28];
			row.castspeed = grid[i][29];
			row.matk = grid[i][30];
			row.def = grid[i][31];
			row.mdef = grid[i][32];
			row.Str = grid[i][33];
			row.Dex = grid[i][34];
			row.Int = grid[i][35];
			row.Wis = grid[i][36];
			row.AllStat = grid[i][37];
			row.AdvanStat = grid[i][38];
			row.Hit = grid[i][39];
			row.Exp = grid[i][40];
			row.Gold = grid[i][41];
			row.Hp = grid[i][42];
			row.Mp = grid[i][43];
			row.Crit = grid[i][44];
			row.CritDmg = grid[i][45];
			row.MinAllDmg = grid[i][46];
			row.MaxAllDmg = grid[i][47];
			row.RanStatId = grid[i][48];
			row.SetID = grid[i][49];
			row.SpeMehod = grid[i][50];
			row.SpeMehodP = grid[i][51];
			row.SuccSpe = grid[i][52];
			row.istwohand = grid[i][53];
			row.smeltid = grid[i][54];
			row.smeltcount = grid[i][55];
			row.maxdotcount = grid[i][56];
			row.dotdmgup = grid[i][57];
			row.AdvanEquipID = grid[i][58];
			row.AdvanNeedItem = grid[i][59];
			row.AdvanNeedItemHowmany = grid[i][60];
			row.AdvanGold = grid[i][61];
			row.AdvanPercent = grid[i][62];
			row.ChangeId = grid[i][63];
			row.UpgradeNeedID = grid[i][64];

			rowList.Add(row);
		}
		isLoaded = true;
	}

	public int NumRows()
	{
		return rowList.Count;
	}

	public Row GetAt(int i)
	{
		if(rowList.Count <= i)
			return null;
		return rowList[i];
	}

	public Row Find_DESC(string find)
	{
		return rowList.Find(x => x.DESC == find);
	}
	public List<Row> FindAll_DESC(string find)
	{
		return rowList.FindAll(x => x.DESC == find);
	}
	public Row Find_id(string find)
	{
		return rowList.Find(x => x.id == find);
	}
	public List<Row> FindAll_id(string find)
	{
		return rowList.FindAll(x => x.id == find);
	}
	public Row Find_Name(string find)
	{
		return rowList.Find(x => x.Name == find);
	}
	public List<Row> FindAll_Name(string find)
	{
		return rowList.FindAll(x => x.Name == find);
	}
	public Row Find_Type(string find)
	{
		return rowList.Find(x => x.Type == find);
	}
	public List<Row> FindAll_Type(string find)
	{
		return rowList.FindAll(x => x.Type == find);
	}
	public Row Find_SubType(string find)
	{
		return rowList.Find(x => x.SubType == find);
	}
	public List<Row> FindAll_SubType(string find)
	{
		return rowList.FindAll(x => x.SubType == find);
	}
	public Row Find_Rangetype(string find)
	{
		return rowList.Find(x => x.Rangetype == find);
	}
	public List<Row> FindAll_Rangetype(string find)
	{
		return rowList.FindAll(x => x.Rangetype == find);
	}
	public Row Find_Sprite(string find)
	{
		return rowList.Find(x => x.Sprite == find);
	}
	public List<Row> FindAll_Sprite(string find)
	{
		return rowList.FindAll(x => x.Sprite == find);
	}
	public Row Find_EquipSprite(string find)
	{
		return rowList.Find(x => x.EquipSprite == find);
	}
	public List<Row> FindAll_EquipSprite(string find)
	{
		return rowList.FindAll(x => x.EquipSprite == find);
	}
	public Row Find_ArrowSprite(string find)
	{
		return rowList.Find(x => x.ArrowSprite == find);
	}
	public List<Row> FindAll_ArrowSprite(string find)
	{
		return rowList.FindAll(x => x.ArrowSprite == find);
	}
	public Row Find_ArrowSpeed(string find)
	{
		return rowList.Find(x => x.ArrowSpeed == find);
	}
	public List<Row> FindAll_ArrowSpeed(string find)
	{
		return rowList.FindAll(x => x.ArrowSpeed == find);
	}
	public Row Find_ArrowSize(string find)
	{
		return rowList.Find(x => x.ArrowSize == find);
	}
	public List<Row> FindAll_ArrowSize(string find)
	{
		return rowList.FindAll(x => x.ArrowSize == find);
	}
	public Row Find_ArrowRotation(string find)
	{
		return rowList.Find(x => x.ArrowRotation == find);
	}
	public List<Row> FindAll_ArrowRotation(string find)
	{
		return rowList.FindAll(x => x.ArrowRotation == find);
	}
	public Row Find_HitEffect(string find)
	{
		return rowList.Find(x => x.HitEffect == find);
	}
	public List<Row> FindAll_HitEffect(string find)
	{
		return rowList.FindAll(x => x.HitEffect == find);
	}
	public Row Find_HitSound(string find)
	{
		return rowList.Find(x => x.HitSound == find);
	}
	public List<Row> FindAll_HitSound(string find)
	{
		return rowList.FindAll(x => x.HitSound == find);
	}
	public Row Find_HitCount(string find)
	{
		return rowList.Find(x => x.HitCount == find);
	}
	public List<Row> FindAll_HitCount(string find)
	{
		return rowList.FindAll(x => x.HitCount == find);
	}
	public Row Find_Tier(string find)
	{
		return rowList.Find(x => x.Tier == find);
	}
	public List<Row> FindAll_Tier(string find)
	{
		return rowList.FindAll(x => x.Tier == find);
	}
	public Row Find_BattlePoint(string find)
	{
		return rowList.Find(x => x.BattlePoint == find);
	}
	public List<Row> FindAll_BattlePoint(string find)
	{
		return rowList.FindAll(x => x.BattlePoint == find);
	}
	public Row Find_Rare(string find)
	{
		return rowList.Find(x => x.Rare == find);
	}
	public List<Row> FindAll_Rare(string find)
	{
		return rowList.FindAll(x => x.Rare == find);
	}
	public Row Find_RarePercent(string find)
	{
		return rowList.Find(x => x.RarePercent == find);
	}
	public List<Row> FindAll_RarePercent(string find)
	{
		return rowList.FindAll(x => x.RarePercent == find);
	}
	public Row Find_RareCoung(string find)
	{
		return rowList.Find(x => x.RareCoung == find);
	}
	public List<Row> FindAll_RareCoung(string find)
	{
		return rowList.FindAll(x => x.RareCoung == find);
	}
	public Row Find_RanSmeltCount(string find)
	{
		return rowList.Find(x => x.RanSmeltCount == find);
	}
	public List<Row> FindAll_RanSmeltCount(string find)
	{
		return rowList.FindAll(x => x.RanSmeltCount == find);
	}
	public Row Find_SmeltUpStat(string find)
	{
		return rowList.Find(x => x.SmeltUpStat == find);
	}
	public List<Row> FindAll_SmeltUpStat(string find)
	{
		return rowList.FindAll(x => x.SmeltUpStat == find);
	}
	public Row Find_craftrare(string find)
	{
		return rowList.Find(x => x.craftrare == find);
	}
	public List<Row> FindAll_craftrare(string find)
	{
		return rowList.FindAll(x => x.craftrare == find);
	}
	public Row Find_craftrarelist(string find)
	{
		return rowList.Find(x => x.craftrarelist == find);
	}
	public List<Row> FindAll_craftrarelist(string find)
	{
		return rowList.FindAll(x => x.craftrarelist == find);
	}
	public Row Find_material(string find)
	{
		return rowList.Find(x => x.material == find);
	}
	public List<Row> FindAll_material(string find)
	{
		return rowList.FindAll(x => x.material == find);
	}
	public Row Find_attacktype(string find)
	{
		return rowList.Find(x => x.attacktype == find);
	}
	public List<Row> FindAll_attacktype(string find)
	{
		return rowList.FindAll(x => x.attacktype == find);
	}
	public Row Find_minatk(string find)
	{
		return rowList.Find(x => x.minatk == find);
	}
	public List<Row> FindAll_minatk(string find)
	{
		return rowList.FindAll(x => x.minatk == find);
	}
	public Row Find_maxatk(string find)
	{
		return rowList.Find(x => x.maxatk == find);
	}
	public List<Row> FindAll_maxatk(string find)
	{
		return rowList.FindAll(x => x.maxatk == find);
	}
	public Row Find_atkspeed(string find)
	{
		return rowList.Find(x => x.atkspeed == find);
	}
	public List<Row> FindAll_atkspeed(string find)
	{
		return rowList.FindAll(x => x.atkspeed == find);
	}
	public Row Find_castspeed(string find)
	{
		return rowList.Find(x => x.castspeed == find);
	}
	public List<Row> FindAll_castspeed(string find)
	{
		return rowList.FindAll(x => x.castspeed == find);
	}
	public Row Find_matk(string find)
	{
		return rowList.Find(x => x.matk == find);
	}
	public List<Row> FindAll_matk(string find)
	{
		return rowList.FindAll(x => x.matk == find);
	}
	public Row Find_def(string find)
	{
		return rowList.Find(x => x.def == find);
	}
	public List<Row> FindAll_def(string find)
	{
		return rowList.FindAll(x => x.def == find);
	}
	public Row Find_mdef(string find)
	{
		return rowList.Find(x => x.mdef == find);
	}
	public List<Row> FindAll_mdef(string find)
	{
		return rowList.FindAll(x => x.mdef == find);
	}
	public Row Find_Str(string find)
	{
		return rowList.Find(x => x.Str == find);
	}
	public List<Row> FindAll_Str(string find)
	{
		return rowList.FindAll(x => x.Str == find);
	}
	public Row Find_Dex(string find)
	{
		return rowList.Find(x => x.Dex == find);
	}
	public List<Row> FindAll_Dex(string find)
	{
		return rowList.FindAll(x => x.Dex == find);
	}
	public Row Find_Int(string find)
	{
		return rowList.Find(x => x.Int == find);
	}
	public List<Row> FindAll_Int(string find)
	{
		return rowList.FindAll(x => x.Int == find);
	}
	public Row Find_Wis(string find)
	{
		return rowList.Find(x => x.Wis == find);
	}
	public List<Row> FindAll_Wis(string find)
	{
		return rowList.FindAll(x => x.Wis == find);
	}
	public Row Find_AllStat(string find)
	{
		return rowList.Find(x => x.AllStat == find);
	}
	public List<Row> FindAll_AllStat(string find)
	{
		return rowList.FindAll(x => x.AllStat == find);
	}
	public Row Find_AdvanStat(string find)
	{
		return rowList.Find(x => x.AdvanStat == find);
	}
	public List<Row> FindAll_AdvanStat(string find)
	{
		return rowList.FindAll(x => x.AdvanStat == find);
	}
	public Row Find_Hit(string find)
	{
		return rowList.Find(x => x.Hit == find);
	}
	public List<Row> FindAll_Hit(string find)
	{
		return rowList.FindAll(x => x.Hit == find);
	}
	public Row Find_Exp(string find)
	{
		return rowList.Find(x => x.Exp == find);
	}
	public List<Row> FindAll_Exp(string find)
	{
		return rowList.FindAll(x => x.Exp == find);
	}
	public Row Find_Gold(string find)
	{
		return rowList.Find(x => x.Gold == find);
	}
	public List<Row> FindAll_Gold(string find)
	{
		return rowList.FindAll(x => x.Gold == find);
	}
	public Row Find_Hp(string find)
	{
		return rowList.Find(x => x.Hp == find);
	}
	public List<Row> FindAll_Hp(string find)
	{
		return rowList.FindAll(x => x.Hp == find);
	}
	public Row Find_Mp(string find)
	{
		return rowList.Find(x => x.Mp == find);
	}
	public List<Row> FindAll_Mp(string find)
	{
		return rowList.FindAll(x => x.Mp == find);
	}
	public Row Find_Crit(string find)
	{
		return rowList.Find(x => x.Crit == find);
	}
	public List<Row> FindAll_Crit(string find)
	{
		return rowList.FindAll(x => x.Crit == find);
	}
	public Row Find_CritDmg(string find)
	{
		return rowList.Find(x => x.CritDmg == find);
	}
	public List<Row> FindAll_CritDmg(string find)
	{
		return rowList.FindAll(x => x.CritDmg == find);
	}
	public Row Find_MinAllDmg(string find)
	{
		return rowList.Find(x => x.MinAllDmg == find);
	}
	public List<Row> FindAll_MinAllDmg(string find)
	{
		return rowList.FindAll(x => x.MinAllDmg == find);
	}
	public Row Find_MaxAllDmg(string find)
	{
		return rowList.Find(x => x.MaxAllDmg == find);
	}
	public List<Row> FindAll_MaxAllDmg(string find)
	{
		return rowList.FindAll(x => x.MaxAllDmg == find);
	}
	public Row Find_RanStatId(string find)
	{
		return rowList.Find(x => x.RanStatId == find);
	}
	public List<Row> FindAll_RanStatId(string find)
	{
		return rowList.FindAll(x => x.RanStatId == find);
	}
	public Row Find_SetID(string find)
	{
		return rowList.Find(x => x.SetID == find);
	}
	public List<Row> FindAll_SetID(string find)
	{
		return rowList.FindAll(x => x.SetID == find);
	}
	public Row Find_SpeMehod(string find)
	{
		return rowList.Find(x => x.SpeMehod == find);
	}
	public List<Row> FindAll_SpeMehod(string find)
	{
		return rowList.FindAll(x => x.SpeMehod == find);
	}
	public Row Find_SpeMehodP(string find)
	{
		return rowList.Find(x => x.SpeMehodP == find);
	}
	public List<Row> FindAll_SpeMehodP(string find)
	{
		return rowList.FindAll(x => x.SpeMehodP == find);
	}
	public Row Find_SuccSpe(string find)
	{
		return rowList.Find(x => x.SuccSpe == find);
	}
	public List<Row> FindAll_SuccSpe(string find)
	{
		return rowList.FindAll(x => x.SuccSpe == find);
	}
	public Row Find_istwohand(string find)
	{
		return rowList.Find(x => x.istwohand == find);
	}
	public List<Row> FindAll_istwohand(string find)
	{
		return rowList.FindAll(x => x.istwohand == find);
	}
	public Row Find_smeltid(string find)
	{
		return rowList.Find(x => x.smeltid == find);
	}
	public List<Row> FindAll_smeltid(string find)
	{
		return rowList.FindAll(x => x.smeltid == find);
	}
	public Row Find_smeltcount(string find)
	{
		return rowList.Find(x => x.smeltcount == find);
	}
	public List<Row> FindAll_smeltcount(string find)
	{
		return rowList.FindAll(x => x.smeltcount == find);
	}
	public Row Find_maxdotcount(string find)
	{
		return rowList.Find(x => x.maxdotcount == find);
	}
	public List<Row> FindAll_maxdotcount(string find)
	{
		return rowList.FindAll(x => x.maxdotcount == find);
	}
	public Row Find_dotdmgup(string find)
	{
		return rowList.Find(x => x.dotdmgup == find);
	}
	public List<Row> FindAll_dotdmgup(string find)
	{
		return rowList.FindAll(x => x.dotdmgup == find);
	}
	public Row Find_AdvanEquipID(string find)
	{
		return rowList.Find(x => x.AdvanEquipID == find);
	}
	public List<Row> FindAll_AdvanEquipID(string find)
	{
		return rowList.FindAll(x => x.AdvanEquipID == find);
	}
	public Row Find_AdvanNeedItem(string find)
	{
		return rowList.Find(x => x.AdvanNeedItem == find);
	}
	public List<Row> FindAll_AdvanNeedItem(string find)
	{
		return rowList.FindAll(x => x.AdvanNeedItem == find);
	}
	public Row Find_AdvanNeedItemHowmany(string find)
	{
		return rowList.Find(x => x.AdvanNeedItemHowmany == find);
	}
	public List<Row> FindAll_AdvanNeedItemHowmany(string find)
	{
		return rowList.FindAll(x => x.AdvanNeedItemHowmany == find);
	}
	public Row Find_AdvanGold(string find)
	{
		return rowList.Find(x => x.AdvanGold == find);
	}
	public List<Row> FindAll_AdvanGold(string find)
	{
		return rowList.FindAll(x => x.AdvanGold == find);
	}
	public Row Find_AdvanPercent(string find)
	{
		return rowList.Find(x => x.AdvanPercent == find);
	}
	public List<Row> FindAll_AdvanPercent(string find)
	{
		return rowList.FindAll(x => x.AdvanPercent == find);
	}
	public Row Find_ChangeId(string find)
	{
		return rowList.Find(x => x.ChangeId == find);
	}
	public List<Row> FindAll_ChangeId(string find)
	{
		return rowList.FindAll(x => x.ChangeId == find);
	}
	public Row Find_UpgradeNeedID(string find)
	{
		return rowList.Find(x => x.UpgradeNeedID == find);
	}
	public List<Row> FindAll_UpgradeNeedID(string find)
	{
		return rowList.FindAll(x => x.UpgradeNeedID == find);
	}

}