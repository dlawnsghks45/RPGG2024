using System;
using UnityEngine;
using UnityEngine.UI;

public class adsrewardslot : MonoBehaviour
{
    public string enumdaily = "";
    
    public Image Sprite;
    public Button BuyButton;
    public Text Titlename;
    public Text GiveCountText;

    public int buycountarraynum;
    public GameObject buycountobj;
    public Text BuyCountText;

    private void Start()
    {
        uimanager.Instance.adsSlots.Add(this);
        buycountarraynum = (int)Enum.Parse(typeof(Timemanager.ContentEnumDaily),
            enumdaily);
        RefreshBuyCount();
    }

    private void OnEnable()
    {
        RefreshBuyCount();
    }
    
    public void RefreshBuyCount()
    {
        buycountobj.SetActive(true);
        BuyCountText.text =
            string.Format(Inventory.GetTranslate("Trade/ÀÏ°¡´ÉÈ½¼ö"),
                Timemanager.Instance.DailyContentCount[buycountarraynum]
                , Timemanager.Instance.DailyContentCount_Standard[buycountarraynum]);

//                Debug.Log( Timemanager.Instance.DailyContentCount_Standard[buycountarraynum]+ "Ä«¿îÆ®" + buycountarraynum);

        if (Timemanager.Instance.DailyContentCount[buycountarraynum] == 0)
        {
            BuyButton.interactable = false;
            BuyCountText.color = Color.red;
        }
        else
        {
            BuyButton.interactable = true;
            BuyCountText.color = Color.cyan;
        }
    }


    public void Bt_BuyItem()
    {
        if (PlayerBackendData.Instance.isadsfree)
        {
            if (Timemanager.Instance.ConSumeCount_DailyAscny(buycountarraynum, 1))
            {
                Debug.Log("±¤°í ¹°Ç° ¹Þ¾Ñµû ");
                AdmobRewardAd.Instance.GiveReward(enumdaily);
                AdmobRewardAd.Instance.SetSlots(this);
            }
        }
        else if (Advertisements.Instance.IsRewardVideoAvailable())
        {
            if (Timemanager.Instance.ConSumeCount_DailyAscny(buycountarraynum, 1))
            {
                AdmobRewardAd.Instance.ShowRewardVideo(enumdaily,this);
            }
        }
    }
}
