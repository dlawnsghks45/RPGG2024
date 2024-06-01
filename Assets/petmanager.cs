using System.Collections;
using System.Collections.Generic;
using BackEnd;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;

public class petmanager : MonoBehaviour
{
    private static petmanager _instance = null;
    public static petmanager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(petmanager)) as petmanager;

                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }
            return _instance;
        }
    }
    public Dictionary<string, PetSlot> listslot = new Dictionary<string, PetSlot>();
    public PetSlot petobj;
    public Transform A_trans;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < PetDB.Instance.NumRows(); i++)
        {
            PetSlot a = Instantiate(petobj, A_trans);
            a.Init(PetDB.Instance.Find_panelnum(i.ToString()).id);
            listslot.Add(a.Petid, a);
            a.gameObject.SetActive(true);
        }
    }
    public UIView selectobj;
    public string selectid;
    public Image selectimage;
    public Text selectaname;
    public Text selectahavecount;
    public Text selectinfo;
    public Text selectinfoEquip;


    public GameObject UseButton;
    public GameObject AlreadyUseButton;
    public GameObject[] StarBack;
    public GameObject[] StarCount;


    public float GetStat(string isPercent, string Stat)
    {
      //  Debug.Log(Stat);
//        Debug.Log(isPercent);
        if (isPercent.Equals("P"))
        {
            return float.Parse(Stat) * 100f;
        }
        else
        {
            return float.Parse(Stat);
        }
    }
    
    public void Bt_ShowPet(string id)
    {
        petdatabase database = PlayerBackendData.Instance.PetData[id];
        PetDB.Row data = PetDB.Instance.Find_id(database.Petid);
        selectid = id;
        selectimage.sprite = SpriteManager.Instance.GetSprite(data.sprite);
        selectaname.text = Inventory.GetTranslate(data.name);
        selectaname.color = Inventory.Instance.GetRareColor(data.rare);
        bool isequip = false;
        UseButton.SetActive(false);
        AlreadyUseButton.SetActive(false);
        
        foreach (var t in StarBack)
        {
            t.SetActive(false);
        }
        foreach (var t in StarCount)
        {
            t.SetActive(false);
        }
        
      
        string[] stathavenum = data.stathavenum.Split(';');

        if (bool.Parse(data.statpercent))
        {
            float p1 = float.Parse(stathavenum[database.Petstar]) * 100f;
            selectinfo.text = string.Format(Inventory.GetTranslate(data.info),p1);
        }
        else
        {
            selectinfo.text = string.Format(Inventory.GetTranslate(data.info),stathavenum[database.Petstar]);
        }
                                        
        
          //능력치
          string[] ispercent = data.ispercent.Split(';');
          string[] stat0 = data.Stat0.Split(';');
          string[] stat1 = data.Stat1.Split(';');
          string[] stat2 = data.Stat2.Split(';');
          string[] stat3 = data.Stat3.Split(';');
          string[] stat4 = data.Stat4.Split(';');

          
          switch (ispercent.Length)
          {
              case 1:
                  selectinfoEquip.text = string.Format(Inventory.GetTranslate(data.info_equip),
                      GetStat(ispercent[0], stat0[database.Petstar]).ToString("N1"));
                  break;
              case 2:
                  selectinfoEquip.text = string.Format(Inventory.GetTranslate(data.info_equip),
                      GetStat(ispercent[0], stat0[database.Petstar]).ToString("N1"),
                      GetStat(ispercent[1], stat1[database.Petstar]).ToString("N1"));
                  break;
              case 3:
                  selectinfoEquip.text = string.Format(Inventory.GetTranslate(data.info_equip),
                      GetStat(ispercent[0], stat0[database.Petstar]).ToString("N1"),
                      GetStat(ispercent[1], stat1[database.Petstar]).ToString("N1"),
                      GetStat(ispercent[2], stat2[database.Petstar]).ToString("N1")
                      );
                  break;
              case 4:
                  selectinfoEquip.text = string.Format(Inventory.GetTranslate(data.info_equip),
                      GetStat(ispercent[0], stat0[database.Petstar]).ToString("N1"),
                      GetStat(ispercent[1], stat1[database.Petstar]).ToString("N1"),
                      GetStat(ispercent[2], stat2[database.Petstar]).ToString("N1"),
                      GetStat(ispercent[3], stat3[database.Petstar]).ToString("N1")
                  );
                  break;
              case 5:
                  selectinfoEquip.text = string.Format(Inventory.GetTranslate(data.info_equip),
                      GetStat(ispercent[0], stat0[database.Petstar]).ToString("N1"),
                      GetStat(ispercent[1], stat1[database.Petstar]).ToString("N1"),
                      GetStat(ispercent[2], stat2[database.Petstar]).ToString("N1"),
                      GetStat(ispercent[3], stat3[database.Petstar]).ToString("N1"),
                      GetStat(ispercent[4], stat4[database.Petstar]).ToString("N1")
                  );
                  break;
          }          
          
                //별최대개수
        for (int i = 0; i < int.Parse(PetDB.Instance.Find_id(database.Petid).starcount); i++)
        {
            StarBack[i].SetActive(true);
        }
        //별개수
        for (int i = 0; i < database.Petstar; i++)
        {
            StarCount[i].SetActive(true);
        }
        
        if (database.Havecount > 1)
            selectahavecount.text = $"X{database.Havecount}";
        else
        {
            selectahavecount.text = "";
        }
        
        if (!database.Ishave)
        {
            //없다
        }
        else
        {
            //가지고 있다
            
            //장착 안함
            if (database.Isequip)
            {
                //장착중
                AlreadyUseButton.SetActive(true);
            }
            else
            {
                UseButton.SetActive(true);
            }
        }
        
        selectobj.Show(false);
    }


    //펫얻기
    //초월
    public void Bt_GetPet(string id)
    {
        if (!PlayerBackendData.Instance.PetData[id].Ishave)
        {
            //가지고있다면 늘린다.
            PlayerBackendData.Instance.PetData[id].Ishave = true;
        }
        else
        {
            PlayerBackendData.Instance.PetData[id].Havecount++;
        }
        petmanager.Instance.listslot[id].Init(id);
    }
    public UIView PetShopObj;
    
    
    public GameObject Item1button;
    public GameObject Item10button;
    public GameObject Item1button2;
    public GameObject Item10button2;
    
    public void OpenPetShop()
    {
        gauge.RefreshBar(PlayerBackendData.Instance.PetCount[0], 40);
        Item1button.SetActive(false);
        Item10button.SetActive(false);
        Item1button2.SetActive(false);
        Item10button2.SetActive(false);
        if (PlayerBackendData.Instance.CheckItemCount("50006") > 0)
        {
            Item1button.SetActive(true);
            Item1button2.SetActive(true);
        }
        if (PlayerBackendData.Instance.CheckItemCount("50006") >= 10)
        {
            Item10button.SetActive(true);
            Item10button2.SetActive(true);
        }
        PetShopObj.Show();
    }

    

    public string[] rare1;
    public string[] rare2;
    public string[] rare3;
    public string[] rare4;
    public string[] rare5;
    public string[] rare6;

    public UIView OpenPetObj;
    public petboxslot[] openboxslots;
    public Animator openboxani;
    public GameObject RareUpObj;
    public List<string> RewardList = new List<string>();

    [SerializeField] private gaugebarslot gauge;

    public void Bt_OpenCrystalBox2000()
    {
        if (PlayerBackendData.Instance.GetCash() >= 2000)
        {
            PlayerData.Instance.DownCash(2000);
            OpenStartPetBox(1);
        }
        else
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/불꽃부족"),alertmanager.alertenum.주의);
        }
    }
    
    public void Bt_OpenCrystalBox19000()
    {
        if (PlayerBackendData.Instance.GetCash() >= 19000)
        {
            PlayerData.Instance.DownCash(19000);
            OpenStartPetBox(10);
        }
        else
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/불꽃부족"),alertmanager.alertenum.주의);
        }
    }
    public void Bt_OpenCrystalBoxItem1()
    {
        if (PlayerBackendData.Instance.CheckItemCount("50006") >= 1)
        {
            PlayerBackendData.Instance.RemoveItem("50006",1);
            OpenStartPetBox(1);
        }
        else
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/아이템부족"),alertmanager.alertenum.주의);
        }
    }
    
    public void Bt_OpenCrystalBoxItem10()
    {
        if (PlayerBackendData.Instance.CheckItemCount("50006") >= 10)
        {
            PlayerBackendData.Instance.RemoveItem("50006",10);
            OpenStartPetBox(10);
        }
        else
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/아이템부족"),alertmanager.alertenum.주의);
        }
    }




    public void OpenStartPetBox(int opencount)
    {
        openboxani.SetTrigger("2_open");
        RewardList.Clear();
        OpenPetObj.Show(false);
        RareUpObj.SetActive(false);
        for (int i = 0; i < openboxslots.Length; i++)
        {
            openboxslots[i].gameObject.SetActive(false);
        }

        openExitobj.SetActive(false);
        openbuyobj.SetActive(false);

        PlayerBackendData.Instance.PetCount[0] += opencount;
        bool israre6open = false;

        if (PlayerBackendData.Instance.PetCount[0] >= 40)
        {
            PlayerBackendData.Instance.PetCount[0] -= 40;
            israre6open = true;
        }
        israre6 = false;

        //뽑기 계산
        for (int i = 0; i < opencount; i++)
        {
            Random.InitState(PlayerBackendData.Instance.GetRandomSeed() + (int)Time.deltaTime);
            int ran = Random.Range(0, 101);
//ㅣ            Debug.Log(ran);
            int r = 0;

            if (israre6open)
            {
                if (ran > 3)
                {
                    //6단계가 안나오면
                    //일부로 6단계로 맞춤
                    ran = 0;
                    israre6open = false;
                }

            }

            if (ran is <= 100 and >= 71)
            {
                //1급
                r = Random.Range(0, rare1.Length);
                RewardList.Add(rare1[r]);

            }
            else if (ran is <= 70 and >= 47)
            {
                //1급47
                r = Random.Range(0, rare2.Length);
                RewardList.Add(rare2[r]);

            }
            else if (ran is <= 46 and >= 27)
            {
                //1급
                r = Random.Range(0, rare3.Length);
                RewardList.Add(rare3[r]);

            }
            else if (ran is <= 26 and >= 13)
            {
                //1급
                r = Random.Range(0, rare4.Length);
                RewardList.Add(rare4[r]);

            }
            else if (ran is <= 12 and >= 4)
            {
                //1급
                r = Random.Range(0, rare5.Length);
                RewardList.Add(rare5[r]);

            }
            else if (ran is <= 3 and >= 0)
            {
                //1급
                israre6 = true;
                r = Random.Range(0, rare6.Length);
                RareUpObj.SetActive(true);
                RewardList.Add(rare6[r]);
                chatmanager.Instance.ChattoPet(rare6[r],"6");
                
            }
        }


        foreach (var t in RewardList)
        {
            Bt_GetPet(t);
        }

        TutorialTotalManager.Instance.CheckGuideQuest("petopen");

        PlayerBackendData userData = PlayerBackendData.Instance;
        Param paramEquip = new Param();
        paramEquip.Add("PetData", userData.PetData);
        paramEquip.Add("PetCount", userData.PetCount);
        paramEquip.Add("nowPetid", userData.nowPetid);
        if (PlayerBackendData.Instance.nowPetid != "")
        {
            paramEquip.Add("nowPetData", userData.PetData[userData.nowPetid]);
        }
        paramEquip.Add("inventory", userData.ItemInventory);
        paramEquip.Add("Crystal", userData.GetCash());

        Where where = new Where();
        where.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);

        SendQueue.Enqueue(Backend.GameData.Update, "PlayerData", where, paramEquip, (callback) =>
        {
            Debug.Log(callback);
            // 이후 처리
            if (callback.IsSuccess())
            {
            
                openboxani.SetTrigger("1_open");
                gauge.RefreshBar(PlayerBackendData.Instance.PetCount[0], 40);
                StartCoroutine(boxopen());
                OpenPetShop();
                LogManager.PetLog(RewardList.ToArray());
                Savemanager.Instance.SaveInventory();
                Savemanager.Instance.SaveCash();
                Savemanager.Instance.Save();
                alertmanager.Instance.NotiCheck_Pet();
            }
        });
    }

    public void SavePet()
    {
        
        PlayerBackendData userData = PlayerBackendData.Instance;
        Param paramEquip = new Param();
        
        paramEquip.Add("CollectionNew", userData.RenewalCollectData);
        paramEquip.Add("PetData", userData.PetData);
        paramEquip.Add("PetCount", userData.PetCount);
        paramEquip.Add("nowPetid", userData.nowPetid);
        if (PlayerBackendData.Instance.nowPetid != "")
        {
            paramEquip.Add("nowPetData", userData.PetData[userData.nowPetid]);
        }
        Where where = new Where();
        where.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);

        SendQueue.Enqueue(Backend.GameData.Update, "PlayerData", where, paramEquip, (callback) =>
        {
            // 이후 처리
            if (callback.IsSuccess())
            {
            
            }
        });
    }

    public GameObject openExitobj;
    public GameObject openbuyobj;
    public UIToggle[] Toggles;

    private bool israre6= false;
    
    IEnumerator boxopen()
    {
        yield return SpriteManager.Instance.GetWaitforSecond(2.3f);
        //계산 출력
        for (int i = 0; i < RewardList.Count; i++)
        {
            openboxslots[i].gameObject.SetActive(true);
            openboxslots[i].ShowPet(RewardList[i]);
            yield return SpriteManager.Instance.GetWaitforSecond(0.15f);
        }
        yield return SpriteManager.Instance.GetWaitforSecond(1f);
        openbuyobj.SetActive(true);
        openExitobj.SetActive(true);
    }

    public int panelstate; //기본 0 합성1 분해2
    //펫 분해
    public UIView PetSmeltObj;
    public List<string> SmeltListid = new List<string>();
    public List<int> SmeltListhw = new List<int>();

    //현재 개수
    public Text[] SmeltCountRare;
    public int[] SmeltCountRareCount;
    
    
    //총 보상 개수
    public int SmeltTotalRewarcCount;
    public Text SmeltRewardCount;
    
    
    public void Bt_SmeltingObjShow()
    {
        SmeltListid.Clear();
        SmeltListhw.Clear();
        Bt_SetState(2);
        PetSmeltObj.Show(false);
        SmeltRewardCount.text = "0";

        foreach (var t in SmeltCountRare)
        {
            t.text = "0";
        }

        for (int i = 0; i < SmeltCountRareCount.Length; i++)
        {
            SmeltCountRareCount[i] = 0;
        }
    }


    public void Bt_SellectSmeltingData(string keyid,bool isall = false,bool isminus = false)
    {
        Debug.Log(PlayerBackendData.Instance.PetData[keyid].Havecount);
        int index = SmeltListid.IndexOf(keyid);
        if (index != -1)
        {
            if (!isminus)
            {
                if (SmeltListhw[index] >= PlayerBackendData.Instance.PetData[keyid].Havecount)
                {
                    Debug.Log("더이상등록이 불가능");
                    return;
                }
            }

            if (isall)
            {
                SmeltListhw[index] = PlayerBackendData.Instance.PetData[keyid].Havecount;
            }
            else if(!isminus)
            {
                SmeltListhw[index]++;
            }
            else
            {
                SmeltListhw[index]--;
                listslot[keyid].RefreshSmelt(SmeltListhw[index]);
                RefreshSmeltCount();
                //빼서 0이되면
                return;
            }
            //마이너스
            if (SmeltListhw[index].Equals(0))
            {
                SmeltListid.RemoveAt(index);
                SmeltListhw.RemoveAt(index);
                listslot[keyid].RefreshSmelt(0);
            }
            else
            {
                listslot[keyid].RefreshSmelt(SmeltListhw[index]);
            }
        }
        else
        {
            //없다
            SmeltListid.Add(keyid);
            SmeltListhw.Add(1);
            listslot[keyid].RefreshSmelt(SmeltListhw[^1]);
            
        }
        RefreshSmeltCount();
    }
    void RefreshSmeltCount()
    {
        for (int i = 0; i < SmeltCountRareCount.Length; i++)
        {
            SmeltCountRareCount[i] = 0;
        }
        
        for (int i = 0; i < SmeltListid.Count; i++)
        {
           PetDB.Row datas = PetDB.Instance.Find_id(SmeltListid[i]);
           int rare = int.Parse(datas.rare)-1;
           SmeltCountRareCount[rare] += SmeltListhw[i];
        }
        for (int i = 0; i < SmeltCountRare.Length; i++)
        {
            SmeltCountRare[i].text = SmeltCountRareCount[i].ToString();
        }
        RefreshTotalSmelt();
    }
    void RefreshTotalSmelt()
    {
        SmeltTotalRewarcCount = 0;   
        for (int i = 0; i < SmeltListid.Count; i++)
        {
            SmeltTotalRewarcCount += int.Parse(PetDB.Instance.Find_id(SmeltListid[i]).smeltcount) *
                                     SmeltListhw[i];
        }
        SmeltRewardCount.text = SmeltTotalRewarcCount.ToString();
    }
    public void Bt_ExitSmelt()
    {
        Bt_SetState(0);
        foreach (var VARIABLE in listslot)
        {
            VARIABLE.Value.Init(VARIABLE.Key);
        }
    }
    public void Bt_SetState(int num)
    {
        panelstate = num;
    }
    public void Bt_FinishSmelt()
    {
        if (SmeltListid.Count == 0)
        {
            SmeltTotalRewarcCount = 0;
            Bt_SetState(0);
            PetSmeltObj.Hide(false);
            return;
        }
        for (int i = 0; i < SmeltListid.Count; i++)
        {
            PlayerBackendData.Instance.PetData[SmeltListid[i]].Havecount -= SmeltListhw[i];
            listslot[SmeltListid[i]].Init(SmeltListid[i]);
        }
        Inventory.Instance.AddItem("50007", SmeltTotalRewarcCount);
        
        PlayerBackendData userData = PlayerBackendData.Instance;
        Param paramEquip = new Param();
        paramEquip.Add("PetData", userData.PetData);
        paramEquip.Add("PetCount", userData.PetCount);
        paramEquip.Add("nowPetid", userData.nowPetid);
        if (PlayerBackendData.Instance.nowPetid != "")
        {
            paramEquip.Add("nowPetData", userData.PetData[userData.nowPetid]);
        }

        paramEquip.Add("inventory", userData.ItemInventory);
        paramEquip.Add("Crystal", userData.GetCash());

        Where where = new Where();
        where.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);

        List<string> id = new List<string>();
        List<string> hw = new List<string>();

        id.Add("50007");
        hw.Add(SmeltTotalRewarcCount.ToString());
        SendQueue.Enqueue(Backend.GameData.Update, "PlayerData", where, paramEquip, (callback) =>
        {
            // 이후 처리
            if (callback.IsSuccess())
            {
                SmeltTotalRewarcCount = 0;
                Bt_SetState(0);
                Inventory.Instance.ShowEarnItem(id.ToArray(),hw.ToArray(),false);
                Savemanager.Instance.SaveInventory();
                Savemanager.Instance.SaveCash();
                LogManager.LogSaveInven();
                Savemanager.Instance.Save();
            }
            else
            {
                Inventory.Instance.AddItem("50007", SmeltTotalRewarcCount);
                for (int i = 0; i < SmeltListid.Count; i++)
                {
                    PlayerBackendData.Instance.PetData[SmeltListid[i]].Havecount += SmeltListhw[i];
                    listslot[SmeltListid[i]].Init(SmeltListid[i]);
                    Bt_SetState(0);
                    Savemanager.Instance.SaveInventory();
                    Savemanager.Instance.SaveCash();
                    Savemanager.Instance.Save();
                }
            }
        });
        PetSmeltObj.Hide(false);
    }


    public void SelectAllSmeltRare(string rare)
    {
        foreach (var VARIABLE in listslot)
        {
            if (VARIABLE.Value.Rare.Equals(rare))
            {
                VARIABLE.Value.Bt_SmeltAll();
            }
        }
    }
    
    
   //초월
   public Animator[] starani;
   public UIView upgradepanel;
   public Text upgradename;
   public Text upgradestathave;
   public Text upgradestatinfo;

   
   public Image upgrademonster;
   public Text upgradeneedhw;
   public Text upgradeneedpethw;

   public void Bt_ShowUpgrade()
   {
       int nowstar = PlayerBackendData.Instance.PetData[selectid].Petstar;
       PetDB.Row Datas = PetDB.Instance.Find_id(selectid);
       
       if (nowstar.Equals(4))
       {
           Debug.Log("최대 치 입니다.");
           upgradepanel.Hide(false);
           return;
       }
/*
       if (PlayerBackendData.Instance.PetData[selectid].Havecount < nowstar)
       {
            Debug.Log("펫이 부족합니다.");
           upgradepanel.Hide(false);
           return;
       }

  */     
       for (int i = 0; i < starani.Length; i++)
       {
           starani[i].gameObject.SetActive(false);
       }

       for (int i = 0; i < nowstar; i++)
       {
           starani[i].gameObject.SetActive(true);
       }

       upgradename.text = Inventory.GetTranslate(Datas.name);
       upgradename.color = Inventory.Instance.GetRareColor(Datas.rare);
       
       string[] stathavenum = Datas.stathavenum.Split(';');
       if (Datas.statpercent.Equals("TRUE"))
       {
           float p1 = float.Parse(stathavenum[nowstar]) * 100f;
           float p2= float.Parse(stathavenum[nowstar+1]) * 100f;
           upgradestathave.text = string.Format(Inventory.GetTranslate(Datas.info_upgrade),p1,p2);
       }
       else
       {
           upgradestathave.text = string.Format(Inventory.GetTranslate(Datas.info_upgrade),stathavenum[nowstar],stathavenum[nowstar+1]);
       }

        //능력치
          string[] ispercent = Datas.ispercent.Split(';');
          string[] stat0 = Datas.Stat0.Split(';');
          string[] stat1 = Datas.Stat1.Split(';');
          string[] stat2 = Datas.Stat2.Split(';');
          string[] stat3 = Datas.Stat3.Split(';');
          string[] stat4 = Datas.Stat4.Split(';');

          
          switch (ispercent.Length)
          {
              case 1:
                  Debug.Log(ispercent[0]);
                  Debug.Log(GetStat(ispercent[0], stat0[nowstar]));
                  upgradestatinfo.text = string.Format(Inventory.GetTranslate(Datas.info_equip_upgrade),
                      GetStat(ispercent[0], stat0[nowstar]).ToString("N1"),GetStat(ispercent[0], stat0[nowstar+1]).ToString("N1")
                      );
                  break;
              case 2:
                  upgradestatinfo.text = string.Format(Inventory.GetTranslate(Datas.info_equip_upgrade),
                      GetStat(ispercent[0], stat0[nowstar]).ToString("N1"),
                      GetStat(ispercent[1], stat1[nowstar]).ToString("N1"),
                      GetStat(ispercent[0], stat0[nowstar+1]).ToString("N1"),
                      GetStat(ispercent[1], stat1[nowstar+1]).ToString("N1")
                  );
                  break;
              case 3:
                  upgradestatinfo.text = string.Format(Inventory.GetTranslate(Datas.info_equip_upgrade),
                      GetStat(ispercent[0], stat0[nowstar]).ToString("N1"),
                      GetStat(ispercent[1], stat1[nowstar]).ToString("N1"),
                      GetStat(ispercent[2], stat2[nowstar]).ToString("N1"),
                      GetStat(ispercent[0], stat0[nowstar+1]).ToString("N1"),
                      GetStat(ispercent[1], stat1[nowstar+1]).ToString("N1"),
                      GetStat(ispercent[2], stat2[nowstar+1]).ToString("N1")
                  );
                  break;

              case 4:
                  upgradestatinfo.text = string.Format(Inventory.GetTranslate(Datas.info_equip_upgrade),
                      GetStat(ispercent[0], stat0[nowstar]).ToString("N1"),
                      GetStat(ispercent[1], stat1[nowstar]).ToString("N1"),
                      GetStat(ispercent[2], stat2[nowstar]).ToString("N1"),
                      GetStat(ispercent[3], stat3[nowstar]).ToString("N1"),
                      GetStat(ispercent[0], stat0[nowstar+1]).ToString("N1"),
                      GetStat(ispercent[1], stat1[nowstar+1]).ToString("N1"),
                      GetStat(ispercent[2], stat2[nowstar+1]).ToString("N1"),
                      GetStat(ispercent[3], stat3[nowstar+1]).ToString("N1")
                  );
                  break;
              case 5:
                  upgradestatinfo.text = string.Format(Inventory.GetTranslate(Datas.info_equip_upgrade),
                      GetStat(ispercent[0], stat0[nowstar]).ToString("N1"),
                      GetStat(ispercent[1], stat1[nowstar]).ToString("N1"),
                      GetStat(ispercent[2], stat2[nowstar]).ToString("N1"),
                      GetStat(ispercent[3], stat3[nowstar]).ToString("N1"),
                      GetStat(ispercent[4], stat4[nowstar]).ToString("N1"),
                      GetStat(ispercent[0], stat0[nowstar+1]).ToString("N1"),
                      GetStat(ispercent[1], stat1[nowstar+1]).ToString("N1"),
                      GetStat(ispercent[2], stat2[nowstar+1]).ToString("N1"),
                      GetStat(ispercent[3], stat3[nowstar+1]).ToString("N1"),
                      GetStat(ispercent[4], stat4[nowstar+1]).ToString("N1")
                  );
                  break;
          }          
       
       string[] upgradeneednum = Datas.upgradehw.Split(';');
       upgradeneedhw.text = $"{upgradeneednum[nowstar]} ({PlayerBackendData.Instance.CheckItemCount("50007")})" ;
       upgrademonster.sprite = SpriteManager.Instance.GetSprite(Datas.sprite);
       upgradeneedpethw.text = $"{ (nowstar + 1).ToString()} ({PlayerBackendData.Instance.PetData[selectid].Havecount})" ;
       
       upgradepanel.Show(false);
   }

   public void Bt_FinishUpgrade()
   {
       if (!Settingmanager.Instance.CheckServerOn())
       {
           return;
       }
       
       int nowstar = PlayerBackendData.Instance.PetData[selectid].Petstar;
       PetDB.Row Datas = PetDB.Instance.Find_id(selectid);

       if (nowstar.Equals(4))
       {
           Debug.Log("최대 치 입니다.");
           return;
       }

       string[] upgradeneednum = Datas.upgradehw.Split(';');

       if (PlayerBackendData.Instance.CheckItemCount("50007") < int.Parse(upgradeneednum[nowstar]))
       {
           alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/아이템부족"),alertmanager.alertenum.주의);

           return;
       }

       if (PlayerBackendData.Instance.PetData[selectid].Havecount <= nowstar)
       {
           Debug.Log("펫이 부족합니다.");
           upgradepanel.Hide(false);
           return;
       }

       //다 충족했다!!
       PlayerBackendData.Instance.PetData[selectid].Havecount -= nowstar + 1;
       PlayerBackendData.Instance.RemoveItem("50007", int.Parse(upgradeneednum[nowstar]));
       PlayerBackendData.Instance.PetData[selectid].Petstar++; //늘린다.
       starani[PlayerBackendData.Instance.PetData[selectid].Petstar-1].gameObject.SetActive(true);
       starani[PlayerBackendData.Instance.PetData[selectid].Petstar-1].SetTrigger("show");
       Soundmanager.Instance.PlayerSound2("Sound/Special Click 05",1f);
       listslot[selectid].Init(selectid);
       Bt_ShowPet(selectid);
       Bt_ShowUpgrade();

       PlayerBackendData userData = PlayerBackendData.Instance;
       Param paramEquip = new Param();
       paramEquip.Add("PetData", userData.PetData);
       paramEquip.Add("PetCount", userData.PetCount);
       paramEquip.Add("nowPetid", userData.nowPetid);
       if (PlayerBackendData.Instance.nowPetid != "")
       {
           paramEquip.Add("nowPetData", userData.PetData[userData.nowPetid]);
       }

       paramEquip.Add("inventory", userData.ItemInventory);
       paramEquip.Add("Crystal", userData.GetCash());

       Where where = new Where();
       where.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);

       SendQueue.Enqueue(Backend.GameData.Update, "PlayerData", where, paramEquip, (callback) =>
       {
           // 이후 처리
           if (callback.IsSuccess())
           {
               PlayerData.Instance.RefreshPlayerstat();
               Savemanager.Instance.SaveInventory();
               Savemanager.Instance.SaveCash();
               LogManager.LogSaveInven();
               Savemanager.Instance.Save();
           }
           else
           {
               Inventory.Instance.AddItem("50007", int.Parse(upgradeneednum[nowstar]));
               PlayerBackendData.Instance.PetData[selectid].Havecount +=
                   PlayerBackendData.Instance.PetData[selectid].Petstar;
               PlayerBackendData.Instance.PetData[selectid].Petstar--;
               listslot[selectid].Init(selectid);
               PlayerData.Instance.RefreshPlayerstat();
               Savemanager.Instance.SaveInventory();
               Savemanager.Instance.SaveCash();
               Savemanager.Instance.Save();
           }
       });
   }

   public void Bt_EquipPet()
   {
       if (!Settingmanager.Instance.CheckServerOn())
       {
           return;
       }
       
       MapDB.Row mapdata_Now = MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage);
       if (mapdata_Now.maptype != "0")
       {
           alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI4/사냥터에서만변경가능"), alertmanager.alertenum.주의);
           return;
       }

       
       if (PlayerBackendData.Instance.PetData[selectid].Ishave)
       {
           if (PlayerBackendData.Instance.nowPetid != "")
           {
               //장착해제
               PlayerBackendData.Instance.PetData[PlayerBackendData.Instance.nowPetid].Isequip = false;
               listslot[PlayerBackendData.Instance.nowPetid].Init(PlayerBackendData.Instance.nowPetid);
           }
           PlayerBackendData.Instance.nowPetid = selectid;
           PlayerData.Instance.SetpetImage(SpriteManager.Instance.GetSprite(PetDB.Instance.Find_id(selectid).sprite),PetDB.Instance.Find_id(selectid).sprite);
           
           PlayerBackendData.Instance.PetData[PlayerBackendData.Instance.nowPetid].Isequip = true;
           listslot[PlayerBackendData.Instance.nowPetid].Init(PlayerBackendData.Instance.nowPetid);
           PlayerData.Instance.SetPetRare(PlayerBackendData.Instance.PetData[PlayerBackendData.Instance.nowPetid].Petstar);
           PlayerData.Instance.RefreshPlayerstat();
           
           Bt_ShowPet(PlayerBackendData.Instance.nowPetid);
           //저장
           PlayerBackendData userData = PlayerBackendData.Instance;
           Param paramEquip = new Param();
           paramEquip.Add("PetData", userData.PetData);
           paramEquip.Add("PetCount", userData.PetCount);
           paramEquip.Add("nowPetid", userData.nowPetid);
           if (PlayerBackendData.Instance.nowPetid != "")
           {
               paramEquip.Add("nowPetData", userData.PetData[userData.nowPetid]);
           }
           paramEquip.Add("inventory", userData.ItemInventory);
           paramEquip.Add("Crystal", userData.GetCash());

           Where where = new Where();
           where.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);

           TutorialTotalManager.Instance.CheckGuideQuest("petequip");

           
           SendQueue.Enqueue(Backend.GameData.Update, "PlayerData", where, paramEquip, (callback) =>
           {
               // 이후 처리
               if (callback.IsSuccess())
               {
               }
           });
       }
   }

   public UIView PetMixpanel;

   public Petmixmother[] MixMotherDatas;
    
   
   
   public void Bt_OpenMixPanel()
   {
       PetMixpanel.Show(false);

       for (int i = 0; i < MixMotherDatas.Length; i++)
       {
           MixMotherDatas[i].ResetAll();
       }
       
       Bt_SetState(1);
       PetMixDic.Clear();
   }

   
   public void Bt_ExitMixPanel()
   {
       PetMixpanel.Hide(false);
       Bt_SetState(0);
   }

   [SerializeField]private int nowselectmother;
   [SerializeField]private int nowselectsonsnum;
   public void ShowSelectMixItem(int mother,int son)
   {
       nowselectmother = mother;
       nowselectsonsnum = son;
       PetMixpanel.Hide(false);
   }

   public Dictionary<string, int> PetMixDic = new Dictionary<string, int>();

   public void EndSelectMixPet(string petid)
   {
       //개수가 부족
       if (PetMixDic.TryGetValue(petid, out var value))
       {
           if (PlayerBackendData.Instance.PetData[petid].Havecount == value)
           {
               Debug.Log("다썻다");
               return;
           }
       }
       //아무것도 없다.
       if (MixMotherDatas[nowselectmother].NowRare.Equals("-1"))
       {
           MixMotherDatas[nowselectmother].NowRare = PetDB.Instance.Find_id(petid).rare;
           MixMotherDatas[nowselectmother].SetData(nowselectsonsnum, petid);
           PetMixpanel.Show(false);
           Debug.Log("개수" + PetMixDic[petid]);
           return;
       }

       if (MixMotherDatas[nowselectmother].NowRare.Equals(PetDB.Instance.Find_id(petid).rare))
       {
           //넣는다
            MixMotherDatas[nowselectmother].SetData(nowselectsonsnum, petid);
            
            PetMixpanel.Show(false);
            Debug.Log("개수" + PetMixDic[petid]);
       }
       else
       {
           Debug.Log("넣을 수 없다.");
       }
   }
   
   public bool EndSelectMixPetBool(string petid)
   {
       if (PlayerBackendData.Instance.PetData[petid].Havecount <= 0)
           return false;
       //개수가 부족
       if (PetMixDic.TryGetValue(petid, out var value))
       {
           if (PlayerBackendData.Instance.PetData[petid].Havecount <= value)
           {
               Debug.Log("다썻다");
               return false;
           }
       }
       //아무것도 없다.
       if (MixMotherDatas[nowselectmother].NowRare.Equals("-1"))
       {
           MixMotherDatas[nowselectmother].NowRare = PetDB.Instance.Find_id(petid).rare;
           MixMotherDatas[nowselectmother].SetData(nowselectsonsnum, petid);
           PetMixpanel.Show(false);
           return true;
       }

       if (MixMotherDatas[nowselectmother].NowRare.Equals(PetDB.Instance.Find_id(petid).rare))
       {
           //넣는다
           MixMotherDatas[nowselectmother].SetData(nowselectsonsnum, petid);
            
           PetMixpanel.Show(false);
           return true;
       }
       else
       {
           Debug.Log("넣을 수 없다.");
           return false;
       }
   }

   public bool ismix;
   public bool issave;

   public List<string> log_removepet = new List<string>();
   public List<string> log_rewardpet = new List<string>();
   public void Bt_StartMixPet()
   {
       if (!Settingmanager.Instance.CheckServerOn())
       {
           return;
       }
       
       if (ismix)
           return;

       for (int i = 0; i < MixMotherDatas.Length; i++)
       {
           MixMotherDatas[i].StartMix();
       }

       if (issave)
       {
           PlayerBackendData userData = PlayerBackendData.Instance;
           Param paramEquip = new Param();
           paramEquip.Add("PetData", userData.PetData);
           paramEquip.Add("PetCount", userData.PetCount);
           paramEquip.Add("nowPetid", userData.nowPetid);
           if (PlayerBackendData.Instance.nowPetid != "")
           {
               paramEquip.Add("nowPetData", userData.PetData[userData.nowPetid]);
           }
           paramEquip.Add("inventory", userData.ItemInventory);
           paramEquip.Add("Crystal", userData.GetCash());

           Where where = new Where();
           where.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);


           SendQueue.Enqueue(Backend.GameData.Update, "PlayerData", where, paramEquip, (callback) =>
           {
               // 이후 처리
               if (callback.IsSuccess())
               {
                   Debug.Log(callback);
                   LogManager.PetMixLog(log_removepet.ToArray(),log_rewardpet.ToArray());
                   log_removepet.Clear();
                   log_rewardpet.Clear();
               }
               else
               {
                   for (int j = 0; j < MixMotherDatas.Length; j++)
                   {
                       for (int i = 0; i < MixMotherDatas[j].PetMixslots.Length; i++)
                       {
                           PlayerBackendData.Instance.PetData[MixMotherDatas[j].PetMixslots[i].petid].Havecount++;
                           petmanager.Instance.listslot[MixMotherDatas[j].PetMixslots[i].petid].Init(MixMotherDatas[j].PetMixslots[i].petid);
                       }
                       petmanager.Instance.listslot[MixMotherDatas[j].rewardid].Init(MixMotherDatas[j].rewardid);
                   }
                   log_removepet.Clear();
                   log_rewardpet.Clear();
               }
           });
       }
   }

   public void Bt_AutoRare(string rare)
   {
       for (int i = 0; i < MixMotherDatas.Length; i++)
       {
           MixMotherDatas[i].ResetAll();
           for (int j = 0; j < MixMotherDatas[i].PetMixslots.Length; j++)
           {
               foreach (var VARIABLE in PlayerBackendData.Instance.PetData)
               {
                   if(!PetDB.Instance.Find_id(VARIABLE.Value.Petid).rare.Equals(rare))
                       continue;
                   nowselectmother = i;
                   nowselectsonsnum = j;
                   if(!EndSelectMixPetBool(VARIABLE.Value.Petid))
                       continue;
                   else
                   {
                       break;
                   }
               }
           }
       }
       
       
   }
}
