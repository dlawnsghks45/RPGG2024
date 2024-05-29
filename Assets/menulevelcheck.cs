using System;
using Doozy.Engine.UI;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using UnityEngine.UI;

public class menulevelcheck : MonoBehaviour
{
    [SerializeField]
    int adlv;
    [SerializeField]
    int lv;
    public string type;

    [SerializeField]
    Color[] Colors; //0ȭ��Ʈ //���� 

    public Text TitleText;
    public Image IconImage;
    public Image BackImage;
    public GameObject OpenPanel;
    public UIView OpenPanel_UV;
  
    public Text LockText;


    private void OnEnable()
    {
       chevklock();
    }

    private bool islock;

   

    void chevklock()
    {
        if (adlv != 0)
        {
            if(adlv > PlayerBackendData.Instance.GetAdLv())
            {
                LockText.gameObject.SetActive(true);
                LockText.text = $"{Inventory.GetTranslate("UI8/��跩ũ")} {adlv}";
                BackImage.color = Colors[1];
                IconImage.color = Colors[1];
                TitleText.color = Colors[1];
            }
            else
            {
                LockText.gameObject.SetActive(false);

                BackImage.color = Colors[0];
                IconImage.color = Colors[0];
                TitleText.color = Colors[0];

                islock = true;
            }
        }
        else
        {
            if (lv > PlayerBackendData.Instance.GetLv())
            {
                LockText.gameObject.SetActive(true);
                LockText.text =  $"{Inventory.GetTranslate("UI8/��跹��")} {lv}";
                BackImage.color = Colors[1];
                IconImage.color = Colors[1];
                TitleText.color = Colors[1];

            }
            else
            {
                LockText.gameObject.SetActive(false);
                BackImage.color = Colors[0];
                IconImage.color = Colors[0];
                TitleText.color = Colors[0];

                islock = true;
            }
        }
        
      
       
    }
    
    public void Bt_TouchPanel()
    {
        if (adlv > PlayerBackendData.Instance.GetAdLv())
        {
            //������ �����մϴ�.
            alertmanager.Instance.ShowAlert(string.Format(TranslateManager.Instance.GetTranslate("UI/���尡�����Ƿ���"), PlayerData.Instance.gettierstar(adlv.ToString())),alertmanager.alertenum.����);
            //alertmanager.Instance.ShowAlert("dd",alertmanager.alertenum.����);
        }
        else
        {
            if (type != "")
            {
                switch (type)
                {
                    case "autofarm":
                        Autofarmmanager.Instance.Bt_OpenAuto();
                        break;
                    case "altar":
                        altarmanager.Instance.Bt_OpenPanel();
                        break;
                    case "avarta":
                        avatamanager.Instance.View.Show(false);
                        avatamanager.Instance.RefreshBt_();
                        break;  
                    case "preset":
                        presetmanager.Instance.Bt_OpenPresetPanel();
                        break;
                    
                    case "batterysave":
                        batterysaver.Instance.Bt_BatterySaver();
                        break;
                    case "collect":
                        CollectionRenewalManager.Instance.Bt_ShowCollectionPanel();
                        break;
                    case "Raid":
                        MapDB.Row mapdata_Now = MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage);
                        if (mapdata_Now.maptype != "0")
                        {
                            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI7/������ �� �Ұ�"), alertmanager.alertenum.����);
                            return;
                        }
                        OpenPanel_UV.Show(true);
                        break;
                    
                }
            }
            else
            {
                if(OpenPanel!= null)
                {
                    OpenPanel.SetActive(true);
                }
                else
                {
                    OpenPanel_UV.Show(true);
                }
            }
        
        }
    }
}
