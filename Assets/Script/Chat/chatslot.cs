using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class chatslot : MonoBehaviour
{
    public Text nickname;
    public Text TierLv;
    public Text PlayerLv;
    public TextMeshProUGUI Contents;
    public RectTransform rect;
    string nick;

    public GameObject SystemIcon;
    
    public Image PlayerAvarta;
    public Image PlayerWeapon;
    public Image PlayerSubWeapon;


    bool issys;

    //avarta나 weapon등은 이미지 경로로 바로 뿌린다.
    public void Chat(string visualdata, string lv, string adlv,string bprank, string realnick, string nick, string content,
        bool issystem)
    {
        SystemIcon.SetActive(false);
        issys = issystem;
        if (!issystem)
        {
            string[] visualdatas = visualdata.Split(';');

            if (visualdatas[0] != "")
            {
                PlayerAvarta.sprite = SpriteManager.Instance.GetSprite(visualdatas[0]);
                PlayerAvarta.enabled = true;
            }
            else
                PlayerAvarta.enabled = false;

            if (visualdatas[1] != "")
            {
                PlayerWeapon.sprite = SpriteManager.Instance.GetSprite(visualdatas[1]);
                PlayerWeapon.enabled = true;

            }
            else
                PlayerWeapon.enabled = false;

            if (visualdatas[2] != "")
            {
                PlayerSubWeapon.sprite = SpriteManager.Instance.GetSprite(visualdatas[2]);
                PlayerSubWeapon.enabled = true;

            }
            else
                PlayerSubWeapon.enabled = false;

            this.nick = nick;

            //랭킹있으면 랭킹
            nickname.text = $"{string.Format(Inventory.GetTranslate("UI2/채팅랭킹"), bprank)}{nick}";
            PlayerLv.text = $"Lv.{lv}";
            TierLv.text = string.Format(Inventory.GetTranslate("UI2/모험랭크"), adlv);
        }
        else
        {
            SystemIcon.SetActive(true);
            PlayerAvarta.enabled = false;
            PlayerSubWeapon.enabled = false;
            PlayerWeapon.enabled = false;
            nickname.text = Inventory.GetTranslate("UI6/채팅_시스템");
            PlayerLv.text = "";
            TierLv.text = "";
        }

        //Debug.Log("메시지는" + content);
        Contents.text = content;
        LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
    }

    public void ShowSystem( string nick, string content)
    {
        issys = true;
        PlayerAvarta.enabled = false;
        PlayerSubWeapon.enabled = false;
        PlayerWeapon.enabled = false;
        nickname.text = Inventory.GetTranslate("UI6/채팅_시스템");
        PlayerLv.text = "";
        TierLv.text = "";
        Contents.text = content;
    }

    public void ShowPartyRaidChat(string nick, string content)
    {
        Contents.text = $"<color=yellow>{nick}</color> : {content}";
    }
    public void ShowPartyRaidSystemChat(string content)
    {
        Contents.text = $"{content}";
    }
    public void Bt_ShowUser()
    {
        if (issys)// || PlayerBackendData.Instance.nickname==nick)
            return;
        else
        {
            otherusermanager.Instance.Bt_ShowChatUserData(PlayerAvarta.sprite,PlayerWeapon.sprite,PlayerSubWeapon.sprite,nick);
            uimanager.Instance.AddUiview(chatmanager.Instance.Panel,true);
        }
    }
}