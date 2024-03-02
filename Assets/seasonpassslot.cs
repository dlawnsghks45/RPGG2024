using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class seasonpassslot : MonoBehaviour
{
    public Text Lv;
    public Image LvPanel;

    //일반
    public Image BRImage;
    public Text BRName;

    // 프리미엄
    public Image PRImage;
    public Text PRName;

    //일반
    public string br_id;
    public string br_howmany;
    
    // 프리미엄
    public string pr_id;
    public string pr_howmany;

    public GameObject RewardGBasic;
    public GameObject RewardGPR;

    public GameObject FinishBasic;
    public GameObject FinishPR;
    public int num = 0;
    public void Refresh(SeasonPassDB.Row data)
    {
        Lv.text = data.lv;
        pr_id = data.PRid;
        pr_howmany = data.PRhowmany;
        br_id = data.BRid;
        br_howmany = data.BRhowmany;

        BRImage.sprite = SpriteManager.Instance.GetSprite(ItemdatabasecsvDB.Instance.Find_id(br_id).sprite);
        BRName.text = $"{Inventory.GetTranslate(ItemdatabasecsvDB.Instance.Find_id(br_id).name)}\n X{br_howmany}";
        Inventory.Instance.ChangeItemRareColor(BRName,ItemdatabasecsvDB.Instance.Find_id(br_id).rare);
//        Debug.Log(pr_id);
        PRImage.sprite = SpriteManager.Instance.GetSprite(ItemdatabasecsvDB.Instance.Find_id(pr_id).sprite);
        PRName.text = $"{Inventory.GetTranslate(ItemdatabasecsvDB.Instance.Find_id(pr_id).name)}\n X{pr_howmany}";
        Inventory.Instance.ChangeItemRareColor(PRName,ItemdatabasecsvDB.Instance.Find_id(pr_id).rare);

        
        RewardGBasic.SetActive(false);
        RewardGPR.SetActive(false);
        FinishBasic.SetActive(false);
        FinishPR.SetActive(false);
        
        num = int.Parse(data.lv) - 1;
        
        if (SeasonPass.Instance.GetLv() >= int.Parse(data.lv))
        {
            LvPanel.color = Color.cyan;
            
            //보상을받지않았다면
            if (!PlayerBackendData.Instance.SeasonPassBasicReward[int.Parse(data.lv) - 1])
            {
                RewardGBasic.SetActive(true);
                if (!SeasonPass.Instance.minlvfind)
                {
                    SeasonPass.Instance.minlv = num;
                    SeasonPass.Instance.minlvfind = true;
                }
            }
            else
            {
                FinishBasic.SetActive(true);
                
            }
            
            if (PlayerBackendData.Instance.SeasonPassPremium)
            {
                if (!PlayerBackendData.Instance.SeasonPassPremiumReward[int.Parse(data.lv) - 1])
                {
                    RewardGPR.SetActive(true);
                    if (!SeasonPass.Instance.minlvfind)
                    {
                        SeasonPass.Instance.minlv = num;
                        SeasonPass.Instance.minlvfind = true;
                    }
                }
                else
                {
                    FinishPR.SetActive(true);
                }
            }
        }
        else
        {
            LvPanel.color = Color.white;
        }
    }

    public bool IsBasicReward()
    {
        if (SeasonPass.Instance.GetLv() >= num +1)
        {
            //보상을받지않았다면
            if (!PlayerBackendData.Instance.SeasonPassBasicReward[num])
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    public bool IsPremiumReward()
    {
        if (SeasonPass.Instance.GetLv() >= num +1)
        {
            //보상을받지않았다면
            if (!PlayerBackendData.Instance.SeasonPassPremiumReward[num])
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }


    public void Bt_ShowIteminfoBr() 
    {
        Inventory.Instance.ShowInventoryItem_NoMine(br_id);
    }
    public void Bt_ShowIteminfoPr() 
    {
        Inventory.Instance.ShowInventoryItem_NoMine(pr_id);
    }

    public void Bt_GetBasicReward()
    {
        return;
        if (PlayerBackendData.Instance.SeasonPassBasicReward[num])
            return;
        List<string> id = new List<string>();
        List<string> hw = new List<string>();
        id.Add(br_id);
        hw.Add(br_howmany);
        Inventory.Instance.ShowEarnItem2(id.ToArray(),hw.ToArray(),false);
        Inventory.Instance.AddItem(br_id,int.Parse(pr_howmany),true);
        PlayerBackendData.Instance.SeasonPassBasicReward[num] = true;
        FinishBasic.SetActive(true);
        RewardGBasic.SetActive(false);
        SeasonPass.Instance.SaveSeasonReward();
    }

    public void Bt_GetPremiumReward()
    {
        return;
        if (PlayerBackendData.Instance.SeasonPassPremiumReward[num])
            return;
        
        List<string> id = new List<string>();
        List<string> hw = new List<string>();
        id.Add(br_id);
        hw.Add(br_howmany);
        Inventory.Instance.ShowEarnItem2(id.ToArray(),hw.ToArray(),false);
        Inventory.Instance.AddItem(pr_id,int.Parse(pr_howmany),true);
        PlayerBackendData.Instance.SeasonPassPremiumReward[num] = true;
        FinishPR.SetActive(true);
        RewardGPR.SetActive(false);
        SeasonPass.Instance.SaveSeasonReward();
    }
}

