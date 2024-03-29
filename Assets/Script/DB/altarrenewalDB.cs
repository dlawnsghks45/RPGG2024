// This code automatically generated by TableCodeGen
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class altarrenewalDB : MonoBehaviour
{
    
    public TextAsset ItemDatabase;

   
    public void Awake()
    {
        ItemDatabase = Resources.Load("CSV/altarrenewal") as TextAsset;
        Load(ItemDatabase);
    }

    //싱글톤만들기. 변경
    private static altarrenewalDB _instance = null;
    public static altarrenewalDB Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(altarrenewalDB)) as altarrenewalDB;

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
		public string type;
		public string needid;
		public string lvscale;
		public string needhowmany;
		public string needtype;
		public string stat;
		public string percent;

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
			row.type = grid[i][1];
			row.needid = grid[i][2];
			row.lvscale = grid[i][3];
			row.needhowmany = grid[i][4];
			row.needtype = grid[i][5];
			row.stat = grid[i][6];
			row.percent = grid[i][7];

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
	public Row Find_type(string find)
	{
		return rowList.Find(x => x.type == find);
	}
	public List<Row> FindAll_type(string find)
	{
		return rowList.FindAll(x => x.type == find);
	}
	public Row Find_needid(string find)
	{
		return rowList.Find(x => x.needid == find);
	}
	public List<Row> FindAll_needid(string find)
	{
		return rowList.FindAll(x => x.needid == find);
	}
	public Row Find_lvscale(string find)
	{
		return rowList.Find(x => x.lvscale == find);
	}
	public List<Row> FindAll_lvscale(string find)
	{
		return rowList.FindAll(x => x.lvscale == find);
	}
	public Row Find_needhowmany(string find)
	{
		return rowList.Find(x => x.needhowmany == find);
	}
	public List<Row> FindAll_needhowmany(string find)
	{
		return rowList.FindAll(x => x.needhowmany == find);
	}
	public Row Find_needtype(string find)
	{
		return rowList.Find(x => x.needtype == find);
	}
	public List<Row> FindAll_needtype(string find)
	{
		return rowList.FindAll(x => x.needtype == find);
	}
	public Row Find_stat(string find)
	{
		return rowList.Find(x => x.stat == find);
	}
	public List<Row> FindAll_stat(string find)
	{
		return rowList.FindAll(x => x.stat == find);
	}
	public Row Find_percent(string find)
	{
		return rowList.Find(x => x.percent == find);
	}
	public List<Row> FindAll_percent(string find)
	{
		return rowList.FindAll(x => x.percent == find);
	}

}