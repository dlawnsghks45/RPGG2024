using System.Collections.Generic;
using System.Linq;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;

public class TalismanManager : MonoBehaviour
{
    //싱글톤만들기.
    private static TalismanManager _instance = null;
    public static TalismanManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(TalismanManager)) as TalismanManager;

                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }
            return _instance;
        }
    }
    
    
    public bool isequiping = false;
    public GameObject isequipingpanel;

    public talismansetpanel[] setslots;
    public talismanequipslot[] slots;
    public GameObject NoSetobj;
    private void Start()
    {
        InitInven();
        //Refresh();
        Bt_ChangePreset(PlayerBackendData.Instance.nowtalismanpreset);
        toggle[PlayerBackendData.Instance.nowtalismanpreset].IsOn = true;
    }

    public void Refresh()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].refersh(i);
        }
        CheckTalismanSet();
        PlayerData.Instance.RefreshPlayerstat();
    }


    public void CheckTalismanSet()
    {
        foreach (var VARIABLE in setslots)
        {
            VARIABLE.gameObject.SetActive(false);
        }

        NoSetobj.SetActive(true);
        
        PlayerBackendData.Instance.Talisman2Set.Clear();
        PlayerBackendData.Instance.Talisman3Set.Clear();
        PlayerBackendData.Instance.Talisman5Set.Clear();


        int[] setcounts = new int[9];
        
        for (int i = 0; i < PlayerBackendData.Instance.GiveEquipTalismanData().Length; i++)
        {
            if(PlayerBackendData.Instance.GiveEquipTalismanData()[i] != null)
            {
                setcounts[int.Parse(TalismanDB.Instance.Find_id(PlayerBackendData.Instance.GiveEquipTalismanData()[i].Itemid).num)]++;
            }
        }

        int equipsetnum = 0;

        for (int i = 0; i < setcounts.Length; i++)
        {
            Inventory.Instance.StringRemove();
            TalismanDB.Row data = TalismanDB.Instance.GetAt(i);
            bool isset = false;

            if (equipsetnum == 3)
            {
                Debug.Log("풀세트");
                break;
            }

            if (setcounts[i] >= 2)
            {
                Inventory.Instance.StringWrite("<color=cyan>");
                Inventory.Instance.StringWrite(Inventory.GetTranslate(data.setinfo1));
                Inventory.Instance.StringWrite("</color>");
                PlayerBackendData.Instance.Talisman2Set.Add(TalismanDB.Instance.GetAt(i).id);
                NoSetobj.SetActive(false);

                if (setcounts[i] >= 3)
                {
                    Inventory.Instance.StringWrite("\n<color=cyan>");
                    Inventory.Instance.StringWrite(Inventory.GetTranslate(data.setinfo2));
                    Inventory.Instance.StringWrite("</color>");
                    PlayerBackendData.Instance.Talisman3Set.Add(TalismanDB.Instance.GetAt(i).id);
                }
                else
                {
                    Inventory.Instance.StringWrite("\n<color=grey>");
                    Inventory.Instance.StringWrite(Inventory.GetTranslate(data.setinfo2));
                    Inventory.Instance.StringWrite("</color>");
                }
            
                if (setcounts[i] >= 5)
                {
                    Inventory.Instance.StringWrite("\n<color=cyan>");
                    Inventory.Instance.StringWrite(Inventory.GetTranslate(data.setinfo3));
                    Inventory.Instance.StringWrite("</color>");
                    PlayerBackendData.Instance.Talisman5Set.Add(TalismanDB.Instance.GetAt(i).id);
                }
                else
                {
                    Inventory.Instance.StringWrite("\n<color=grey>");
                    Inventory.Instance.StringWrite(Inventory.GetTranslate(data.setinfo3));
                    Inventory.Instance.StringWrite("</color>");
                }


                setslots[equipsetnum].Name.text = Inventory.GetTranslate(data.name);
                setslots[equipsetnum].Setinfo.text = Inventory.Instance.StringEnd();
                setslots[equipsetnum].Images.sprite = SpriteManager.Instance.GetSprite(data.sprite);
                setslots[equipsetnum].gameObject.SetActive(true);
                equipsetnum++;
            }
        }
    }


    public Transform[] Talismaninvenslot;
    public talismanslot talismanobj;
    public List<talismanslot> talismaninven = new List<talismanslot>();
    public UIView InvenPanel;

    public void InitInven()
    {
        foreach (var VARIABLE in Talismaninvenslot)
        {
            VARIABLE.gameObject.SetActive(false);            
        }
        inittalisman(PlayerBackendData.Instance.TalismanData.Count);        
        int num = 0;
        foreach (var v in PlayerBackendData.Instance.TalismanData)
        {
            talismaninven[num].Refresh(v.Value);
            talismaninven[num].gameObject.SetActive(true);
            TalismanDB.Row data = TalismanDB.Instance.Find_id(v.Value.Itemid);
            talismaninven[num].transform.SetParent(Talismaninvenslot[int.Parse(data.num)]);
            if (!Talismaninvenslot[int.Parse(data.num)].gameObject.activeSelf)
                Talismaninvenslot[int.Parse(data.num)].gameObject.SetActive(true);
            num++;
        }
    }

    public void Bt_ShowInven()
    {
        InvenPanel.Show(false);
        RefreshInven();
    }

    public void RefreshInven()
    {
        foreach (var v in talismaninven)
        {
            v.gameObject.SetActive(false);
        }

        if (PlayerBackendData.Instance.TalismanData.Count > talismaninven.Count)
        {
            inittalisman(PlayerBackendData.Instance.TalismanData.Count-talismaninven.Count+1);
        }
        int num = 0;
        foreach (var v in PlayerBackendData.Instance.TalismanData)
        {
            talismaninven[num].Refresh(v.Value);
            talismaninven[num].gameObject.SetActive(true);
            TalismanDB.Row data = TalismanDB.Instance.Find_id(v.Value.Itemid);
            talismaninven[num].transform.SetParent(Talismaninvenslot[int.Parse(data.num)]);
            if (!Talismaninvenslot[int.Parse(data.num)].gameObject.activeSelf)
                Talismaninvenslot[int.Parse(data.num)].gameObject.SetActive(true);
            num++;
        }
    }
    
    public void inittalisman(int pluscount = 30)
    {
        for (int i = 0; i < pluscount; i++)
        {
            talismanslot temp = Instantiate(talismanobj,Talismaninvenslot[0]);
            temp.gameObject.SetActive(false);
            talismaninven.Add(temp);
        }
    }


    public void AddTalisman(string id)
    {
        PlayerBackendData.Instance.MakeTalisman(id);
        Savemanager.Instance.SaveTalisman();
        Savemanager.Instance.Save();
    }



    #region 탈리스만 정보

    private talismanslot nowselectpanel;
    public string nowselectkey;
    public int nowequipnum;
    public UIView Infopnel;
    public Image info_image;
    public Text info_nametext;
    public Text[] info_eskilltext;
    public Text info_Settext;
    public UIToggle LockUi;
    private Talismandatabase nowselectdata;
    public void LockItem()
    {
        nowselectdata.Islock = LockUi.IsOn;
        if (nowselectpanel != null)
            nowselectpanel.Refresh(nowselectdata);
        Savemanager.Instance.SaveTalisman();
        Savemanager.Instance.Save();
    }

    public GameObject EquipButton;
    public GameObject UnEquipButton;
    public GameObject MixSelectButton;
    
    public void Bt_ShowTalisman(Talismandatabase data,talismanslot nowpanel,int equipnum = 0)
    {
        Infopnel.Show(false);
        TalismanDB.Row dbdata = TalismanDB.Instance.Find_id(data.Itemid);
        nowselectdata = data;
        nowselectkey = data.Keyid;
        nowselectpanel = nowpanel;
        nowequipnum = equipnum;
        info_image.sprite = SpriteManager.Instance.GetSprite(dbdata.sprite);
        //특수효과
        for (int i = 0; i < info_eskilltext.Length; i++)
        {
            info_eskilltext[i].gameObject.SetActive(false);
        }

        int colornum = 0;
        if (data.Eskill != null)
        {
            for (int i = 0; i < data.Eskill.Count; i++)
            {
                info_eskilltext[i].text = Inventory.GetTranslate(EquipSkillDB.Instance.Find_id(data.Eskill[i]).info);
                info_eskilltext[i].gameObject.SetActive(true);
                Inventory.Instance.ChangeItemRareColor(info_eskilltext[i],
                    EquipSkillDB.Instance.Find_id(data.Eskill[i]).rare);
                colornum += int.Parse(EquipSkillDB.Instance.Find_id(data.Eskill[i]).rare);

            }
        }
        
        info_nametext.text = Inventory.GetTranslate(dbdata.name);
        info_nametext.color = TalismanManager.Instance.GetTalismanColor(colornum);


        LockUi.IsOn = data.Islock;
        Inventory.Instance.StringRemove();
        Inventory.Instance.StringWrite(Inventory.GetTranslate(dbdata.setinfo1));
        Inventory.Instance.StringWrite("\n");
        Inventory.Instance.StringWrite(Inventory.GetTranslate(dbdata.setinfo2));
        Inventory.Instance.StringWrite("\n");
        Inventory.Instance.StringWrite(Inventory.GetTranslate(dbdata.setinfo3));
        info_Settext.text = Inventory.Instance.StringEnd();
       
        UnEquipButton.SetActive(false);
        EquipButton.SetActive(false);
        MixSelectButton.SetActive(false);

        if (Talismanmixmanager.Instance.ismix)
        {
            MixSelectButton.SetActive(true);
        }
        else
        {
            if (nowpanel != null)
            {
                EquipButton.SetActive(true);
            }
            else
            {
                UnEquipButton.SetActive(true);
            }
        }
    }


    public void Bt_UnEquipItem()
    {
        MapDB.Row mapdata_Now = MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage);
               
        if (PartyRaidRoommanager.Instance.partyroomdata.isstart)
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI7/콘텐츠 중 불가능"), alertmanager.alertenum.주의);
            return;
        }
        if (mapdata_Now.maptype != "0")
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI4/사냥터에서만변경가능"), alertmanager.alertenum.주의);
            toggle[PlayerBackendData.Instance.nowtalismanpreset].IsOn = true;
            return;
        }
        
        //장착한걸 뺌.
        PlayerBackendData.Instance.TalismanData.Add(PlayerBackendData.Instance.TalismanPreset[PlayerBackendData.Instance.nowtalismanpreset]
            .Talismanset[nowequipnum].Keyid,PlayerBackendData.Instance.TalismanPreset[PlayerBackendData.Instance.nowtalismanpreset]
            .Talismanset[nowequipnum]);
        PlayerBackendData.Instance.TalismanPreset[PlayerBackendData.Instance.nowtalismanpreset]
            .Talismanset[nowequipnum] = null;
        Refresh();
        
        Infopnel.Hide(false);
        Savemanager.Instance.SaveTalisman();
        Savemanager.Instance.Save();
    }
    
    #endregion


    public Color GetTalismanColor(int colornum)
    {
        int num = colornum / 3;

        return Inventory.Instance.GetRareColor(num.ToString());
    }


    public void Bt_EquipTalisman()
    {
        MapDB.Row mapdata_Now = MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage);
               
        if (PartyRaidRoommanager.Instance.partyroomdata.isstart)
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI7/콘텐츠 중 불가능"), alertmanager.alertenum.주의);
            return;
        }
        if (mapdata_Now.maptype != "0")
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI4/사냥터에서만변경가능"), alertmanager.alertenum.주의);
            toggle[PlayerBackendData.Instance.nowtalismanpreset].IsOn = true;
            return;
        }
        InvenPanel.Hide(false);
        Infopnel.Hide(false);
        isequiping = true;
        isequipingpanel.SetActive(true);
    }

    public void Bt_SelectMixTalisman()
    {
        if (nowselectdata.Islock)
        {
            Debug.Log("잠김");
            return;
        }

        bool ishave = false;

        for (int i = 0; i < Talismanmixmanager.Instance.mixslots.Length; i++)
        {
            if (Talismanmixmanager.Instance.mixslots[i].keyid.Equals(nowselectdata.Keyid))
            {
                ishave = true;
                break;
            }
        }

        if (!ishave)
        {
            InvenPanel.Hide(false);
            Infopnel.Hide(false);
            Talismanmixmanager.Instance.mixslots[Talismanmixmanager.Instance.nowselectmixnum].SetItem(nowselectdata);
            nowselectpanel.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("이미 등록한것이다.");
        }
    }

    public UIToggle[] toggle;
    public void Bt_ChangePreset(int num)
    {
        MapDB.Row mapdata_Now = MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage);
               
        if (PartyRaidRoommanager.Instance.partyroomdata.isstart)
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI7/콘텐츠 중 불가능"), alertmanager.alertenum.주의);
            toggle[PlayerBackendData.Instance.nowtalismanpreset].IsOn = true;
            return;
        }
        if (mapdata_Now.maptype != "0")
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI4/사냥터에서만변경가능"), alertmanager.alertenum.주의);
            toggle[PlayerBackendData.Instance.nowtalismanpreset].IsOn = true;
            return;
        }

        
        PlayerBackendData.Instance.nowtalismanpreset = num;
        Refresh();
        Savemanager.Instance.SaveTalisman();
        Savemanager.Instance.Save();
    }
}

