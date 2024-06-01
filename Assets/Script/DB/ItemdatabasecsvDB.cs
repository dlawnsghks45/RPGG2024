// This code automatically generated by TableCodeGen
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemdatabasecsvDB : MonoBehaviour
{
    
    public TextAsset ItemDatabase;

   
    public void Awake()
    {
        ItemDatabase = Resources.Load("CSV/Itemdatabasecsv") as TextAsset;
        Load(ItemDatabase);
    }

    //싱글톤만들기. 변경
    private static ItemdatabasecsvDB _instance = null;
    public static ItemdatabasecsvDB Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(ItemdatabasecsvDB)) as ItemdatabasecsvDB;

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
		public string descriptionKor;
		public string sprite;
		public string name;
		public string description;
		public string itemtype;
		public string itemsubtype;
		public string rare;
		public string Recotype;
		public string droprare;
		public string buy;
		public string sell;
		public string dropalert;
		public string itemiconnew;
		public string A;
		public string B;
		public string C;
		public string D;
		public string Isstack;
		public string IsCrafting;
		public string IsEquipBox;

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
			row.descriptionKor = grid[i][1];
			row.sprite = grid[i][2];
			row.name = grid[i][3];
			row.description = grid[i][4];
			row.itemtype = grid[i][5];
			row.itemsubtype = grid[i][6];
			row.rare = grid[i][7];
			row.Recotype = grid[i][8];
			row.droprare = grid[i][9];
			row.buy = grid[i][10];
			row.sell = grid[i][11];
			row.dropalert = grid[i][12];
			row.itemiconnew = grid[i][13];
			row.A = grid[i][14];
			row.B = grid[i][15];
			row.C = grid[i][16];
			row.D = grid[i][17];
			row.Isstack = grid[i][18];
			row.IsCrafting = grid[i][19];
			row.IsEquipBox = grid[i][20];

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
	public Row Find_descriptionKor(string find)
	{
		return rowList.Find(x => x.descriptionKor == find);
	}
	public List<Row> FindAll_descriptionKor(string find)
	{
		return rowList.FindAll(x => x.descriptionKor == find);
	}
	public Row Find_sprite(string find)
	{
		return rowList.Find(x => x.sprite == find);
	}
	public List<Row> FindAll_sprite(string find)
	{
		return rowList.FindAll(x => x.sprite == find);
	}
	public Row Find_name(string find)
	{
		return rowList.Find(x => x.name == find);
	}
	public List<Row> FindAll_name(string find)
	{
		return rowList.FindAll(x => x.name == find);
	}
	public Row Find_description(string find)
	{
		return rowList.Find(x => x.description == find);
	}
	public List<Row> FindAll_description(string find)
	{
		return rowList.FindAll(x => x.description == find);
	}
	public Row Find_itemtype(string find)
	{
		return rowList.Find(x => x.itemtype == find);
	}
	public List<Row> FindAll_itemtype(string find)
	{
		return rowList.FindAll(x => x.itemtype == find);
	}
	public Row Find_itemsubtype(string find)
	{
		return rowList.Find(x => x.itemsubtype == find);
	}
	public List<Row> FindAll_itemsubtype(string find)
	{
		return rowList.FindAll(x => x.itemsubtype == find);
	}
	public Row Find_rare(string find)
	{
		return rowList.Find(x => x.rare == find);
	}
	public List<Row> FindAll_rare(string find)
	{
		return rowList.FindAll(x => x.rare == find);
	}
	public Row Find_Recotype(string find)
	{
		return rowList.Find(x => x.Recotype == find);
	}
	public List<Row> FindAll_Recotype(string find)
	{
		return rowList.FindAll(x => x.Recotype == find);
	}
	public Row Find_droprare(string find)
	{
		return rowList.Find(x => x.droprare == find);
	}
	public List<Row> FindAll_droprare(string find)
	{
		return rowList.FindAll(x => x.droprare == find);
	}
	public Row Find_buy(string find)
	{
		return rowList.Find(x => x.buy == find);
	}
	public List<Row> FindAll_buy(string find)
	{
		return rowList.FindAll(x => x.buy == find);
	}
	public Row Find_sell(string find)
	{
		return rowList.Find(x => x.sell == find);
	}
	public List<Row> FindAll_sell(string find)
	{
		return rowList.FindAll(x => x.sell == find);
	}
	public Row Find_dropalert(string find)
	{
		return rowList.Find(x => x.dropalert == find);
	}
	public List<Row> FindAll_dropalert(string find)
	{
		return rowList.FindAll(x => x.dropalert == find);
	}
	public Row Find_itemiconnew(string find)
	{
		return rowList.Find(x => x.itemiconnew == find);
	}
	public List<Row> FindAll_itemiconnew(string find)
	{
		return rowList.FindAll(x => x.itemiconnew == find);
	}
	public Row Find_A(string find)
	{
		return rowList.Find(x => x.A == find);
	}
	public List<Row> FindAll_A(string find)
	{
		return rowList.FindAll(x => x.A == find);
	}
	public Row Find_B(string find)
	{
		return rowList.Find(x => x.B == find);
	}
	public List<Row> FindAll_B(string find)
	{
		return rowList.FindAll(x => x.B == find);
	}
	public Row Find_C(string find)
	{
		return rowList.Find(x => x.C == find);
	}
	public List<Row> FindAll_C(string find)
	{
		return rowList.FindAll(x => x.C == find);
	}
	public Row Find_D(string find)
	{
		return rowList.Find(x => x.D == find);
	}
	public List<Row> FindAll_D(string find)
	{
		return rowList.FindAll(x => x.D == find);
	}
	public Row Find_Isstack(string find)
	{
		return rowList.Find(x => x.Isstack == find);
	}
	public List<Row> FindAll_Isstack(string find)
	{
		return rowList.FindAll(x => x.Isstack == find);
	}
	public Row Find_IsCrafting(string find)
	{
		return rowList.Find(x => x.IsCrafting == find);
	}
	public List<Row> FindAll_IsCrafting(string find)
	{
		return rowList.FindAll(x => x.IsCrafting == find);
	}
	public Row Find_IsEquipBox(string find)
	{
		return rowList.Find(x => x.IsEquipBox == find);
	}
	public List<Row> FindAll_IsEquipBox(string find)
	{
		return rowList.FindAll(x => x.IsEquipBox == find);
	}

}