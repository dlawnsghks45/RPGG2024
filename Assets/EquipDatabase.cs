using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector.Demos;
using Sirenix.Utilities;
using UnityEngine;
using Random = System.Random;


public class EquipDatabase : IEquatable<object>
{
    private bool isEquip;
    private string itemid;
    private string KeyId;
    private int CraftRare;
    private string itemrare;
    private string PrefixId; //번개같은 등등
    private int MaxStoneCount; //제련 카운트 최대 치 이며 해당 수치까지 제련이 가능
    private int StoneCount; //제련 현재 카운트
    private int[] SmeltStatCount; //제련을 할 때 해당 칸을 보여준다 0은 빈칸 1은 성공 2는 실패
    private int SmeltSuccCount; //실패횟수
    private int SmeltFailCount; //성공횟수
    private int EnchantNum; //현재강화
    private int EnchantFail; //강화실패
    private int Alldmg; //피해증가
    private bool islock;
    private bool ishaveEquipSkill;
    private List<string> EquipSkill = new List<string>();


    public EquipDatabase(string itemid)
    {
        this.itemid = itemid;
        Itemrare = EquipItemDB.Instance.Find_id(itemid).Rare;
    }

    public EquipDatabase(LitJson.JsonData data)
    {
        Itemrare = data["Itemrare"].ToString();
        IsEquip =  bool.Parse(data["IsEquip"].ToString());
        SmeltSuccCount1 = int.Parse(data["SmeltSuccCount1"].ToString());
        CraftRare1 = int.Parse(data["CraftRare1"].ToString());
        Itemid = data["Itemid"].ToString();
        SmeltFailCount1 = int.Parse(data["SmeltFailCount1"].ToString());
        StoneCount1 = int.Parse(data["StoneCount1"].ToString());

        for(int i = 0;i < data["EquipSkill1"].Count;i++)
        {
            EquipSkill.Add(data["EquipSkill1"][i].ToString());
        }
        IshaveEquipSkill = bool.Parse(data["IshaveEquipSkill"].ToString());

        KeyId1 = data["KeyId1"].ToString();
        EnchantNum1 = int.Parse(data["EnchantNum1"].ToString());

        if (data.ContainsKey("EnchantFail1"))
        {
            EnchantFail1 = int.Parse(data["EnchantFail1"].ToString());
        }
        else
        {
            EnchantFail1 = 0;
        }
        
        if (data.ContainsKey("Alldmg1"))
        {
            Alldmg1 = int.Parse(data["Alldmg1"].ToString());
        }
        else
        {
            Alldmg1 = 0;
        }
        
        
        MaxStoneCount1 = int.Parse(data["MaxStoneCount1"].ToString());

        SmeltStatCount = new int[data["SmeltStatCount1"].Count];
        for (int i = 0; i < data["SmeltStatCount1"].Count; i++)
        {
            SmeltStatCount[i] = int.Parse(data["SmeltStatCount1"][i].ToString());
        }
        IsLock = bool.Parse(data["IsLock"].ToString());
    }

    public EquipDatabase(string itemid, string keyId, int craftRare, string itemrare, string prefixId, int maxStoneCount, int stoneCount,bool islock =false,int enchantnum = 0,bool haveequipskill = false, List<string> Equipskill = null)
    {
        this.itemid = itemid;
        KeyId = keyId;
        CraftRare = craftRare;
        this.itemrare = itemrare;
        PrefixId = prefixId;
        MaxStoneCount = maxStoneCount;
        StoneCount = stoneCount;
        IsEquip = false;
        this.islock = islock;
        EnchantNum1 = enchantnum;
        EnchantFail1 = 0;
        Alldmg1 = 0;
        IshaveEquipSkill = haveequipskill;
        this.EquipSkill = Equipskill;
    }
// 전승이 가능한지 확인
    public bool CheckIsSucc(string speid)
    {
        string[] temp = EquipItemDB.Instance.Find_id(itemid).SuccSpe.Split(';');

        bool cansucc = false;

        for (int i = 0; i < temp.Length; i++)
        {
            Debug.Log(temp[i]);
            if (temp[i].Equals(speid))
                cansucc = true;
        }


        return cansucc;
    }
    
    
    public int GetBattlePoint()
    {
        int total = int.Parse(EquipItemDB.Instance.Find_id(itemid).BattlePoint);
        int craft =  (int)(total * (CraftRare1 * 0.2f));
        int rare = (int)(total * (int.Parse(Itemrare) * 0.1f));

        EquipItemDB.Row equipdata = EquipItemDB.Instance.Find_id(itemid);
//        Debug.Log("개ㅑ수"+EquipSkill1.Count);

        float eq = 0;
        for (int i = equipdata.SpeMehodP != "0" ? 1:0; i < EquipSkill1.Count; i++)
        {
            if(EquipSkill[i] == "" ||EquipSkill[i] == "0" )
                continue;
//         Debug.Log(EquipSkill[i] + i);
            if (EquipSkillDB.Instance.Find_id(EquipSkill[i]).lv == "1") eq += 0.02f;
            if (EquipSkillDB.Instance.Find_id(EquipSkill[i]).lv == "2") eq += 0.05f;
            if (EquipSkillDB.Instance.Find_id(EquipSkill[i]).lv == "3") eq += 0.08f;
            if (EquipSkillDB.Instance.Find_id(EquipSkill[i]).lv == "4") eq += 0.11f;
            if (EquipSkillDB.Instance.Find_id(EquipSkill[i]).lv == "5") eq += 0.14f;
            if (EquipSkillDB.Instance.Find_id(EquipSkill[i]).lv == "6") eq += 0.17f;
            if (EquipSkillDB.Instance.Find_id(EquipSkill[i]).lv == "7") eq += 0.20f;
            if (EquipSkillDB.Instance.Find_id(EquipSkill[i]).lv == "8") eq += 0.23f;
            if (EquipSkillDB.Instance.Find_id(EquipSkill[i]).lv == "9") eq += 0.26f;
            if (EquipSkillDB.Instance.Find_id(EquipSkill[i]).lv == "10") eq += 0.30f;
        }

        int equipskill = total * (int)eq; 
            
        int smelt = (int)(total * getsmeltstat()); 
        int upgrade = (int)(total * getupgradestat()); 
        
        
        return total + craft + rare + equipskill+smelt + upgrade;
    }
    
    public EquipDatabase(string itemid, string keyId, string maxStoneCount, bool isequip = false, bool islowest = false, bool islock = false)
    {
        this.itemid = itemid;
        KeyId = keyId;
        int rares;
        this.islock = islock;

        float temp = Time.time * 100f;
        int seed = (int)temp + PlayerBackendData.Instance.GetRandomSeed();
        UnityEngine.Random.InitState(seed);
        //랜덤 제련
        EquipItemDB.Row equipdata = EquipItemDB.Instance.Find_id(itemid);
        string[] ransmelt = maxStoneCount.Split(';');
        if (islowest)
        {
            rares = 0;
            this.itemrare = "0";
            MaxStoneCount = 0;
            if (maxStoneCount == "0")
                MaxStoneCount = 0;
            else
                MaxStoneCount = UnityEngine.Random.Range(int.Parse(ransmelt[0]), int.Parse(ransmelt[1]) + 1);
        }
        else
        {
            if (equipdata.craftrare != "0")
            {
                rares = int.Parse(equipdata.craftrare);
            }
            else
            {
                rares = GetCraftTier();
            }
            
            this.itemrare = GetRare();
            if (maxStoneCount == "0")
                MaxStoneCount = 0;
            else
                MaxStoneCount = UnityEngine.Random.Range(int.Parse(ransmelt[0]), int.Parse(ransmelt[1]) + 1);
        }
        PrefixId = "";
        IsEquip = isequip;
        CraftRare = rares;
        StoneCount = 0;
        SmeltStatCount = new int[MaxStoneCount];
        EnchantNum = 0;
        EnchantFail = 0;


        //ㅁㅈㅇㅈㅁㅇㅁ

        //고유스킬넣기
        if (equipdata.MinAllDmg != "0")
        {
            //특수효과가있다
            //민
            int alldmg = UnityEngine.Random.Range(int.Parse(equipdata.MinAllDmg), int.Parse(equipdata.MaxAllDmg)+1);
            Alldmg1 = alldmg;
        }
        else
        {
            Alldmg1 = 0;
        }

            //고유스킬넣기
            if (equipdata.SpeMehodP != "0")
            {
                //특수효과가있다
                ishaveEquipSkill = true;
                EquipSkill1.Add(equipdata.SpeMehodP);
            }

            //특수스킬넣기
        if (equipdata.SpeMehod != "0")
        {
            string speid = equipdata.SpeMehod;
            //Debug.Log(speid);
            EquipSkillRandomGiveDB.Row speds = EquipSkillRandomGiveDB.Instance.Find_id(speid);

            List<string> skilloption = new List<string>();
            List<string> selectedskill = new List<string>();
            skilloption = speds.equipskills.Split(';').ToList();
            //확률에 따라 랜덤으로 지급
            //A는 나올 옵션 개수
            int percent = int.Parse(speds.percent);
            int optioncount = int.Parse(speds.A);

            int rd = UnityEngine.Random.Range(0, 101);

            if(rd <= percent)
            {
                //옵션나온다
                int oc = UnityEngine.Random.Range(1, optioncount+1);
                for (int i = 0; i < oc; i++)
                {
                    int ran = UnityEngine.Random.Range(0, skilloption.Count);

                    List<string> giveskill = new List<string>();
                    //스킬레벨 설정
                    for (int j = 0; j < EquipSkillDB.Instance.NumRows(); j++)
                    {
                        if (EquipSkillDB.Instance.GetAt(j).coreid != skilloption[ran]) continue;
                        giveskill.Add(EquipSkillDB.Instance.GetAt(j).id);

                        if (EquipSkillDB.Instance.GetAt(j).coreid != skilloption[ran])
                        {
                            break;
                        }
                    }

                    //스킬을 넣음
                    int ran2 = UnityEngine.Random.Range(0, giveskill.Count);
                    selectedskill.Add(giveskill[ran2]);

                    //스택형이 아니라면 뺀다
                    if (!bool.Parse(EquipSkillDB.Instance.Find_id(skilloption[ran]).isstack))
                        skilloption.RemoveAt(ran);
                }
                //특수효과가있다
                ishaveEquipSkill = true;

                foreach (var t in selectedskill)
                {
                    EquipSkill1.Add(t);
                }
            }
        }
    }

