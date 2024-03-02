using System;
using Doozy.Engine.UI;
using System.Collections;
using System.Collections.Generic;
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
    public Image IconImage;
    public GameObject OpenPanel;
    public UIView OpenPanel_UV;
    public GameObject LockIcon;


    private void OnEnable()
    {
       chevklock();
    }

    private bool islock;

    private void Start()
    {
       InvokeRepeating(nameof(chevklock), 1f,3f);
    }

    void chevklock()
    {
        if (islock)
        {
            LockIcon.SetActive(false);
            CancelInvoke(nameof(chevklock));
            return;
        }

        if(adlv > PlayerBackendData.Instance.GetAdLv())
        {
            LockIcon.SetActive(true);
            IconImage.color = Colors[1];
        }
        else
        {
            LockIcon.SetActive(false);
            IconImage.color = Colors[0];
            islock = true;
        }
        
        if (adlv.Equals(0) && lv > PlayerBackendData.Instance.GetLv())
        {
            LockIcon.SetActive(true);
            IconImage.color = Colors[1];
        }
        else
        {
            LockIcon.SetActive(false);
            IconImage.color = Colors[0];
            islock = true;
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
