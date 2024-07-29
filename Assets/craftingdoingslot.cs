using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class craftingdoingslot : MonoBehaviour
{
    public int index;


    public GameObject onpanel;
    public GameObject offpanel;


    public Image CraftingImage;
    public Text ItemNameText;
    public Text NowleftTime;
    public bool ispremium;
    public bool isfinish;
    public GameObject LockPanel;
    public GameObject GetResultButton;
    public GameObject FinishUsingFireButton;

   

    public double nowsecond;
    DateTime dt;
    CraftTableDB.Row craftdata;
    public void SetCraft(string craftid, DateTime endtime, DateTime time)
    {
        isfinish = false;
        TimeSpan dateDiff = endtime - time;

        onpanel.SetActive(true);
        offpanel.SetActive(false);
        LockPanel.SetActive(false);
        craftdata = CraftTableDB.Instance.Find_id(craftid);

        if (craftdata.isequip == "TRUE")
        {
            CraftingImage.sprite = SpriteManager.Instance.GetSprite(EquipItemDB.Instance.Find_id(craftdata.Successid).Sprite);
            ItemNameText.text =
                $"{Inventory.GetTranslate(EquipItemDB.Instance.Find_id(craftdata.Successid).Name)}<color=cyan>[{PlayerBackendData.Instance.craftdatecount[index]}]</color>";
            ItemNameText.color = Inventory.Instance.GetRareColor(EquipItemDB.Instance.Find_id(craftdata.Successid).Rare);
        }
        else
        {
            CraftingImage.sprite = SpriteManager.Instance.GetSprite(ItemdatabasecsvDB.Instance.Find_id(craftdata.Successid).sprite);
            ItemNameText.text =
                $"{Inventory.GetTranslate(ItemdatabasecsvDB.Instance.Find_id(craftdata.Successid).name)}<color=cyan>[{PlayerBackendData.Instance.craftdatecount[index]}]</color>";
            ItemNameText.color = Inventory.Instance.GetRareColor(ItemdatabasecsvDB.Instance.Find_id(craftdata.Successid).rare);
        }

        if (dateDiff.TotalSeconds < 0)
        {
            isfinish = true;
            Refresh();
        }
        else
        {
            nowsecond = dateDiff.TotalSeconds;
            dt = new DateTime(dateDiff.Ticks);
            dt.AddSeconds(nowsecond);
    
            StartCoroutine(TimeStart());
            Refresh();
        }
    }

    public void Refresh()
    {
        if (isfinish)
        {
            NowleftTime.text = "00:00:00";
            GetResultButton.SetActive(true);
            FinishUsingFireButton.SetActive(false);
            if (PlayerBackendData.Instance.tutoid != Tutorialmanager.Instance.maxlv)
            {
                if (craftdata.id.Equals("1021") || craftdata.id.Equals("10") &&
                    TutorialDB.Instance.Find_id(PlayerBackendData.Instance.tutoid).type.Equals("craft"))
                {
                    Tutorialmanager.Instance.NewTuto1[10].SetActive(true);
                }
            }
        }
        else
        {
            
            //시, 분, 초 선언

            int hours2, minute2, second2;

            hours2 = (int)nowsecond / 3600;//시 공식
            minute2 = (int)nowsecond % 3600 / 60;//분을 구하기위해서 입력되고 남은값에서 또 60을 나눈다.
            second2 = (int)nowsecond % 3600 % 60;//마지막 남은 시간에서 분을 뺀 나머지 시간을 초로 계산함
        
            NowleftTime.text = ($"{hours2:00}:{minute2:00}:{second2:00}");
            GetResultButton.SetActive(false);
            FinishUsingFireButton.SetActive(true);
        }
    }

    public void MakeTimeZero()
    {
        dt = dt.AddSeconds(-nowsecond);
        nowsecond = 0;
        Refresh();
    }

    WaitForSeconds wait = new WaitForSeconds(1f);
    IEnumerator TimeStart()
    {
        while (!isfinish)
        {
            yield return wait;
            nowsecond--;
            if (nowsecond <= 0)
            {
                isfinish = true;
                Refresh();
            }
            else
            {
                dt = dt.AddSeconds(-1);
                Refresh();
            }
        }
    }
    [Button (Name ="11")]
    public void CheckPremium()
    {
        offpanel.SetActive(true);
        onpanel.SetActive(false);

        if (ispremium)
        {
            if(PlayerBackendData.Instance.ispremium)
            {
                LockPanel.SetActive(false);
            }
            else
            {
                if(PlayerBackendData.Instance.craftmakingid[index] != "")
                {
                    //사용중이라서 잠금을 잠군다..
                    LockPanel.SetActive(false);
                }
                else
                {
                    //사용중이지않고 프리미엄이 없다면 잠군다.
                    LockPanel.SetActive(true);
                }
            }
        }
        else
        {
            //사용중이지않고 프리미엄이 없다면 잠군다.
            LockPanel.SetActive(false);
        }
    }
   

    //제작완료
    public void Bt_FinishCrafting()
    {
        if (isfinish)
        {
            if (!Settingmanager.Instance.CheckServerOn())
            {
                return;
            }

            CraftManager.Instance.GiveResult(craftdata, index);
        }
    }

    public void Bt_RightFinishUsingFire()
    {
        if (!isfinish)
            CraftManager.Instance.ShowCraftRightFinishPanel(craftdata, nowsecond,index);
    }
}
