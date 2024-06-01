using CodeStage.AntiCheat.ObscuredTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BackEnd;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerBackendData : MonoBehaviour
{
    public static bool istest = false;

    private static PlayerBackendData _instance = null;

    public static PlayerBackendData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(PlayerBackendData)) as PlayerBackendData;

                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }

            return _instance;
        }
    }

    
    public List<int> randomseed = new List<int>();

    public void initrandomseed()
    {
        randomseed.Clear();
        for (int i = 0; i < 1000; i++)
        {
            randomseed.Add(Random.Range(0, 1000000));
        }
    }

    private int seednum = 0;
    public int GetRandomSeed()
    {
        int rn = randomseed[seednum];
        seednum++;
        if (seednum == randomseed.Count - 1)
        {
            seednum = 0;
            initrandomseed();   
        }
        return rn;
    }

    
    
    public string Id;
    public string nickname;

    public void Start()
    {
        initrandomseed();
        Screen.SetResolution(720, 1280, false);
        Application.targetFrameRate = 60;
        for (int i = 0; i < TalismanPreset.Length; i++)
        {
            TalismanPreset[i] = new PresetTalisman();
        }
        DontDestroyOnLoad(this);
    }





    #region 플레이어 정보 수치




    public string playersaveindate; //플레이어 저장인데이트 불러온기준
    public string playerindate;
    public ObscuredString[] PlayerTableIndate = new ObscuredString[(int)IndatesEnum.Length];

    public enum IndatesEnum
    {
        TimeData,
        PlayerData,
        Length
    }


    public string GetPlayerAvatadata()
    {
        return
            $"{ClassId};{GetEquipData()[0].Itemid};{GetEquipData()[1].Itemid};{avata_avata};{avata_weapon};{avata_subweapon}";
    }
    
    public string GetPlayerAvatadataPartyRaid()
    {
        string avata;
        string weaponavata;
        string subavata;
 

        //아바타
        if (avata_avata != "")
        {
            avata = AvartaDB.Instance.Find_id(avata_avata).sprite;
        }
        else
        {
            avata = ClassDB.Instance.Find_id(ClassId).classsprite;
        }
        
        if (avata_weapon != "")
        {
            weaponavata = AvartaDB.Instance.Find_id(avata_weapon).sprite;
        }
        else
        {
            weaponavata = EquipItemDB.Instance.Find_id(GetEquipData()[0].Itemid).EquipSprite;
        }
        
        if (avata_subweapon != "")
        {
            subavata = AvartaDB.Instance.Find_id(avata_subweapon).sprite;
        }
        else
        {
            subavata = EquipItemDB.Instance.Find_id(GetEquipData()[1].Itemid).EquipSprite;
        }

        string pet;
        string petrare;

        if (nowPetid != "")
        {
            pet = PetDB.Instance.Find_id(nowPetid).sprite;
            petrare = PetData[nowPetid].Petstar.ToString();
        }
        else
        {
            pet = "";
            petrare = "";
        }
        /*
  *   public string avatapath;
public string weaponpatn;
public string weaponrare;
public string subweaponpath;
public string subweaponrare;
public string petpath;
public string petrare;
  */
        return
            $"{avata};{weaponavata};{GetEquipData()[0].CraftRare1};{subavata};{GetEquipData()[1].CraftRare1};{pet};{petrare}";
    }
    //룰렛
    public int[] RouletteCount = new int[30];
    public int[] PetCount = new int[10];
    
    //시즌패스
    public ObscuredDecimal SeasonPassExp = 2000;
    public int SeasonPassNum;
    public bool SeasonPassPremium;
    public bool[] SeasonPassBasicReward = new bool[50];
    public bool[] SeasonPassPremiumReward = new bool[50];
    
    //어빌리티 특성
    public string[] Abilitys = new string[8];
    
    //오프라인
    public OfflineData Offlinedata;
    
    
    //펫
    public string nowPetid = "";
    
    //일일오픈
    public bool[] DailyOffBool = new bool[3];
    public int nowpresetnumber;
    public bool isloadpreset;
    public PresetItem[] Presets = new PresetItem[5];
    public bool ishaveguild = false;
    ObscuredDecimal money;
    ObscuredDecimal Cash;
    ObscuredDecimal Mile; //마일리지
    ObscuredDecimal Exp = 0;
    ObscuredDecimal MaxExp = 0;
    ObscuredInt Lv = 1; //캐릭 레벨
    public int ServerLv = 1; //캐릭 레벨
    public decimal ServerEXP = 1; //캐릭 레벨
    public int ServerSaveNum = 0; //캐릭 레벨
    [SerializeField]
    ObscuredInt AdLv = 1; //모험가 레벨
    [SerializeField] ObscuredInt FieldLv; //필드레벨 레벨
    public int[] ContentLevel = new int[30];
    
    public ObscuredInt AntCaveLv;
    public ObscuredInt AntCaveLvMax;
    public ObscuredInt AntTotalClear;
    public int ClientSaveNum = 0; //캐릭 레벨

   public DateTime[] PlayerTimes = new DateTime[15];

   public string avata_avata;
   public string avata_weapon;
   public string avata_subweapon;
   public bool[] playeravata = new bool[100];


   //시간제 패키지
   public DateTime[] PlayerShopTimes = new DateTime[50];
   public bool[] PlayerShopTimesbuys = new bool[50];
   
   
    public enum timesenum
    {
        ellibless,
        autofarm,
        Length
    }
    
    //업적
    ObscuredDecimal Ach_Exp = 0;
    ObscuredDecimal Ach_MaxExp = 0;
    ObscuredInt Ach_Lv = 1; //캐릭 레벨
    public Dictionary<string, Achievedata> PlayerAchieveData = new Dictionary<string, Achievedata>();

    //탈리스만
    public Dictionary<string, Talismandatabase> TalismanData = new Dictionary<string, Talismandatabase>();
    //public bool[] TalismanLock = new bool[8];
    public List<string> Talisman2Set = new List<string>();
    public List<string> Talisman3Set = new List<string>();
    public List<string> Talisman5Set = new List<string>();
    public int nowtalismanpreset = 0;
    public PresetTalisman[] TalismanPreset = new PresetTalisman[5];

    public Talismandatabase[] GiveEquipTalismanData()
    {
        return TalismanPreset[nowtalismanpreset].Talismanset;
    }

   //퀘스트
   
   //퀘스트 개별
   public float[] QuestCount = new float[100];
   public bool[] QuestIsFinish = new bool[100];
   public float[] QuestTotalCount = new float[10]; //달성도 보상
   
    
    //소탕가능여부
    public List<string> sotang_raid = new List<string>();
    //소탕여부 포함되지않으면 소탕이안됨
    public List<string> sotang_dungeon = new List<string>();

    //튜토
    public string tutoid = "0";
    public int tutoguideid = 0;
    public bool tutoguideisfinish;
    public bool tutoguidepremium;
    public string SavedCheckTime;
    public int tutocount;
    
    //제단
    ObscuredInt Altar_Lv = 0; //제단 // 0 = 1렙 
    public int[] Altar_Lvs = new int[10]; //제단 // 0 = 1렙 

    //수집
    
    

    public int spawncount = 1   ; //사냥터 소환마리수


    public string nowstage; //현재 스테이지

    public string ClassId = "C1000"; //직업레벨
    public List<string> Skills = new List<string>(); //가지고있는 스킬

    public bool AddSkill(string id)
    {
        if (id == "")
            return false;
        if(Skills.Contains(id))
        {
            return false;
        }
        else
        {
            Skills.Add(id);
            Skills.Sort();
            Savemanager.Instance.SaveSkillData();
            Savemanager.Instance.Save();
            return true;
        }
    }

    public void SetFieldLV(int num)
    {
        FieldLv = num;
    }

    public int GetFieldLv()
    {
        return FieldLv;
    }

    public decimal GetExp()
    {
        return Exp;
    }
    public decimal GetMaxExp()
    {
        return MaxExp;
    }

    public decimal GetAchExp()
    {
        return Ach_Exp;
    }
    public decimal GetMaxAchExp()
    {
        return Ach_MaxExp;
    }

    //제단
    public int GetAltarlv()
    {
        return Altar_Lv;
    }

    public void SetAltarLv(int lv)
    {
        Altar_Lv = lv;
    }
    public void SetAltarLvUp()
    {
        if (Altar_Lv == 2000)
            return;
        Altar_Lv ++;
    }

    public int GetGoldAltarlv(altarmanager.AltarType type)
    {
        return Altar_Lvs[(int)type];
    }
    public int[] GetAllAltarlv()
    {
        return Altar_Lvs;
    }
    public void SetGoldAltarLv(int lv,altarmanager.AltarType type)
    {
        Altar_Lvs[(int)type] = lv;
    }
    public void SetGoldAltarLvUp(altarmanager.AltarType type)
    {
        Altar_Lvs[(int)type]++;
    }

    public void SetMaxExp(decimal maxexp)
    {
        MaxExp = maxexp;
    }
    public void AddExp(decimal exp)
    {
        Exp += exp;
    }
    public void SetMaxAchExp(decimal maxexp)
    {
        Ach_MaxExp = maxexp;
    }
    public void AddAchExp(decimal exp)
    {
        //Debug.Log(("경ㅁ험치 추가"));
        Ach_Exp += exp;
    }
    public void AddSeasonPassExp(decimal exp)
    {
        //Debug.Log(("경ㅁ험치 추가"));
        SeasonPassExp += exp;
    }

    public decimal GetMoney()
    {
        return money.GetDecrypted();
    }

    public void SetMoney(decimal set)
    {
        money = set;
    }

    public decimal GetCash()
    {
        return Cash.GetDecrypted();
    }

    public void SetCash(decimal set)
    {
        Cash = set;
    }
    
    public decimal GetMile()
    {
        return Mile.GetDecrypted();
    }

    public void SetMile(decimal set)
    {
        Mile = set;
    }

    public int GetBattlePoint()
    {
        return PlayerData.Instance.GetEquipPoint(PlayerBackendData.Instance.EquipEquiptment0);
    }
    
    public int GetLv()
    {
        return Lv;
    }
    public void AddLv(int lv)
    {
        Lv = Lv + lv;
    }
    public void SetLv(int lv)
    {
        Lv = lv;
    }
    public int GetAchLv()
    {
        return Ach_Lv;
    }
    public void AddAchLv(int lv)
    {
        Ach_Lv = Ach_Lv + lv;
    }
    public void SetAchLv(int lv)
    {
        Ach_Lv = lv;
    }

    public int GetAdLv()
    {
        return AdLv;
    }
    public void AddAdLv(int lv)
    {
        AdLv = AdLv + lv;
    }
    public void SetAdLv(int lv)
    {
        AdLv = lv;
    }
    //플레이어의 스탯
    /*
     * 힘   : 생명력 과 공격력이 소폭 오른다.
     * 민첩 : 생명력 소폭과 공격력이 대폭 오른다.
     * 지능 : 마법 공격력이 대폭 오른다.
     * 지혜 : 최대 생명력과 최대 정신력이 오
     */
    //무기


    #endregion

    #region 업적
    public void InitAchieveData()
    {
        PlayerAchieveData.Clear();
        //   Debug.Log("초기화완료");
        for (int i = 0; i < AchievementDB.Instance.NumRows(); i++)
        {
            AchievementDB.Row data = AchievementDB.Instance.GetAt(i);
            if (data.coreid == "" || PlayerAchieveData.ContainsKey(data.coreid)) continue;
            PlayerAchieveData.Add(data.coreid, new Achievedata(data.id,data.coreid,0,int.Parse(data.count),data.subtype,false));
        }
    }

    #endregion


    #region  수집
    public Dictionary<string, CollectDatabase> CollectData = new Dictionary<string, CollectDatabase>();
    public bool[] RenewalCollectData = new bool[200];
    
    public void InitCollection()
    {
        CollectData.Clear();
        //   Debug.Log("초기화완료");
        for (int i = 0; i < CollectionDB.Instance.NumRows(); i++)
        {
            CollectionDB.Row data = CollectionDB.Instance.GetAt(i);
////            Debug.Log("데이터 " + data.id);
            switch (data.collecttype)
            {
                case "item":
                    if (data.id != "")
                    {
                        CollectData.TryAdd(data.id, new CollectDatabase(data.id));
                    }
                    break;
                case "equip":
                    if (data.id != "")
                    {
                        CollectData.TryAdd(data.id, new CollectDatabase(data.id));
                    }
                    break;
            }
        }
        Debug.Log("수집개수는" + CollectionRenewalDB.Instance.NumRows());
        PlayerBackendData.Instance.RenewalCollectData = new bool[CollectionRenewalDB.Instance.NumRows()];


    }

    
    #endregion    

    
    #region 클래스
    public Dictionary<string, ClassDatabase> ClassData = new Dictionary<string, ClassDatabase>();
    public Dictionary<string, petdatabase> PetData = new Dictionary<string, petdatabase>();
    
    
    
    //카드 데이터 초기 등록

    public SettingData settingdata = new SettingData();
    
    //클래스 패시브
    public string[] PassiveClassId = new string[30];

    public void InitClassData()
    {
        ClassData.Clear();
        //   Debug.Log("초기화완료");
        for (int i = 0; i < ClassDB.Instance.NumRows(); i++)
        {
            ClassDB.Row data = ClassDB.Instance.GetAt(i);
            string[] skill = new string[12];
            if (data.id != "" && !ClassData.ContainsKey(data.id))
            {
                ClassData.Add(data.id, new ClassDatabase(false,data.id, 0, skill,"0",false));
            }
        }
    }

    public void InitPetData()
    {
        PetData.Clear();
        //   Debug.Log("초기화완료");
        for (int i = 0; i < PetDB.Instance.NumRows(); i++)
        {
            PetDB.Row data = PetDB.Instance.GetAt(i);
            if (data.id != "" && !PetData.ContainsKey(data.id))
            {
                PetData.Add(data.id, new petdatabase(data.id, 0,0,false,false));
            }
        }
    }

    #endregion

    #region 장비들

    public int nowpreset = 0;



    //무기
    public Dictionary<string, EquipDatabase> Equiptment0 = new Dictionary<string, EquipDatabase>();
    //보조무기
    public Dictionary<string, EquipDatabase> Equiptment1 = new Dictionary<string, EquipDatabase>();
    //투구
    public Dictionary<string, EquipDatabase> Equiptment2 = new Dictionary<string, EquipDatabase>();
    //갑옷
    public Dictionary<string, EquipDatabase> Equiptment3 = new Dictionary<string, EquipDatabase>();
    //장갑
    public Dictionary<string, EquipDatabase> Equiptment4 = new Dictionary<string, EquipDatabase>();
    //신발
    public Dictionary<string, EquipDatabase> Equiptment5 = new Dictionary<string, EquipDatabase>();
    //반지
    public Dictionary<string, EquipDatabase> Equiptment6 = new Dictionary<string, EquipDatabase>();
    //목걸이
    public Dictionary<string, EquipDatabase> Equiptment7 = new Dictionary<string, EquipDatabase>();
    //등
    public Dictionary<string, EquipDatabase> Equiptment8 = new Dictionary<string, EquipDatabase>();
    //펫
    public Dictionary<string, EquipDatabase> Equiptment9 = new Dictionary<string, EquipDatabase>();
    //룬
    public Dictionary<string, EquipDatabase> Equiptment10= new Dictionary<string, EquipDatabase>();
    //휘장
    public Dictionary<string, EquipDatabase> Equiptment11= new Dictionary<string, EquipDatabase>();
    //장착 장비
    public EquipDatabase[] EquipEquiptment0 = new EquipDatabase[15];
    public EquipDatabase[] EquipEquiptment1= new EquipDatabase[15];
    public EquipDatabase[] EquipEquiptment2 = new EquipDatabase[15];
    #endregion

    #region 인벤아이템
   // public List<string> Itemid = new List<string>();
   // public List<int> Itemhowmany = new List<int>();
    public List<ItemInven> ItemInventory = new List<ItemInven>();

    public int GetItemIndex(string itemid)
    {
        return ItemInventory.FindIndex(r => r.Id == itemid);
    }
    public bool ishaveitemcount(string itemid, int count)
    {
        int index = GetItemIndex(itemid);

        if (index != -1)
        {
            if (ItemInventory[index].Howmany >= count)
                return true;
            else
                return false;
        }
        else
            return false;
    }
    public void Additem(string itemid,int howmany)
    {
        ItemdatabasecsvDB.Row itemdata = ItemdatabasecsvDB.Instance.Find_id(itemid);
        //중첩형이면

        if(GetItemIndex(itemid) != -1)
        {
            //중첩 확인 후 등록
            if (itemdata.Isstack == "TRUE")
            {
                //새로 등록
                
                int index = GetItemIndex(itemid);
                ItemInventory[index].Howmany += howmany;
            }
            else
            {
                //새로 등록
                ItemInventory.Add(new ItemInven(itemid, howmany, ""));
            }
        }
        else
        {
            //새로 등록
                ItemInventory.Add(new ItemInven(itemid, howmany, ""));
        }
    }

    public void RemoveItem(string itemid, int howmany)
    {
        int index = GetItemIndex(itemid);
        if (index == -1)
            return;

        ItemInventory[index].Howmany -= howmany;
        if(ItemInventory[index].Howmany <= 0)
        {
            ItemInventory.RemoveAt(index);
        }
    }

    //아이템 개수확인
    public int CheckItemCount(string itemid)
    {
        switch (ItemdatabasecsvDB.Instance.Find_id(itemid).itemsubtype)
        {
            case "108":
                return (int)GetCash();
                break;
            default:
                int index = GetItemIndex(itemid);

                return index == -1 ? 0 : ItemInventory[index].Howmany;
                break;
        }
        
        
     
    }

    bool IsItemCountTrue(string id, int howmany)
    {
        int index = GetItemIndex(id);

        if (index == -1) return false;

        if (ItemInventory[index].Howmany >= howmany)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckItemAndRemove(string id, int howmany)
    {
        if (IsItemCountTrue(id, howmany))
        {
            RemoveItem(id,howmany);
            return true;
        }
        else
        {
            return false;
        }
        
    }
    #endregion

    #region 제작관련
    public string[] craftmakingid = new string[4]; //제작 하는 아이디
    public string[] craftdatetime = new string[4]; //제작 종료 시간
    public int[] craftdatecount = new int[4]; //제작 종료 시간
    #endregion

    #region 죽음패널티
    public int DeathPenaltySecond; //죽으면 시간이 현재시간에서 추가된다.

    #endregion

    #region 프리미엄
    public bool ispremium;
    public string premiumenddate;
    public bool isadsfree;

    #endregion

    public EquipDatabase[] GetEquipData()
    {
        return nowpreset switch
        {
            0 => EquipEquiptment0,
            1 => EquipEquiptment1,
            2 => EquipEquiptment2,
            _ => EquipEquiptment0
        };
    }
    public EquipDatabase GetEquipDataByKeyID(string keyid)
    {
        switch (nowpreset)
        {
            case 0:
                foreach (var t in EquipEquiptment0)
                {
                    if (t == null) continue;
                    if (t.KeyId1 == keyid)
                    {
                        return t;
                    }
                }

                return null;
        }
        return null;
    }

    public EquipDatabase GetEquipDataByKey(string equipid)
    {
        switch (nowpreset)
        {
            case 0:
                foreach (var t in EquipEquiptment0)
                {
                    if (t == null) continue;
                    if (t.Itemid == equipid)
                    {
                        return t;
                    }
                }

                return null;
        }
        return null;
    }

    public Dictionary<string, EquipDatabase> GetTypeEquipment(string type)
    {
        switch (type)
        {
            case "Weapon":
            case "0":
                return Equiptment0;
            case "SWeapon":
            case "1":
                return Equiptment1;
            case "Helmet":
            case "2":
                return Equiptment2;
            case "Chest":
            case "3":
                return Equiptment3;
            case "Glove":
            case "4":
                return Equiptment4;
            case "Boot":
            case "5":
                return Equiptment5;
            case "Ring":
            case "6":
                return Equiptment6;
            case "Necklace":
            case "7":
                return Equiptment7;
            case "Wing":
            case "8":
                return Equiptment8;
            case "Pet":
            case "9":
                return Equiptment9;
            case "Rune":
            case "10":
                return Equiptment10;
            case "Insignia":
            case "11":
                return Equiptment11;
            default:
                return Equiptment0;

        }
    }

    public void MakeTalisman(string id)
    {

        while (true)
        {
            string keyids = artifactname(id);
            if(TalismanData.ContainsKey(keyids))
                continue;
            
            TalismanData.Add(keyids, new Talismandatabase(keyids, id));
            return;
        }
    }    
    public Talismandatabase MakeTalismanDatabase(string id)
    {

        while (true)
        {
            string keyids = artifactname(id);
            if(TalismanData.ContainsKey(keyids))
                continue;

            Talismandatabase a = new Talismandatabase(keyids, id);
            
            TalismanData.Add(a.Keyid, a);
            return a;
        }
    }    
    //장비 아이디 설정
    private string artifactname(string id)
    {
        System.Random rand = new System.Random(UnityEngine.Random.Range(0, 1000));
        string input = "abcdefghijklmnopqrstuvwxyz";

        var chars = Enumerable.Range(0, 15).Select(x => input[rand.Next(0, input.Length)]);
        //   Debug.Log(new string(chars.ToArray()));
        return $"{new string(chars.ToArray())}{id}";
    }
    public EquipDatabase AddEquipment(EquipDatabase equipdata)
    {
        //Debug.Log("장비 제작");
        string type = EquipItemDB.Instance.Find_id(equipdata.Itemid).Type;
        while (true)
        {
            switch (type)
            {
                case "Weapon":
                    Equiptment0.Add(equipdata.KeyId1,equipdata);

                    break;
                case "SWeapon":
                    Equiptment1.Add(equipdata.KeyId1,equipdata);

                    break;
                case "Helmet":
                    Equiptment2.Add(equipdata.KeyId1,equipdata);

                    break;
                case "Chest":
                    Equiptment3.Add(equipdata.KeyId1,equipdata);

                    break;
                case "Glove":
                    Equiptment4.Add(equipdata.KeyId1,equipdata);
                    break;
                case "Boot":
                    Equiptment5.Add(equipdata.KeyId1,equipdata);
                    break;
                case "Ring":
                    Equiptment6.Add(equipdata.KeyId1,equipdata);
                    break;
                case "Necklace":
                    Equiptment7.Add(equipdata.KeyId1,equipdata);
                    break;
                case "Wing":
                    Equiptment8.Add(equipdata.KeyId1,equipdata);
                    break;
                case "Pet":
                    Equiptment9.Add(equipdata.KeyId1,equipdata);
                    break;

                case "Rune":
                    Equiptment10.Add(equipdata.KeyId1,equipdata);
                    break;


                case "Insignia":
                    Equiptment11.Add(equipdata.KeyId1,equipdata);
                    break;

                default:
                    break;
            }
            return equipdata;
        }
    }
    //장비 제작
    public EquipDatabase MakeEquipment(string id)
    {
        //Debug.Log("장비 제작");
        EquipDatabase makedata = null;
        string artikey;
        string type = EquipItemDB.Instance.Find_id(id).Type;
        while (true)
        {
            artikey = artifactname(id);
            if (GetTypeEquipment(type).ContainsKey(artikey)) continue;
            EquipItemDB.Row data = EquipItemDB.Instance.Find_id(id);
            makedata = new EquipDatabase(id, artikey, data.RanSmeltCount);
            switch (type)
            {
                case "Weapon":
                    Equiptment0.Add(artikey, makedata);

                    break;
                case "SWeapon":
                    Equiptment1.Add(artikey, makedata);

                    break;
                case "Helmet":
                    Equiptment2.Add(artikey, makedata);

                    break;
                case "Chest":
                    Equiptment3.Add(artikey, makedata);

                    break;
                case "Glove":
                    Equiptment4.Add(artikey, makedata);
                    break;
                case "Boot":
                    Equiptment5.Add(artikey, makedata);
                    break;
                case "Ring":
                    Equiptment6.Add(artikey, makedata);
                    break;
                case "Necklace":
                    Equiptment7.Add(artikey, makedata);
                    break;
                case "Wing":
                    Equiptment8.Add(artikey, makedata);
                    break;
                case "Pet":
                    Equiptment9.Add(artikey, makedata);
                    break;

                case "Rune":
                    Equiptment10.Add(artikey, makedata);
                    break;
                case "Insignia":
                    Equiptment11.Add(artikey, makedata);
                    break;
                default:
                    break;
            }
            return makedata;
        }
    }

     //장비 제작
    public void MakeEquipment_Min(string id)
    {
        //Debug.Log("장비 제작");
        EquipDatabase makedata = null;
        string artikey;
        string type = EquipItemDB.Instance.Find_id(id).Type;
        while (true)
        {
            artikey = artifactname(id);
            if (GetTypeEquipment(type).ContainsKey(artikey)) continue;
            EquipItemDB.Row data = EquipItemDB.Instance.Find_id(id);
            makedata = new EquipDatabase(id, artikey, "10;10",false,true);
            switch (type)
            {
                case "Weapon":
                    Equiptment0.Add(artikey, makedata);

                    break;
                case "SWeapon":
                    Equiptment1.Add(artikey, makedata);

                    break;
                case "Helmet":
                    Equiptment2.Add(artikey, makedata);

                    break;
                case "Chest":
                    Equiptment3.Add(artikey, makedata);

                    break;
                case "Glove":
                    Equiptment4.Add(artikey, makedata);
                    break;
                case "Boot":
                    Equiptment5.Add(artikey, makedata);
                    break;
                case "Ring":
                    Equiptment6.Add(artikey, makedata);
                    break;
                case "Necklace":
                    Equiptment7.Add(artikey, makedata);
                    break;
                case "Wing":
                    Equiptment8.Add(artikey, makedata);
                    break;
                case "Pet":
                    Equiptment9.Add(artikey, makedata);
                    break;

                case "Rune":
                    Equiptment10.Add(artikey, makedata);
                    break;
                case "Insignia":
                    Equiptment11.Add(artikey, makedata);
                    break;
                default:
                    break;
            }
            return ;
        }
    }
    
    //장비 제작
    public void MakeEquipmentAndEquip(string id)
    {
        string artikey;
        string type = EquipItemDB.Instance.Find_id(id).Type;
        while (true)
        {
            artikey = artifactname(id);
            if (GetTypeEquipment(type).ContainsKey(artikey)) continue;
            EquipItemDB.Row data = EquipItemDB.Instance.Find_id(id);
            switch (type)
            {
                case "Weapon":
                    Equiptment0.Add(artikey, new EquipDatabase(id, artikey, data.RanSmeltCount,false,true));
                    Equiptment0[artikey].IsEquip = true;
                    GetEquipData()[0] = Equiptment0[artikey];
                    break;
                case "SWeapon":
                    Equiptment1.Add(artikey, new EquipDatabase(id, artikey, data.RanSmeltCount, false, true));
                    Equiptment1[artikey].IsEquip = true;
                    GetEquipData()[1] = Equiptment1[artikey];

                    break;
                case "Helmet":
                    Equiptment2.Add(artikey, new EquipDatabase(id, artikey, data.RanSmeltCount, false, true));

                    break;
                case "Chest":
                    Equiptment3.Add(artikey, new EquipDatabase(id, artikey, data.RanSmeltCount, false, true));

                    break;
                case "Glove":
                    Equiptment4.Add(artikey, new EquipDatabase(id, artikey, data.RanSmeltCount, false, true));
                    break;
                case "Boot":
                    Equiptment5.Add(artikey, new EquipDatabase(id, artikey, data.RanSmeltCount, false, true));
                    break;
                case "Ring":
                    Equiptment6.Add(artikey, new EquipDatabase(id, artikey, data.RanSmeltCount, false, true));
                    break;
                case "Necklace":
                    Equiptment7.Add(artikey, new EquipDatabase(id, artikey, data.RanSmeltCount, false, true));
                    break;
                case "Wing":
                    Equiptment8.Add(artikey, new EquipDatabase(id, artikey, data.RanSmeltCount, false, true));

                    break;
                case "Pet":
                    Equiptment9.Add(artikey, new EquipDatabase(id, artikey, data.RanSmeltCount, false, true));

                    break;

                case "Rune":
                    Equiptment10.Add(artikey, new EquipDatabase(id, artikey, data.RanSmeltCount, false, true));

                    break;
                case "Insignia":
                    Equiptment11.Add(artikey, new EquipDatabase(id, artikey, data.RanSmeltCount, false, true));

                    break;
                default:
                    break;
            }
            break;
        }
    }

    public void StartCheck()
    {
        StartCoroutine(RefreshTheBackendToken());
    }
    
    public IEnumerator RefreshTheBackendToken()
    {
        while (true)
        {
//            Debug.Log("체크 시작");
            yield return SpriteManager.Instance.GetWaitforSecond(3600);
        SendQueue.Enqueue(Backend.BMember.RefreshTheBackendToken, (callback) =>
        {
            if(callback.IsSuccess())
                Debug.Log("액세스 토큰이 살아있습니다");
                Debug.Log("액세스 토큰이 살아있습니다");
            
            if (callback.IsMaintenanceError()) // 서버 상태가 '점검'일 시
            {
                //점검 팝업창 + 로그인 화면으로 보내기
                //return false;
                Application.Quit();
            }
            else if (callback.IsTooManyRequestError()) // 단기간에 많은 요청을 보낼 경우 발생하는 403 Forbbiden 발생 시
            {
                //너무 많은 요청을 보내는 중 
                //return false;
                Application.Quit();
            }
            else
            {
                //재시도를 해도 액세스토큰 재발급이 불가능한 경우
                //커스텀 로그인 혹은 페데레이션 로그인을 통해 수동 로그인을 진행해야합니다.
                //중복 로그인일 경우 401 bad refreshToken 에러와 함께 발생할 수 있습니다.
                if(callback.GetErrorCode() == "401")
                    Application.Quit();
                
            }
            
        });
        }
    }
    
    
    public string testid;
    public void MakeTest()
    {
        MakeEquipment("1000");
        MakeEquipment("1001");
        MakeEquipment("1002");
        MakeEquipment("1003");
        MakeEquipment("1004");
        MakeEquipment("1005");
        MakeEquipment("1006");
        MakeEquipment("1007");
        MakeEquipment("1008");
        MakeEquipment("1009");
        MakeEquipment("1010");
        MakeEquipment("1011");
        MakeEquipment("1012");
        MakeEquipment("1013");
        MakeEquipment("1014");
        MakeEquipment("1015");
        MakeEquipment("1016");
        Savemanager.Instance.SaveEquip();
    }
    
    public void MakeSkill()
    {
       
        AddSkill("1024");
        AddSkill("1029");
    }
}

