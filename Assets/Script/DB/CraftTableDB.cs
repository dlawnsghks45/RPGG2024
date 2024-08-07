// This code automatically generated by TableCodeGen
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CraftTableDB : MonoBehaviour
{
    
    public TextAsset ItemDatabase;

   
    public void Awake()
    {
        ItemDatabase = Resources.Load("CSV/CraftTable") as TextAsset;
        Load(ItemDatabase);
    }

    //싱글톤만들기. 변경
    private static CraftTableDB _instance = null;
    public static CraftTableDB Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(CraftTableDB)) as CraftTableDB;

                if (_instance == null)
                {
                }
            }
            return _instance;
        }
    }


	public class Row
	{
        		public string description;
		public string id;
		public string crafttime;
		public string maprank;
		public string isequip;
		public string ismakemany;
		public string PanelType;
		public string issuccessequip;
		public string Successid;
		public string MinSuccesshowmany;
		public string MaxSuccesshowmany;
		public string BigSuccessid;
		public string MinBigSuccesshowmany;
		public string MaxBigSuccesshowmany;
		public string FailId;
		public string MinFailHowmany;
		public string MaxFailHowmany;
		public string SuccessPercent;
		public string BigSuccessPercent;
		public string ResourceHowmany;
		public string needgold;
		public string A;
		public string AH;
		public string B;
		public string BH;
		public string C;
		public string CH;
		public string D;
		public string DH;
		public string E;
		public string EH;
		public string F;
		public string FH;

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
			row.description = grid[i][0];
			row.id = grid[i][1];
			row.crafttime = grid[i][2];
			row.maprank = grid[i][3];
			row.isequip = grid[i][4];
			row.ismakemany = grid[i][5];
			row.PanelType = grid[i][6];
			row.issuccessequip = grid[i][7];
			row.Successid = grid[i][8];
			row.MinSuccesshowmany = grid[i][9];
			row.MaxSuccesshowmany = grid[i][10];
			row.BigSuccessid = grid[i][11];
			row.MinBigSuccesshowmany = grid[i][12];
			row.MaxBigSuccesshowmany = grid[i][13];
			row.FailId = grid[i][14];
			row.MinFailHowmany = grid[i][15];
			row.MaxFailHowmany = grid[i][16];
			row.SuccessPercent = grid[i][17];
			row.BigSuccessPercent = grid[i][18];
			row.ResourceHowmany = grid[i][19];
			row.needgold = grid[i][20];
			row.A = grid[i][21];
			row.AH = grid[i][22];
			row.B = grid[i][23];
			row.BH = grid[i][24];
			row.C = grid[i][25];
			row.CH = grid[i][26];
			row.D = grid[i][27];
			row.DH = grid[i][28];
			row.E = grid[i][29];
			row.EH = grid[i][30];
			row.F = grid[i][31];
			row.FH = grid[i][32];

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

	public Row Find_description(string find)
	{
		return rowList.Find(x => x.description == find);
	}
	public List<Row> FindAll_description(string find)
	{
		return rowList.FindAll(x => x.description == find);
	}
	public Row Find_id(string find)
	{
		return rowList.Find(x => x.id == find);
	}
	public List<Row> FindAll_id(string find)
	{
		return rowList.FindAll(x => x.id == find);
	}
	public Row Find_crafttime(string find)
	{
		return rowList.Find(x => x.crafttime == find);
	}
	public List<Row> FindAll_crafttime(string find)
	{
		return rowList.FindAll(x => x.crafttime == find);
	}
	public Row Find_maprank(string find)
	{
		return rowList.Find(x => x.maprank == find);
	}
	public List<Row> FindAll_maprank(string find)
	{
		return rowList.FindAll(x => x.maprank == find);
	}
	public Row Find_isequip(string find)
	{
		return rowList.Find(x => x.isequip == find);
	}
	public List<Row> FindAll_isequip(string find)
	{
		return rowList.FindAll(x => x.isequip == find);
	}
	public Row Find_ismakemany(string find)
	{
		return rowList.Find(x => x.ismakemany == find);
	}
	public List<Row> FindAll_ismakemany(string find)
	{
		return rowList.FindAll(x => x.ismakemany == find);
	}
	public Row Find_PanelType(string find)
	{
		return rowList.Find(x => x.PanelType == find);
	}
	public List<Row> FindAll_PanelType(string find)
	{
		return rowList.FindAll(x => x.PanelType == find);
	}
	public Row Find_issuccessequip(string find)
	{
		return rowList.Find(x => x.issuccessequip == find);
	}
	public List<Row> FindAll_issuccessequip(string find)
	{
		return rowList.FindAll(x => x.issuccessequip == find);
	}
	public Row Find_Successid(string find)
	{
		return rowList.Find(x => x.Successid == find);
	}
	public List<Row> FindAll_Successid(string find)
	{
		return rowList.FindAll(x => x.Successid == find);
	}
	public Row Find_MinSuccesshowmany(string find)
	{
		return rowList.Find(x => x.MinSuccesshowmany == find);
	}
	public List<Row> FindAll_MinSuccesshowmany(string find)
	{
		return rowList.FindAll(x => x.MinSuccesshowmany == find);
	}
	public Row Find_MaxSuccesshowmany(string find)
	{
		return rowList.Find(x => x.MaxSuccesshowmany == find);
	}
	public List<Row> FindAll_MaxSuccesshowmany(string find)
	{
		return rowList.FindAll(x => x.MaxSuccesshowmany == find);
	}
	public Row Find_BigSuccessid(string find)
	{
		return rowList.Find(x => x.BigSuccessid == find);
	}
	public List<Row> FindAll_BigSuccessid(string find)
	{
		return rowList.FindAll(x => x.BigSuccessid == find);
	}
	public Row Find_MinBigSuccesshowmany(string find)
	{
		return rowList.Find(x => x.MinBigSuccesshowmany == find);
	}
	public List<Row> FindAll_MinBigSuccesshowmany(string find)
	{
		return rowList.FindAll(x => x.MinBigSuccesshowmany == find);
	}
	public Row Find_MaxBigSuccesshowmany(string find)
	{
		return rowList.Find(x => x.MaxBigSuccesshowmany == find);
	}
	public List<Row> FindAll_MaxBigSuccesshowmany(string find)
	{
		return rowList.FindAll(x => x.MaxBigSuccesshowmany == find);
	}
	public Row Find_FailId(string find)
	{
		return rowList.Find(x => x.FailId == find);
	}
	public List<Row> FindAll_FailId(string find)
	{
		return rowList.FindAll(x => x.FailId == find);
	}
	public Row Find_MinFailHowmany(string find)
	{
		return rowList.Find(x => x.MinFailHowmany == find);
	}
	public List<Row> FindAll_MinFailHowmany(string find)
	{
		return rowList.FindAll(x => x.MinFailHowmany == find);
	}
	public Row Find_MaxFailHowmany(string find)
	{
		return rowList.Find(x => x.MaxFailHowmany == find);
	}
	public List<Row> FindAll_MaxFailHowmany(string find)
	{
		return rowList.FindAll(x => x.MaxFailHowmany == find);
	}
	public Row Find_SuccessPercent(string find)
	{
		return rowList.Find(x => x.SuccessPercent == find);
	}
	public List<Row> FindAll_SuccessPercent(string find)
	{
		return rowList.FindAll(x => x.SuccessPercent == find);
	}
	public Row Find_BigSuccessPercent(string find)
	{
		return rowList.Find(x => x.BigSuccessPercent == find);
	}
	public List<Row> FindAll_BigSuccessPercent(string find)
	{
		return rowList.FindAll(x => x.BigSuccessPercent == find);
	}
	public Row Find_ResourceHowmany(string find)
	{
		return rowList.Find(x => x.ResourceHowmany == find);
	}
	public List<Row> FindAll_ResourceHowmany(string find)
	{
		return rowList.FindAll(x => x.ResourceHowmany == find);
	}
	public Row Find_needgold(string find)
	{
		return rowList.Find(x => x.needgold == find);
	}
	public List<Row> FindAll_needgold(string find)
	{
		return rowList.FindAll(x => x.needgold == find);
	}
	public Row Find_A(string find)
	{
		return rowList.Find(x => x.A == find);
	}
	public List<Row> FindAll_A(string find)
	{
		return rowList.FindAll(x => x.A == find);
	}
	public Row Find_AH(string find)
	{
		return rowList.Find(x => x.AH == find);
	}
	public List<Row> FindAll_AH(string find)
	{
		return rowList.FindAll(x => x.AH == find);
	}
	public Row Find_B(string find)
	{
		return rowList.Find(x => x.B == find);
	}
	public List<Row> FindAll_B(string find)
	{
		return rowList.FindAll(x => x.B == find);
	}
	public Row Find_BH(string find)
	{
		return rowList.Find(x => x.BH == find);
	}
	public List<Row> FindAll_BH(string find)
	{
		return rowList.FindAll(x => x.BH == find);
	}
	public Row Find_C(string find)
	{
		return rowList.Find(x => x.C == find);
	}
	public List<Row> FindAll_C(string find)
	{
		return rowList.FindAll(x => x.C == find);
	}
	public Row Find_CH(string find)
	{
		return rowList.Find(x => x.CH == find);
	}
	public List<Row> FindAll_CH(string find)
	{
		return rowList.FindAll(x => x.CH == find);
	}
	public Row Find_D(string find)
	{
		return rowList.Find(x => x.D == find);
	}
	public List<Row> FindAll_D(string find)
	{
		return rowList.FindAll(x => x.D == find);
	}
	public Row Find_DH(string find)
	{
		return rowList.Find(x => x.DH == find);
	}
	public List<Row> FindAll_DH(string find)
	{
		return rowList.FindAll(x => x.DH == find);
	}
	public Row Find_E(string find)
	{
		return rowList.Find(x => x.E == find);
	}
	public List<Row> FindAll_E(string find)
	{
		return rowList.FindAll(x => x.E == find);
	}
	public Row Find_EH(string find)
	{
		return rowList.Find(x => x.EH == find);
	}
	public List<Row> FindAll_EH(string find)
	{
		return rowList.FindAll(x => x.EH == find);
	}
	public Row Find_F(string find)
	{
		return rowList.Find(x => x.F == find);
	}
	public List<Row> FindAll_F(string find)
	{
		return rowList.FindAll(x => x.F == find);
	}
	public Row Find_FH(string find)
	{
		return rowList.Find(x => x.FH == find);
	}
	public List<Row> FindAll_FH(string find)
	{
		return rowList.FindAll(x => x.FH == find);
	}

}