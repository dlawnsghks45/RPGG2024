// This code automatically generated by TableCodeGen
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquipSkillDB : MonoBehaviour
{
    
    public TextAsset ItemDatabase;

   
    public void Awake()
    {
        ItemDatabase = Resources.Load("CSV/EquipSkill") as TextAsset;
        Load(ItemDatabase);
    }

    //싱글톤만들기. 변경
    private static EquipSkillDB _instance = null;
    public static EquipSkillDB Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(EquipSkillDB)) as EquipSkillDB;

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
		public string coreid;
		public string description;
		public string description2;
		public string name;
		public string info;
		public string lv;
		public string maxlv;
		public string probability;
		public string value;
		public string c;
		public string type;
		public string subtype;
		public string rare;
		public string isstack;
		public string isset;
		public string isusebar;

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
			row.coreid = grid[i][1];
			row.description = grid[i][2];
			row.description2 = grid[i][3];
			row.name = grid[i][4];
			row.info = grid[i][5];
			row.lv = grid[i][6];
			row.maxlv = grid[i][7];
			row.probability = grid[i][8];
			row.value = grid[i][9];
			row.c = grid[i][10];
			row.type = grid[i][11];
			row.subtype = grid[i][12];
			row.rare = grid[i][13];
			row.isstack = grid[i][14];
			row.isset = grid[i][15];
			row.isusebar = grid[i][16];

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
	public Row Find_coreid(string find)
	{
		return rowList.Find(x => x.coreid == find);
	}
	public List<Row> FindAll_coreid(string find)
	{
		return rowList.FindAll(x => x.coreid == find);
	}
	public Row Find_description(string find)
	{
		return rowList.Find(x => x.description == find);
	}
	public List<Row> FindAll_description(string find)
	{
		return rowList.FindAll(x => x.description == find);
	}
	public Row Find_description2(string find)
	{
		return rowList.Find(x => x.description2 == find);
	}
	public List<Row> FindAll_description2(string find)
	{
		return rowList.FindAll(x => x.description2 == find);
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
	public Row Find_lv(string find)
	{
		return rowList.Find(x => x.lv == find);
	}
	public List<Row> FindAll_lv(string find)
	{
		return rowList.FindAll(x => x.lv == find);
	}
	public Row Find_maxlv(string find)
	{
		return rowList.Find(x => x.maxlv == find);
	}
	public List<Row> FindAll_maxlv(string find)
	{
		return rowList.FindAll(x => x.maxlv == find);
	}
	public Row Find_probability(string find)
	{
		return rowList.Find(x => x.probability == find);
	}
	public List<Row> FindAll_probability(string find)
	{
		return rowList.FindAll(x => x.probability == find);
	}
	public Row Find_value(string find)
	{
		return rowList.Find(x => x.value == find);
	}
	public List<Row> FindAll_value(string find)
	{
		return rowList.FindAll(x => x.value == find);
	}
	public Row Find_c(string find)
	{
		return rowList.Find(x => x.c == find);
	}
	public List<Row> FindAll_c(string find)
	{
		return rowList.FindAll(x => x.c == find);
	}
	public Row Find_type(string find)
	{
		return rowList.Find(x => x.type == find);
	}
	public List<Row> FindAll_type(string find)
	{
		return rowList.FindAll(x => x.type == find);
	}
	public Row Find_subtype(string find)
	{
		return rowList.Find(x => x.subtype == find);
	}
	public List<Row> FindAll_subtype(string find)
	{
		return rowList.FindAll(x => x.subtype == find);
	}
	public Row Find_rare(string find)
	{
		return rowList.Find(x => x.rare == find);
	}
	public List<Row> FindAll_rare(string find)
	{
		return rowList.FindAll(x => x.rare == find);
	}
	public Row Find_isstack(string find)
	{
		return rowList.Find(x => x.isstack == find);
	}
	public List<Row> FindAll_isstack(string find)
	{
		return rowList.FindAll(x => x.isstack == find);
	}
	public Row Find_isset(string find)
	{
		return rowList.Find(x => x.isset == find);
	}
	public List<Row> FindAll_isset(string find)
	{
		return rowList.FindAll(x => x.isset == find);
	}
	public Row Find_isusebar(string find)
	{
		return rowList.Find(x => x.isusebar == find);
	}
	public List<Row> FindAll_isusebar(string find)
	{
		return rowList.FindAll(x => x.isusebar == find);
	}

}