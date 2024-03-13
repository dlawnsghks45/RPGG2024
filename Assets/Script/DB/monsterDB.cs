// This code automatically generated by TableCodeGen
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class monsterDB : MonoBehaviour
{
    
    public TextAsset ItemDatabase;

   
    public void Awake()
    {
        ItemDatabase = Resources.Load("CSV/monster") as TextAsset;
        Load(ItemDatabase);
    }

    //싱글톤만들기. 변경
    private static monsterDB _instance = null;
    public static monsterDB Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(monsterDB)) as monsterDB;

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
		public string DescriptionKor;
		public string mapname;
		public string name;
		public string hp;
		public string dmg;
		public string crit;
		public string attackcount;
		public string attacktime;
		public string montype;
		public string sprite;
		public string dropid;
		public string bossdrop;
		public string breakPoint;
		public string breakTime;
		public string breaknewstart;
		public string breakadddmg;
		public string israge;
		public string ragepercent;
		public string ispenalty;

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
			row.DescriptionKor = grid[i][1];
			row.mapname = grid[i][2];
			row.name = grid[i][3];
			row.hp = grid[i][4];
			row.dmg = grid[i][5];
			row.crit = grid[i][6];
			row.attackcount = grid[i][7];
			row.attacktime = grid[i][8];
			row.montype = grid[i][9];
			row.sprite = grid[i][10];
			row.dropid = grid[i][11];
			row.bossdrop = grid[i][12];
			row.breakPoint = grid[i][13];
			row.breakTime = grid[i][14];
			row.breaknewstart = grid[i][15];
			row.breakadddmg = grid[i][16];
			row.israge = grid[i][17];
			row.ragepercent = grid[i][18];
			row.ispenalty = grid[i][19];

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
	public Row Find_DescriptionKor(string find)
	{
		return rowList.Find(x => x.DescriptionKor == find);
	}
	public List<Row> FindAll_DescriptionKor(string find)
	{
		return rowList.FindAll(x => x.DescriptionKor == find);
	}
	public Row Find_mapname(string find)
	{
		return rowList.Find(x => x.mapname == find);
	}
	public List<Row> FindAll_mapname(string find)
	{
		return rowList.FindAll(x => x.mapname == find);
	}
	public Row Find_name(string find)
	{
		return rowList.Find(x => x.name == find);
	}
	public List<Row> FindAll_name(string find)
	{
		return rowList.FindAll(x => x.name == find);
	}
	public Row Find_hp(string find)
	{
		return rowList.Find(x => x.hp == find);
	}
	public List<Row> FindAll_hp(string find)
	{
		return rowList.FindAll(x => x.hp == find);
	}
	public Row Find_dmg(string find)
	{
		return rowList.Find(x => x.dmg == find);
	}
	public List<Row> FindAll_dmg(string find)
	{
		return rowList.FindAll(x => x.dmg == find);
	}
	public Row Find_crit(string find)
	{
		return rowList.Find(x => x.crit == find);
	}
	public List<Row> FindAll_crit(string find)
	{
		return rowList.FindAll(x => x.crit == find);
	}
	public Row Find_attackcount(string find)
	{
		return rowList.Find(x => x.attackcount == find);
	}
	public List<Row> FindAll_attackcount(string find)
	{
		return rowList.FindAll(x => x.attackcount == find);
	}
	public Row Find_attacktime(string find)
	{
		return rowList.Find(x => x.attacktime == find);
	}
	public List<Row> FindAll_attacktime(string find)
	{
		return rowList.FindAll(x => x.attacktime == find);
	}
	public Row Find_montype(string find)
	{
		return rowList.Find(x => x.montype == find);
	}
	public List<Row> FindAll_montype(string find)
	{
		return rowList.FindAll(x => x.montype == find);
	}
	public Row Find_sprite(string find)
	{
		return rowList.Find(x => x.sprite == find);
	}
	public List<Row> FindAll_sprite(string find)
	{
		return rowList.FindAll(x => x.sprite == find);
	}
	public Row Find_dropid(string find)
	{
		return rowList.Find(x => x.dropid == find);
	}
	public List<Row> FindAll_dropid(string find)
	{
		return rowList.FindAll(x => x.dropid == find);
	}
	public Row Find_bossdrop(string find)
	{
		return rowList.Find(x => x.bossdrop == find);
	}
	public List<Row> FindAll_bossdrop(string find)
	{
		return rowList.FindAll(x => x.bossdrop == find);
	}
	public Row Find_breakPoint(string find)
	{
		return rowList.Find(x => x.breakPoint == find);
	}
	public List<Row> FindAll_breakPoint(string find)
	{
		return rowList.FindAll(x => x.breakPoint == find);
	}
	public Row Find_breakTime(string find)
	{
		return rowList.Find(x => x.breakTime == find);
	}
	public List<Row> FindAll_breakTime(string find)
	{
		return rowList.FindAll(x => x.breakTime == find);
	}
	public Row Find_breaknewstart(string find)
	{
		return rowList.Find(x => x.breaknewstart == find);
	}
	public List<Row> FindAll_breaknewstart(string find)
	{
		return rowList.FindAll(x => x.breaknewstart == find);
	}
	public Row Find_breakadddmg(string find)
	{
		return rowList.Find(x => x.breakadddmg == find);
	}
	public List<Row> FindAll_breakadddmg(string find)
	{
		return rowList.FindAll(x => x.breakadddmg == find);
	}
	public Row Find_israge(string find)
	{
		return rowList.Find(x => x.israge == find);
	}
	public List<Row> FindAll_israge(string find)
	{
		return rowList.FindAll(x => x.israge == find);
	}
	public Row Find_ragepercent(string find)
	{
		return rowList.Find(x => x.ragepercent == find);
	}
	public List<Row> FindAll_ragepercent(string find)
	{
		return rowList.FindAll(x => x.ragepercent == find);
	}
	public Row Find_ispenalty(string find)
	{
		return rowList.Find(x => x.ispenalty == find);
	}
	public List<Row> FindAll_ispenalty(string find)
	{
		return rowList.FindAll(x => x.ispenalty == find);
	}

}