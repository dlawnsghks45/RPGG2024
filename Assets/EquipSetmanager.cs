using Doozy.Engine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EquipSetmanager : MonoBehaviour
{
    //세트아이템 
    //싱글톤만들기.
    private static EquipSetmanager _instance = null;
    public static EquipSetmanager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(EquipSetmanager)) as EquipSetmanager;

                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }
            return _instance;
        }
    }
    //아이템 장착 시 세트아이템 확인

    public SetDBDB.Row Data;
 
    List<string> EquipA = new List<string>();
    List<string> EquipB = new List<string>();
    List<string> EquipC = new List<string>();
    List<string> EquipD = new List<string>();
    List<string> EquipE = new List<string>();
    List<string> EquipF = new List<string>();


    bool[] Setbool;
    string[] SetString;
    [SerializeField]
    public List<string> SetIDs = new List<string>();


    public void EquipSetItem()
    {
        SetIDs.Clear();

        foreach (var t in Inventory.Instance.EquipSlots)
            t.ShowSetParticle(false);

        for (int i = 0; i < PlayerBackendData.Instance.GetEquipData().Length; i++)
        {
            if (PlayerBackendData.Instance.GetEquipData()[i] == null) continue;
            if (EquipItemDB.Instance.Find_id(PlayerBackendData.Instance.GetEquipData()[i].Itemid).SetID == "0")
                continue;

            if (SetIDs.Contains(EquipItemDB.Instance.Find_id(PlayerBackendData.Instance.GetEquipData()[i].Itemid)
                    .SetID)) continue;
            if (CheckSetItem(PlayerBackendData.Instance.GetEquipData()[i].Itemid,true))
            {
                if (!SetIDs.Contains(Data.coreidd))
                    SetIDs.Add(Data.id);
            }
        }

        if (SetIDs.Contains("1005") || SetIDs.Contains("1006") || SetIDs.Contains("1007"))
        {
            TutorialTotalManager.Instance.CheckGuideQuest("equipallarmor");
        }
        
        
    }
    /*
    public bool CheckSetItem(string EquipID,bool isslotparticle = false)
    {
        //Debug.Log(EquipID);
        Data = SetDBDB.Instance.Find_id(EquipItemDB.Instance.Find_id(EquipID).SetID);

        
        Setbool = new bool[int.Parse(Data.setcount)];
        SetString = new string[int.Parse(Data.setcount)];

        EquipA.Clear();
        EquipB.Clear();
        EquipC.Clear();
        EquipD.Clear();
        EquipE.Clear();
        EquipF.Clear();


        if (Data.equipA != "")
        {
            EquipA.Add(Data.equipA);
            SetString[0] = Data.equipA;
        }
        if (Data.equipAA != "")
        {
            EquipA.Add(Data.equipAA);
        }

        if (Data.equipB != "")
        {
            EquipB.Add(Data.equipB);
            SetString[1] = Data.equipB;
        }

        if (Data.equipBB != "")
        {
            EquipB.Add(Data.equipBB);
        }

        if (Data.equipC != "")
        {
            EquipC.Add(Data.equipC);
            SetString[2] = Data.equipC;
        }

        if (Data.equipCC != "")
        {
            EquipC.Add(Data.equipCC);
        }

        if (Data.equipD != "")
        {
            EquipD.Add(Data.equipD);
            SetString[3] = Data.equipD;
        }

        if (Data.equipDD != "")
        {
            EquipD.Add(Data.equipDD);
        }

        if (Data.equipE != "")
        {
            EquipE.Add(Data.equipE);
            SetString[4] = Data.equipE;
        }

        if (Data.equipEE != "")
        {
            EquipE.Add(Data.equipEE);
        }

        if (Data.equipF != "")
        {
            EquipF.Add(Data.equipF);
            SetString[5] = Data.equipF;
        }

        if (Data.equipFF != "")
        {
            EquipF.Add(Data.equipFF);
        }
        //모든 장비 타입 검색
        for (var i = 0; i < PlayerBackendData.Instance.GetEquipData().Length;i++)
        {
            if (PlayerBackendData.Instance.GetEquipData()[i] == null) continue;
            string id = PlayerBackendData.Instance.GetEquipData()[i].Itemid;
            if (EquipA.Contains(id))
            {
                Setbool[0] = true;
                SetString[0] = id;
            }

            if (EquipB.Contains(id))
            {
                Setbool[1] = true;
                SetString[1] = id;
            }

            if (EquipC.Contains(id))
            {
                Setbool[2] = true;
                SetString[2] = id;
            }

            if (EquipD.Contains(id))
            {
                Setbool[3] = true;
                SetString[3] = id;
            }

            if (EquipE.Contains(id))
            {
                Setbool[4] = true;
                SetString[4] = id;
            }

            if (EquipF.Contains(id))
            {
                Setbool[5] = true;
                SetString[5] = id;
            }

        }
        bool isallon = true;
        foreach (var t in Setbool)
        {
            if (!t)
                isallon = false;
        }

        if(isallon)
        {
            if (!isslotparticle) return true;
            foreach (var t in SetString)
            {
                //해당 부위를 불러온다.
                equipitemslot slots = Inventory.Instance.GetEquipSlots(EquipItemDB.Instance.Find_id(t).Type);
                if (slots.data != null)
                {
                    if (slots.data.Itemid.Equals(t))
                        slots.ShowSetParticle(true);
                    else
                        slots.ShowSetParticle(false);
                }
                else
                    slots.ShowSetParticle(false);
            }
            return true;
        }
        else
        {
            return false;
        }

    }
*/
    public bool CheckSetItem(string EquipID,bool isslotparticle = false)
    {
        //Debug.Log(EquipID);
        Data = SetDBDB.Instance.Find_id(EquipItemDB.Instance.Find_id(EquipID).SetID);

        List<string> setnum = new List<string>();
        int setcount = int.Parse(Data.setcount);
        int nowmycount = 0;
        for (var i = 0; i < PlayerBackendData.Instance.GetEquipData().Length;i++)
        {
            if (PlayerBackendData.Instance.GetEquipData()[i] == null) continue;
            string id = PlayerBackendData.Instance.GetEquipData()[i].Itemid;
            
            if(Data.id.Equals(EquipItemDB.Instance.Find_id(id).SetID))
            {
                nowmycount++;
                setnum.Add(EquipItemDB.Instance.Find_id(id).Type);
            }
        }
        if(nowmycount >= (setcount))
        {
            if (!isslotparticle) return true;
            foreach (var t in setnum)
            {
                //해당 부위를 불러온다.
                equipitemslot slots = Inventory.Instance.GetEquipSlots(t);
                if (slots.data != null)
                {
                 //   if (slots.data.Itemid.Equals(t))
                        slots.ShowSetParticle(true);
                  //  else
                   //     slots.ShowSetParticle(false);
                }
                else
                    slots.ShowSetParticle(false);
            }
            return true;
        }
        else
        {
            return false;
        }

    }
    public UIView infopanel;

    public Text SetNametext;
    public Text SetInfotext;
    public Text SetInfotextEquiped;

    public void Bt_ShowSetInfo()
    {
        infopanel.Show(false);
        infopanel.transform.SetAsLastSibling();
        SetNametext.text = Inventory.GetTranslate(Data.name);

        SetInfotext.text = Inventory.GetTranslate(Data.description);


        foreach (var t in Equips)
        {
            t.gameObject.SetActive(false);
        }

        int equipedcount = 0;

        for (int i = 0; i < EquipItemDB.Instance.NumRows(); i++)
        {
            if (EquipItemDB.Instance.GetAt(i).SetID == "") continue;
            if (!EquipItemDB.Instance.GetAt(i).SetID.Equals(Data.id)) continue;
            for (int j = 0; j < Equips.Length; j++)
            {
                if (Equips[j].gameObject.activeSelf) continue;
                if (PlayerBackendData.Instance.GetEquipDataByKey(EquipItemDB.Instance.GetAt(i).id) != null)
                {
                    //장비가 있다면 낀 장비로 보여준다.
                    Equips[j].SetEquipItem_Mine(
                        PlayerBackendData.Instance.GetEquipDataByKey(EquipItemDB.Instance.GetAt(i).id));
                    Equips[j].m_TextMeshPro.color = Color.cyan;
                    Equips[j].gameObject.SetActive(true);
                    equipedcount++;
                }
                else
                {
                    //장비가 없다면 없는 장비로 보여준다.
                    Equips[j].SetEquipItem_NotMine(EquipItemDB.Instance.GetAt(i).id);
                    Equips[j].m_TextMeshPro.color = Color.gray;
                    Equips[j].gameObject.SetActive(true);
                }
                break;
            }
        }
        
        SetInfotextEquiped.text = $"{Inventory.GetTranslate(Data.name)} ({equipedcount}/{Data.setcount})";
        if (equipedcount < int.Parse(Data.setcount))
        {
            SetInfotextEquiped.color = Color.gray;
            SetInfotext.color = Color.gray;
        }
        else
        {
            SetInfotextEquiped.color = Color.cyan;
            SetInfotext.color = Color.cyan;
        }
    }

    public TMProHyperLink[] Equips;
}
