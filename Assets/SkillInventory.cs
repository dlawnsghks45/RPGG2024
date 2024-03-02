using System;
using Doozy.Engine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class SkillInventory : MonoBehaviour
{
    private static SkillInventory _instance = null;
    public static SkillInventory Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(SkillInventory)) as SkillInventory;
                if (_instance == null)
                {
                    //Debug.Log("Player script Error");¤§
                }
            }
            return _instance;
        }
    }


    public skillminislot skillslotobj;
    public List<skillminislot> skillslotlist=new List<skillminislot>();
    public Transform[] skilltras; //0Â÷ //1Â÷
    
    public Skillinfoslot[] Skillinfoslots;

    public string SelectSkillid;
    public Skillinfoslot Skillinfoslots_Change;
    public SkillChangeSlot[] SkillChangeSlots;
    public UIView ChangePanel;
    public GameObject ChangePanel_ChangeSlot;
    public UIView SkillInventoryPanel;

    public Toggle PhysicToggle;
    public Toggle MagicToggle;

    private void Start()
    {
        poolslots();
    }

    private void poolslots()
    {
        for (int i = 0; i < 60; i++)
        {
            skillminislot slots = Instantiate(skillslotobj,skilltras[0]);
            slots.gameObject.SetActive(false);
            skillslotlist.Add(slots);
        }
    }
    public void ShowSkillInventory()
    {
        int i = 0;
        List<string> skillid = new List<string>();



        foreach (var data in PlayerBackendData.Instance.Skills.Select(t => SkillDB.Instance.Find_Id(t)))
        {
            if (!skillid.Contains(data.Id))
            {
                skillid.Add(data.Id);

                switch (data.skillsort)
                {
                    case "1":
                        skillslotlist[i].RefreshSkill(data);
                        skillslotlist[i].transform.SetParent(skilltras[0]);
                        break;
                    case "2":
                        skillslotlist[i].RefreshSkill(data);
                        skillslotlist[i].transform.SetParent(skilltras[1]);
                        break;
                    case "3":
                        skillslotlist[i].RefreshSkill(data);
                        skillslotlist[i].transform.SetParent(skilltras[2]);
                        break;
                    case "4":
                        skillslotlist[i].RefreshSkill(data);
                        skillslotlist[i].transform.SetParent(skilltras[3]);
                        break;
                    case "5":
                        skillslotlist[i].RefreshSkill(data);
                        skillslotlist[i].transform.SetParent(skilltras[4]);
                        break;
                    case "6":
                        skillslotlist[i].RefreshSkill(data);
                        skillslotlist[i].transform.SetParent(skilltras[5]);
                        break;
                    case "7":
                        skillslotlist[i].RefreshSkill(data);
                        skillslotlist[i].transform.SetParent(skilltras[6]);
                        break;
                    case "20":
                        skillslotlist[i].RefreshSkill(data);
                        skillslotlist[i].transform.SetParent(skilltras[7]);
                        break;
                }

                skillslotlist[i].gameObject.SetActive(true);
                i++;
            }
        }
    }

    public void ShowChangePanel()
    {
        Skillinfoslots_Change.Refresh(SelectSkillid);

        for (int i = 0; i < SkillChangeSlots.Length;i++)
        {
            SkillChangeSlots[i].Refresh(PlayerBackendData.Instance.ClassData[PlayerBackendData.Instance.ClassId].Skills1[i]);
        }

        for (int i = 0; i < Skillmanager.Instance.mainplayer.castingmanager.skillslots.Length; i++)
        {
            if(Skillmanager.Instance.mainplayer.castingmanager.skillslots[i].islock)
            {
                SkillChangeSlots[i].LockSkillSlot();
            }
        }

        ChangePanel.Show(false);
        ChangePanel_ChangeSlot.SetActive(true);
    }
    
    public void ShowChangePanel(string id)
    {
        Skillinfoslots_Change.Refresh(id);
        ChangePanel.Show(false);
        ChangePanel_ChangeSlot.SetActive(false);
    }


    public void CloseChangePanel()
    {
        ChangePanel.Hide(true);
    }
}
