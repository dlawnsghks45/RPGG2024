// This code automatically generated by TableCodeGen
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RouletteDB : MonoBehaviour
{
    
    public TextAsset ItemDatabase;

   
    public void Awake()
    {
        ItemDatabase = Resources.Load("CSV/Roulette") as TextAsset;
        Load(ItemDatabase);
    }

    //싱글톤만들기. 변경
    private static RouletteDB _instance = null;
    public static RouletteDB Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(RouletteDB)) as RouletteDB;

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
		public string description;
		public string name;
		public string num;
		public string needitem;
		public string needhowmany;
		public string reward;
		public string howmany;
		public string Chance;
		public string Rare;
		public string extragive;
		public string extragivehw;
		public string extragivecount;
		public string Percent;

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
			row.description = grid[i][1];
			row.name = grid[i][2];
			row.num = grid[i][3];
			row.needitem = grid[i][4];
			row.needhowmany = grid[i][5];
			row.reward = grid[i][6];
			row.howmany = grid[i][7];
			row.Chance = grid[i][8];
			row.Rare = grid[i][9];
			row.extragive = grid[i][10];
			row.extragivehw = grid[i][11];
			row.extragivecount = grid[i][12];
			row.Percent = grid[i][13];

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
	public Row Find_description(string find)
	{
		return rowList.Find(x => x.description == find);
	}
	public List<Row> FindAll_description(string find)
	{
		return rowList.FindAll(x => x.description == find);
	}
	public Row Find_name(string find)
	{
		return rowList.Find(x => x.name == find);
	}
	public List<Row> FindAll_name(string find)
	{
		return rowList.FindAll(x => x.name == find);
	}
	public Row Find_num(string find)
	{
		return rowList.Find(x => x.num == find);
	}
	public List<Row> FindAll_num(string find)
	{
		return rowList.FindAll(x => x.num == find);
	}
	public Row Find_needitem(string find)
	{
		return rowList.Find(x => x.needitem == find);
	}
	public List<Row> FindAll_needitem(string find)
	{
		return rowList.FindAll(x => x.needitem == find);
	}
	public Row Find_needhowmany(string find)
	{
		return rowList.Find(x => x.needhowmany == find);
	}
	public List<Row> FindAll_needhowmany(string find)
	{
		return rowList.FindAll(x => x.needhowmany == find);
	}
	public Row Find_reward(string find)
	{
		return rowList.Find(x => x.reward == find);
	}
	public List<Row> FindAll_reward(string find)
	{
		return rowList.FindAll(x => x.reward == find);
	}
	public Row Find_howmany(string find)
	{
		return rowList.Find(x => x.howmany == find);
	}
	public List<Row> FindAll_howmany(string find)
	{
		return rowList.FindAll(x => x.howmany == find);
	}
	public Row Find_Chance(string find)
	{
		return rowList.Find(x => x.Chance == find);
	}
	public List<Row> FindAll_Chance(string find)
	{
		return rowList.FindAll(x => x.Chance == find);
	}
	public Row Find_Rare(string find)
	{
		return rowList.Find(x => x.Rare == find);
	}
	public List<Row> FindAll_Rare(string find)
	{
		return rowList.FindAll(x => x.Rare == find);
	}
	public Row Find_extragive(string find)
	{
		return rowList.Find(x => x.extragive == find);
	}
	public List<Row> FindAll_extragive(string find)
	{
		return rowList.FindAll(x => x.extragive == find);
	}
	public Row Find_extragivehw(string find)
	{
		return rowList.Find(x => x.extragivehw == find);
	}
	public List<Row> FindAll_extragivehw(string find)
	{
		return rowList.FindAll(x => x.extragivehw == find);
	}
	public Row Find_extragivecount(string find)
	{
		return rowList.Find(x => x.extragivecount == find);
	}
	public List<Row> FindAll_extragivecount(string find)
	{
		return rowList.FindAll(x => x.extragivecount == find);
	}
	public Row Find_Percent(string find)
	{
		return rowList.Find(x => x.Percent == find);
	}
	public List<Row> FindAll_Percent(string find)
	{
		return rowList.FindAll(x => x.Percent == find);
	}

}