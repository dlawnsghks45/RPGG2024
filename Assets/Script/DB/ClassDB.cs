// This code automatically generated by TableCodeGen
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClassDB : MonoBehaviour
{
    
    public TextAsset ItemDatabase;

   
    public void Awake()
    {
        ItemDatabase = Resources.Load("CSV/Class") as TextAsset;
        Load(ItemDatabase);
    }

    //싱글톤만들기. 변경
    private static ClassDB _instance = null;
    public static ClassDB Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(ClassDB)) as ClassDB;

                if (_instance == null)
                {
                }
            }
            return _instance;
        }
    }


	public class Row
	{
        		public string id;
		public string Description;
		public string name;
		public string info;
		public string classsprite;
		public string tier;
		public string Hp;
		public string Mp;
		public string strperlv;
		public string dexperlv;
		public string intperlv;
		public string wisperlv;
		public string hitperlv;
		public string hpperlv;
		public string mpperlv;
		public string passive;
		public string standweapon;
		public string standsubweapon;
		public string giveskill;
		public string skillslotcount;
		public string skillcastingcount;
		public string RequiredItemID;
		public string RequiredItemHowmany;
		public string NextClass;
		public string NeedPlayerRank;
		public string mainstat;

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
			row.id = grid[i][0];
			row.Description = grid[i][1];
			row.name = grid[i][2];
			row.info = grid[i][3];
			row.classsprite = grid[i][4];
			row.tier = grid[i][5];
			row.Hp = grid[i][6];
			row.Mp = grid[i][7];
			row.strperlv = grid[i][8];
			row.dexperlv = grid[i][9];
			row.intperlv = grid[i][10];
			row.wisperlv = grid[i][11];
			row.hitperlv = grid[i][12];
			row.hpperlv = grid[i][13];
			row.mpperlv = grid[i][14];
			row.passive = grid[i][15];
			row.standweapon = grid[i][16];
			row.standsubweapon = grid[i][17];
			row.giveskill = grid[i][18];
			row.skillslotcount = grid[i][19];
			row.skillcastingcount = grid[i][20];
			row.RequiredItemID = grid[i][21];
			row.RequiredItemHowmany = grid[i][22];
			row.NextClass = grid[i][23];
			row.NeedPlayerRank = grid[i][24];
			row.mainstat = grid[i][25];

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

	public Row Find_id(string find)
	{
		return rowList.Find(x => x.id == find);
	}
	public List<Row> FindAll_id(string find)
	{
		return rowList.FindAll(x => x.id == find);
	}
	public Row Find_Description(string find)
	{
		return rowList.Find(x => x.Description == find);
	}
	public List<Row> FindAll_Description(string find)
	{
		return rowList.FindAll(x => x.Description == find);
	}
	public Row Find_name(string find)
	{
		return rowList.Find(x => x.name == find);
	}
	public List<Row> FindAll_name(string find)
	{
		return rowList.FindAll(x => x.name == find);
	}
	public Row Find_info(string find)
	{
		return rowList.Find(x => x.info == find);
	}
	public List<Row> FindAll_info(string find)
	{
		return rowList.FindAll(x => x.info == find);
	}
	public Row Find_classsprite(string find)
	{
		return rowList.Find(x => x.classsprite == find);
	}
	public List<Row> FindAll_classsprite(string find)
	{
		return rowList.FindAll(x => x.classsprite == find);
	}
	public Row Find_tier(string find)
	{
		return rowList.Find(x => x.tier == find);
	}
	public List<Row> FindAll_tier(string find)
	{
		return rowList.FindAll(x => x.tier == find);
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
	public Row Find_strperlv(string find)
	{
		return rowList.Find(x => x.strperlv == find);
	}
	public List<Row> FindAll_strperlv(string find)
	{
		return rowList.FindAll(x => x.strperlv == find);
	}
	public Row Find_dexperlv(string find)
	{
		return rowList.Find(x => x.dexperlv == find);
	}
	public List<Row> FindAll_dexperlv(string find)
	{
		return rowList.FindAll(x => x.dexperlv == find);
	}
	public Row Find_intperlv(string find)
	{
		return rowList.Find(x => x.intperlv == find);
	}
	public List<Row> FindAll_intperlv(string find)
	{
		return rowList.FindAll(x => x.intperlv == find);
	}
	public Row Find_wisperlv(string find)
	{
		return rowList.Find(x => x.wisperlv == find);
	}
	public List<Row> FindAll_wisperlv(string find)
	{
		return rowList.FindAll(x => x.wisperlv == find);
	}
	public Row Find_hitperlv(string find)
	{
		return rowList.Find(x => x.hitperlv == find);
	}
	public List<Row> FindAll_hitperlv(string find)
	{
		return rowList.FindAll(x => x.hitperlv == find);
	}
	public Row Find_hpperlv(string find)
	{
		return rowList.Find(x => x.hpperlv == find);
	}
	public List<Row> FindAll_hpperlv(string find)
	{
		return rowList.FindAll(x => x.hpperlv == find);
	}
	public Row Find_mpperlv(string find)
	{
		return rowList.Find(x => x.mpperlv == find);
	}
	public List<Row> FindAll_mpperlv(string find)
	{
		return rowList.FindAll(x => x.mpperlv == find);
	}
	public Row Find_passive(string find)
	{
		return rowList.Find(x => x.passive == find);
	}
	public List<Row> FindAll_passive(string find)
	{
		return rowList.FindAll(x => x.passive == find);
	}
	public Row Find_standweapon(string find)
	{
		return rowList.Find(x => x.standweapon == find);
	}
	public List<Row> FindAll_standweapon(string find)
	{
		return rowList.FindAll(x => x.standweapon == find);
	}
	public Row Find_standsubweapon(string find)
	{
		return rowList.Find(x => x.standsubweapon == find);
	}
	public List<Row> FindAll_standsubweapon(string find)
	{
		return rowList.FindAll(x => x.standsubweapon == find);
	}
	public Row Find_giveskill(string find)
	{
		return rowList.Find(x => x.giveskill == find);
	}
	public List<Row> FindAll_giveskill(string find)
	{
		return rowList.FindAll(x => x.giveskill == find);
	}
	public Row Find_skillslotcount(string find)
	{
		return rowList.Find(x => x.skillslotcount == find);
	}
	public List<Row> FindAll_skillslotcount(string find)
	{
		return rowList.FindAll(x => x.skillslotcount == find);
	}
	public Row Find_skillcastingcount(string find)
	{
		return rowList.Find(x => x.skillcastingcount == find);
	}
	public List<Row> FindAll_skillcastingcount(string find)
	{
		return rowList.FindAll(x => x.skillcastingcount == find);
	}
	public Row Find_RequiredItemID(string find)
	{
		return rowList.Find(x => x.RequiredItemID == find);
	}
	public List<Row> FindAll_RequiredItemID(string find)
	{
		return rowList.FindAll(x => x.RequiredItemID == find);
	}
	public Row Find_RequiredItemHowmany(string find)
	{
		return rowList.Find(x => x.RequiredItemHowmany == find);
	}
	public List<Row> FindAll_RequiredItemHowmany(string find)
	{
		return rowList.FindAll(x => x.RequiredItemHowmany == find);
	}
	public Row Find_NextClass(string find)
	{
		return rowList.Find(x => x.NextClass == find);
	}
	public List<Row> FindAll_NextClass(string find)
	{
		return rowList.FindAll(x => x.NextClass == find);
	}
	public Row Find_NeedPlayerRank(string find)
	{
		return rowList.Find(x => x.NeedPlayerRank == find);
	}
	public List<Row> FindAll_NeedPlayerRank(string find)
	{
		return rowList.FindAll(x => x.NeedPlayerRank == find);
	}
	public Row Find_mainstat(string find)
	{
		return rowList.Find(x => x.mainstat == find);
	}
	public List<Row> FindAll_mainstat(string find)
	{
		return rowList.FindAll(x => x.mainstat == find);
	}

}