public enum EquipmenyEnum
{
    주무기,
    보조무기,
    투구,
    갑옷,
    장갑,
    신발,
    반지,
    목걸이,
    등,
    펫,
    룬,
    휘장,
    Length
}



public class EquiptmentData
{
    EquipDatabase[] equipdata = new EquipDatabase[15];

    public void EquiptmentDatas()
    {
        Equipdata = new EquipDatabase[15];
    }

    public EquiptmentData(EquipDatabase[] equipdata)
    {
        Equipdata = equipdata;
    }

    public EquiptmentData()
    {
        Equipdata = new EquipDatabase[15];
    }

    public EquipDatabase[] Equipdata { get => equipdata; set => equipdata = value; }
}

public class Ability
{
    public int[] abilityData;

    public Ability() => abilityData = new int[23];
}

public record SettingData
{
    public float Backsound = 1f;
    public float Effectsound = 1f;
    public int ButtonSound = 0;

    public int DmgCountNum = 2;
    public int DmgShowNum = 0;

    public int EffectColor = 3;
    public int EffectShow = 0;

    public float Hp = 0.5f;
    public float Mp = 0.5f;
    
    public int ItemPanel = 0;
    public int ItemDrop = 0;
    public int EskillPanel = 0;
    public int SystemChat = 0;

    public SettingData()
    {
        
    }

    public void SetData(float backsound, float effectsound, int dmgCountNum, int dmgShowNum, int effectColor, int effectShow, float hp, float mp, int itemPanel, int itemDrop, int eskillPanel, int systemChat,int ButtonSound)
    {
        Backsound = backsound;
        Effectsound = effectsound;
        DmgCountNum = dmgCountNum;
        DmgShowNum = dmgShowNum;
        EffectColor = effectColor;
        EffectShow = effectShow;
        Hp = hp;
        Mp = mp;
        ItemPanel = itemPanel;
        ItemDrop = itemDrop;
        EskillPanel = eskillPanel;
        SystemChat = systemChat;
        this.ButtonSound = ButtonSound;
    }
}

public class OfflineData
{
    public string mapid; //맵 아이디
    public int time; //시간
    public int level; //난이도

    public OfflineData()
    {
        
    }
    public OfflineData(string mapid, int time, int level)
    {
        this.mapid = mapid;
        this.time = time;
        this.level = level;
    }
}