    public void SendSkill(int pluscount = 0)
    {
        if (pluscount == 1)
        {
            EquipSkill1[0] = EquipItemDB.Instance.Find_id(itemid).SpeMehodP;
        }
    }
    public List<string> GetESkill(int pluscount = 0)
    {
        equipoptionchanger.Instance.lockcountEs = 0;
        SuccManager.Instance.lockcountEs = 0;
        
        for (int i = 0; i < equipoptionchanger.Instance.eskillnowpanel.Length; i++)
        {
            if (equipoptionchanger.Instance.eskillnowpanel[i].LockEs.IsOn)
                equipoptionchanger.Instance.lockcountEs++;
        }
        for (int i = 0; i < SuccManager.Instance.EquipSkills.Length; i++)
        {
            if (SuccManager.Instance.EquipSkills[i].LockEs.IsOn)
                SuccManager.Instance.lockcountEs++;
        }
        
        Debug.Log(EquipSkill1.Count);
        EquipItemDB.Row equipdata = EquipItemDB.Instance.Find_id(itemid);
        List<string> skilloption = new List<string>();
        List<string> selectedskill = new List<string>();
        List<string> LockSkill = new List<string>();
        int num = EquipItemDB.Instance.Find_id(itemid).SpeMehodP != "0" ? 1 : 0;

        int curcount = EquipSkill1.Count; //3

        //잠금이있을수있으니 미리넣는다.

        bool ESislock = false;
       // if (equipoptionchanger.Instance.lockcountEs > 0 || SuccManager.Instance.lockcountEs > 0)
       // {
            ESislock = true;
            for (int i = 0; i < EquipSkill.Count; i++)
            {
                LockSkill.Add(EquipSkill[i]);
            }
       // }
        
        if (curcount-num == 0) //특수효과가 1이면 0 0이면 개수만큼
            curcount = 1 + num; //이라면 0
        
        //Debug.Log("현재 카운트는" + curcount);
        //고유 스킬을 넣음
        if (num == 1)
        {
            if (SuccManager.Instance.IsSucc)
            {
                selectedskill.Add(EquipItemDB.Instance.Find_id(itemid).SpeMehodP);
            }
            else
            {
                selectedskill.Add(EquipItemDB.Instance.Find_id(itemid).SpeMehodP);
            }

            
            //잠금이라면 
            if (ESislock && pluscount == 0)
                LockSkill.RemoveAt(0);

            curcount -= 1;
        }
        //Debug.Log("현재 카운트 " + curcount);
        //특수스킬넣기
        string speid = equipdata.SpeMehod;
        EquipSkillRandomGiveDB.Row speds = EquipSkillRandomGiveDB.Instance.Find_id(speid);
        skilloption = speds.equipskills.Split(';').ToList();
        //확률에 따라 랜덤으로 지급
        //A는 나올 옵션 개수
        //옵셔는ㄹ어나는 확률
        //옵션 잠금이라면 잠금된 옵션이 중복불가라면 없앰.

        if (ESislock)
        {
            //잠금아니면 다 널처리
            if (SuccManager.Instance.IsSucc)
            {
                for (int i = 0; i < LockSkill.Count; i++)
                {
                    // Debug.Log(i);
                    if (!SuccManager.Instance.EquipSkills[i].LockEs.IsOn)
                    {
                        Debug.Log("전승 잠금");
                       //LockSkill[i] = "";
                    }
                    else
                    {
                        if (!bool.Parse(EquipSkillDB.Instance.Find_id(LockSkill[i]).isstack))
                        {
                            if (skilloption.Contains(EquipSkillDB.Instance.Find_id(LockSkill[i]).coreid))
                                skilloption.Remove(EquipSkillDB.Instance.Find_id(LockSkill[i]).coreid);
                            //스킬은 뺀다.
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < LockSkill.Count; i++)
                {
                    // Debug.Log(i);
                    if (!equipoptionchanger.Instance.eskillnowpanel[i].LockEs.IsOn)
                    {
//                        Debug.Log("안전승 잠금");
                       // LockSkill[i] = "";
                    }
                    else
                    {
                        if (!bool.Parse(EquipSkillDB.Instance.Find_id(LockSkill[i]).isstack))
                        {
                            if (skilloption.Contains(EquipSkillDB.Instance.Find_id(LockSkill[i]).coreid))
                                skilloption.Remove(EquipSkillDB.Instance.Find_id(LockSkill[i]).coreid);
                            //스킬은 뺀다.
                        }
                    }
                }
            }
        }




        //10퍼 확률로 옵션 추가
        int upcount = UnityEngine.Random.Range(0, 101) < 10 ? 1 : 0;

        if (equipoptionchanger.Instance.islessequipskill)
            upcount = 0;
        
        
        if (upcount == 1)
        {
            //Debug.Log("개수 업!");
        }

        if (curcount == 5 || SuccManager.Instance.IsSucc)
            upcount = 0;
        //옵션나온다 옵션의 개수
        int oc = curcount + upcount+pluscount;
        Debug.Log("oc는" + oc);
        for (int i = 0; i < oc; i++)
        {
             Debug.Log(i);
            if (!SuccManager.Instance.IsSucc && equipoptionchanger.Instance.eskillnowpanel[i].LockEs.IsOn)
            {
                //잠금이라면     
               // Debug.Log("잠금"+i);
                Debug.Log("개수"+LockSkill.Count);
               Debug.Log(LockSkill[i]);
                    selectedskill.Add(LockSkill[i]);
                //스택형이 아니라면 뺀다
                if (!bool.Parse(EquipSkillDB.Instance.Find_id(LockSkill[i]).isstack))
                    skilloption.Remove(LockSkill[i]);
            }
            else if (SuccManager.Instance.IsSucc && SuccManager.Instance.EquipSkills[i].LockEs.IsOn)
            {
                for (int j = 0; j < LockSkill.Count; j++)
//                    Debug.Log(LockSkill[j]);
                //잠금이라면     
                Debug.Log(i);
                Debug.Log(LockSkill[i]);
                selectedskill.Add(LockSkill[i]);
                
                //스택형이 아니라면 뺀다
                if (!bool.Parse(EquipSkillDB.Instance.Find_id(LockSkill[i]).isstack))
                    skilloption.Remove(LockSkill[i]);
            }
            else
            {

                int ran = UnityEngine.Random.Range(0, skilloption.Count);

                List<string> giveskill = new List<string>();
                //스킬레벨 설정
                for (int j = 0; j < EquipSkillDB.Instance.NumRows(); j++)
                {
                    if (EquipSkillDB.Instance.GetAt(j).coreid != skilloption[ran]) continue;
                    //모든 코어 아이디의 레벨을 가져옴.

                    if (SuccManager.Instance.IsSucc)
                    {
                        if (int.Parse(EquipSkillDB.Instance.GetAt(j).lv) < 4)
                        {
                            //3렙까지만 넣음
                            giveskill.Add(EquipSkillDB.Instance.GetAt(j).id);
                        }
                    }
                    else
                    {
                        giveskill.Add(EquipSkillDB.Instance.GetAt(j).id);
                    }

                    if (EquipSkillDB.Instance.GetAt(j).coreid != skilloption[ran])
                    {
                        //코어가 다르면 다른 라인이다.
                        break;
                    }
                }
                    Debug.Log("넣다");
                int ran2 = UnityEngine.Random.Range(0, giveskill.Count);
                selectedskill.Add(giveskill[ran2]);

                //스택형이 아니라면 뺀다
                if (!bool.Parse(EquipSkillDB.Instance.Find_id(skilloption[ran]).isstack))
                    skilloption.RemoveAt(ran);
            }
        }

  
        
        return selectedskill;
    }
    public void SetEquipSkillsE(string[] giveskill)
    {
        EquipSkill1.Clear();
        
        for (int i = 0; i < giveskill.Length; i++)
        {
//            Debug.Log("넣었다" + giveskill[i]);
            if(giveskill[i] == "" || giveskill[i] == "0")
                continue;
            EquipSkill1.Add(giveskill[i]);
        }

        
        if (EquipSkill1.Count != 0)
            ishaveEquipSkill = true;
    }
    public void SetEquipSkills(string[] giveskill)
    {
        EquipSkill1.Clear();
        Debug.Log("다");
        //고유가 있으면 맨 앞줄 변경
        if (EquipItemDB.Instance.Find_id(SuccManager.Instance.ResultSlot.data.itemid).SpeMehodP != "0")
        {
            Debug.Log("다2");
            if (EquipItemDB.Instance.Find_id(SuccManager.Instance.ResourceSlot.data.itemid).SpeMehodP != "0")
            {
                Debug.Log("다23");

                giveskill[0] = EquipItemDB.Instance.Find_id(SuccManager.Instance.ResultSlot.data.itemid).SpeMehodP;
            }
            else
            {
                Debug.Log("다42");

                List<string> li = new List<string>();
                li = giveskill.ToList();
                for(int i = 0 ; i < li.Count;i++)
                    Debug.Log(li[i]);
                li.Insert(0, EquipItemDB.Instance.Find_id(SuccManager.Instance.ResultSlot.data.itemid).SpeMehodP);
                Debug.Log("다42");
                for(int i = 0 ; i < li.Count;i++)
                    Debug.Log(li[i]);
                giveskill = li.ToArray();
            }
        }
        
        Debug.Log("다44");

    
        
        for (int i = 0; i < giveskill.Length; i++)
        {
//            Debug.Log("넣었다" + giveskill[i]);
            if(giveskill[i] == "" || giveskill[i] == "0")
                continue;
            EquipSkill1.Add(giveskill[i]);
        }

        
        if (EquipSkill1.Count != 0)
            ishaveEquipSkill = true;
    }

    public EquipDatabase()
    {

    }
    public EquipDatabase(EquipDatabase data)
    {
        Itemid = data.Itemid;
        KeyId1 = data.KeyId1;
        CraftRare1 = data.CraftRare1;
        Itemrare = data.Itemrare;
        MaxStoneCount1 = data.MaxStoneCount1;
        StoneCount1 = data.StoneCount1;
        EnchantNum1 = data.EnchantNum1;
        EnchantFail = data.EnchantFail1;
        EquipSkill1 = data.EquipSkill1;
        Alldmg = data.Alldmg1;
        IshaveEquipSkill = data.IshaveEquipSkill;
        SmeltStatCount1 = data.SmeltStatCount1;
    }
    public string Itemid { get => itemid; set => itemid = value; }
    public string KeyId1 { get => KeyId; set => KeyId = value; }
    public int CraftRare1 { get => CraftRare; set => CraftRare = value; }
    public string Itemrare { get => itemrare; set => itemrare = value; }
    public string PrefixId1 { get => PrefixId; set => PrefixId = value; }
    public int MaxStoneCount1 { get => MaxStoneCount; set => MaxStoneCount = value; }
    public int StoneCount1 { get => StoneCount; set => StoneCount = value; }
    public int[] SmeltStatCount1 { get => SmeltStatCount; set => SmeltStatCount = value; }
    public bool IsEquip { get => isEquip; set => isEquip = value; }
    public bool IsLock { get => islock; set => islock = value; }
    public int SmeltSuccCount1 { get => SmeltSuccCount; set => SmeltSuccCount = value; }
    public int SmeltFailCount1 { get => SmeltFailCount; set => SmeltFailCount = value; }
    public int EnchantNum1 { get => EnchantNum; set => EnchantNum = value; }
    public int EnchantFail1 { get => EnchantFail; set => EnchantFail = value; }

    public int Alldmg1 { get => Alldmg; set => Alldmg = value; }
    public bool IshaveEquipSkill { get => ishaveEquipSkill; set => ishaveEquipSkill = value; }
    public List<string> EquipSkill1 { get => EquipSkill; set => EquipSkill = value; }

    private int GetCraftTier()
    {
        float temp = Time.time * 100f;
        int seed = (int)temp + PlayerBackendData.Instance.GetRandomSeed();
        UnityEngine.Random.InitState(seed);
        
//        Debug.Log("시드는" + seed);
        float ran = UnityEngine.Random.Range(0f, 101f);

        return ran switch
        {
            >= 61f and <= 100 => 0,
            <= 60f and >= 31 => 1,
            <= 30f and >= 16 => 2,
            <= 15 when ran >= 6 => 3,
            <= 5f and >= 1f => 4,
            <= 0f => 5,
            _ => 0
        };
    }
    
    public bool GetCraftTierUp()
    {
        float temp = Time.time * 100f;
        int seed = (int)temp + PlayerBackendData.Instance.GetRandomSeed();
        UnityEngine.Random.InitState(seed);
        int ran = UnityEngine.Random.Range(0, 101);
//        Debug.Log("0번뜨면 최종 단계 랜덤 번호 : " + ran );
        int prevrare = CraftRare1;
        bool isbool = false;
        switch (ran)
        {
            case >= 66 and <= 100 when CraftRare1 < 0: //66 ~ 100  35%
                CraftRare1 = 0;
                isbool = true;
                break;
            case <= 65 and >= 36 when CraftRare1 < 1: // 36~70  30%
                CraftRare1 = 1;
                isbool = true;
                break;
            case <= 35 and >= 21 when CraftRare1 < 2: // 21~35 15%
                CraftRare1 = 2;
                isbool = true;
                break;
            case <= 20 and >= 9 when CraftRare1 < 3:  // 9~20  10%
                CraftRare1 = 3;
                isbool = true;
                break;
            case <= 8 and >= 2 when CraftRare1 < 4:   // 2~8  7%
                CraftRare1 = 4;
                isbool = true;
                break;
            case <= 1 when CraftRare1 < 5:    //0~1 2%
                CraftRare1 = 5;
                isbool = true;
                break;
            default:
                isbool = false;
                break;
        }

        if (isbool)
        {
            Settingmanager.Instance.OnlyInvenSave();
        }
        LogManager.CraftRareLog(prevrare,CraftRare1,Inventory.Instance.data.KeyId1);
        return isbool;
    }

    public bool GetCraftTierUp_5()
    {
        float temp = Time.time * 100f;
        int seed = (int)temp + PlayerBackendData.Instance.GetRandomSeed();
        UnityEngine.Random.InitState(seed);
        int ran = UnityEngine.Random.Range(1, 101);
//        Debug.Log("0번뜨면 최종 단계 랜덤 번호 : " + ran );
        int prevrare = CraftRare1;
        bool isbool = false;
Debug.Log(" 랜덤" + ran);
        if (ran <= 2)
        {
            CraftRare1 = 6;
            chatmanager.Instance.ChattoCRAFTUP(Inventory.Instance.data.itemid);
            isbool = true;
        }
        
        if (isbool)
        {
            Settingmanager.Instance.OnlyInvenSave();
        }
        LogManager.CraftRareLog(prevrare,CraftRare1,Inventory.Instance.data.KeyId1);
        return isbool;
    }
    
    //현재 등급 이상이 나온다. 
    public bool GetRareUp()
    {
        string[] randompercent = EquipItemDB.Instance.Find_id(itemid).RarePercent.Split(';');
        float temp = Time.time * 100f;
        int seed = (int)temp + PlayerBackendData.Instance.GetRandomSeed();
        UnityEngine.Random.InitState(seed);
        int ran = UnityEngine.Random.Range(1, 100);
        int minval = int.Parse(Itemrare); //현재 등급이 제일 낮은 등급
        string prev = Itemrare;
        //현재 등급이 더 높은 거면 진행
        bool isbool = false;
        if (randompercent[6] != "0" && minval < 6 )
        {
            if (randompercent[6] == "100" || int.Parse(randompercent[6]) >= ran)
            {
                Itemrare = "6";
            }
        }

        //현재 등급이 더 높은 거면 진행
        if (randompercent[5] != "0" && minval < 5 )
        {
            if (randompercent[5] == "100" || int.Parse(randompercent[5]) >= ran)
            {
                minval = 5;
                Itemrare = "5";

                isbool = true;


            }
        }
        
        //현재 등급이 더 높은 거면 진행
        if (randompercent[4] != "0" && minval < 4 )
        {
            if (randompercent[4] == "100" || int.Parse(randompercent[4]) >= ran)
            {
                minval = 4;
                Itemrare = "4";

                isbool = true;


            }
        }
        
        //현재 등급이 더 높은 거면 진행
        if (randompercent[3] != "0" && minval < 3 )
        {
            if (randompercent[3] == "100" || int.Parse(randompercent[3]) >= ran)
            {
                minval = 3;
                Itemrare = "3";

                isbool = true;


            }
        }
        
        //현재 등급이 더 높은 거면 진행
        if (randompercent[2] != "0" && minval < 2 )
        {
            if (randompercent[2] == "100" || int.Parse(randompercent[2]) >= ran)
            {
                minval = 2;
                Itemrare = "2";
                isbool = true;
            }
        }
        
        //현재 등급이 더 높은 거면 진행
        if (randompercent[1] != "0" && minval < 1 )
        {
            if (randompercent[1] == "100" || int.Parse(randompercent[1]) >= ran)
            {
                minval = 1;
                Itemrare = "1";
                isbool = true;
            }
        }

        
        LogManager.RareLog(prev,Itemrare,Inventory.Instance.data.KeyId1);

        return isbool;

    }
    public bool GetRareUpShininh()
    {
        float temp = Time.time * 100f;
        int seed = (int)temp + PlayerBackendData.Instance.GetRandomSeed();
        UnityEngine.Random.InitState(seed);
        int ran = UnityEngine.Random.Range(1, 100);
        int minval = int.Parse(Itemrare); //현재 등급이 제일 낮은 등급
        //현재 등급이 더 높은 거면 진행
        if (ran <= 2)
        {
                Itemrare = "7";
                LogManager.RareLog("6",Itemrare,Inventory.Instance.data.KeyId1);
                chatmanager.Instance.ChattoRAREUP(Inventory.Instance.data.itemid);
                return true;
        }
        else
        {
            LogManager.RareLog("6",Itemrare,Inventory.Instance.data.KeyId1);
            return false;
        }
    }

    string GetRare()
    {
        string[] randompercent = EquipItemDB.Instance.Find_id(itemid).RarePercent.Split(';');
        float temp = Time.time * 100f;
        int seed = (int)temp + PlayerBackendData.Instance.GetRandomSeed();
        UnityEngine.Random.InitState(seed);
        int ran = UnityEngine.Random.Range(1, 100);
        string minval = "0";
       
        

        if (randompercent[6] != "0")
        {
            minval = "6";
            if (randompercent[6] == "100" || int.Parse(randompercent[6]) >= ran)
            {
                return "6";
            }
        }
        
        
        if (randompercent[5] != "0")
        {
            minval = "5";
            if (randompercent[5] == "100" || int.Parse(randompercent[5]) >= ran)
            {
                //커먼
                return "5";
            }
        }

        if (randompercent[4] != "0")
        {
            minval = "4";

            if (randompercent[4] == "100" || int.Parse(randompercent[4]) >= ran)
            {
                //커먼
                return "4";
            }
        }

        if (randompercent[3] != "0")
        {
            minval = "3";
            if (randompercent[3] == "100" || int.Parse(randompercent[3]) >= ran)
            {
                //커먼
                return "3";
            }
        }

        if (randompercent[2] != "0")
        {
            minval = "2";

            if (randompercent[2] == "100" || int.Parse(randompercent[2]) >= ran)
            {
                //커먼
                return "2";
            }
        }

        if (randompercent[1] != "0")
        {
            minval = "1";

            if (randompercent[1] == "100" || int.Parse(randompercent[1]) >= ran)
            {
                //커먼
                return "1";
            }
        }

        if (randompercent[0] != "0")
        {
            minval = "0";

            if (randompercent[0] == "100" || int.Parse(randompercent[0]) >= ran)
            {
                //커먼
                return "0";
            }
        }

        return minval;
    }   

    //제련 성공
    public void SuccessSmelt()
    {
        SmeltStatCount[StoneCount1] = 1; //1은 성공
        SmeltSuccCount1++;
        StoneCount1++;
        if (SmeltSuccCount1 > 6)
            chatmanager.Instance.ChattoSmeltUp(SmeltSuccCount1, itemid,Itemrare);
    }

    public void BigSuccessSmelt()
    {
        SmeltStatCount[StoneCount1] = 1; //1은 성공
        StoneCount1++;
        SmeltStatCount[StoneCount1] = 1; //1은 성공
        StoneCount1++;

        SmeltSuccCount1+=2;
        if (SmeltSuccCount1 > 6)
        chatmanager.Instance.ChattoSmeltUp(SmeltSuccCount1,itemid,Itemrare);
    }
    //제련 실패
    public void FailSmelt()
    {
        SmeltStatCount[StoneCount1] = 2; //1은 성공
        SmeltFailCount1++;
        StoneCount1++;
    }

    public void ResetSmelt()
    {
        for(int i = 0; i < SmeltStatCount.Length;i++)
        {
            SmeltStatCount[i] = 0;
        }
        SmeltSuccCount1= 0;
        SmeltFailCount1=0;
        StoneCount1=0;
    }

    public string GetInfo()
    {
        Inventory.Instance.StringRemove();
        Inventory.Instance.StringWrite(string.Format(Inventory.GetTranslate("Inventory/품질"), Inventory.GetTranslate(
            $"CraftRare/{CraftRare}")));
        Inventory.Instance.StringWrite("\n<color=#FFFF00>");

        Inventory.Instance.StringWrite(PlayerData.Instance.gettierstar(EquipItemDB.Instance.Find_id(itemid).Tier));
        Inventory.Instance.StringWrite("</color>\n");
        //Inventory.Instance.StringWrite(string.Format(Inventory.GetTranslate("Inventory/재질"), Inventory.GetTranslate(string.Format("Material/{0}", EquipItemDB.Instance.Find_id(itemid).material))));
        //Inventory.Instance.StringWrite("\n");
        Inventory.Instance.StringWrite(string.Format(Inventory.GetTranslate("Inventory/분류"), Inventory.GetTranslate(
            $"Material/{EquipItemDB.Instance.Find_id(itemid).Type}")));

        // 타입 : 투구

        return Inventory.Instance.StringEnd();
    }

    public string GetInfoNotMine()
    {
        Inventory.Instance.StringRemove();
        Inventory.Instance.StringWrite(string.Format(Inventory.GetTranslate("Inventory/품질기본"), Inventory.GetTranslate("CraftRare/0"), Inventory.GetTranslate("CraftRare/5")));
        Inventory.Instance.StringWrite("\n<color=#FFFF00>");

        Inventory.Instance.StringWrite(PlayerData.Instance.gettierstar(EquipItemDB.Instance.Find_id(itemid).Tier));
        Inventory.Instance.StringWrite("</color>\n");
        //Inventory.Instance.StringWrite(string.Format(Inventory.GetTranslate("Inventory/재질"), Inventory.GetTranslate(string.Format("Material/{0}", EquipItemDB.Instance.Find_id(itemid).material))));
        //Inventory.Instance.StringWrite("\n");
        Inventory.Instance.StringWrite(string.Format(Inventory.GetTranslate("Inventory/분류"), Inventory.GetTranslate(
            $"Material/{EquipItemDB.Instance.Find_id(itemid).Type}")));

        // 타입 : 투구

        return Inventory.Instance.StringEnd();
    }

    public string GetItemName()
    {
        Inventory.Instance.StringRemove();

        if (EnchantNum1 != 0)
        {
            Inventory.Instance.StringWrite($"+{EnchantNum1} ");
        }
        Inventory.Instance.StringWrite(Inventory.GetTranslate(EquipItemDB.Instance.Find_id(Itemid).Name));
        return Inventory.Instance.StringEnd();
    }

    public string GetItemNameNotMine()
    {
        Inventory.Instance.StringRemove();
        if (EnchantNum1 != 0)
        {
            Inventory.Instance.StringWrite($"+{EnchantNum1} ");
        }
        Inventory.Instance.StringWrite(Inventory.GetTranslate(EquipItemDB.Instance.Find_id(Itemid).Name));
        return Inventory.Instance.StringEnd();
    }
    public int GetBattlePointNotMine()
    {
        return int.Parse(EquipItemDB.Instance.Find_id(itemid).BattlePoint);
    }
    private int GetEquipTypeNum()
    {
        return EquipItemDB.Instance.Find_id(itemid).Type switch
        {
            "Weapon" => 0,
            "SWeapon" => 1,
            "Helmet" => 2,
            "Chest" => 3,
            "Glove" => 4,
            "Boot" => 5,
            "Ring" => 6,
            "Necklace" => 7,
            "Wing" => 8,
            "Pet" => 9,
            _ => 0
        };
    }

    //제련효율증가
    float GetSmeltStat()
    {
        switch (SmeltSuccCount1)
        
        {
            /*
                 case 0:
                return SmeltSuccCount1 * 0.1f;
            case 1:
                return SmeltSuccCount1 * 0.11f;
            case 2:
                return SmeltSuccCount1 * 0.12f;
            case 3:
               return SmeltSuccCount1 * 0.12f;
            case 4:
                return SmeltSuccCount1 * 0.13f;
            case 5:
                return SmeltSuccCount1 * 0.13f;
            case 6:
                return SmeltSuccCount1 * 0.14f;
            case 7:
                return SmeltSuccCount1 * 0.17f;
            case 8:
                return SmeltSuccCount1 * 0.18f;
            case 9:
                return SmeltSuccCount1 * 0.19f;
            case 10:
                return SmeltSuccCount1 * 0.22f;
            */

case 0:
    return SmeltSuccCount1 * 0.17f;
case 1:
    return SmeltSuccCount1 * 0.22f;
case 2:
    return SmeltSuccCount1 * 0.23f;
case 3:
   return SmeltSuccCount1 * 0.24f;
case 4:
    return SmeltSuccCount1 * 0.26f;
case 5:
    return SmeltSuccCount1 * 0.27f;
case 6:
    return SmeltSuccCount1 * 0.3f;
case 7:
    return SmeltSuccCount1 * 0.33f;
case 8:
    return SmeltSuccCount1 * 0.38f;
case 9:
    return SmeltSuccCount1 * 0.43f;
case 10:
    return SmeltSuccCount1 * 0.50f;
        }
        return SmeltSuccCount1 * 0.1f;
    }

    public string GetItemStat()
    {
        bool ishaveequip = false;
        EquipDatabase equippeddata = null;
        //무기가 있다면 해당 무기와 비교
        if (PlayerBackendData.Instance.GetEquipData()[GetEquipTypeNum()] != null)
        {
            ishaveequip = true;
            equippeddata = PlayerBackendData.Instance.GetEquipData()[GetEquipTypeNum()];
        }

        Inventory.Instance.StringRemove();
        EquipItemDB.Row data = EquipItemDB.Instance.Find_id(itemid);

        float smeltpercent = GetSmeltStat();

        //공격력
        if (data.minatk != "0")
        {
            float atk = float.Parse(data.minatk);
            float rareatk = atk * getrarepercent();
            float craftrareatk = atk * getcraftrarepercent();
            float smeltstat = atk * smeltpercent;
            float enchantstat = atk * getEnchantpercent();
            float totalatk = atk + rareatk + craftrareatk+ smeltstat +enchantstat;
            Inventory.Instance.StringWrite(string.Format(Inventory.GetTranslate("Stat/물리 공격력"), (totalatk *100f).ToString("N0")));
            Inventory.Instance.StringWrite(" (");
            //공격력
            Inventory.Instance.StringWrite((atk*100f).ToString("N0"));

            if (enchantstat != 0)
            {
                Inventory.Instance.StringWrite(" +");
                Inventory.Instance.StringWrite($"<color=yellow>{enchantstat*100f:N0}</color>");
            }
            if (rareatk != 0)
            {
                Inventory.Instance.StringWrite(" +");
                Inventory.Instance.StringWrite(getrareStat(rareatk*100f));
            }

            if (craftrareatk != 0)
            {
                Inventory.Instance.StringWrite(" +");
                Inventory.Instance.StringWrite(getcraftrareStat(craftrareatk*100f));
            }

            if (smeltpercent != 0)
            {
                Inventory.Instance.StringWrite(" +");
                Inventory.Instance.StringWrite($"<color=#FF0000>{(smeltstat * 100f):N0}</color>");
            }

            Inventory.Instance.StringWrite(")");

            //낀장비가 있다면 비교
           if(ishaveequip)
            {
                
                if(equippeddata.GetStat(4) < GetStat(4))
                {
                    //낀장비의 스탯이 더 낮으면
                    Inventory.Instance.StringWrite(" <color=#00ff00>↑");
                    Inventory.Instance.StringWrite((Mathf.Abs(equippeddata.GetStat(4) - GetStat(4)) * 100f).ToString("N0"));
                    Inventory.Instance.StringWrite("%</color>");

                }
                else if (equippeddata.GetStat(4) > GetStat(4))
                {
                    //낀장비의 스탯이 더 높으면
                    Inventory.Instance.StringWrite(" <color=#FF0000>↓");
                    Inventory.Instance.StringWrite(((equippeddata.GetStat(4) - GetStat(4) )* 100f).ToString("N0"));
                    Inventory.Instance.StringWrite("%</color>");
                }
                //장비가 있다 현재 공격과 적의 공격을 비교
            }



            Inventory.Instance.StringWrite("\n");
            //장비를 장착중이라면 해당 장비와 스탯을 비교한다.
        }

        //마법공격력
        if (data.matk != "0")
        {
            float atk = float.Parse(data.matk);
            float rareatk = atk * getrarepercent();
            float craftrareatk = atk * getcraftrarepercent();
            float smeltstat = atk * smeltpercent;
            float enchantstat = atk * getEnchantpercent();

            float totalatk = atk + rareatk + craftrareatk+ smeltstat + enchantstat;
            Inventory.Instance.StringWrite(string.Format(Inventory.GetTranslate("Stat/마법 공격력"), (totalatk * 100f).ToString("N0")));


            Inventory.Instance.StringWrite(" (");
            //공격력
            Inventory.Instance.StringWrite((atk *100f).ToString("N0"));

            if (enchantstat != 0)
            {
                Inventory.Instance.StringWrite(" +");
                Inventory.Instance.StringWrite($"<color=yellow>{enchantstat*100f:N0}</color>");
            }
            
            if (rareatk != 0)
            {
                Inventory.Instance.StringWrite(" +");
                Inventory.Instance.StringWrite(getrareStat(rareatk * 100f));
            }

            if (craftrareatk != 0)
            {
                Inventory.Instance.StringWrite(" +");
                Inventory.Instance.StringWrite(getcraftrareStat((craftrareatk * 100f)));
            }

            if (smeltpercent != 0)
            {
                Inventory.Instance.StringWrite(" +");
                Inventory.Instance.StringWrite($"<color=#FF0000>{(smeltstat * 100f):N0}</color>");
            }

            Inventory.Instance.StringWrite(")");

            //낀장비가 있다면 비교
            if (ishaveequip)
            {

                if (equippeddata.GetStat(5) < GetStat(5))
                {
                    //낀장비의 스탯이 더 높으면
                    Inventory.Instance.StringWrite(" <color=#00ff00>↑");
                    Inventory.Instance.StringWrite(Mathf.Abs((equippeddata.GetStat(5) - GetStat(5)) * 100f).ToString("N0"));
                    Inventory.Instance.StringWrite("</color>");

                }
                else if (equippeddata.GetStat(5) > GetStat(5))
                {
                    //낀장비의 스탯이 더 높으면
                    Inventory.Instance.StringWrite(" <color=#FF0000>↓");
                    Inventory.Instance.StringWrite(((equippeddata.GetStat(5) - GetStat(4)) * 100f).ToString("N0"));
                    Inventory.Instance.StringWrite("</color>");
                }
                //장비가 있다 현재 공격과 적의 공격을 비교
            }



            Inventory.Instance.StringWrite("\n");
            //장비를 장착중이라면 해당 장비와 스탯을 비교한다.
        }

        //방어력
        if (data.def != "0")
        {
            float def = float.Parse(data.def);
            float raredef = def * getrarepercent();
            float craftraredef = def * getcraftrarepercent();
            float smeltstat = def * smeltpercent;
            float totaldef = def + raredef + craftraredef + smeltstat;
            Inventory.Instance.StringWrite(string.Format(Inventory.GetTranslate("Stat/물리 방어력"), totaldef.ToString("N0")));


            Inventory.Instance.StringWrite(" (");
            //공격력
            Inventory.Instance.StringWrite(def.ToString("N0"));

            if (raredef != 0)
            {
                Inventory.Instance.StringWrite(" +");
                Inventory.Instance.StringWrite(getrareStat(raredef));
            }

            if (craftraredef != 0)
            {
                Inventory.Instance.StringWrite(" +");
                Inventory.Instance.StringWrite(getcraftrareStat(craftraredef));
            }
            if (smeltpercent != 0)
            {
                Inventory.Instance.StringWrite(" +");
                Inventory.Instance.StringWrite(getSmeltStat(smeltstat));
            }
            Inventory.Instance.StringWrite(")");

            //낀장비가 있다면 비교
            if (ishaveequip)
            {

                if (equippeddata.GetStat(6) < GetStat(6))
                {
                    //낀장비의 스탯이 더 높으면
                    Inventory.Instance.StringWrite(" <color=#00ff00>↑");
                    Inventory.Instance.StringWrite(Mathf.Abs(equippeddata.GetStat(6) - GetStat(6)).ToString("N0"));
                    Inventory.Instance.StringWrite("</color>");

                }
                else if (equippeddata.GetStat(6) > GetStat(6))
                {
                    //낀장비의 스탯이 더 높으면
                    Inventory.Instance.StringWrite(" <color=#FF0000>↓");
                    Inventory.Instance.StringWrite((equippeddata.GetStat(6) - GetStat(6)).ToString("N0"));
                    Inventory.Instance.StringWrite("</color>");
                }
                //장비가 있다 현재 공격과 적의 공격을 비교
            }

            Inventory.Instance.StringWrite("\n");
        }
        //방어력
        if (data.mdef != "0")
        {
            float def = float.Parse(data.mdef);
            float raredef = def * getrarepercent();
            float craftraredef = def * getcraftrarepercent();
            float smeltstat = def * smeltpercent;
            float totaldef = def + raredef + craftraredef+ smeltstat;
            Inventory.Instance.StringWrite(string.Format(Inventory.GetTranslate("Stat/마법 방어력"), totaldef.ToString("N0")));


            Inventory.Instance.StringWrite(" (");
            //공격력
            Inventory.Instance.StringWrite(def.ToString("N0"));

            if (raredef != 0)
            {
                Inventory.Instance.StringWrite(" +");
                Inventory.Instance.StringWrite(getrareStat(raredef));
            }

            if (craftraredef != 0)
            {
                Inventory.Instance.StringWrite(" +");
                Inventory.Instance.StringWrite(getcraftrareStat(craftraredef));
            }
            if (smeltpercent != 0)
            {
                Inventory.Instance.StringWrite(" +");
                Inventory.Instance.StringWrite(getSmeltStat(smeltstat));
            }
            Inventory.Instance.StringWrite(")");

            //낀장비가 있다면 비교
            if (ishaveequip)
            {

                if (equippeddata.GetStat(7) < GetStat(7))
                {
                    //낀장비의 스탯이 더 높으면
                    Inventory.Instance.StringWrite(" <color=#00ff00>↑");
                    Inventory.Instance.StringWrite(Mathf.Abs(equippeddata.GetStat(7) - GetStat(7)).ToString("N0"));
                    Inventory.Instance.StringWrite("</color>");

                }
                else if (equippeddata.GetStat(7) > GetStat(7))
                {
                    //낀장비의 스탯이 더 높으면
                    Inventory.Instance.StringWrite(" <color=#FF0000>↓");
                    Inventory.Instance.StringWrite((equippeddata.GetStat(7) - GetStat(7)).ToString("N0"));
                    Inventory.Instance.StringWrite("</color>");
                }
                //장비가 있다 현재 공격과 적의 공격을 비교
            }

            Inventory.Instance.StringWrite("\n");
        }
          //적응형능력치
        if (data.AllStat != "0")
        {
            int str = int.Parse(data.AllStat);
            int rareatk = (int)(str * getrarepercent());
            
            Debug.Log(rareatk);
            
            int craftrareatk = (int)(str * getcraftrarepercent());
            int smeltstat = (int)(str * smeltpercent);
            int enchantstat = (int)(str * getEnchantpercent());
            int totalstr = str + smeltstat + rareatk + craftrareatk + enchantstat;
            Inventory.Instance.StringWrite(string.Format(Inventory.GetTranslate("Stat/주 능력치"), totalstr.ToString("N0")));
            Inventory.Instance.StringWrite(" (");
            Inventory.Instance.StringWrite(str.ToString("N0"));
            
            
            if (enchantstat != 0)
            {
                Inventory.Instance.StringWrite(" +");
                Inventory.Instance.StringWrite($"<color=yellow>{enchantstat:N0}</color>");
            }
            
            if (rareatk != 0)
            {
                Inventory.Instance.StringWrite(" +");
                Inventory.Instance.StringWrite(getrareStat(rareatk));
            }

            if (craftrareatk != 0)
            {
                Inventory.Instance.StringWrite(" +");
                Inventory.Instance.StringWrite(getcraftrareStat(craftrareatk));
            }

            if (smeltpercent != 0)
            {
                Inventory.Instance.StringWrite(" +");
                Inventory.Instance.StringWrite(getSmeltStat(smeltstat));
            }
            Inventory.Instance.StringWrite(")");
            
            
            //낀장비가 있다면 비교
            if(ishaveequip)
            {
                
                if(equippeddata.GetStat(12) < GetStat(12))
                {
                    //낀장비의 스탯이 더 낮으면
                    Inventory.Instance.StringWrite(" <color=#00ff00>↑");
                    Inventory.Instance.StringWrite((Mathf.Abs(equippeddata.GetStat(12) - GetStat(12))).ToString("N0"));
                    Inventory.Instance.StringWrite("</color>");

                }
                else if (equippeddata.GetStat(12) > GetStat(12))
                {
                    //낀장비의 스탯이 더 높으면
                    Inventory.Instance.StringWrite(" <color=#FF0000>↓");
                    Inventory.Instance.StringWrite(((equippeddata.GetStat(12) - GetStat(12) )).ToString("N0"));
                    Inventory.Instance.StringWrite("</color>");
                }
                //장비가 있다 현재 공격과 적의 공격을 비교
            }
            
            
            Inventory.Instance.StringWrite("\n");
        }
        //제련
        //힘
        if (data.Str != "0")
        {
            int str = int.Parse(data.Str);
            Debug.Log("레어퍼센트" + getrarepercent());
            int rareatk = (int)(str * getrarepercent());
            int craftrareatk = (int)(str * getcraftrarepercent());
            int smeltstat = (int)(str * smeltpercent);
            int enchantstat = (int)(str * getEnchantpercent());
            int totalstr = str + smeltstat + rareatk + craftrareatk + enchantstat;
            Inventory.Instance.StringWrite(string.Format(Inventory.GetTranslate("Stat/힘"), totalstr.ToString("N0")));
            Inventory.Instance.StringWrite(" (");
            Inventory.Instance.StringWrite(str.ToString("N0"));
            
            
            if (enchantstat != 0)
            {
                Inventory.Instance.StringWrite(" +");
                Inventory.Instance.StringWrite($"<color=yellow>{enchantstat:N0}</color>");
            }
            
            
            if (rareatk != 0)
            {
                Inventory.Instance.StringWrite(" +");
                Inventory.Instance.StringWrite(getrareStat(rareatk));
            }

            if (craftrareatk != 0)
            {
                Inventory.Instance.StringWrite(" +");
                Inventory.Instance.StringWrite(getcraftrareStat(craftrareatk));
            }

            if (smeltpercent != 0)
            {
                Inventory.Instance.StringWrite(" +");
                Inventory.Instance.StringWrite(getSmeltStat(smeltstat));
            }
            Inventory.Instance.StringWrite(")");
            
            
            //낀장비가 있다면 비교
            if(ishaveequip)
            {
                
                if(equippeddata.GetStat(0) < GetStat(0))
                {
                    //낀장비의 스탯이 더 낮으면
                    Inventory.Instance.StringWrite(" <color=#00ff00>↑");
                    Inventory.Instance.StringWrite((Mathf.Abs(equippeddata.GetStat(0) - GetStat(0))).ToString("N0"));
                    Inventory.Instance.StringWrite("</color>");

                }
                else if (equippeddata.GetStat(0) > GetStat(0))
                {
                    //낀장비의 스탯이 더 높으면
                    Inventory.Instance.StringWrite(" <color=#FF0000>↓");
                    Inventory.Instance.StringWrite(((equippeddata.GetStat(0) - GetStat(0) )).ToString("N0"));
                    Inventory.Instance.StringWrite("</color>");
                }
                //장비가 있다 현재 공격과 적의 공격을 비교
            }
            
            
            Inventory.Instance.StringWrite("\n");
        }

        //민첩
        if (data.Dex != "0" )
        {
            int str = int.Parse(data.Dex);
            int rareatk = (int)(str * getrarepercent());
            int craftrareatk = (int)(str * getcraftrarepercent());
            int smeltstat = (int)(str * smeltpercent);
            int enchantstat = (int)(str * getEnchantpercent());
            int totaldex = str + smeltstat + rareatk + craftrareatk + enchantstat;
            Inventory.Instance.StringWrite(string.Format(Inventory.GetTranslate("Stat/민첩"), totaldex.ToString("N0")));

            Inventory.Instance.StringWrite(" (");
            Inventory.Instance.StringWrite(str.ToString("N0"));
            
            if (enchantstat != 0)
            {
                Inventory.Instance.StringWrite(" +");
                Inventory.Instance.StringWrite($"<color=yellow>{enchantstat:N0}</color>");
            }
            
            
            if (rareatk != 0)
            {
                Inventory.Instance.StringWrite(" +");
                Inventory.Instance.StringWrite(getrareStat(rareatk));
            }

            if (craftrareatk != 0)
            {
                Inventory.Instance.StringWrite(" +");
                Inventory.Instance.StringWrite(getcraftrareStat(craftrareatk));
            }

            if (smeltpercent != 0)
            {
                Inventory.Instance.StringWrite(" +");
                Inventory.Instance.StringWrite(getSmeltStat(smeltstat));
            }
            Inventory.Instance.StringWrite(")");
            
            //낀장비가 있다면 비교
            if(ishaveequip)
            {
                
                if(equippeddata.GetStat(1) < GetStat(1))
                {
                    //낀장비의 스탯이 더 낮으면
                    Inventory.Instance.StringWrite(" <color=#00ff00>↑");
                    Inventory.Instance.StringWrite((Mathf.Abs(equippeddata.GetStat(1) - GetStat(1))).ToString("N0"));
                    Inventory.Instance.StringWrite("</color>");

                }
                else if (equippeddata.GetStat(1) > GetStat(1))
                {
                    //낀장비의 스탯이 더 높으면
                    Inventory.Instance.StringWrite(" <color=#FF0000>↓");
                    Inventory.Instance.StringWrite(((equippeddata.GetStat(1) - GetStat(1) )).ToString("N0"));
                    Inventory.Instance.StringWrite("</color>");
                }
                //장비가 있다 현재 공격과 적의 공격을 비교
            }
            
            Inventory.Instance.StringWrite("\n");
        }

        //지능
        if (data.Int != "0")
        {
            int str = int.Parse(data.Int);
            int rareatk = (int)(str * getrarepercent());
            int craftrareatk = (int)(str * getcraftrarepercent());
            int smeltstat = (int)(str * smeltpercent);
            int enchantstat = (int)(str * getEnchantpercent());
            int totalInt = str + smeltstat + rareatk + craftrareatk + enchantstat;
            Inventory.Instance.StringWrite(string.Format(Inventory.GetTranslate("Stat/지능"), totalInt.ToString("N0")));

            Inventory.Instance.StringWrite(" (");
            Inventory.Instance.StringWrite(str.ToString("N0"));
            
            
            if (enchantstat != 0)
            {
                Inventory.Instance.StringWrite(" +");
                Inventory.Instance.StringWrite($"<color=yellow>{enchantstat:N0}</color>");
            }
            
            
            if (rareatk != 0)
            {
                Inventory.Instance.StringWrite(" +");
                Inventory.Instance.StringWrite(getrareStat(rareatk));
            }

            if (craftrareatk != 0)
            {
                Inventory.Instance.StringWrite(" +");
                Inventory.Instance.StringWrite(getcraftrareStat(craftrareatk));
            }

            if (smeltpercent != 0)
            {
                Inventory.Instance.StringWrite(" +");
                Inventory.Instance.StringWrite(getSmeltStat(smeltstat));
            }
            Inventory.Instance.StringWrite(")");
            
            //낀장비가 있다면 비교
            if(ishaveequip)
            {
                
                if(equippeddata.GetStat(2) < GetStat(2))
                {
                    //낀장비의 스탯이 더 낮으면
                    Inventory.Instance.StringWrite(" <color=#00ff00>↑");
                    Inventory.Instance.StringWrite((Mathf.Abs(equippeddata.GetStat(2) - GetStat(2))).ToString("N0"));
                    Inventory.Instance.StringWrite("</color>");

                }
                else if (equippeddata.GetStat(2) > GetStat(2))
                {
                    //낀장비의 스탯이 더 높으면
                    Inventory.Instance.StringWrite(" <color=#FF0000>↓");
                    Inventory.Instance.StringWrite(((equippeddata.GetStat(2) - GetStat(2) )).ToString("N0"));
                    Inventory.Instance.StringWrite("</color>");
                }
                //장비가 있다 현재 공격과 적의 공격을 비교
            }
            
            Inventory.Instance.StringWrite("\n");
        }

        //지혜
        if (data.Wis != "0")
        {
            int str = int.Parse(data.Wis);
            int rareatk = (int)(str * getrarepercent());
            int craftrareatk = (int)(str * getcraftrarepercent());
            int smeltstat = (int)(str * smeltpercent);
            int enchantstat = (int)(str * getEnchantpercent());
            int totalstr = str + smeltstat + rareatk + craftrareatk + enchantstat;
            Inventory.Instance.StringWrite(string.Format(Inventory.GetTranslate("Stat/지혜"), totalstr.ToString("N0")));

            Inventory.Instance.StringWrite(" (");
            Inventory.Instance.StringWrite(str.ToString("N0"));
            
            
            if (enchantstat != 0)
            {
                Inventory.Instance.StringWrite(" +");
                Inventory.Instance.StringWrite($"<color=yellow>{enchantstat:N0}</color>");
            }
            
            
            if (rareatk != 0)
            {
                Inventory.Instance.StringWrite(" +");
                Inventory.Instance.StringWrite(getrareStat(rareatk));
            }

            if (craftrareatk != 0)
            {
                Inventory.Instance.StringWrite(" +");
                Inventory.Instance.StringWrite(getcraftrareStat(craftrareatk));
            }

            if (smeltpercent != 0)
            {
                Inventory.Instance.StringWrite(" +");
                Inventory.Instance.StringWrite(getSmeltStat(smeltstat));
            }
            Inventory.Instance.StringWrite(")");
            
            //낀장비가 있다면 비교
            if(ishaveequip)
            {
                
                if(equippeddata.GetStat(3) < GetStat(3))
                {
                    //낀장비의 스탯이 더 낮으면
                    Inventory.Instance.StringWrite(" <color=#00ff00>↑");
                    Inventory.Instance.StringWrite((Mathf.Abs(equippeddata.GetStat(3) - GetStat(3))).ToString("N0"));
                    Inventory.Instance.StringWrite("</color>");

                }
                else if (equippeddata.GetStat(3) > GetStat(3))
                {
                    //낀장비의 스탯이 더 높으면
                    Inventory.Instance.StringWrite(" <color=#FF0000>↓");
                    Inventory.Instance.StringWrite(((equippeddata.GetStat(3) - GetStat(3) )).ToString("N0"));
                    Inventory.Instance.StringWrite("</color>");
                }
                //장비가 있다 현재 공격과 적의 공격을 비교
            }
            
            
            Inventory.Instance.StringWrite("\n");
        }

        
        //상태이상피해
        if (data.dotdmgup != "0")
        {
            float atkspd = float.Parse(data.dotdmgup);
            Inventory.Instance.StringWrite(string.Format(Inventory.GetTranslate("Stat/상태이상피해기본"), (atkspd * 100f).ToString("N0")));
            //낀장비가 있다면 비교
            if (ishaveequip)
            {
                if (equippeddata.GetStat(13) < GetStat(13))
                {
                    //낀장비의 스탯이 더 높으면
                    Inventory.Instance.StringWrite(" <color=#00ff00>↑");
                    Inventory.Instance.StringWrite(
                        $"{(Mathf.Abs(equippeddata.GetStat(13) - GetStat(13)) * 100f):N0}%");
                    Inventory.Instance.StringWrite("</color>");

                }
                else if (equippeddata.GetStat(13) > GetStat(13))
                {
                    //낀장비의 스탯이 더 높으면
                    Inventory.Instance.StringWrite(" <color=#FF0000>↓");
                    Inventory.Instance.StringWrite($"{((equippeddata.GetStat(13) - GetStat(13)) * 100f):N0}%");
                    Inventory.Instance.StringWrite("</color>");
                }
                //장비가 있다 현재 공격과 적의 공격을 비교
            }
            Inventory.Instance.StringWrite("\n");
            //장비를 장착중이라면 해당 장비와 스탯을 비교한다.
        }
        
        //공격속도
        if (data.atkspeed != "0")
        {
            float atkspd = float.Parse(data.atkspeed);
            Inventory.Instance.StringWrite(string.Format(Inventory.GetTranslate("Stat/공격 속도"), (atkspd * 100f).ToString("N0")));
            //낀장비가 있다면 비교
            if (ishaveequip)
            {
                if (equippeddata.GetStat(8) < GetStat(8))
                {
                    //낀장비의 스탯이 더 높으면
                    Inventory.Instance.StringWrite(" <color=#00ff00>↑");
                    Inventory.Instance.StringWrite(
                        $"{(Mathf.Abs(equippeddata.GetStat(8) - GetStat(8)) * 100f):N0}%");
                    Inventory.Instance.StringWrite("</color>");

                }
                else if (equippeddata.GetStat(8) > GetStat(8))
                {
                    //낀장비의 스탯이 더 높으면
                    Inventory.Instance.StringWrite(" <color=#FF0000>↓");
                    Inventory.Instance.StringWrite($"{((equippeddata.GetStat(8) - GetStat(8)) * 100f):N0}%");
                    Inventory.Instance.StringWrite("</color>");
                }
                //장비가 있다 현재 공격과 적의 공격을 비교
            }
            Inventory.Instance.StringWrite("\n");
            //장비를 장착중이라면 해당 장비와 스탯을 비교한다.
        }
        //시전속도
        if (data.castspeed != "0")
        {
            float atkspd = float.Parse(data.castspeed);
            Inventory.Instance.StringWrite(string.Format(Inventory.GetTranslate("Stat/시전 속도"), (atkspd * 100f).ToString("N0")));
            //공격력

            //낀장비가 있다면 비교
            if (ishaveequip)
            {

                if (equippeddata.GetStat(9) < GetStat(9))
                {
                    //낀장비의 스탯이 더 높으면
                    Inventory.Instance.StringWrite(" <color=#00ff00>↑");
                    Inventory.Instance.StringWrite(
                        $"{(Mathf.Abs(equippeddata.GetStat(9) - GetStat(9)) * 100f):N0}%");
                    Inventory.Instance.StringWrite("</color>");

                }
                else if (equippeddata.GetStat(9) > GetStat(9))
                {
                    //낀장비의 스탯이 더 높으면
                    Inventory.Instance.StringWrite(" <color=#FF0000>↓");
                    Inventory.Instance.StringWrite($"{((equippeddata.GetStat(9) - GetStat(9)) * 100f):N0}%");
                    Inventory.Instance.StringWrite("</color>");
                }
                //장비가 있다 현재 공격과 적의 공격을 비교
            }



            Inventory.Instance.StringWrite("\n");
            //장비를 장착중이라면 해당 장비와 스탯을 비교한다.
        }

        //체력
        if (data.Hp != "0")
        {
            float atkspd = float.Parse(data.Hp);
            Inventory.Instance.StringWrite(string.Format(Inventory.GetTranslate("Stat/체력"), (atkspd * 100f).ToString("N0")));
            //공격력

            //낀장비가 있다면 비교
            if (ishaveequip)
            {

                if (equippeddata.GetStat(10) < GetStat(10))
                {
                    //낀장비의 스탯이 더 높으면
                    Inventory.Instance.StringWrite(" <color=#00ff00>↑");
                    Inventory.Instance.StringWrite(
                        $"{(Mathf.Abs(equippeddata.GetStat(10) - GetStat(10)) * 100f):N0}%");
                    Inventory.Instance.StringWrite("</color>");

                }
                else if (equippeddata.GetStat(10) > GetStat(10))
                {
                    //낀장비의 스탯이 더 높으면
                    Inventory.Instance.StringWrite(" <color=#FF0000>↓");
                    Inventory.Instance.StringWrite($"{((equippeddata.GetStat(10) - GetStat(10)) * 100f):N0}%");
                    Inventory.Instance.StringWrite("</color>");
                }
                //장비가 있다 현재 공격과 적의 공격을 비교
            }
            /*
            float str = float.Parse(data.Hp);
            float smeltstr = str * smeltpercent;
            float totalstr = str + smeltstr;
            Inventory.Instance.StringWrite(string.Format(Inventory.GetTranslate("Stat/체력"), (totalstr * 100f).ToString("N0")));

            Inventory.Instance.StringWrite(" (");
            //공격력
            Inventory.Instance.StringWrite((str*100f).ToString("N0"));

            if (smeltstr != 0)
            {
                Inventory.Instance.StringWrite(" +");
                Inventory.Instance.StringWrite((smeltstr*100f).ToString("N0"));
            }

            Inventory.Instance.StringWrite(")");

            if (ishaveequip)
            {
                if (equippeddata.GetStat(10) < GetStat(10))
                {
                    //낀장비의 스탯이 더 높으면
                    Inventory.Instance.StringWrite(" <color=#00ff00>↑");
                    Inventory.Instance.StringWrite(
                        $"{((Mathf.Abs(equippeddata.GetStat(10) - GetStat(10)) * 100f)):N0}%");
                    Inventory.Instance.StringWrite("</color>");

                }
                else if (equippeddata.GetStat(10) > GetStat(10))
                {
                    //낀장비의 스탯이 더 높으면
                    Inventory.Instance.StringWrite(" <color=#FF0000>↓");
                    Inventory.Instance.StringWrite(
                        $"{((equippeddata.GetStat(10) - GetStat(10)) * 100f):N0}%");
                    Inventory.Instance.StringWrite("</color>");
                }
                //장비가 있다 현재 공격과 적의 공격을 비교
            }


*/
            Inventory.Instance.StringWrite("\n");
        }
        if (data.Mp != "0")
        {
            float atkspd = float.Parse(data.Mp);
            Inventory.Instance.StringWrite(string.Format(Inventory.GetTranslate("Stat/정신력"), (atkspd * 100f).ToString("N0")));
            //공격력

            //낀장비가 있다면 비교
            if (ishaveequip)
            {

                if (equippeddata.GetStat(11) < GetStat(11))
                {
                    //낀장비의 스탯이 더 높으면
                    Inventory.Instance.StringWrite(" <color=#00ff00>↑");
                    Inventory.Instance.StringWrite(
                        $"{(Mathf.Abs(equippeddata.GetStat(11) - GetStat(11)) * 100f):N0}%");
                    Inventory.Instance.StringWrite("</color>");

                }
                else if (equippeddata.GetStat(11) > GetStat(11))
                {
                    //낀장비의 스탯이 더 높으면
                    Inventory.Instance.StringWrite(" <color=#FF0000>↓");
                    Inventory.Instance.StringWrite($"{((equippeddata.GetStat(11) - GetStat(11)) * 100f):N0}%");
                    Inventory.Instance.StringWrite("</color>");
                }
                //장비가 있다 현재 공격과 적의 공격을 비교
            }
            /*
            float str = float.Parse(data.Mp);
            float smeltstr = str * smeltpercent;
            float totalstr = str + smeltstr;

            Inventory.Instance.StringWrite(string.Format(Inventory.GetTranslate("Stat/정신력"), (totalstr * 100f).ToString("N0")));

            Inventory.Instance.StringWrite(" (");
            //공격력
            Inventory.Instance.StringWrite((str*100f).ToString("N0"));

            if (smeltstr != 0)
            {
                Inventory.Instance.StringWrite(" +");
                Inventory.Instance.StringWrite((smeltstr * 100f).ToString("N0"));
            }

            Inventory.Instance.StringWrite(")");

            if (ishaveequip)
            {
                if (equippeddata.GetStat(11) < GetStat(11))
                {
                    //낀장비의 스탯이 더 높으면
                    Inventory.Instance.StringWrite(" <color=#00ff00>↑");
                    Inventory.Instance.StringWrite(
                        $"{((Mathf.Abs(equippeddata.GetStat(11) - GetStat(11)) * 100f)):N0}%");
                    Inventory.Instance.StringWrite("</color>");

                }
                else if (equippeddata.GetStat(11) > GetStat(11))
                {
                    //낀장비의 스탯이 더 높으면
                    Inventory.Instance.StringWrite(" <color=#FF0000>↓");
                    Inventory.Instance.StringWrite(
                        $"{((equippeddata.GetStat(11) - GetStat(11)) * 100f):N0}%");
                    Inventory.Instance.StringWrite("</color>");
                }
                //장비가 있다 현재 공격과 적의 공격을 비교
            }
*/
            Inventory.Instance.StringWrite("\n");
        }

        if (data.Crit != "0")
        {
            int str = int.Parse(data.Crit);
            Inventory.Instance.StringWrite(string.Format(Inventory.GetTranslate("Stat/치명타확률"), str.ToString("N0")));

            Inventory.Instance.StringWrite("\n");
        }
        if (data.CritDmg != "0")
        {
            float str = float.Parse(data.CritDmg);
            Inventory.Instance.StringWrite(string.Format(Inventory.GetTranslate("Stat/치명타피해"), (str*100f).ToString("N0")));
            Inventory.Instance.StringWrite("\n");
        }

        if (Alldmg1 != 0)
        {
            float max = float.Parse(data.MaxAllDmg);
            Inventory.Instance.StringWrite(string.Format(Inventory.GetTranslate("Stat/피해 증가"), Alldmg1.ToString("N0")));
            Inventory.Instance.StringWrite("\n");
        }

        return Inventory.Instance.StringEnd();
    }

    public string GetItemStatUnReal()
    {
        //무기가 있다면 해당 무기와 비교

        Inventory.Instance.StringRemove();
        EquipItemDB.Row data = EquipItemDB.Instance.Find_id(itemid);

        float smeltpercent = GetSmeltStat();

        //공격력
        if (data.minatk != "0")
        {
            float atk = float.Parse(data.minatk);
            float rareatk = atk * getrarepercent();
            float craftrareatk = atk * getcraftrarepercent();
            float smeltstat = atk * smeltpercent;
            float totalatk = atk + rareatk + craftrareatk + smeltstat;

            if (atk == totalatk)
            {
                //~가 없다
                Inventory.Instance.StringWrite($"{Inventory.GetTranslate("Stat/물리 공격력")} : {totalatk*100f:N0}");
            }
            else
            {
                //~가 있다
                Inventory.Instance.StringWrite(
                    $"{Inventory.GetTranslate("Stat/물리공격력기본")} : {atk*100f:N0}% ~ {totalatk*100f:N0}%");
            }

            Inventory.Instance.StringWrite("\n");
            //장비를 장착중이라면 해당 장비와 스탯을 비교한다.
        }

        //마법공격력
        if (data.matk != "0")
        {
            float atk = float.Parse(data.matk);
            float rareatk = atk * getrarepercent();
            float craftrareatk = atk * getcraftrarepercent();
            float smeltstat = atk * smeltpercent;
            float totalatk = atk + rareatk + craftrareatk + smeltstat;


            if (atk == totalatk)
            {
                //~가 없다
                Inventory.Instance.StringWrite($"{Inventory.GetTranslate("Stat/마법 공격력")} : {totalatk*100f:N0}%");
            }
            else
            {
                //~가 있다
                Inventory.Instance.StringWrite(
                    $"{Inventory.GetTranslate("Stat/마법공격력기본")} : {atk*100f:N0}% ~ {totalatk*100f:N0}%");
            }


            Inventory.Instance.StringWrite("\n");
            //장비를 장착중이라면 해당 장비와 스탯을 비교한다.
        }


        //방어력
        if (data.def != "0")
        {
            float def = float.Parse(data.def);
            float raredef = def * getrarepercent();
            float craftraredef = def * getcraftrarepercent();//물리 방어력기본
            float smeltstat = def * smeltpercent;
            float totaldef = def + raredef + craftraredef + smeltstat;


            if (def == totaldef)
            {
                //~가 없다
                Inventory.Instance.StringWrite($"{Inventory.GetTranslate("Stat/물리 방어력기본")} : {totaldef:N0}");
            }
            else
            {
                //~가 있다
                Inventory.Instance.StringWrite(
                    $"{Inventory.GetTranslate("Stat/물리 방어력기본")} : {def:N0} ~ {totaldef:N0}");
            }

            Inventory.Instance.StringWrite("\n");
        }
        //마법방어력
        if (data.mdef != "0")
        {
            float def = float.Parse(data.mdef);
            float raredef = def * getrarepercent();
            float craftraredef = def * getcraftrarepercent();
            float smeltstat = def * smeltpercent;
            float totaldef = def + raredef + craftraredef + smeltstat;

            if (def == totaldef)
            {
                //~가 없다
                Inventory.Instance.StringWrite($"{Inventory.GetTranslate("Stat/마법 방어력기본")} : {totaldef:N0}");
            }
            else
            {
                //~가 있다
                Inventory.Instance.StringWrite(
                    $"{Inventory.GetTranslate("Stat/마법 방어력기본")} : {def:N0} ~ {totaldef:N0}");
            }

            Inventory.Instance.StringWrite("\n");
        }
        //제련
        //힘
        if (data.AllStat != "0")
        {
            int str = int.Parse(data.AllStat);
            int smeltstat = (int)(str * smeltpercent);
            int totalstr = str + smeltstat;
            if (str == totalstr)
            {
                //~가 없다
                Inventory.Instance.StringWrite($"{Inventory.GetTranslate("Stat/주 능력치기본")} : {totalstr:N0}");
            }
            else
            {
                //~가 있다
                Inventory.Instance.StringWrite(
                    $"{Inventory.GetTranslate("Stat/주 능력치기본")} : {str:N0} ~ {totalstr:N0}");
            }

            Inventory.Instance.StringWrite("\n");
        }
        //제련
        //힘
        if (data.Str != "0")
        {
            int str = int.Parse(data.Str);
            int smeltstat = (int)(str * smeltpercent);
            int totalstr = str + smeltstat;
            if (str == totalstr)
            {
                //~가 없다
                Inventory.Instance.StringWrite($"{Inventory.GetTranslate("Stat/힘기본")} : {totalstr:N0}");
            }
            else
            {
                //~가 있다
                Inventory.Instance.StringWrite(
                    $"{Inventory.GetTranslate("Stat/힘기본")} : {str:N0} ~ {totalstr:N0}");
            }

            Inventory.Instance.StringWrite("\n");
        }

        //민첩
        if (data.Dex != "0")
        {
            int dex = int.Parse(data.Dex);
            int smeltstat = (int)(dex * smeltpercent);
            int totaldex = dex + smeltstat;

            if (dex  == totaldex)
            {
                //~가 없다
                Inventory.Instance.StringWrite($"{Inventory.GetTranslate("Stat/민첩기본")} : {totaldex:N0}");
            }
            else
            {
                //~가 있다
                Inventory.Instance.StringWrite(
                    $"{Inventory.GetTranslate("Stat/민첩기본")} : {dex:N0} ~ {totaldex:N0}");
            }


            Inventory.Instance.StringWrite("\n");
        }

        //지능
        if (data.Int != "0")
        {
            int Int = int.Parse(data.Int);
            int smeltstat = (int)(Int * smeltpercent);
            int totalInt = Int + smeltstat;


            if (Int == totalInt)
            {
                //~가 없다
                Inventory.Instance.StringWrite($"{Inventory.GetTranslate("Stat/지능기본")} : {totalInt:N0}");
            }
            else
            {
                //~가 있다
                Inventory.Instance.StringWrite(
                    $"{Inventory.GetTranslate("Stat/지능기본")} : {Int:N0} ~ {totalInt:N0}");
            }


            Inventory.Instance.StringWrite("\n");
        }

        //지혜
        if (data.Wis != "0")
        {
            int str = int.Parse(data.Wis);
            int smeltstat = (int)(str * smeltpercent);
            int totalstr = str + smeltstat;


            if (str == totalstr)
            {
                //~가 없다
                Inventory.Instance.StringWrite($"{Inventory.GetTranslate("Stat/지혜기본")} : {totalstr:N0}");
            }
            else
            {
                //~가 있다
                Inventory.Instance.StringWrite(
                    $"{Inventory.GetTranslate("Stat/지혜기본")} : {str:N0} ~ {totalstr:N0}");
            }

            Inventory.Instance.StringWrite("\n");
        }

        //공격속도
        if (data.dotdmgup != "0")
        {
            float atkspd = float.Parse(data.dotdmgup);

            Inventory.Instance.StringWrite($"{Inventory.GetTranslate("Stat/상태이상피해")} : {atkspd*100f:N0}%");

            Inventory.Instance.StringWrite("\n");
            //장비를 장착중이라면 해당 장비와 스탯을 비교한다.
        }
        
        //공격속도
        if (data.atkspeed != "0")
        {
            float atkspd = float.Parse(data.atkspeed);

            Inventory.Instance.StringWrite($"{Inventory.GetTranslate("Stat/공격속도기본")} : {atkspd*100:N0}%");

            Inventory.Instance.StringWrite("\n");
            //장비를 장착중이라면 해당 장비와 스탯을 비교한다.
        }
        //시전속도
        if (data.castspeed != "0")
        {
            float atkspd = float.Parse(data.castspeed);


            Inventory.Instance.StringWrite($"{Inventory.GetTranslate("Stat/시전속도기본")} : {atkspd*100f:N0}%");

            Inventory.Instance.StringWrite("\n");
            //장비를 장착중이라면 해당 장비와 스탯을 비교한다.
        }

        //체력
        if (data.Hp != "0")
        {
            float str = float.Parse(data.Hp);
            float smeltstr = 0;
            float totalstr = str + smeltstr;

            Inventory.Instance.StringWrite($"{Inventory.GetTranslate("Stat/생명력기본")} : {totalstr*100f:N0}%");

            Inventory.Instance.StringWrite("\n");
        }
        if (data.Mp != "0")
        {
            float str = float.Parse(data.Mp);
            float smeltstr = 0;
            float totalstr = str + smeltstr;


            Inventory.Instance.StringWrite($"{Inventory.GetTranslate("Stat/정신력기본")} : {totalstr*100f:N0}%");


            Inventory.Instance.StringWrite("\n");
        }

        //체력
        if (data.Crit != "0")
        {
            int str = int.Parse(data.Crit);

            Inventory.Instance.StringWrite($"{Inventory.GetTranslate("Stat/치명타확률기본")} : {str:N0}%");

            Inventory.Instance.StringWrite("\n");
        }

        //체력
        if (data.CritDmg != "0")
        {
            float str = float.Parse(data.CritDmg);

            Inventory.Instance.StringWrite($"{Inventory.GetTranslate("Stat/치명타피해기본")} : {(str * 100f):N0}%");

            Inventory.Instance.StringWrite("\n");
        }
        
        //체력
        if (data.MinAllDmg != "0")
        {
            float min = float.Parse(data.MinAllDmg);
            float max = float.Parse(data.MaxAllDmg);

            
            //~가 있다
            Inventory.Instance.StringWrite(
                $"{Inventory.GetTranslate("Stat/피해 증가기본")} : {min:N0}% ~ {max:N0}%");

            Inventory.Instance.StringWrite("\n");
        }

        return Inventory.Instance.StringEnd();
    }
    float getcraftrarepercent()
    {
        return CraftRare switch
        {
            0 => 0,
            1 => 0.2f,
            2 => 0.4f,
            3 => 0.6f,
            4 => 0.8f,
            5 => 1f,
            6 => 1.5f,
            _ => 1
        };
    }
    float getsmeltstat()
    {
        return SmeltSuccCount1 switch
        {
            0 => 0,
            1 => 0.1f,
            2 => 0.2f,
            3 => 0.3f,
            4 => 0.4f,
            5 => 0.5f,
            6 => 0.65f,
            7 => 0.8f,
            8 => 1.00f,
            9 => 1.3f,
            10 => 1.6f,
            _ => 0
        };
    }
    //장비 강화
    float getupgradestat()
    {
        return EnchantNum1 * 0.015f;
    }
    string getcraftrareStat(float stat)
    {
        return CraftRare switch
        {
            0 => stat.ToString("N0"),
            1 => $"<color=#0055FF>{stat:N0}</color>",
            2 => $"<color=#FF7000>{stat:N0}</color>",
            3 => $"<color=#E500FF>{stat:N0}</color>",
            4 => $"<color=#FF003F>{stat:N0}</color>",
            5 => $"<color=#00FFC9>{stat:N0}</color>",
            6 => $"<color=#F8FF5F>{stat:N0}</color>",
            7 => $"<color=#F8FF5F>{stat:N0}</color>",
            _ => stat.ToString("N0")
        };
    }
    string getSmeltStat(float stat)
    {
        return SmeltSuccCount1 switch
        {
            0 => stat.ToString("N0"),
            1 => $"<color=#0055FF>{stat:N0}</color>",
            2 => $"<color=#FF7000>{stat:N0}</color>",
            3 => $"<color=#FF7000>{stat:N0}</color>",
            4 => $"<color=#E500FF>{stat:N0}</color>",
            5 => $"<color=#E500FF>{stat:N0}</color>",
            6 => $"<color=#FF003F>{stat:N0}</color>",
            7 => $"<color=#FF003F>{stat:N0}</color>",
            8 => $"<color=#00FFC9>{stat:N0}</color>",
            9 => $"<color=#00FFC9>{stat:N0}</color>",
            10 => $"<color=#F8FF5F>{stat:N0}</color>",
            _ => stat.ToString("N0")
        };
    }
    string getrareStat(float stat)
     {
         return itemrare switch
         {
             "0" => stat.ToString("N0"),
             "1" => $"<color=#9EFF00>{stat:N0}</color>",
             "2" => $"<color=#0055FF>{stat:N0}</color>",
             "3" => $"<color=#FF7000>{stat:N0}</color>",
             "4" => $"<color=#E500FF>{stat:N0}</color>",
             "5" => $"<color=#FF003F>{stat:N0}</color>",
             "6" => $"<color=#00FFC9>{stat:N0}</color>",
             "7" => $"<color=#F8FF5F>{stat:N0}</color>",
             _ => stat.ToString("N0")
         };
     }
    
    
    public int GetEquipSkillCount()
    {
        int totalcount = EquipSkill1.Count;

        if (EquipItemDB.Instance.Find_id(itemid).SpeMehodP != "0")
        {
            totalcount--;
        }

////        Debug.Log("효과개수는" + totalcount);
        return totalcount;
    }

    float getrarepercent()
    {
//        Debug.Log(Itemrare+"아이템등급");
        switch (itemrare)
        {
            case "0":
                return 0;
            case "1":
                return 0.2f;
            case "2":
                return 0.4f;
            case "3":
                return 0.6f;
            case "4":
                return 0.8f;
            case "5":
                return 1f;
            case "6":
                return 1.2f;
            case "7":
                return 1.7f;
            default:
                return 1;
        }
    }

    //강화 스탯
    float getEnchantpercent()
    {
        if (EnchantNum1.Equals(0))
            return 0;
        
        return float.Parse(EquipUpgradeDB.Instance.Find_num(EnchantNum1.ToString()).upgradestat);
     
    }
    
    
    public bool ishaveitem(string id)
    {
        if (itemid == id)
            return true;
        else
            return false;
    }


    #region 스탯

    public float GetStat(int num)
    {
        EquipItemDB.Row data = EquipItemDB.Instance.Find_id(itemid);
        float raredata = getrarepercent();
        float craftdata = getcraftrarepercent();
        float enchantdata = getEnchantpercent();

        return num switch
        {
            0 => //힘
                float.Parse(data.Str)+ (float.Parse(data.Str) * raredata) + (float.Parse(data.Str) * enchantdata) +
                (float.Parse(data.Str) * craftdata) + (float.Parse(data.Str) * (GetSmeltStat())),
            1 => //민첩
                float.Parse(data.Dex)+ (float.Parse(data.Dex) * raredata) + (float.Parse(data.Dex) * enchantdata) +
                (float.Parse(data.Dex) * craftdata) + (float.Parse(data.Dex) * (GetSmeltStat())),
            2 => //지능
                float.Parse(data.Int)+ (float.Parse(data.Int) * raredata) + (float.Parse(data.Int) * enchantdata) +
                (float.Parse(data.Int) * craftdata) + (float.Parse(data.Int) * (GetSmeltStat())),
            3 => //지혜
                float.Parse(data.Wis)+ (float.Parse(data.Wis) * raredata) + (float.Parse(data.Wis) * enchantdata) +
                (float.Parse(data.Wis) * craftdata) + (float.Parse(data.Wis) * (GetSmeltStat())),
            4 => //물공
                float.Parse(data.minatk) + (float.Parse(data.minatk) * raredata) + (float.Parse(data.minatk) * enchantdata) +
                (float.Parse(data.minatk) * craftdata) + (float.Parse(data.minatk) * (GetSmeltStat())),
            5 => //마공
                float.Parse(data.matk) + (float.Parse(data.matk) * raredata) + (float.Parse(data.matk) * craftdata) +
                (float.Parse(data.matk) * enchantdata) +
                (float.Parse(data.matk) * (GetSmeltStat())),
            6 => //방어
                //  Debug.Log("물방" + float.Parse(data.def) + (float.Parse(data.def) * raredata) + (float.Parse(data.def) * craftdata));
                float.Parse(data.def) + (float.Parse(data.def) * raredata) + (float.Parse(data.def) * craftdata),
            7 => //마방
                float.Parse(data.mdef) + (float.Parse(data.mdef) * raredata) + (float.Parse(data.mdef) * craftdata),
            8 => //공속
                float.Parse(data.atkspeed),
            9 => //시속
                float.Parse(data.castspeed),
            10 => //HP
                float.Parse(data.Hp),
            11 => //MP
                float.Parse(data.Mp),
            12 => //주능력치
                float.Parse(data.AllStat)+ (float.Parse(data.AllStat) * raredata) + (float.Parse(data.AllStat) * enchantdata) +
                (float.Parse(data.AllStat) * craftdata) + (float.Parse(data.AllStat) * (GetSmeltStat())),
            13 => //상태이상-
                float.Parse(data.dotdmgup),
            14 => //치명타
                float.Parse(data.Crit),
            15 => //치명타피해
                float.Parse(data.CritDmg),
            16 => //보스피해
                0,
            17 => //재사용시간
                0,
            _ => -135878
        };
    }


    #endregion


}