public class Talismandatabase
{
    public Talismandatabase()
    {
      
    }
    public Talismandatabase(string keyid, string itemid, List<string> eskill,bool ISLOCK)
    {
        this.Keyid = keyid;
        this.Itemid = itemid;
        this.Eskill = eskill;
        this.Islock = ISLOCK;
    }
    public Talismandatabase(string keyid, string itemid)
    {
        this.Keyid = keyid;
        this.Itemid = itemid;
        Debug.Log(itemid);
        string speid =   TalismanDB.Instance.Find_id(itemid).eskill;
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

        if (rd <= percent)
        {
            //옵션나온다
            int oc = UnityEngine.Random.Range(1, optioncount + 1);
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

            this.Eskill = selectedskill;
        }
        this.Islock = false;
    }
    
    public Talismandatabase(string keyid, string itemid,int esnum)
    {
        this.Keyid = keyid;
        this.Itemid = itemid;
        string speid =   TalismanDB.Instance.Find_id(itemid).eskill;
        //Debug.Log(speid);
        EquipSkillRandomGiveDB.Row speds = EquipSkillRandomGiveDB.Instance.Find_id(speid);

////        Debug.Log(esnum);
        List<string> skilloption = new List<string>();
        List<string> selectedskill = new List<string>();
        skilloption = speds.equipskills.Split(';').ToList();
        //확률에 따라 랜덤으로 지급
        //A는 나올 옵션 개수
        int percent = int.Parse(speds.percent);
        int optioncount = int.Parse(speds.A);

        int rd = UnityEngine.Random.Range(0, 101);

        if (esnum == 0)
        {
            if (rd <= percent)
            {
                //옵션나온다
                int oc = UnityEngine.Random.Range(1, optioncount + 1);
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

                this.Eskill = selectedskill;
            }
        }
        else
        {
            //옵션나온다
            int oc = esnum;
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

            this.Eskill = selectedskill;
        }
        
        
        this.Islock = false;
    }
    
    public List<string> Eskill { get; set; }

    public string Itemid { get; set; }

    public string Keyid { get; set; }

    public bool Islock { get; set; }
    public Talismandatabase(LitJson.JsonData data)
    {
        Itemid = data["Itemid"].ToString();
        Eskill = new List<string>();
        Eskill.Clear();
        if (data["Eskill"].ToString() != "True")
        {
            for (int i = 0; i < data["Eskill"].Count; i++)
            {
                Eskill.Add(data["Eskill"][i].ToString());
            }
        }

        Keyid = data["Keyid"].ToString();
        Islock = bool.Parse(data["Islock"].ToString());
    }

   
}


public class PresetTalisman
{
    public Talismandatabase[] Talismanset;

    public PresetTalisman()
    {
        Talismanset = new Talismandatabase[8];
    }
    
    public PresetTalisman(LitJson.JsonData data)
    {
        Talismanset = new Talismandatabase[8];
        for (int i = 0; i < data["Talismanset"].Count; i++)
        {
                Debug.Log(i);
                Debug.Log(data["Talismanset"][i].ToString());
            if (data["Talismanset"][i].ToString() != "True")
            {   
                Talismanset[i] = new Talismandatabase(data["Talismanset"][i]);
            }
        }
    }
}