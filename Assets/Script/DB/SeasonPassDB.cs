// This code automatically generated by TableCodeGen
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SeasonPassDB : MonoBehaviour
{
    
    public TextAsset ItemDatabase;

   
    public void Awake()
    {
        ItemDatabase = Resources.Load("CSV/SeasonPass") as TextAsset;
        Load(ItemDatabase);
    }

    //싱글톤만들기. 변경
    private static SeasonPassDB _instance = null;
    public static SeasonPassDB Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(SeasonPassDB)) as SeasonPassDB;

                if (_instance == null)
                {
                }
            }
            return _instance;
        }
    }


	public class Row
	{
        		public string lv;
		public string BRid;
		public string BRhowmany;
		public string PRid;
		public string PRhowmany;

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
			row.lv = grid[i][0];
			row.BRid = grid[i][1];
			row.BRhowmany = grid[i][2];
			row.PRid = grid[i][3];
			row.PRhowmany = grid[i][4];

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

	public Row Find_lv(string find)
	{
		return rowList.Find(x => x.lv == find);
	}
	public List<Row> FindAll_lv(string find)
	{
		return rowList.FindAll(x => x.lv == find);
	}
	public Row Find_BRid(string find)
	{
		return rowList.Find(x => x.BRid == find);
	}
	public List<Row> FindAll_BRid(string find)
	{
		return rowList.FindAll(x => x.BRid == find);
	}
	public Row Find_BRhowmany(string find)
	{
		return rowList.Find(x => x.BRhowmany == find);
	}
	public List<Row> FindAll_BRhowmany(string find)
	{
		return rowList.FindAll(x => x.BRhowmany == find);
	}
	public Row Find_PRid(string find)
	{
		return rowList.Find(x => x.PRid == find);
	}
	public List<Row> FindAll_PRid(string find)
	{
		return rowList.FindAll(x => x.PRid == find);
	}
	public Row Find_PRhowmany(string find)
	{
		return rowList.Find(x => x.PRhowmany == find);
	}
	public List<Row> FindAll_PRhowmany(string find)
	{
		return rowList.FindAll(x => x.PRhowmany == find);
	}

}