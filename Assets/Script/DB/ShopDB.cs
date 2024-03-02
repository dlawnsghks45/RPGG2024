// This code automatically generated by TableCodeGen
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopDB : MonoBehaviour
{
    
    public TextAsset ItemDatabase;

   
    public void Awake()
    {
        ItemDatabase = Resources.Load("CSV/Shop") as TextAsset;
        Load(ItemDatabase);
    }

    //싱글톤만들기. 변경
    private static ShopDB _instance = null;
    public static ShopDB Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(ShopDB)) as ShopDB;

                if (_instance == null)
                {
                }
            }
            return _instance;
        }
    }


	public class Row
	{
        		public string ids;
		public string namedes;
		public string infodes;
		public string name;
		public string info;
		public string items;
		public string howmanys;
		public string sprite;
		public string productid;
		public string price;
		public string producttype;
		public string enumname;
		public string isnew;

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
			row.ids = grid[i][0];
			row.namedes = grid[i][1];
			row.infodes = grid[i][2];
			row.name = grid[i][3];
			row.info = grid[i][4];
			row.items = grid[i][5];
			row.howmanys = grid[i][6];
			row.sprite = grid[i][7];
			row.productid = grid[i][8];
			row.price = grid[i][9];
			row.producttype = grid[i][10];
			row.enumname = grid[i][11];
			row.isnew = grid[i][12];

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

	public Row Find_ids(string find)
	{
		return rowList.Find(x => x.ids == find);
	}
	public List<Row> FindAll_ids(string find)
	{
		return rowList.FindAll(x => x.ids == find);
	}
	public Row Find_namedes(string find)
	{
		return rowList.Find(x => x.namedes == find);
	}
	public List<Row> FindAll_namedes(string find)
	{
		return rowList.FindAll(x => x.namedes == find);
	}
	public Row Find_infodes(string find)
	{
		return rowList.Find(x => x.infodes == find);
	}
	public List<Row> FindAll_infodes(string find)
	{
		return rowList.FindAll(x => x.infodes == find);
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
	public Row Find_items(string find)
	{
		return rowList.Find(x => x.items == find);
	}
	public List<Row> FindAll_items(string find)
	{
		return rowList.FindAll(x => x.items == find);
	}
	public Row Find_howmanys(string find)
	{
		return rowList.Find(x => x.howmanys == find);
	}
	public List<Row> FindAll_howmanys(string find)
	{
		return rowList.FindAll(x => x.howmanys == find);
	}
	public Row Find_sprite(string find)
	{
		return rowList.Find(x => x.sprite == find);
	}
	public List<Row> FindAll_sprite(string find)
	{
		return rowList.FindAll(x => x.sprite == find);
	}
	public Row Find_productid(string find)
	{
		return rowList.Find(x => x.productid == find);
	}
	public List<Row> FindAll_productid(string find)
	{
		return rowList.FindAll(x => x.productid == find);
	}
	public Row Find_price(string find)
	{
		return rowList.Find(x => x.price == find);
	}
	public List<Row> FindAll_price(string find)
	{
		return rowList.FindAll(x => x.price == find);
	}
	public Row Find_producttype(string find)
	{
		return rowList.Find(x => x.producttype == find);
	}
	public List<Row> FindAll_producttype(string find)
	{
		return rowList.FindAll(x => x.producttype == find);
	}
	public Row Find_enumname(string find)
	{
		return rowList.Find(x => x.enumname == find);
	}
	public List<Row> FindAll_enumname(string find)
	{
		return rowList.FindAll(x => x.enumname == find);
	}
	public Row Find_isnew(string find)
	{
		return rowList.Find(x => x.isnew == find);
	}
	public List<Row> FindAll_isnew(string find)
	{
		return rowList.FindAll(x => x.isnew == find);
	}

}