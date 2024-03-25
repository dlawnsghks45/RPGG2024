using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
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


    public string PartyMasterName;
    public Image PartyRaidBackground;
    public Image PartyRaidMonimg;
    public Text PartyRaidName;
    public Text PartyRaidUserCount;
    public Text PartyRaidLevel;
    public Text PartyRaidJoinlevel;
    
    
    
    
    
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
            adlvs = int.Parse(adlv);
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

    public int adlvs;

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

    public void ShowPartyAdChat(string lv,string bprank, string nicks,
        string content, string mapid, string level, string mastercode, string rank,string membercount)
    {
        this.nick = nicks;

        //랭킹있으면 랭킹
        nickname.text = $"{string.Format(Inventory.GetTranslate("UI2/채팅랭킹"), bprank)}{nick}";
       
        //Debug.Log("메시지는" + content);
        Contents.text = content;

        PartyMasterName = mastercode;
        PartyRaidMonimg.sprite = SpriteManager.Instance.GetSprite(monsterDB.Instance
            .Find_id(MapDB.Instance.Find_id(PartyRaidRoommanager.Instance.partyroomdata.nowmap).monsterid).sprite);
        PartyRaidBackground.sprite =
            SpriteManager.Instance.GetSprite(MapDB.Instance.Find_id(PartyRaidRoommanager.Instance.partyroomdata.nowmap)
                .maplayer0);
        PartyRaidName.text =
            Inventory.GetTranslate(MapDB.Instance.Find_id(PartyRaidRoommanager.Instance.partyroomdata.nowmap).name);
        PartyRaidUserCount.text = $"{membercount}/4";
        PartyRaidLevel.text = string.Format(Inventory.GetTranslate("UI7/난이도"), level);
        PartyRaidJoinlevel.text = string.Format(Inventory.GetTranslate("UI7/0_가입제한"), rank);
        ranknum = int.Parse(rank);
        LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
    }

    private int ranknum;

    public void ShowPartyRaidChat(string nick, string content)
    {
        Contents.text = $"<color=yellow>{nick}</color> : {content}";
    }
    public void ShowPartyRaidSystemChat(string content,Color color)
    {
        PlayerAvarta.color = color;
        Contents.text = $"{content}";
    }
    public void Bt_ShowUser()
    {
        if (issys)// || PlayerBackendData.Instance.nickname==nick)
            return;
        else
        {
            otherusermanager.Instance.Bt_ShowChatUserData(PlayerAvarta.sprite,PlayerWeapon.sprite,PlayerSubWeapon.sprite,nick,adlvs);
            uimanager.Instance.AddUiview(chatmanager.Instance.Panel,true);
        }
    }

    
    
    
    

    public void Bt_ADTouch()
    {
        if(nick == PlayerBackendData.Instance.nickname)
            return;
        PartyRaidRoommanager.Instance.joinname = nick;
        PartyRaidRoommanager.Instance.rankjoin = ranknum;
        PartyRaidRoommanager.Instance.ShowAdmenPanel();
    }
}