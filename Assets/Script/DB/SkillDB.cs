// This code automatically generated by TableCodeGen
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillDB : MonoBehaviour
{
    
    public TextAsset ItemDatabase;

   
    public void Awake()
    {
        ItemDatabase = Resources.Load("CSV/Skill") as TextAsset;
        Load(ItemDatabase);
    }

    //싱글톤만들기. 변경
    private static SkillDB _instance = null;
    public static SkillDB Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(SkillDB)) as SkillDB;

                if (_instance == null)
                {
                }
            }
            return _instance;
        }
    }


	public class Row
	{
        		public string Id;
		public string SkillKorName;
		public string SkillClass;
		public string Name;
		public string Info;
		public string Sprite;
		public string Rare;
		public string AniArrow;
		public string JustArrow;
		public string ArrowSize;
		public string ArrowSpeed;
		public string ArrowRotation;
		public string ArrowShootCount;
		public string ArrowShootInterval;
		public string Type;
		public string SkillType;
		public string BuffType;
		public string RangeType;
		public string RangeTypeOld;
		public string SkillRange;
		public string AttackCount;
		public string Crit;
		public string CritDmg;
		public string Atk;
		public string Matk;
		public string HitEffect;
		public string PlayerEffect;
		public string Sound;
		public string CastTime;
		public string StartDmgTerm;
		public string CoolTime;
		public string UseHp;
		public string UseMp;
		public string breakpoint;
		public string DotType;
		public string DotCount;
		public string skillsort;

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
			row.Id = grid[i][0];
			row.SkillKorName = grid[i][1];
			row.SkillClass = grid[i][2];
			row.Name = grid[i][3];
			row.Info = grid[i][4];
			row.Sprite = grid[i][5];
			row.Rare = grid[i][6];
			row.AniArrow = grid[i][7];
			row.JustArrow = grid[i][8];
			row.ArrowSize = grid[i][9];
			row.ArrowSpeed = grid[i][10];
			row.ArrowRotation = grid[i][11];
			row.ArrowShootCount = grid[i][12];
			row.ArrowShootInterval = grid[i][13];
			row.Type = grid[i][14];
			row.SkillType = grid[i][15];
			row.BuffType = grid[i][16];
			row.RangeType = grid[i][17];
			row.RangeTypeOld = grid[i][18];
			row.SkillRange = grid[i][19];
			row.AttackCount = grid[i][20];
			row.Crit = grid[i][21];
			row.CritDmg = grid[i][22];
			row.Atk = grid[i][23];
			row.Matk = grid[i][24];
			row.HitEffect = grid[i][25];
			row.PlayerEffect = grid[i][26];
			row.Sound = grid[i][27];
			row.CastTime = grid[i][28];
			row.StartDmgTerm = grid[i][29];
			row.CoolTime = grid[i][30];
			row.UseHp = grid[i][31];
			row.UseMp = grid[i][32];
			row.breakpoint = grid[i][33];
			row.DotType = grid[i][34];
			row.DotCount = grid[i][35];
			row.skillsort = grid[i][36];

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

	public Row Find_Id(string find)
	{
		return rowList.Find(x => x.Id == find);
	}
	public List<Row> FindAll_Id(string find)
	{
		return rowList.FindAll(x => x.Id == find);
	}
	public Row Find_SkillKorName(string find)
	{
		return rowList.Find(x => x.SkillKorName == find);
	}
	public List<Row> FindAll_SkillKorName(string find)
	{
		return rowList.FindAll(x => x.SkillKorName == find);
	}
	public Row Find_SkillClass(string find)
	{
		return rowList.Find(x => x.SkillClass == find);
	}
	public List<Row> FindAll_SkillClass(string find)
	{
		return rowList.FindAll(x => x.SkillClass == find);
	}
	public Row Find_Name(string find)
	{
		return rowList.Find(x => x.Name == find);
	}
	public List<Row> FindAll_Name(string find)
	{
		return rowList.FindAll(x => x.Name == find);
	}
	public Row Find_Info(string find)
	{
		return rowList.Find(x => x.Info == find);
	}
	public List<Row> FindAll_Info(string find)
	{
		return rowList.FindAll(x => x.Info == find);
	}
	public Row Find_Sprite(string find)
	{
		return rowList.Find(x => x.Sprite == find);
	}
	public List<Row> FindAll_Sprite(string find)
	{
		return rowList.FindAll(x => x.Sprite == find);
	}
	public Row Find_Rare(string find)
	{
		return rowList.Find(x => x.Rare == find);
	}
	public List<Row> FindAll_Rare(string find)
	{
		return rowList.FindAll(x => x.Rare == find);
	}
	public Row Find_AniArrow(string find)
	{
		return rowList.Find(x => x.AniArrow == find);
	}
	public List<Row> FindAll_AniArrow(string find)
	{
		return rowList.FindAll(x => x.AniArrow == find);
	}
	public Row Find_JustArrow(string find)
	{
		return rowList.Find(x => x.JustArrow == find);
	}
	public List<Row> FindAll_JustArrow(string find)
	{
		return rowList.FindAll(x => x.JustArrow == find);
	}
	public Row Find_ArrowSize(string find)
	{
		return rowList.Find(x => x.ArrowSize == find);
	}
	public List<Row> FindAll_ArrowSize(string find)
	{
		return rowList.FindAll(x => x.ArrowSize == find);
	}
	public Row Find_ArrowSpeed(string find)
	{
		return rowList.Find(x => x.ArrowSpeed == find);
	}
	public List<Row> FindAll_ArrowSpeed(string find)
	{
		return rowList.FindAll(x => x.ArrowSpeed == find);
	}
	public Row Find_ArrowRotation(string find)
	{
		return rowList.Find(x => x.ArrowRotation == find);
	}
	public List<Row> FindAll_ArrowRotation(string find)
	{
		return rowList.FindAll(x => x.ArrowRotation == find);
	}
	public Row Find_ArrowShootCount(string find)
	{
		return rowList.Find(x => x.ArrowShootCount == find);
	}
	public List<Row> FindAll_ArrowShootCount(string find)
	{
		return rowList.FindAll(x => x.ArrowShootCount == find);
	}
	public Row Find_ArrowShootInterval(string find)
	{
		return rowList.Find(x => x.ArrowShootInterval == find);
	}
	public List<Row> FindAll_ArrowShootInterval(string find)
	{
		return rowList.FindAll(x => x.ArrowShootInterval == find);
	}
	public Row Find_Type(string find)
	{
		return rowList.Find(x => x.Type == find);
	}
	public List<Row> FindAll_Type(string find)
	{
		return rowList.FindAll(x => x.Type == find);
	}
	public Row Find_SkillType(string find)
	{
		return rowList.Find(x => x.SkillType == find);
	}
	public List<Row> FindAll_SkillType(string find)
	{
		return rowList.FindAll(x => x.SkillType == find);
	}
	public Row Find_BuffType(string find)
	{
		return rowList.Find(x => x.BuffType == find);
	}
	public List<Row> FindAll_BuffType(string find)
	{
		return rowList.FindAll(x => x.BuffType == find);
	}
	public Row Find_RangeType(string find)
	{
		return rowList.Find(x => x.RangeType == find);
	}
	public List<Row> FindAll_RangeType(string find)
	{
		return rowList.FindAll(x => x.RangeType == find);
	}
	public Row Find_RangeTypeOld(string find)
	{
		return rowList.Find(x => x.RangeTypeOld == find);
	}
	public List<Row> FindAll_RangeTypeOld(string find)
	{
		return rowList.FindAll(x => x.RangeTypeOld == find);
	}
	public Row Find_SkillRange(string find)
	{
		return rowList.Find(x => x.SkillRange == find);
	}
	public List<Row> FindAll_SkillRange(string find)
	{
		return rowList.FindAll(x => x.SkillRange == find);
	}
	public Row Find_AttackCount(string find)
	{
		return rowList.Find(x => x.AttackCount == find);
	}
	public List<Row> FindAll_AttackCount(string find)
	{
		return rowList.FindAll(x => x.AttackCount == find);
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
	public Row Find_Atk(string find)
	{
		return rowList.Find(x => x.Atk == find);
	}
	public List<Row> FindAll_Atk(string find)
	{
		return rowList.FindAll(x => x.Atk == find);
	}
	public Row Find_Matk(string find)
	{
		return rowList.Find(x => x.Matk == find);
	}
	public List<Row> FindAll_Matk(string find)
	{
		return rowList.FindAll(x => x.Matk == find);
	}
	public Row Find_HitEffect(string find)
	{
		return rowList.Find(x => x.HitEffect == find);
	}
	public List<Row> FindAll_HitEffect(string find)
	{
		return rowList.FindAll(x => x.HitEffect == find);
	}
	public Row Find_PlayerEffect(string find)
	{
		return rowList.Find(x => x.PlayerEffect == find);
	}
	public List<Row> FindAll_PlayerEffect(string find)
	{
		return rowList.FindAll(x => x.PlayerEffect == find);
	}
	public Row Find_Sound(string find)
	{
		return rowList.Find(x => x.Sound == find);
	}
	public List<Row> FindAll_Sound(string find)
	{
		return rowList.FindAll(x => x.Sound == find);
	}
	public Row Find_CastTime(string find)
	{
		return rowList.Find(x => x.CastTime == find);
	}
	public List<Row> FindAll_CastTime(string find)
	{
		return rowList.FindAll(x => x.CastTime == find);
	}
	public Row Find_StartDmgTerm(string find)
	{
		return rowList.Find(x => x.StartDmgTerm == find);
	}
	public List<Row> FindAll_StartDmgTerm(string find)
	{
		return rowList.FindAll(x => x.StartDmgTerm == find);
	}
	public Row Find_CoolTime(string find)
	{
		return rowList.Find(x => x.CoolTime == find);
	}
	public List<Row> FindAll_CoolTime(string find)
	{
		return rowList.FindAll(x => x.CoolTime == find);
	}
	public Row Find_UseHp(string find)
	{
		return rowList.Find(x => x.UseHp == find);
	}
	public List<Row> FindAll_UseHp(string find)
	{
		return rowList.FindAll(x => x.UseHp == find);
	}
	public Row Find_UseMp(string find)
	{
		return rowList.Find(x => x.UseMp == find);
	}
	public List<Row> FindAll_UseMp(string find)
	{
		return rowList.FindAll(x => x.UseMp == find);
	}
	public Row Find_breakpoint(string find)
	{
		return rowList.Find(x => x.breakpoint == find);
	}
	public List<Row> FindAll_breakpoint(string find)
	{
		return rowList.FindAll(x => x.breakpoint == find);
	}
	public Row Find_DotType(string find)
	{
		return rowList.Find(x => x.DotType == find);
	}
	public List<Row> FindAll_DotType(string find)
	{
		return rowList.FindAll(x => x.DotType == find);
	}
	public Row Find_DotCount(string find)
	{
		return rowList.Find(x => x.DotCount == find);
	}
	public List<Row> FindAll_DotCount(string find)
	{
		return rowList.FindAll(x => x.DotCount == find);
	}
	public Row Find_skillsort(string find)
	{
		return rowList.Find(x => x.skillsort == find);
	}
	public List<Row> FindAll_skillsort(string find)
	{
		return rowList.FindAll(x => x.skillsort == find);
	}

}