using System;
using BackEnd;
using BackEnd.Tcp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;
using Doozy.Engine.UI;

public class chatmanager : MonoBehaviour
{
    private void OnDestroy()
    {
        _instance = null;
    }


    public UIView Panel;
    
    //싱글톤만들기.
    private static chatmanager _instance = null;
    public static chatmanager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(chatmanager)) as chatmanager;

                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }
            return _instance;
        }
    }

    List<ChatChannel> channellist = new List<ChatChannel>();
    List<ChatChannel> channellist_Guild = new List<ChatChannel>();
    public ChatChannel nowjoinedchat;
    public ChatChannel nowjoinedchat_guild;

    public chatslot chatitempool; //채널 프리팹
    public chatslot chatitempool_Guild; //채널 프리팹
    public Transform ChatTrans;
    public Transform ChatTrans_Guild;

    public CanvasGroup chatpanelcanvas;
    public Slider chatinvicibleslider;


    //차단

    public void BlockUser(string nickaname)
    {
     //   Debug.Log(nickaname);
        Backend.Chat.BlockUser(nickaname, (blockCallback) => {
            // 성공
        //    Debug.Log(blockCallback);
            if (blockCallback)
            {
                alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI4/차단 완료"), alertmanager.alertenum.일반);
                otherusermanager.Instance.RefreshChatUserData();
            }
            // 실패
            else {
                
            }
        //    Debug.Log(blockCallback);
        //    Debug.Log(nickaname);

        });
       // Debug.Log(nickaname);
    }
    public void UnBlockUser(string nickaname)
    {
        bool isUnblock = Backend.Chat.UnblockUser(nickaname);

        if (isUnblock)
        {
            Debug.Log("해제에 성공했습니다");
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI4/차단 해제"),alertmanager.alertenum.일반);
            otherusermanager.Instance.RefreshChatUserData();
        }
        else
            Debug.Log("해당 유저는 차단 목록에 존재하지 않습니다");
    }
    
    
    
    public void ChangeSliderPanel()
    {
        chatpanelcanvas.alpha = chatinvicibleslider.value;
    }
    void Start()
    {
        //채널가져오기
        // 첫 번째 방법 (동기)
        var bro = Backend.Initialize(true);
        if (bro.IsSuccess())
        {
            // 초기화 성공 시 로직
            ChatHandlers();
            Invoke(nameof(GetChannelList), 1f);
            Backend.Chat.SetFilterReplacementChar('*');
            Backend.Chat.SetFilterUse(false);
        }
        else
        {
            // 초기화 실패 시 로직
        }
    }

    //길드채팅인가?
    public UIToggle toggle_public; 
    public UIToggle toggle_guild;
    
    //채팅 슬라이드
    public Scrollbar ChatPanelScrollbar;
    bool isscrollup;
    
    //채팅 슬라이드
    public Scrollbar ChatPanelScrollbar_Guild;
    bool isscrollup_Guold;
     void RefreshChatPanel()
    {
        if (!isscrollup) //스크롤을 스스로 올리지 않았다면 내려간다.
            ChatPanelScrollbar.value = 0;
    }
     void RefreshChatPanel_Guild()
    {
        if (!isscrollup_Guold) //스크롤을 스스로 올리지 않았다면 내려간다.
            ChatPanelScrollbar_Guild.value = 0;
    }
    //채팅창의 바를 올리면 chatpanel밸류가 통하지 않는다.
    public void CheckChatScrollbarPanel()
    {
        isscrollup = ChatPanelScrollbar.value > 0;
    }

    private void GetChatData(string nick,string message,bool isguild = false)
    {
        string[] chatdata = message.Split(";|");
        /*
         * 0은 비쥬얼 데이터
         * 1은 레벨
         * 2는 모험가 레벨
         * 3은 채팅
         */
        if (chatdata[4] != "")
        {
            // Debug.Log("dd");
            ShowChat(chatdata[0], chatdata[1], chatdata[2],chatdata[3], nick, chatdata[4],isguild);
        }
        else
        {
            // Debug.Log("d");
            //ShowChat(args.From.NickName, string.Format(publicstrnew, args.From.NickName), chatdata[1]);
            ShowChat(chatdata[0], chatdata[1], chatdata[2],chatdata[3], nick, chatdata[4],isguild);
        }
    }

    public Animator GuildBuffLvUpani;
    public Text GuildBuffLvText;
    public Text GuildBuffInfo;
    public Image GuildBuffImage;
    
    const string publicsystem = "#publicsys#";

    void ChatHandlers()
    {
        //일반채팅
        Backend.Chat.OnChat += (args) =>
        {
            if (args.ErrInfo == ErrorInfo.Success)
            {
                if (args.Message.Contains(publicsystem))
                {
                    string[] stringdata = args.Message.Split(';');
//                    Debug.Log(stringdata);
                    //[0]은 시스템 번호 1은 다른 거
                    switch (stringdata[1])
                    {
                        case "SU":
                            ShowSystemChat(args.From.NickName,
                                string.Format(Inventory.GetTranslate("UI6/채팅_제련성공"),
                                    stringdata[2], //이름
                                    Inventory.Instance.GetRareColorFF(stringdata[3]),
                                    Inventory.GetTranslate(EquipItemDB.Instance.Find_id(stringdata[4]).Name), //징비색
                                    stringdata[5]), false);
                            break;
                        case "RO":
                            //얻은아이템
                            ItemdatabasecsvDB.Row item = ItemdatabasecsvDB.Instance.Find_id(stringdata[4]);
                            //열쇠
                            ItemdatabasecsvDB.Row item2 = ItemdatabasecsvDB.Instance.Find_id(stringdata[3]);
                            ShowSystemChat(args.From.NickName,
                                string.Format(Inventory.GetTranslate("UI6/채팅_룰렛뽑기"),
                                    stringdata[2],
                                    Inventory.Instance.GetRareColorFF(item.rare),
                                    Inventory.GetTranslate(item.name),
                                    Inventory.Instance.GetRareColorFF(item2.rare),
                                    Inventory.GetTranslate(item2.name), stringdata[5]),
                                false);
                            break;
                        case "EUP":
                            ShowSystemChat(args.From.NickName,
                                string.Format(Inventory.GetTranslate("UI6/채팅_강화성공"),
                                    stringdata[2], //이름
                                    Inventory.Instance.GetRareColorFF(stringdata[3])
                                    , stringdata[5],
                                    Inventory.GetTranslate(EquipItemDB.Instance.Find_id(stringdata[4])
                                        .Name)), false);
                            break;
                        case "PET":
                            ShowSystemChat(args.From.NickName,
                                string.Format(Inventory.GetTranslate("UI6/채팅_펫나옴"),
                                    stringdata[2], //이름
                                    Inventory.Instance.GetRareColorFF(stringdata[3])
                                    , Inventory.Instance.GetRareColorFF(stringdata[3]),
                                    Inventory.GetTranslate(PetDB.Instance.Find_id(stringdata[4])
                                        .name)), false);
                            break;


                        #region 파티레이드
                        //파티장이 유저에게 초대를 보낸걸 리시브
                        case "PI":
                            if (stringdata[3] != (PlayerBackendData.Instance.nickname))
                            {
                                break;
                            }

                            if (PartyRaidRoommanager.Instance.nowmyleadernickname !=
                                PlayerBackendData.Instance.nickname)
                            {
                                Debug.Log("리더는 현재 내가 아니다.");
                                break;
                            }
                            
                            Debug.Log("초대를 받았다");
                            //system;PI;내이름;유저이름;mapname;level
                            PartyRaidRoommanager.Instance.ShowInvitedPanel(stringdata[4], int.Parse(stringdata[5]), stringdata[2]);
                            break;
                        case "PA":
                            if (stringdata[2] != (PlayerBackendData.Instance.nickname))
                            {
                                break;
                            }
                            Debug.Log("파티원이 내 초대를 수락 했다.");
                            //system;PA;리더이름;파티원이름
                            //그래서 받은 파장이 파티원에게 데이터를 보냄.
                            string[] Partymemberdata= args.Message.Split('&');
                            bool ishave= false;
                            for (int i = 0; i < PartyRaidRoommanager.Instance.PartyMember.Length; i++)
                            {
                                //데이터를 빈자리에 넣는다.
                                if (PartyRaidRoommanager.Instance.PartyMember[i].data == null)
                                {
                                    Debug.Log("빈자리에데이터"+i);
                                    PartyRaidRoommanager.Instance.PartyMember[i].SetPlayerData(Partymemberdata[1],i);
                                    ishave = true;
                                    break;
                                }
                            }

                            if (ishave)
                            {
                                //자리가 있어서 초대
                                Debug.Log("자리가있다");
                                Debug.Log(stringdata[3]);
                                string[] a = args.Message.Split('&');
                                string[] b = a[0].Split(';');
                                PartyraidChatManager.Instance.Chat_GivePartyData(b[3]);
                            }
                            else
                            {
                                Debug.Log("자리가없다");
                                //자리가 없다
                            }
                            break;
                        //파티원에게 데이터를 줌.
                        case "PGD":
                            if (PartyRaidRoommanager.Instance.nowmyleadernickname != stringdata[2])
                                break;
                            //system; 0
                            //PGD 1;
                            //파장이름; 2
                            //해당유저; 3
                            //맵아이디; 4
                            //맵난이도 5
                            //&유저1; 0
                            //유저2; 1
                            //유저3; 2
                            //유저4 3
                            alertmanager.Instance.ShowAlert(string.Format(Inventory.GetTranslate("UI7/파티가입함"),stringdata[3]),alertmanager.alertenum.일반);
                            string[] PartyData = args.Message.Split('&');
                            string[] Partyplayerdata = PartyData[1].Split('#');
                            int usercount = 0;
                            for (int i = 0; i < Partyplayerdata.Length; i++)
                            {
                                if (Partyplayerdata[i] != "")
                                {
                                    usercount++;
                                    PartyRaidRoommanager.Instance.PartyMember[i].SetPlayerData(Partyplayerdata[i],i);
                                }
                            }

                            string[] roomddata = PartyData[0].Split(';');
                         
                            
                            PartyRaidRoommanager.Instance.partyroomdata = new PartyRoom(roomddata[4],
                                int.Parse(roomddata[5]), roomddata[2], usercount);

                            PartyRaidRoommanager.Instance.RefreshPartyData();
                            
                            //그래서 받은 파장이 파티원에게 데이터를 보냄.
                            break;
                        
                        //파티장이 방을 깸.
                        case "PRB":
                            //system;PRB;파티장이름
                            if(PartyRaidRoommanager.Instance.nowmyleadernickname != stringdata[2])
                                break;
                            
                            alertmanager.Instance.ShowAlert(string.Format(Inventory.GetTranslate("UI7/파티해체함"),stringdata[2]),alertmanager.alertenum.일반);
                            Debug.Log("파티장이 방을 깼다 다 각자 다시 파장을 한다");
                            PartyRaidRoommanager.Instance.Bt_MakeRoom();
                            break;
                        
                        //파티원이 나감
                        case "PRO":
                            //파티원이 나갔다 현재 내 데이터에서 이 파티원을 빼자.
                            //system;0
                            //PRO;1
                            //파티장이름;2
                            //나간유저이름;3
                            //나간유저 파티자리4

                            //리더가 다르면 나간다.
                            Debug.Log(stringdata[2]);
                            Debug.Log(PartyRaidRoommanager.Instance.nowmyleadernickname);
                            if(PartyRaidRoommanager.Instance.nowmyleadernickname != stringdata[2])
                                break;
                            
                            
                            alertmanager.Instance.ShowAlert(string.Format(Inventory.GetTranslate("UI7/파티탈퇴함"),stringdata[3]),alertmanager.alertenum.일반);
                            Debug.Log("나간 유저의 파티 자리는" + stringdata[4]);
                            PartyRaidRoommanager.Instance.PartyMember[int.Parse(stringdata[4])].ExitPlayer();
                            
                            break;
                        //파티원이 나감
                        case "PWD":
                            //파티원을 강퇴함
                            Debug.Log("3은" + stringdata[3]);
                            if (stringdata[3] != PlayerBackendData.Instance.nickname)
                            {
                                break;
                            }
                            
                            alertmanager.Instance.ShowAlert(string.Format(Inventory.GetTranslate("UI7/파티탈퇴함"),stringdata[3]),alertmanager.alertenum.일반);
                            Debug.Log("강퇴" + stringdata[4]); 
                            PartyRaidRoommanager.Instance.Bt_ExitRoom();


                            break;
                        case "PRC":
                            //레이드 시작 패널열기 파장은 자동 준비확인이다.
                            //system;PRC;파티장이름;
                            if(PartyRaidRoommanager.Instance.nowmyleadernickname != stringdata[2])
                                break;
                            //레디창을 보여줌
                            PartyRaidRoommanager.Instance.ShowRaidReadyPanel();
                            break;
                        case "PRYR":
                            //레이드 준비완료를 했다.
                            //system;PRYR;파티장이름;신청자이름;신청자자리번호;
                            if(PartyRaidRoommanager.Instance.nowmyleadernickname != stringdata[2])
                                break;
                            //레디창을 보여줌
                            PartyRaidRoommanager.Instance.readyslots[int.Parse(stringdata[4])].SetReady();
                            break;
                        case "PRNY":
                            //레이드 준비완료를 했다.
                            //system;PRYR;파티장이름;신청자이름;신청자자리번호;
                            if(PartyRaidRoommanager.Instance.nowmyleadernickname != stringdata[2])
                                break;
                            //레디창을 보여줌
                            PartyRaidRoommanager.Instance.readyyesnopanel.SetActive(false);
                            //누구가 준비를 취소하였습니다.
                            alertmanager.Instance.ShowAlert(string.Format(Inventory.GetTranslate("UI7/준비취소함"),stringdata[3]),alertmanager.alertenum.일반);
                            break;
                        #endregion



                    }
                }
                else
                {
                    GetChatData(args.From.NickName, args.Message);
                }

                // 자신의 메시지일 경우
                if (!args.From.IsRemote)
                {
                    //  Debug.Log("나 : " + args.Message);
                }
                // 다른 유저의 메시지일 경우
                else
                {
                    // Debug.Log($"{args.From.NickName}님 : {args.Message}");
                }
            }
            else if (args.ErrInfo.Category == ErrorCode.BannedChat)
            {
                // 도배방지 메세지 
                if (args.ErrInfo.Detail == ErrorCode.BannedChat)
                {
                    //  Debug.Log("메시지를 너무 많이 입력하였습니다. 일정 시간 후에 다시 시도해 주세요");
                }
            }
        };

        //길드채팅 수신
        // 첫 번째 방법
        Backend.Chat.OnGuildChat = (ChatEventArgs args) =>
        {
            if (args.ErrInfo == ErrorInfo.Success)
            {
                //시스템 채팅이다.
                if (args.Message.Contains(guildsystem))
                {
                    string[] stringdata = args.Message.Split(';');
                    //[0]은 시스템 번호 1은 다른 거
                    switch (stringdata[1])
                    {
                        case "SU":
                            ShowSystemChat(args.From.NickName,
                                string.Format(Inventory.GetTranslate("UI6/채팅_제련성공"), 
                                    stringdata[2], //이름
                                    Inventory.Instance.GetRareColorFF(stringdata[3]),
                                    Inventory.GetTranslate(EquipItemDB.Instance.Find_id(stringdata[4]).Name), //징비색
                                    stringdata[5]), false);
                            break;
                        case "WD"://
                            if (stringdata[2].Equals(PlayerBackendData.Instance.nickname))
                            {
                                //내닉네임이면 길드 추방임
                                MyGuildManager.Instance.OffGuildAll();
                            }
                            break;
                        case "BU"://버프 레벨업
                            GuildBuffLvUpani.SetTrigger(Show);
                            int prevlv = int.Parse(stringdata[3]) -1;
                            GuildBuffLvText.text = $"Lv.{prevlv.ToString()} ▶ Lv.{stringdata[3]}";
                            float statlv = float.Parse(GuildBuffDB.Instance.Find_num(stringdata[2]).upperlv) *
                                           float.Parse(stringdata[3]);
                            GuildBuffInfo.text = bool.Parse(GuildBuffDB.Instance.Find_num(stringdata[2]).ispercent) ? $"{((statlv) * 100f):N0}%" : (statlv).ToString("N0");

                            GuildBuffImage.sprite =
                                SpriteManager.Instance.GetSprite(GuildBuffDB.Instance.Find_num(stringdata[2]).sprite);
                            break;
                        case "CP"://직책 변경
                            if (MyGuildManager.Instance.guildMember.Count == 0) return;
                                MyGuildManager.Instance.guildMember[stringdata[2]].position = stringdata[3];
                            if (MyGuildManager.Instance.GuildToggle[1].IsOn)
                            {
                                //길드창이다.
                                MyGuildManager.Instance.SetGuildMemberInfo();
                            }

                            break;
                        case "LvUp"://길드 레벨업
                            
                            //경험치 변경
                            MyGuildManager.Instance.myguildclassdata.curexp = int.Parse(stringdata[4]);
                            //골드 변경
                            MyGuildManager.Instance.myguildclassdata.GuildGold = int.Parse(stringdata[3]);
                            //레벨 변경
                            MyGuildManager.Instance.myguildclassdata.level = int.Parse(stringdata[2]);
                            MyGuildManager.Instance.myguildclassdata.maxexp = 
                                ( int.Parse(stringdata[2]) * 700); //레벨마다 700오름
                            MyGuildManager.Instance.RefreshGuildData();
                            break;
                        case "QS"://퀘스트 시작
                            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("Guild/퀘스트시작"),alertmanager.alertenum.일반);
                            MyGuildManager.Instance.myguildclassdata.questid = stringdata[2];
                            MyGuildManager.Instance.myguildclassdata.questcount = 0;
                            MyGuildManager.Instance.myguildclassdata.isquestfinish = false;
                            MyGuildManager.Instance.RefreshGuildData();
                            break;
                        case "QG"://퀘스트 납품 Quest Give
                            
                            /*
                             *  1000 / 100000
                             *  [닉네임입니다. ] 
                             */
                            //[2]는 현재 [3]은 맥스 [4]는 닉네임 
                            string str = string.Format(Inventory.GetTranslate("Guild/임무납품채팅"), stringdata[2],
                                stringdata[3], stringdata[4]);
                            
                            alertmanager.Instance.ShowAlert2(str,alertmanager.alertenum.일반);
                            MyGuildManager.Instance.myguildclassdata.questcount = int.Parse(stringdata[2]);
                            MyGuildQuestManager.Instance.RefreshQuestPanel();
                            break;
                        case "QF"://퀘스트 종료 
                            alertmanager.Instance.ShowAlert2(Inventory.GetTranslate("Guild/임무완료"),alertmanager.alertenum.일반);
                            MyGuildManager.Instance.myguildclassdata.questid = stringdata[2];
                            MyGuildManager.Instance.myguildclassdata.questcount = int.Parse(stringdata[3]);
                            MyGuildManager.Instance.myguildclassdata.isquestfinish = bool.Parse(stringdata[4]);
                            MyGuildQuestManager.Instance.RefreshQuestPanel();
                            break;
                        case "GRS"://길드레이드 시작
                            alertmanager.Instance.ShowAlert2(Inventory.GetTranslate("Guild/길드레이드수락"),alertmanager.alertenum.일반);
                            MyGuildManager.Instance.myguildclassdata.SetGuildRaid(stringdata[2]);
                            GuildRaidManager.Instance.RefreshRaidRaidHp();
                            break;
                        case "GRA"://길드레이드 공격함
                            MyGuildManager.Instance.myguildclassdata.GuildRaidCurHPs = decimal.Parse(stringdata[5]);
                            int index = MyGuildManager.Instance.myguildclassdata.GuildRaidRankNames.IndexOf(stringdata[2]);
                            //피해량정보가있다
                            if (index != -1)
                            {
                                MyGuildManager.Instance.myguildclassdata.GuildRaidRankDmgs[index] =
                                    decimal.Parse(stringdata[4]);
                            }
                            else
                            {
                                //추가
                                MyGuildManager.Instance.myguildclassdata.GuildRaidRankNames.Add(stringdata[2]);
                                MyGuildManager.Instance.myguildclassdata.GuildRaidRankDmgs.Add(decimal.Parse(stringdata[4]));
                            }
                            
                            GuildRaidManager.Instance.RefreshRaidRaidHp();
                            break;
                        
                      
                    }
                }
                //일반 채팅이다.
                else
                {
                    GetChatData(args.From.NickName, args.Message.ToString(),true);
                }
                
                // 자신의 메시지일 경우
                if (!args.From.IsRemote)
                {
                    //  Debug.Log("나 : " + args.Message);
                }
                // 다른 유저의 메시지일 경우
                else
                {
                    // Debug.Log($"{args.From.NickName}님 : {args.Message}");
                }
            }
            else if (args.ErrInfo.Category == ErrorCode.BannedChat)
            {
                // 도배방지 메세지 
                if (args.ErrInfo.Detail == ErrorCode.BannedChat)
                {
                    //  Debug.Log("메시지를 너무 많이 입력하였습니다. 일정 시간 후에 다시 시도해 주세요");
                }
            }
        };
        
        
        //길드 입장
        Backend.Chat.OnJoinGuildChannel = (JoinChannelEventArgs args) =>
        {
            //Debug.Log(string.Format("OnJoinGuildChannel {0}", args.ErrInfo));
            //입장에 성공한 경우
            if (args.ErrInfo == ErrorInfo.Success)
            {
                // 내가 접속한 경우 
                if (!args.Session.IsRemote)
                {
                    //길드 토글 활성화
                    toggle_guild.gameObject.SetActive(true);
                   // Debug.Log("채널에 접속했습니다");
                    if (chathave_Guild)
                    {
                        Backend.Chat.ChatToChannel(ChannelType.Guild,chatstring_Guild);
                        chathave_Guild = false;
                        chatstring_Guild = "";
                    }
                }
                //다른 유저가 접속한 경우
                else
                {
                   // Debug.Log(args.Session.NickName + "님이 접속했습니다");
                }
            }
            else
            {
                //에러가 발생했을 경우
                //Debug.Log("입장 도중 에러가 발생했습니다 : " + args.ErrInfo.Reason);
            }
        };
        
        //채널 접속시 
        Backend.Chat.OnJoinChannel = (JoinChannelEventArgs args) =>
        {
            //Debug.Log($"OnJoinChannel {args.ErrInfo}");
            //입장에 성공한 경우
            if (args.ErrInfo == ErrorInfo.Success)
            {
                // 내가 접속한 경우 
                if (!args.Session.IsRemote)
                {
                    if (chathave_pulic)
                    {
                        Backend.Chat.ChatToChannel(ChannelType.Public,chatstring_public);
                        chathave_pulic = false;
                        chatstring_public = "";
                    }
                }
                //다른 유저가 접속한 경우
                else
                {
                }
            }
            else
            {
            }
        };
        
/*
        //최근 채팅 불러오기
        Backend.Chat.OnRecentChatLogs = (RecentChatLogsEventArgs args) =>
        {

            //uuid를 이용하여 해당 일반 채널의 최근 채팅 내역 가져오기(25개만)
            BackendReturnObject callback2 = Backend.Chat.GetRecentChat(ChannelType.Public, nowjoinedchat.inDate, 25);
           // Debug.Log("최근 내역");
            //Debug.Log(callback2);
            if (callback2.IsSuccess())
            {
                //성공 시...
                for (int i = 0; i < callback2.Rows().Count; i++)
                {
                    string nickname = callback2.Rows()[i]["nickname"].ToString();
                    string message = callback2.Rows()[i]["message"].ToString();
                    GetChatData(nickname, message);
                }
            }
/*
 
            Debug.Log(bro);
            for (int index = args.LogInfos.Count; index>= 0; index--)
            {
                var t = args.LogInfos[index];
                string[] chatdata = t.Message.Split(";|");
                
                 * 0은 비쥬얼 데이터
                 * 1은 레벨
                 * 2는 모험가 레벨
                 * 3은 채팅
                 
                if (chatdata[4] != "")
                {
                    // Debug.Log("dd");
                    ShowChat(chatdata[0], chatdata[1], chatdata[2], chatdata[3], t.NickName, chatdata[4]);
                }
                else
                {
                    // Debug.Log("d");
                    //ShowChat(args.From.NickName, string.Format(publicstrnew, args.From.NickName), chatdata[1]);
                    ShowChat(chatdata[0], chatdata[1], chatdata[2], chatdata[3], t.NickName, chatdata[4]);
                }
            }
        };
        };
        */
        
        // 길드나가기
        Backend.Chat.OnLeaveGuildChannel = (LeaveChannelEventArgs args) =>
        {
            // 퇴장에 성공한 경우
            if (args.ErrInfo == ErrorInfo.Success)
            {
                // 내가 퇴장한 경우 
                if (!args.Session.IsRemote)
                {
                //    Debug.Log("채널에서 퇴장했습니다");
                    toggle_guild.gameObject.SetActive(false);
                    toggle_public.IsOn = true;
                }
                // 다른 유저가 퇴장한 경우
                else
                {
                //    Debug.Log(args.Session.NickName + "님이 퇴장했습니다");
                }
            }
            else
            {
                // 에러가 발생했을 경우
              //  Debug.Log("퇴장 도중 에러가 발생했습니다 : " + args.ErrInfo.Reason);
            }
        };
    }

    
    public void GetChannelList()
    {
        //채널리스트 초기화
        channellist.Clear();

        BackendReturnObject bro = Backend.Chat.GetGroupChannelList("Global");
        // Debug.Log(bro);
        JsonData channeldata = bro.Rows();
        for (int i = 0; i < channeldata.Count; i++)
        {
            channellist.Add(new ChatChannel(channeldata[i]["inDate"].ToString(),
                channeldata[i]["alias"].ToString(),
                channeldata[i]["serverAddress"].ToString(),
                ushort.Parse(channeldata[i]["serverPort"].ToString()),
                channeldata[i]["joinedUserCount"].ToString(),
                channeldata[i]["maxUserCount"].ToString()));
        }

        ErrorInfo errorInfo;
        foreach (var t in channellist)
        {
            //-10은 10명은 여분자리로 남긴거
            if (int.Parse(t.maxUserCount) - 10 > int.Parse(t.joinedUserCount))
            {
                JoinChannel(t);
                nowjoinedchat = t;
                break;
            }
        }

        // 채팅 채널 리스트 가져오기
        if (!Settingmanager.Instance.ischatbool_public)
        {

            //uuid를 이용하여 해당 일반 채널의 최근 채팅 내역 가져오기(25개만)
            Backend.Chat.GetRecentChat(ChannelType.Public, nowjoinedchat.inDate, 25, callback2 =>
            {
               //   Debug.Log("최근 내역");
               //   Debug.Log(callback2);
                if (callback2.IsSuccess())
                {
                    //성공 시...
                    for (int i = 0; i < callback2.Rows().Count; i++)
                    {
//                        Debug.Log("call");
                        string nickname = callback2.Rows()[i]["nickname"].ToString();
                        string message = callback2.Rows()[i]["message"].ToString();
                      //  Debug.Log(nickname);
                      //  Debug.Log(message);

                        if (!message.Contains(publicsystem))
                        {
                          //  Debug.Log("여기");
                            GetChatData(nickname, message);
                        }
                        else
                        {
                          //  Debug.Log("여기2");
                            string[] stringdata = message.Split(';');
                            //[0]은 시스템 번호 1은 다른 거
                            switch (stringdata[1])
                            {
                                case "SU":
                                    ShowSystemChat(nickname,
                                        string.Format(Inventory.GetTranslate("UI6/채팅_제련성공"),
                                            stringdata[2], //이름
                                            Inventory.Instance.GetRareColorFF(stringdata[3]),
                                            Inventory.GetTranslate(EquipItemDB.Instance.Find_id(stringdata[4])
                                                .Name), //징비색
                                            stringdata[5]), false);
                             //      Debug.Log("call2");

                                    break;
                                case "RO":
                                    //얻은아이템
                                    ItemdatabasecsvDB.Row item = ItemdatabasecsvDB.Instance.Find_id(stringdata[4]);
                                    //열쇠
                                    ItemdatabasecsvDB.Row item2 = ItemdatabasecsvDB.Instance.Find_id(stringdata[3]);
                                    ShowSystemChat(nickname,
                                        string.Format(Inventory.GetTranslate("UI6/채팅_룰렛뽑기"),
                                            stringdata[2],
                                            Inventory.Instance.GetRareColorFF(item.rare),
                                            Inventory.GetTranslate(item.name),
                                            Inventory.Instance.GetRareColorFF(item2.rare),
                                            Inventory.GetTranslate(item2.name), stringdata[5]),
                                        false);
                           //         Debug.Log("call3");

                                    break;
                                case "EUP":
                                    ShowSystemChat(nickname,
                                        string.Format(Inventory.GetTranslate("UI6/채팅_강화성공"),
                                            stringdata[2], //이름
                                            Inventory.Instance.GetRareColorFF(stringdata[4]),
                                            Inventory.GetTranslate(EquipItemDB.Instance.Find_id(stringdata[3])
                                                .Name), //징비색
                                            stringdata[5]), false);
                                 //   Debug.Log("call4");

                                    break;
                                case "PET":
                                    ShowSystemChat(nickname,
                                        string.Format(Inventory.GetTranslate("UI6/채팅_펫나옴"),
                                            stringdata[2], //이름
                                            Inventory.Instance.GetRareColorFF(stringdata[3])
                                            , Inventory.Instance.GetRareColorFF(stringdata[3]),
                                            Inventory.GetTranslate(PetDB.Instance.Find_id(stringdata[4])
                                                .name)), false);
                              //      Debug.Log("call5");

                                    break;
                            }
                        }
                        //    Debug.Log(nickname + " : " + message);
                    }
                }
            });
        }
        else
        {
            Settingmanager.Instance.ischatbool_public = false;
        }

        
        //다끝나면 길드차례
        JoinGuildChannel();

    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    public void JoinGuildChannel()
    {
       // Debug.Log("길드 채널 불러오는 중");
        BackendReturnObject bro = Backend.Chat.GetGroupChannelList("Guild");
       // Debug.Log("길드");
       //
       // Debug.Log(bro);
        if (bro.IsSuccess())
        {
            JsonData data = bro.Rows()[0];
            nowjoinedchat_guild = new ChatChannel(data["inDate"].ToString(),
                data["alias"].ToString(),
                data["serverAddress"].ToString(),
                ushort.Parse(data["serverPort"].ToString()),
                data["joinedUserCount"].ToString(),
                data["maxUserCount"].ToString());

            ErrorInfo errorInfo;
            Backend.Chat.JoinChannel(ChannelType.Guild, nowjoinedchat_guild.serverAddress, nowjoinedchat_guild.serverPort, "Guild", nowjoinedchat_guild.inDate, out errorInfo);



            if (Settingmanager.Instance.ischatbool_guild)
            {
                Settingmanager.Instance.ischatbool_guild = false;
                return;
            }
              //uuid를 이용하여 해당 일반 채널의 최근 채팅 내역 가져오기(25개만)
                    Backend.Chat.GetRecentChat(ChannelType.Guild, nowjoinedchat_guild.inDate, 25, callback2 =>
                    {
                   //     Debug.Log("길드 최근 내역");
                    //    Debug.Log(callback2);
                        if (callback2.IsSuccess())
                        {
                            //성공 시...
                            for (int i = 0; i < callback2.Rows().Count; i++)
                            {
                                string nickname = callback2.Rows()[i]["nickname"].ToString();
                                string message = callback2.Rows()[i]["message"].ToString();
                                GetChatData(nickname, message);
                             //   Debug.Log(nickname + " : " + message);
                            }
                        }
                    });
        }
    }

    #region  일반채팅

    public void ChattoSmeltUp(int Smeltcount,string itemid,string rare)
    {
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        if (isConnect)
        {
            //Debug.Log("길드 채널에 연결되어 있습니다");
            //withdraw
            //시스템이다 / 닉네임 /강퇴처리한다.
            Backend.Chat.ChatToChannel(ChannelType.Public,
                $"{publicsystem};SU;{PlayerBackendData.Instance.nickname};{rare};{itemid};{Smeltcount}");
        }
    }

    
    public void ChattoRoullet(string itemid,string needitem,string count)
    {
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        if (isConnect)
        {
            Backend.Chat.ChatToChannel(ChannelType.Public,
                $"{publicsystem};RO;{PlayerBackendData.Instance.nickname};{itemid};{needitem};{count}");
        }
    }
    public void ChattoUpgradeUp(int Upgradecount,string itemid,string rare)
    {
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        if (isConnect)
        {
//            Debug.Log("여기입니다");
            //Debug.Log("길드 채널에 연결되어 있습니다");
            //withdraw
            //시스템이다 / 닉네임 /강퇴처리한다.
            Backend.Chat.ChatToChannel(ChannelType.Public,
                $"{publicsystem};EUP;{PlayerBackendData.Instance.nickname};{rare};{itemid};{Upgradecount}");
        }
    }
    
    public void ChattoPet(string petid,string rare)
    {
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        if (isConnect)
        {
//            Debug.Log("여기입니다");
            //Debug.Log("길드 채널에 연결되어 있습니다");
            //withdraw
            //시스템이다 / 닉네임 /강퇴처리한다.
            Backend.Chat.ChatToChannel(ChannelType.Public,
                $"{publicsystem};PET;{PlayerBackendData.Instance.nickname};{rare};{petid}");
        }
    }
    
    
    #endregion
    

    #region 길드관련

    const string guildsystem = "#guildsys#";
    //강퇴 시 적이 접속해있다면 채팅을 침.
    public void ChattoGuildWithDraw(string Nickname)
    {
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Guild);
        if (isConnect)
        {
            //Debug.Log("길드 채널에 연결되어 있습니다");
            //withdraw
            //시스템이다 / 닉네임 /강퇴처리한다.
            Backend.Chat.ChatToChannel(ChannelType.Guild,
                $"{guildsystem};WD;{Nickname}");
        }
    }
    public void ChattoGuildBuffUp(int buffindex,int lv)
    {
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Guild);
        if (isConnect)
        {
            //buffup
            //시스템이다 / 버프번호 /버프 레벨 / 버프다
            Backend.Chat.ChatToChannel(ChannelType.Guild,
                $"{guildsystem};BU;{buffindex};{lv}");
        }
    }
    
    public void ChattoGuildChangePosition(string indate,string pos)
    {
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Guild);
        if (isConnect)
        {
            //Debug.Log("길드 채널에 연결되어 있습니다");
            //Change Position
            //시스템이다 / 닉네임 /강퇴처리한다.
            Backend.Chat.ChatToChannel(ChannelType.Guild,
                $"{guildsystem};CP;{indate};{pos}");
        }
    }
    public void ChattoGuildLvupGuild(string lv,string nowgold,string nowexp)
    {
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Guild);
        if (isConnect)
        {
            //Debug.Log("길드 채널에 연결되어 있습니다");
            //Change Position
            //시스템이다 / 닉네임 /강퇴처리한다.
            Backend.Chat.ChatToChannel(ChannelType.Guild,
                $"{guildsystem};LvUp;{lv};{nowgold};{nowexp}");
        }
    }
    
    //길드 퀘스트 시작 알림 
    public void ChattoGuildQuestStart(string questid)
    {
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Guild);
        if (isConnect)
        {
            //Debug.Log("길드 채널에 연결되어 있습니다");
            //Change Position
            //시스템이다 / 닉네임 /강퇴처리한다.
            //QS = QuestStart
            Backend.Chat.ChatToChannel(ChannelType.Guild,
                $"{guildsystem};QS;{questid}");
        }
        else
        {
            JoinGuildChannel();
            ChattoGuildQuestStart(questid);
        }
    }
    //납품
    public void ChattoGiveGuildQuestItem(int cur , int max)
    {
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Guild);
        if (isConnect)
        {
            //Debug.Log("길드 채널에 연결되어 있습니다");
            //Change Position
            //시스템이다 / 닉네임 /강퇴처리한다.
            //QS = QuestStart
            Backend.Chat.ChatToChannel(ChannelType.Guild,
                $"{guildsystem};QG;{cur};{max};{PlayerBackendData.Instance.nickname}");
        }
        else
        {
            JoinGuildChannel();
            ChattoGiveGuildQuestItem(cur,max);
        }
    }
    
    
    //임무 완료
    public void ChattoFinishGuildQuest(string id, int count,bool isfinish)
    {
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Guild);
        if (isConnect)
        {
            //Debug.Log("길드 채널에 연결되어 있습니다");
            //Change Position
            //시스템이다 / 닉네임 /강퇴처리한다.
            //QS = QuestStart
            Backend.Chat.ChatToChannel(ChannelType.Guild,
                $"{guildsystem};QF;{id};{count};{isfinish}");
        }
        else
        {
            JoinGuildChannel();
            ChattoFinishGuildQuest(id,count,isfinish);
        }
    }
    
    //길드레이드 시작
    public void ChattoStartGuildRaid(string id)
    {
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Guild);
        if (isConnect)
        {
           //길레시작
            Backend.Chat.ChatToChannel(ChannelType.Guild,
                $"{guildsystem};GRS;{id}");
        }
        else
        {
            JoinGuildChannel();
            ChattoStartGuildRaid(id);
        }
    }
    
    public void ChattoGuildRaidAttack(string nickname,decimal dmg,decimal mytotaldmg,decimal monhp)
    {
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Guild);
        if (isConnect)
        {
            //GRA = Guild Raid Attack
            Backend.Chat.ChatToChannel(ChannelType.Guild,
                $"{guildsystem};GRA;{nickname};{dmg:N0};{mytotaldmg};{monhp:N0}");
        }
        else
        {
            JoinGuildChannel();
            ChattoGuildRaidAttack(nickname,dmg,mytotaldmg,monhp);
        }
    }
    #endregion
    
    
    public void OffGuild()
    {
        Backend.Chat.LeaveChannel(ChannelType.Guild);
    }

    public void OnGuild()
    {
        JoinGuildChannel();
    }

    private void JoinChannel(ChatChannel chat, bool isguild =false)
    {
        if (isguild)
        {
            ErrorInfo errorInfo;
            Backend.Chat.JoinChannel(ChannelType.Guild, chat.serverAddress, chat.serverPort, "Global", chat.inDate, out errorInfo);
            nowjoinedchat_guild = chat;
        }
        else
        {
            ErrorInfo errorInfo;
            Backend.Chat.JoinChannel(ChannelType.Public, chat.serverAddress, chat.serverPort, "Global", chat.inDate, out errorInfo);
            nowjoinedchat = chat;
            try
            {
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    public void JoinChannel_Public(ChatChannel chat)
    {
        ErrorInfo errorInfo;
        Backend.Chat.JoinChannel(ChannelType.Public, chat.serverAddress, chat.serverPort, "Global", chat.inDate, out errorInfo);
        nowjoinedchat = chat;
    }


    #region 이벤트수신
    // Update is called once per frame
    void Update()
    {
        Backend.Chat.Poll();
    }
    #endregion

    #region Send
    public InputField chatinput_public;
    
    public void Bt_SendChat_Public()
    {
        if (chatinput_public.text == "")
            return;


        // 채팅 서버와 클라이언트가 연결되어 있을 경우
        // isConnect = true

        // 채팅 서버와 클라이언트가 연결되어 있지 않은 경우
        // isConnect = false


        if (toggle_guild.IsOn)
        {
            bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Guild);

            if (isConnect)
            {
                
                //Debug.Log("길드 채널에 연결되어 있습니다");
                Backend.Chat.ChatToChannel(ChannelType.Guild,
                    $"{GetUserVisualData()};|{PlayerBackendData.Instance.GetLv()};|{PlayerBackendData.Instance.GetAdLv()};|{RankingManager.Instance.getmybprank()};|{chatinput_public.text}");
            }
            else
            {
                //Debug.Log("채팅방연결중");
                chathave_Guild = true;
                chatstring_Guild =
                    $"{GetUserVisualData()};|{PlayerBackendData.Instance.GetLv()};|{PlayerBackendData.Instance.GetAdLv()};|{RankingManager.Instance.getmybprank()};|{chatinput_public.text}";
                JoinGuildChannel();
            }

        }
        else
        {
            bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
            
            if (isConnect)
            {
              //  Debug.Log("일반 채널에 연결되어 있습니다");
                Backend.Chat.ChatToChannel(ChannelType.Public,
                    $"{GetUserVisualData()};|{PlayerBackendData.Instance.GetLv()};|{PlayerBackendData.Instance.GetAdLv()};|{RankingManager.Instance.getmybprank()};|{chatinput_public.text}");

            }
            else
            {
              //  Debug.Log("채팅방연결중");
                chathave_pulic = true;
                chatstring_public = $"{GetUserVisualData()};|{PlayerBackendData.Instance.GetLv()};|{PlayerBackendData.Instance.GetAdLv()};|{RankingManager.Instance.getmybprank()};|{chatinput_public.text}";

                //채팅에 실패 했다. 다시 친다.
                GetChannelList();
            }
        }
        chatinput_public.text = "";
    }
    //연결이 끊어졌을 시 채팅을 저장했다가 연결 후 침
    private bool chathave_pulic = false;
     string chatstring_public = "";
    
     private bool chathave_Guild = false;
     string chatstring_Guild = "";
    //유저의 외형을 가져옴
    private string GetUserVisualData()
    {
        return PlayerData.Instance.GetAvartaData();

    }
    #endregion

    public TextMeshProUGUI lobbyText;
    private static readonly int Show = Animator.StringToHash("show");

    public chatslot[] publicchatslot;
    public chatslot[] guildchatslot;
    public chatslot[] systemchatslot;
    public int p_num = 0;
    public int g_num = 0;
    public int s_num = 0;
    
    
    //채팅보여줌
    void ShowChat(string visualdata,string lv,string adlv,string bpranking, string nickname, string message,bool isguild = false)
    {
        if (isguild)
        {
            //chatslot  chatitempools = Instantiate(chatitempool_Guild, ChatTrans_Guild);
            guildchatslot[g_num].Chat(visualdata,lv,adlv,bpranking,"", nickname, message, false);
            guildchatslot[g_num].gameObject.SetActive(true);
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)ChatTrans_Guild);
            RefreshChatPanel_Guild();
            lobbyText.text = $"[{Inventory.GetTranslate("UI2/0_채팅길드")}]{nickname} :{message}";
            guildchatslot[g_num].transform.SetAsLastSibling();
            g_num++;
            if (g_num >= guildchatslot.Length)
            {
                g_num = 0;
            }
        }
        else
        {
           // chatslot  chatitempools = Instantiate(chatitempool, ChatTrans);
            publicchatslot[p_num].Chat(visualdata,lv,adlv,bpranking,"", nickname, message, false);
            publicchatslot[p_num].gameObject.SetActive(true);
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)ChatTrans);
            RefreshChatPanel();
            lobbyText.text = $"[{Inventory.GetTranslate("UI2/0_채팅일반")}]{nickname} :{message}";
            publicchatslot[p_num].transform.SetAsLastSibling();
            p_num++;
            if (p_num >= publicchatslot.Length)
            {
                p_num = 0;
            }
        }
    }

    void ShowSystemChat(string nickname, string message,bool isguild)
    {
        if(!SettingReNewal.Instance.SystemChat[0].IsOn)
            return;
        
        if (isguild)
        {
            //chatslot  chatitempools = Instantiate(chatitempool_Guild, ChatTrans_Guild);
            guildchatslot[g_num].ShowSystem(nickname,message);
            guildchatslot[g_num].gameObject.SetActive(true);
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)ChatTrans_Guild);
            RefreshChatPanel_Guild();
            lobbyText.text = $"{Inventory.GetTranslate("UI6/채팅_시스템")} {message}";
            guildchatslot[g_num].transform.SetAsLastSibling();
            g_num++;
            if (g_num >= guildchatslot.Length)
            {
                g_num = 0;
            }
        }
        else
        {
            // chatslot  chatitempools = Instantiate(chatitempool, ChatTrans);
       //     Debug.Log(nickname);
//            Debug.Log(message);
            systemchatslot[s_num].ShowSystem(nickname,message);
            systemchatslot[s_num].gameObject.SetActive(true);
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)ChatTrans);
            RefreshChatPanel();
            lobbyText.text = $"{Inventory.GetTranslate("UI6/채팅_시스템")} {message}";
            systemchatslot[s_num].transform.SetAsLastSibling();
            s_num++;
            if (s_num >= systemchatslot.Length)
            {
                s_num = 0;
            }
        }
    }
    

}

/*
List<ChatChannel> channellist = new List<ChatChannel>();
List<ChatChannel> channellist_Guild = new List<ChatChannel>();
public ChatChannel nowjoinedchat;
public ChatChannel nowjoinedchat_guild;
//채팅쓰는곳 크기
Rect GuildOnSize = new Rect(520, -70, 705, 88);
Rect GuildOffSize = new Rect(446, -70, 853, 88);
// Start is called before the first frame update
void Start()
{
    //채널가져오기
    // 첫 번째 방법 (동기)
    var bro = Backend.Initialize(true);
    if (bro.IsSuccess())
    {
        // 초기화 성공 시 로직
        ChatHandlers();
       Invoke("GetChannelList",2f);
        Backend.Notification.Connect();
        Backend.Chat.SetFilterReplacementChar('*');
        Backend.Chat.SetFilterUse(true);
    }
    else
    {
        // 초기화 실패 시 로직
    }

}

#region 길드관련
public bool isguild;
public Toggle Guildchattoggle; //true면 길드채팅 false면 일반채팅
public GameObject Channellistbutton;
public CanvasGroup ChatPanel_Public;
public CanvasGroup ChatPanel_Guild;

//길드채팅을 안하게한다..
public void ResetGuildChat()
{
    chatinput_public_Rect.anchoredPosition = new Vector2(GuildOffSize.x, GuildOffSize.y);
    chatinput_public_Rect.sizeDelta = new Vector2(GuildOffSize.width, GuildOffSize.height);
    Guildchattoggle.gameObject.SetActive(false);
    Guildchattoggle.isOn = true;
    nowjoinedchat_guild = null;
    Channellistbutton.SetActive(true);
    UserMemberChange_Guild(0);
    isguild = false;
    ChatPanel_Public.alpha = 1;
    ChatPanel_Public.blocksRaycasts = true;
    ChatPanel_Public.interactable = true;
    ChatPanel_Guild.alpha = 0;
    ChatPanel_Guild.blocksRaycasts = false;
    ChatPanel_Guild.interactable = false;
}

public void SetGuildChatInit()
{
    Guildchattoggle.gameObject.SetActive(true);
    chatinput_public_Rect.anchoredPosition = new Vector2(GuildOnSize.x, GuildOnSize.y);
    chatinput_public_Rect.sizeDelta = new Vector2(GuildOnSize.width, GuildOnSize.height);

}

//길드채널 가입
public void JoinGuildChannel()
{
    BackendReturnObject bro = Backend.Chat.GetGroupChannelList("길드");
    Debug.Log("길드");
    Debug.Log(bro);
    if (bro.IsSuccess())
    {

        JsonData data = bro.Rows()[0];
        // Debug.Log("호호");
        //Debug.Log(bro);
        nowjoinedchat_guild = new ChatChannel(data["inDate"].ToString(),
            data["alias"].ToString(),
            data["serverAddress"].ToString(),
            ushort.Parse(data["serverPort"].ToString()),
            data["joinedUserCount"].ToString(),
            data["maxUserCount"].ToString());

        nowcurusercount_guild = int.Parse(nowjoinedchat_guild.joinedUserCount);
        nowmaxusercount_guild = int.Parse(nowjoinedchat_guild.maxUserCount);

        ErrorInfo errorInfo;
        Backend.Chat.JoinChannel(ChannelType.Guild, nowjoinedchat_guild.serverAddress, nowjoinedchat_guild.serverPort, "길드", nowjoinedchat_guild.inDate, out errorInfo);
        chatmanager.Instance.SetGuildChatInit();
    }
}
public void Toggle_ShowGuildChat()
{
    if (Guildchattoggle.isOn)
    {
        try
        {
            bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Guild);

            if (isConnect)
            {
                Debug.Log("길드채널에 연결되어 있습니다");
            }
            else
            {
                JoinGuildChannel();
            }
            Channellistbutton.SetActive(false);
            UserMemberChange_Guild(0);
            // Debug.Log("길드채팅킴");
            isguild = true;
            ChatPanel_Public.alpha = 0;
            ChatPanel_Public.blocksRaycasts = false;
            ChatPanel_Public.interactable = false;
            ChatPanel_Guild.alpha = 1;
            ChatPanel_Guild.blocksRaycasts = true;
            ChatPanel_Guild.interactable = true;
            chatroomname.text = nowjoinedchat_guild.alias;
        }
        catch
        {

        }
        //길드채팅킴
    }
    else
    {
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);

        if (isConnect)
        {
            Debug.Log("일반채널에 연결됨");
        }
        else
        {
            GetChannelList();
        }

        Channellistbutton.SetActive(true);
        UserMemberChange(0);
        isguild = false;
        ChatPanel_Public.alpha = 1;
        ChatPanel_Public.blocksRaycasts = true;
        ChatPanel_Public.interactable = true;
        ChatPanel_Guild.alpha = 0;
        ChatPanel_Guild.blocksRaycasts = false;
        ChatPanel_Guild.interactable = false;
        chatroomname.text = nowpublicchatroomname;  
    }
}
#endregion

#region 채팅핸들러
public Text chatmembercounttext;
public Text[] participanttext; //채팅참여자 채널
public List<string> par_nickname = new List<string>();
public List<string> par_nickname_guild = new List<string>();
public int nowcurusercount; //현재 채널의 유저수
public int nowmaxusercount;//현재 채널의 최대유저수
public int nowcurusercount_guild; //현재 채널의 유저수
public int nowmaxusercount_guild;//현재 채널의 최대유저수

public string GetUserVisualData()
{
    string str = "";
    return str;

}





void ShowChat(string realnick,string nickname , string message)
{
    //chatitemobj = isguild ? Instantiate(chatitempool, ChatTrans_Guild) : Instantiate(chatitempool, ChatTrans) ;
    chatitemobj = Instantiate(chatitempool, ChatTrans) ;
    chatitemobj_ingame = Instantiate(chatitempool_ingame, ChatTrans_ingame) ; 
    chatitemobj.Chat(realnick,nickname, message,false);
    chatitemobj_ingame.Chat( realnick,nickname, message,false);
    LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
    LayoutRebuilder.ForceRebuildLayoutImmediate(rect_ingame);
    cont.SetLayoutVertical();
    cont_ingame.SetLayoutVertical();
    chatitemobj.gameObject.SetActive(true);
    chatitemobj_ingame.gameObject.SetActive(true);
}

void ShowChatToguild(string realnick,string nickname, string message)
{
    chatitemobj = Instantiate(chatitempool, ChatTrans_Guild);
    chatitemobj.Chat(realnick,nickname, message,false);
    LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
    cont.SetLayoutVertical();
    chatitemobj.gameObject.SetActive(true);
}
//
void ShowChatSystem(string nickname, string message)
{
    chatitemobj = Instantiate(chatitempool, ChatTrans);
    chatitemobj_ingame = Instantiate(chatitempool_ingame, ChatTrans_ingame) ; 
    chatitemobj.Chat("",nickname, message, true);
    chatitemobj_ingame.Chat("", nickname, message, true);
    LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
    LayoutRebuilder.ForceRebuildLayoutImmediate(rect_ingame);
    cont.SetLayoutVertical();
    cont_ingame.SetLayoutVertical();
    chatitemobj.gameObject.SetActive(true);
    chatitemobj_ingame.gameObject.SetActive(true);
}
void ShowChatSystemNameSystem(string nickname, string message)
{
    chatitemobj = Instantiate(chatitempool, ChatTrans);
    chatitemobj_ingame = Instantiate(chatitempool_ingame, ChatTrans_ingame) ; 
    chatitemobj.Chat("", Inventory.GetTranslate("UI/Systemname"), message, true);
    chatitemobj_ingame.Chat("", Inventory.GetTranslate("UI/Systemname"), message, true);
    LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
    LayoutRebuilder.ForceRebuildLayoutImmediate(rect_ingame);
    cont.SetLayoutVertical();
    cont_ingame.SetLayoutVertical();
    chatitemobj.gameObject.SetActive(true);
    chatitemobj_ingame.gameObject.SetActive(true);
}
string publicstr = "[<color=magenta>★{0}</color>]<color=orange>{1}</color>";
string publicstrnew = "[<color=lime>NEW</color>]<color=orange>{0}</color>";
string guildstr = "<color=lime>{0}</color>";
string nowpublicchatroomname;

string sysdropweapon = "<sssysdropweapon>";
string sysdropartifact = "<sssysdropartifact>";
string syslevelartifact = "<sssyslevelartifact>";
string sysjinartifact = "<sssysjinartifact>";

//월드
string sysguildraidfinish = "<sysguildraidfinish>";
public Animator raredropani;
public TextMeshProUGUI raredroptext;
public void ShowDropAlertChat(string text)
{
    raredroptext.text = text;
    raredropani.SetTrigger("show");
}

//사이문자열 추출
public static string GetMiddleString(string str, string begin, string end)
{
    if (string.IsNullOrEmpty(str))
    {
        return null;
    }

    string result = null;
    if (str.IndexOf(begin) > -1)
    {
        str = str.Substring(str.IndexOf(begin) + begin.Length);
        if (str.IndexOf(end) > -1) result = str.Substring(0, str.IndexOf(end));
        else result = str;
    }
    return result;
}

void quitgame()
{
    Application.Quit();
}
void ChatHandlers()
{
    //공지 채팅
    //첫번째 방법
    Backend.Chat.OnNotification = (NotificationEventArgs args) =>
    {
        if (args.Message == string.Format("/out {0}",PlayerBackendData.Instance.nickname))
        {
            Invoke("quitgame", 5f);
            ShowChatSystemNameSystem("", string.Format("<color=orange>{0}</color>", args.Message));
        }
        else if (args.Message.Contains("/quit"))
        {
           // Savemanager.Instance.SaveClassData(false);
           // Alertmanager.Instance.AlertChat(Alertmanager.alertype.기본고급, Inventory.GetTranslate("UI2/5초뒤종료"));
            Invoke("quitgame", 5f);
        ShowChatSystemNameSystem("", string.Format("<color=orange>{0}</color>", args.Message) );
        }
        else
        {
           // Alertmanager.Instance.AlertChat(Alertmanager.alertype.기본고급, args.Message);
        ShowChatSystemNameSystem("", string.Format("<color=orange>{0}</color>", args.Message) );
        }
    };



    //채팅이오면
    Backend.Chat.OnChat = (ChatEventArgs args) =>
    {
        //아이템링크
      //  Debug.Log(args.Message);
        if (args.Message.Contains("<item="))
        {
        //    Debug.Log("링크있다");
            //링크가 있다.
            string itemlink = GetMiddleString(args.Message, "<item=", ">");
            string[] hi = itemlink.Split('$');
            string[] data = hi[1].Split(';');
            string artifactname = "";
            //강화수치가없다면 
            if(data[data.Length - 1] == "0")
            {
                 //artifactname = Inventory.Instance.ChangeItemRareColor(Inventory.GetTranslate(ArtifactDataDB.Instance.Find_id(hi[0]).Name), ArtifactDataDB.Instance.Find_id(hi[0]).Rare);
            }
            else
            {
                 //artifactname = Inventory.Instance.ChangeItemRareColor(string.Format("+{0} {1}", data[data.Length - 1], Inventory.GetTranslate(ArtifactDataDB.Instance.Find_id(hi[0]).Name)), ArtifactDataDB.Instance.Find_id(hi[0]).Rare);
            }

            Inventory.Instance.StringRemove();
            Inventory.Instance.StringWrite("[<link=itemlink");
            Inventory.Instance.StringWrite(itemlink);
            Inventory.Instance.StringWrite(">");
            Inventory.Instance.StringWrite(artifactname);
            Inventory.Instance.StringWrite("</link>]");
            args.Message = args.Message.Replace("<item=" + itemlink + ">", Inventory.Instance.StringEnd());
            //Debug.Log(args.Message);
            if(!args.Message.Contains(sysdropartifact) && !args.Message.Contains(syslevelartifact) && !args.Message.Contains(sysjinartifact))
            {
                ShowChat(args.From.NickName, string.Format(publicstrnew, args.From.NickName), args.Message);
            }

        }



        //길드레이드 완료
        if (args.Message.Contains(sysguildraidfinish))
        {
            args.Message = args.Message.Replace(sysguildraidfinish, "");
            string[] data = args.Message.Split(';'); //1은길드이름 2는 길드난이도

           // string text = string.Format(Inventory.GetTranslate("Guild/길드레이드보상획득"), data[0],Inventory.GetTranslate(MapDB.Instance.Find_id(data[1]).mapname));
            //무기 드롭
           // ShowChatSystemNameSystem(args.From.NickName, text);
           // ShowDropAlertChat(text);
        }
        else            //무기
     if (args.Message.Contains(syslevelartifact))
        {
            //Debug.Log("하이");
            args.Message = args.Message.Replace(syslevelartifact, "");
            //Debug.Log(args.Message);
            string text = string.Format(Inventory.GetTranslate("UI/5_유물강화 성공"), args.From.NickName, args.Message);
            //무기 드롭
            ShowChatSystemNameSystem(args.From.NickName, text);
            ShowDropAlertChat(text);
        }
        else            //무기 득템알람
        if (args.Message.Contains(sysdropartifact))
        {
            args.Message = args.Message.Replace(sysdropartifact, "");
           //  Debug.Log(args.Message);
            string text = string.Format(Inventory.GetTranslate("UI/5_유물획득"), args.From.NickName, args.Message);
            ShowChatSystemNameSystem(args.From.NickName, text);
            ShowDropAlertChat(text);
        }
        else            //진유물 제작
        if (args.Message.Contains(sysjinartifact))
        {
            args.Message = args.Message.Replace(sysjinartifact, "");
            //  Debug.Log(args.Message);
            string text = string.Format(Inventory.GetTranslate("UI/5_진유물성공"), args.From.NickName, args.Message);
            ShowChatSystemNameSystem(args.From.NickName, text);
            ShowDropAlertChat(text);
        }
        else if (args.Message.Contains(sysdropweapon))
        {
            args.Message = args.Message.Replace(sysdropweapon, "");
            //Debug.Log(args.Message);
            //string itemname = Inventory.GetTranslate(ItemdatabaseDB.Instance.Find_id(args.Message).name);
           // itemname =  Inventory.Instance.ChangeItemRareColor(itemname, ItemdatabaseDB.Instance.Find_id(args.Message).rare);

         //   string text = string.Format(Inventory.GetTranslate("UI/5_무기획득"), args.From.NickName, itemname);
            //무기 드롭
           // ShowChatSystemNameSystem(args.From.NickName, text);
           // ShowDropAlertChat(text);
        }
        else
        {
            //일반채팅
            Shownotifi();
            string[] chatdata = args.Message.Split('|');
            if(chatdata[0] != "")
            {
               // Debug.Log("dd");
                ShowChat(args.From.NickName, string.Format(publicstr, chatdata[0], args.From.NickName), chatdata[1]);
            }
            else
            {
               // Debug.Log("d");
                ShowChat(args.From.NickName, string.Format(publicstrnew, args.From.NickName), chatdata[1]);
            } 
        }


    };

    Backend.Chat.OnGuildChat = (ChatEventArgs args) =>
    {
        if (args.Message.Contains("<item="))
        {
           // Debug.Log("링크있다");
            //링크가 있다.
            string itemlink = GetMiddleString(args.Message, "<item=", ">");
            string[] hi = itemlink.Split('$');
            string[] data = hi[1].Split(';');
            string artifactname = "";
            //강화수치가없다면 
            if (data[data.Length - 1] == "0")
            {
              //  artifactname = Inventory.Instance.ChangeItemRareColor(Inventory.GetTranslate(ArtifactDataDB.Instance.Find_id(hi[0]).Name), ArtifactDataDB.Instance.Find_id(hi[0]).Rare);
            }
            else
            {
                //artifactname = Inventory.Instance.ChangeItemRareColor(string.Format("+{0} {1}", data[data.Length - 1], Inventory.GetTranslate(ArtifactDataDB.Instance.Find_id(hi[0]).Name)), ArtifactDataDB.Instance.Find_id(hi[0]).Rare);
            }

            Inventory.Instance.StringRemove();
            Inventory.Instance.StringWrite("[<link=itemlink");
            Inventory.Instance.StringWrite(itemlink);
            Inventory.Instance.StringWrite(">");
            Inventory.Instance.StringWrite(artifactname);
            Inventory.Instance.StringWrite("</link>]");
            args.Message = args.Message.Replace("<item=" + itemlink + ">", Inventory.Instance.StringEnd());
           // Debug.Log(args.Message);
        }


        ShowChatToguild(args.From.NickName, string.Format(guildstr, args.From.NickName), args.Message);
        Shownotifi();
    };

    //최근 채팅 불러오기
    Backend.Chat.OnRecentChatLogs = (RecentChatLogsEventArgs args) =>
    {
       // Debug.Log("최근 채팅" + args.Reason);
        for (int i = 0; i < args.LogInfos.Count; i++)
        {
          //  if (args.LogInfos[i].Message.Contains(sysdropweapon) || args.LogInfos[i].Message.Contains(sysdropartifact))
            //    continue;
            string chatdata = "";
            if (args.LogInfos[i].Message.Contains("<item="))
            {
                //Debug.Log("링크있다");
                //링크가 있다.
                string itemlink = GetMiddleString(args.LogInfos[i].Message, "<item=", ">");
                string[] hi = itemlink.Split('$');
                string[] data = hi[1].Split(';');
                string artifactname = "";
                //강화수치가없다면 
                if (data[data.Length - 1] == "0")
                {
                 //   artifactname = Inventory.Instance.ChangeItemRareColor(Inventory.GetTranslate(ArtifactDataDB.Instance.Find_id(hi[0]).Name), ArtifactDataDB.Instance.Find_id(hi[0]).Rare);
                }
                else
                {
                 //   artifactname = Inventory.Instance.ChangeItemRareColor(string.Format("+{0} {1}", data[data.Length - 1], Inventory.GetTranslate(ArtifactDataDB.Instance.Find_id(hi[0]).Name)), ArtifactDataDB.Instance.Find_id(hi[0]).Rare);
                }

                Inventory.Instance.StringRemove();
                Inventory.Instance.StringWrite("[<link=itemlink");
                Inventory.Instance.StringWrite(itemlink);
                Inventory.Instance.StringWrite(">");
                Inventory.Instance.StringWrite(artifactname);
                Inventory.Instance.StringWrite("</link>]");
                chatdata = args.LogInfos[i].Message.Replace("<item=" + itemlink + ">", Inventory.Instance.StringEnd());
                //Debug.Log(chatdata);
            }

            chatitemobj = Instantiate(chatitempool, ChatTrans);
            chatitemobj_ingame = Instantiate(chatitempool_ingame, ChatTrans_ingame) ; 
            chatitemobj.Chat(args.LogInfos[i].NickName, string.Format(publicstr, args.LogInfos[i].NickName), chatdata, false);
            chatitemobj_ingame.Chat(args.LogInfos[i].NickName, string.Format(publicstr, args.LogInfos[i].NickName), chatdata, false);
            LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
            LayoutRebuilder.ForceRebuildLayoutImmediate(rect_ingame);
            cont.SetLayoutVertical();
            cont_ingame.SetLayoutVertical();
            chatitemobj.gameObject.SetActive(true);
            chatitemobj_ingame.gameObject.SetActive(true);
        }

    };

    //채널에 입장 시 최초 한번, 해당 채널에 접속하고 있는 모든 게이머들의 정보 콜백
    Backend.Chat.OnSessionListInChannel = (SessionListInChannelEventArgs args) =>
    {
        par_nickname.Clear();
        nowcurusercount = args.SessionList.Count;
        nowmaxusercount = int.Parse(nowjoinedchat.maxUserCount);

        string[] chatname = nowjoinedchat.alias.Split('#');
        chatroomname.text =  string.Format("{0} {1}",Inventory.GetTranslate("UI/Channelname"),chatname[1]);
        nowpublicchatroomname = chatroomname.text;
        for (int i = 0; i < participanttext.Length; i++)
        {
            if (participanttext[i].gameObject.activeSelf)
                break;
            participanttext[i].gameObject.SetActive(false);

        }

        for (int i  =0; i < args.SessionList.Count;i++)
        {
            //Debug.Log(("접속유저" + args.SessionList[i].NickName));
                par_nickname.Add(args.SessionList[i].NickName);
                participanttext[i].text = args.SessionList[i].NickName;
                participanttext[i].gameObject.SetActive(true);
        }
        UserMemberChange(int.Parse(nowjoinedchat.joinedUserCount), int.Parse(nowjoinedchat.maxUserCount));
    };

    Backend.Chat.OnSessionListInGuildChannel = (SessionListInChannelEventArgs args) =>
    {
        par_nickname_guild.Clear();
        nowcurusercount_guild = args.SessionList.Count;
        nowmaxusercount_guild = int.Parse(nowjoinedchat_guild.maxUserCount);

        chatroomname.text = nowjoinedchat_guild.alias;
        for (int i = 0; i < participanttext.Length; i++)
        {
            if (participanttext[i].gameObject.activeSelf)
                break;
            participanttext[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < args.SessionList.Count; i++)
        {
                par_nickname_guild.Add(args.SessionList[i].NickName);
                participanttext[i].text = args.SessionList[i].NickName;
                participanttext[i].gameObject.SetActive(true);
        }
        chatmembercounttext.text = string.Format("{0}/{1}", par_nickname_guild.Count, nowjoinedchat.maxUserCount);
    };

    #region 실시간알림
    //실시간 알림ㄴ
    Backend.Notification.OnAuthorize = (bool Result, string Reason) =>
    {
      //  Debug.Log(Result);
     //   Debug.Log(Reason);
    };

    Backend.Notification.OnReceivedGuildApplicant = () => 
    {
     //   Alertmanager.Instance.AlertChat(Alertmanager.alertype.기본고급, Inventory.GetTranslate("UI2/길드가입대기중"));
    };

    Backend.Notification.OnApprovedGuildJoin = () => 
    {
        //길드가입 성공
       // MyGuildManager.Instance.RefreshGuildData();
        //Debug.Log("길드 수락 성공");
        //Alertmanager.Instance.AlertChat(Alertmanager.alertype.기본고급, Inventory.GetTranslate("UI2/길드 가입 성공"));
    };
    #endregion



    #region 퇴장관련
    //자기자신 혹은 다른게이머가 채팅 채널과 접속이 일시적으로 끊어진 경우
    Backend.Chat.OnSessionOfflineChannel = (SessionOfflineEventArgs args) =>
    {
        //  StartCoroutine(ShowText(Inventory.GetTranslate("UI/Systemname"), string.Format(Inventory.GetTranslate("UI/userchatexit"), args.Session.NickName)));
        if (!args.Session.IsRemote)
        {
            Logoutchat();

            for (int i = 0; i  < channellist.Count; i++)
            {
                if(channellist[i].getJoinUserCount() < 190)
                {
                    break;
                }
            }
        }

        UserMemberChange(0);
        if (par_nickname.Contains(args.Session.NickName))
        {
            int index = par_nickname.IndexOf(args.Session.NickName);
            par_nickname.RemoveAt(index);
        }
    };

    //자기자신 혹은 다른 게이머가 채널에서 퇴장한 경우
    Backend.Chat.OnLeaveChannel = (LeaveChannelEventArgs args) =>
    {

        if(!args.Session.IsRemote)
        {
          //  Debug.Log("BBBBBBB");
            Invoke("GetChannelList", 1);
        }
        UserMemberChange(0);
        //StartCoroutine(ShowText(Inventory.GetTranslate("UI/Systemname"), string.Format(Inventory.GetTranslate("UI/userchatexit"), args.Session.NickName)));
        if (par_nickname.Contains(args.Session.NickName))
        {
            int index = par_nickname.IndexOf(args.Session.NickName);
            par_nickname.RemoveAt(index);
        }
    };

    //자기자신 혹은 다른 게이머가 채널에서 퇴장한 경우
    Backend.Chat.OnLeaveGuildChannel = (LeaveChannelEventArgs args) =>
    {
        UserMemberChange_Guild(nowcurusercount_guild--);
       // StartCoroutine(ShowText(Inventory.GetTranslate("UI/Systemname"), string.Format(Inventory.GetTranslate("UI/userchatexit"), args.Session.NickName)));
        if (par_nickname_guild.Contains(args.Session.NickName))
        {
            int index = par_nickname_guild.IndexOf(args.Session.NickName);
            par_nickname_guild.RemoveAt(index);
        }

    };
    #endregion

    #region  접속관련
    //다른게이머가 채팅 채널에 재접속 한 경우
    Backend.Chat.OnSessionOnlineChannel = (SessionOnlineEventArgs args) =>
    {
        StartCoroutine(ShowText(Inventory.GetTranslate("UI/Systemname"), string.Format(Inventory.GetTranslate("UI/userchatjoin"), args.Session.NickName)));

        if (!par_nickname.Contains(args.Session.NickName))
        {
            par_nickname.Add(args.Session.NickName);
            //리프레시
        }
        if(!isguild)
        UserMemberChange(0);
    };

    //자기자신 혹은 다른 게이머가 채널에 입장한 경우, 자기자신이 채널에 재접속 한 경우
    Backend.Chat.OnJoinChannel = (JoinChannelEventArgs args) =>
    {
           // Debug.Log("입장 시도1");
        if (args.ErrInfo == ErrorInfo.Success)
        {
            bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
            //Debug.Log("입장 시도2" + isConnect);
            if (!isConnect)
            {
                GetChannelList();
            }
            else
            {
                if (!args.Session.IsRemote)
                    ShowChatSystemNameSystem("", string.Format(Inventory.GetTranslate("UI/userchatjoin"), args.Session.NickName));
            }
                //같은 닉네임이라면 패스
             //   if (args.Session.NickName.Equals(PlayerData.Instance.PlayerNameText.text))
               // UserMemberChange(0);
           // else
            //    UserMemberChange(0);
            //StartCoroutine(ShowText(Inventory.GetTranslate("UI/Systemname"), string.Format(Inventory.GetTranslate("UI/userchatjoin"), args.Session.NickName)));

            if (!par_nickname.Contains(args.Session.NickName))
            {
                par_nickname.Add(args.Session.NickName);
            }

        }
        else
        {
            //Debug.Log("입장 실패");
            //입장실패
        }

    };

    //자신 혹은 길드원이 접속시
    Backend.Chat.OnJoinGuildChannel = (JoinChannelEventArgs args) =>
    {
        UserMemberChange_Guild(nowcurusercount_guild++);
        StartCoroutine(ShowTextGuild(Inventory.GetTranslate("UI/Systemname"), string.Format(Inventory.GetTranslate("UI/userchatjoin"), args.Session.NickName)));
        if (!par_nickname_guild.Contains(args.Session.NickName))
        {
            par_nickname_guild.Add(args.Session.NickName);
        }

    };
    #endregion

}
[Button (Name = "Out")]
public void Logoutchat()
{
    Backend.Chat.ResetConnect();
}

WaitForSeconds wait = new WaitForSeconds(1f);
IEnumerator ShowText(string nick , string message)
{
    yield return SpriteManager.Instance.GetWaitforSecond(1f);
    ShowChatSystem(nick,message);
}


IEnumerator ShowTextGuild(string nick, string message)
{
    yield return wait;
    ShowChatToguild("",nick, message);
}
public void UserMemberChange(int count)
{
    chatmembercounttext.text = string.Format("{0}/{1}", par_nickname.Count, nowmaxusercount);
}

public void UserMemberChange_Guild(int count)
{
    chatmembercounttext.text = string.Format("{0}/{1}", par_nickname_guild.Count, nowmaxusercount_guild);
}

public void UserMemberChange(int curcount , int maxcount)
{
    chatmembercounttext.text = string.Format("{0}/{1}", curcount, maxcount);
    nowcurusercount = curcount;
    nowmaxusercount = maxcount;
}

#endregion

#region 채팅관련
public InputField chatinput_public;
public InputField chatinput_public_ingame;
public RectTransform chatinput_public_Rect;
public Transform ChatTrans;
public Transform ChatTrans_ingame;
public Transform ChatTrans_Guild;
public chatslot chatitempool; //채널 프리팹
public chatslot chatitemobj;

public chatslot chatitempool_ingame; //채널 프리팹
public chatslot chatitemobj_ingame;


public RectTransform rect;  //채널 새로고침용
public RectTransform rect_ingame;  //채널 새로고침용
public ContentSizeFitter cont; // 채널 새로고침용
public ContentSizeFitter cont_ingame; // 채널 새로고침용

//채팅이왔는데 채팅창이아니라면 알람
public GameObject Chatnotifi;
public Text Chatnotifitext;
int noticount;
public void Shownotifi()
{
    /*
    //타겟이 4가아니라면
    if (NestedScrollManager.Instance.nownum != (int)NestedScrollManager.Btenum.채팅)
    {
        if (!Chatnotifi.activeSelf)
            Chatnotifi.SetActive(true);

        noticount++;

        if (noticount > 100)
            noticount = 99;

        Chatnotifitext.text = noticount.ToString();
    }
    */
/*
    public void FinishNoti()
    {
        Chatnotifi.SetActive(false);
        noticount = 0;
        Chatnotifitext.text = "";
    }


    public void Bt_SendChat_Public()
    {
        if (chatinput_public.text == "")
            return;
        if(isguild)
        {
            Backend.Chat.ChatToChannel(ChannelType.Guild, chatinput_public.text);
        }
        else
        {
            try
            {
               Backend.Chat.ChatToChannel(ChannelType.Public, string.Format("{0}|{1}|{2}",GetUserVisualData(), 0, chatinput_public.text));

            }
            catch
            {
                GetChannelList();
            }
        }
        chatinput_public.text = "";
    }

    public void Bt_SendChat_Public_ingame()
    {
        if (chatinput_public_ingame.text == "")
            return;
            try
            {
               Backend.Chat.ChatToChannel(ChannelType.Public, string.Format("{0}|{1}|{2}",GetUserVisualData(), 0, chatinput_public_ingame.text));
        }
        catch
            {
                GetChannelList();
            }
        chatinput_public_ingame.text = "";
    }
    #endregion
    public GameObject Participantpanel;
    public void ShowPartcipant()
    {
        for(int i = 0; i < participanttext.Length;i++)
        {
            participanttext[i].gameObject.SetActive(false);
        }
        //채팅 참여자의 이름을 노출함
        if (isguild)
        {
            for (int i = 0; i < par_nickname_guild.Count; i++)
            {
                participanttext[i].text = par_nickname_guild[i];
                participanttext[i].gameObject.SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < par_nickname.Count; i++)
            {
                participanttext[i].text = par_nickname[i];
                participanttext[i].gameObject.SetActive(true);
            }
        }
        Participantpanel.SetActive(true);
    }

    #region 득템관련

    //유물 강화 관련
    public void ChatArtifactLevel(string artifactid,string stat)
    {
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);

        if (isConnect)
        {
            string text = string.Format("{0}{1}", syslevelartifact, string.Format(itemlinkformat, artifactid, stat));
            Backend.Chat.ChatToChannel(ChannelType.Public, text);
        }
        else
        {
            GetChannelList();
        }
    }

    public void ChatRareDropWeapon(string itemid)
    {
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        try
        {
            if (isConnect)
            {
                string text = string.Format("{0}{1}", sysdropweapon, itemid);
                Backend.Chat.ChatToChannel(ChannelType.Public, text);
            }

            else
            {
                GetChannelList();
            }
        }
        catch
        {
            GetChannelList();
        }

    }
    public void ChatRareMakeArtifact(string itemid, string stat)
    {
        try
        {

            bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
            if (isConnect)
            {
                string text = string.Format("{0}{1}", sysjinartifact, string.Format(itemlinkformat, itemid, stat));
                Backend.Chat.ChatToChannel(ChannelType.Public, text);
            }
            else
            {
                GetChannelList();
            }
        }
        catch
        {

        }
    }
    public void ChatRareDropArtifact(string itemid,string stat)
    {
        try
        {

        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        if (isConnect)
        {
            string text = string.Format("{0}{1}", sysdropartifact, string.Format(itemlinkformat, itemid, stat));
            Backend.Chat.ChatToChannel(ChannelType.Public, text);
        }
        else
        {
            GetChannelList();
        }
        }
        catch
        {

        }
    }
    //길드레이드 성공
    public void ChatGuildRaidSucc(string Guildname,string mapid)
    {
        bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
        if (isConnect)
        {
            string text = string.Format("{0}{1};{2}", sysguildraidfinish, Guildname, mapid);
            Backend.Chat.ChatToChannel(ChannelType.Public, text);
        }
        else
        {
            GetChannelList();
        }

    }

    string itemlinkformat = "<item={0}${1}>";
    public void ArtifactLink(string artifactid,string stat)
    {
        //Thrower 유저가 펫 상자에서 이거이거를 획득 하셨습니다. 축하합니다.!
        //Debug.Log(stat);
       // Debug.Log(artifactid);
        if (isguild)
        {
            bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Guild);
            if (isConnect)
            {
                Backend.Chat.ChatToChannel(ChannelType.Guild, string.Format(itemlinkformat, artifactid, stat));
            }
            else
            {
                JoinGuildChannel();
            }
        }
        else
        {
            bool isConnect = Backend.Chat.IsChatConnect(ChannelType.Public);
            if (isConnect)
            {
               // Debug.Log("gkgk");
                Backend.Chat.ChatToChannel(ChannelType.Public, string.Format(itemlinkformat, artifactid, stat));
            }
            else
            {
                GetChannelList();
            }
        }
    }
    #endregion

    #region 채널관련
    public Text chatroomname;
    public GameObject Channelpanel;
    public Transform channellisttrans;
    public channellistaslot poolobj;
    public List<channellistaslot> listchannelobj = new List<channellistaslot>();
    int minpoolcount_channel = 30;
    void PoolChannelList()
    {
        for (int i = 0; i < minpoolcount_channel; i ++)
        {
            listchannelobj.Add(Instantiate(poolobj, channellisttrans));
            listchannelobj[i].gameObject.SetActive(false);
        }
    }
    //채널을 보여준다
    public void BtShowChannel()
    {
       // Debug.Log("채널소환");
        //일반채널
        channellist.Clear();
        BackendReturnObject bro = Backend.Chat.GetGroupChannelList("Global");
        JsonData channeldata = bro.Rows();

       // Debug.Log("개수는 " + channeldata.Count);
        for (int i = 0; i < channeldata.Count; i++)
        {
            channellist.Add(new ChatChannel(channeldata[i]["inDate"].ToString(),
               channeldata[i]["alias"].ToString(),
               channeldata[i]["serverAddress"].ToString(),
               ushort.Parse(channeldata[i]["serverPort"].ToString()),
               channeldata[i]["joinedUserCount"].ToString(),
               channeldata[i]["maxUserCount"].ToString()));
        }

        for (int i = 0; i < listchannelobj.Count; i++)
        {
            listchannelobj[i].gameObject.SetActive(false);
        }
        //Debug.Log("개수는 " + channellist.Count);
        for (int i = 0; i < channellist.Count; i++)
        {
            if (channellist.Count > listchannelobj.Count)
                PoolChannelList();
           // Debug.Log(i);
            //Debug.Log(i);
            //Debug.Log(i);
            if (channellist[i].inDate == (nowjoinedchat.inDate))
                UserMemberChange(int.Parse(channellist[i].joinedUserCount), int.Parse(channellist[i].maxUserCount));
            listchannelobj[i].Channel(channellist[i], string.Format("{0} {1}", Inventory.GetTranslate("UI/Channelname"), i + 1), string.Format("{0}/{1}", channellist[i].joinedUserCount, channellist[i].maxUserCount));
            listchannelobj[i].gameObject.SetActive(true);
        }
        Channelpanel.SetActive(true);
    }
    bool isrecent = false;
    public void GetChannelList()
    {
        //채널리스트 초기화
        channellist.Clear();
        
        BackendReturnObject bro = Backend.Chat.GetGroupChannelList("Global");
       // Debug.Log(bro);
        JsonData channeldata = bro.Rows();
        for (int i = 0; i < channeldata.Count; i++)
        {
            channellist.Add(new ChatChannel(channeldata[i]["inDate"].ToString(),
               channeldata[i]["alias"].ToString(),
               channeldata[i]["serverAddress"].ToString(),
               ushort.Parse(channeldata[i]["serverPort"].ToString()),
               channeldata[i]["joinedUserCount"].ToString(),
               channeldata[i]["maxUserCount"].ToString()));
        }

        ErrorInfo errorInfo;
        for (int i = 0; i < channellist.Count; i++)
        {
            //-10은 10명은 여분자리로 남긴거
            if (int.Parse(channellist[i].maxUserCount)-10 > int.Parse(channellist[i].joinedUserCount))
            {
                //Debug.Log("접속중");
                if(!isrecent)
                ShowRecentMessage(channellist[i]); //최시대화불러오기
                //Debug.Log("접속중");
                JoinChannel(channellist[i]);
                nowjoinedchat = channellist[i];
                break;
            }
        }
    }


    public void JoinChannel(ChatChannel chat)
    {
        if(isguild)
        {
            ErrorInfo errorInfo;
            Backend.Chat.JoinChannel(ChannelType.Guild, chat.serverAddress, chat.serverPort, "Global", chat.inDate, out errorInfo);
            nowjoinedchat_guild = chat;
        }
        else
        {
            ErrorInfo errorInfo;
            Backend.Chat.JoinChannel(ChannelType.Public, chat.serverAddress, chat.serverPort, "Global", chat.inDate, out errorInfo);
            nowjoinedchat = chat;
        }
    }

    public void JoinChannel_Public(ChatChannel chat)
    {
            ErrorInfo errorInfo;
            Backend.Chat.JoinChannel(ChannelType.Public, chat.serverAddress, chat.serverPort, "Global", chat.inDate, out errorInfo);
            nowjoinedchat = chat;
    }

    //최근 대화 불러오기
    public void ShowRecentMessage(ChatChannel channel)
    {
        isrecent = true;
        if(isguild)
        {
            BackendReturnObject result = Backend.Chat.GetRecentChat(ChannelType.Guild, channel.inDate, 25);
            for (int i = 0; i < result.Rows().Count; i++)
            {
                string nickname = result.Rows()[i]["nickname"].ToString();
                string message = result.Rows()[i]["message"].ToString();
                Debug.Log("챗");
                Debug.Log(nickname);
                ShowText(nickname, message);
            }
        }
        else
        {
            BackendReturnObject result = Backend.Chat.GetRecentChat(ChannelType.Public, channel.inDate, 25);
            for (int i = 0; i < result.Rows().Count; i++)
            {
                //아이템링크
                //  Debug.Log(args.Message);
                string nickname = result.Rows()[i]["nickname"].ToString();
                string message = result.Rows()[i]["message"].ToString();
                if (message.Contains("<item="))
                {
                    //    Debug.Log("링크있다");
                    //링크가 있다.
                    string itemlink = GetMiddleString(message, "<item=", ">");
                    string[] hi = itemlink.Split('$');
                    string[] data = hi[1].Split(';');
                    string artifactname = "";
                    //강화수치가없다면 
                    if (data[data.Length - 1] == "0")
                    {
                //       artifactname = Inventory.Instance.ChangeItemRareColor(Inventory.GetTranslate(ArtifactDataDB.Instance.Find_id(hi[0]).Name), ArtifactDataDB.Instance.Find_id(hi[0]).Rare);
                    }
                    else
                    {
                   //     artifactname = Inventory.Instance.ChangeItemRareColor(string.Format("+{0} {1}", data[data.Length - 1], Inventory.GetTranslate(ArtifactDataDB.Instance.Find_id(hi[0]).Name)), ArtifactDataDB.Instance.Find_id(hi[0]).Rare);
                    }

                    Inventory.Instance.StringRemove();
                    Inventory.Instance.StringWrite("[<link=itemlink");
                    Inventory.Instance.StringWrite(itemlink);
                    Inventory.Instance.StringWrite(">");
                    Inventory.Instance.StringWrite(artifactname);
                    Inventory.Instance.StringWrite("</link>]");
                    message = message.Replace("<item=" + itemlink + ">", Inventory.Instance.StringEnd());
                    //Debug.Log(message);
                    if (!message.Contains(sysdropartifact) && !message.Contains(syslevelartifact) && !message.Contains(sysjinartifact))
                    {
                        ShowChat(nickname, string.Format(publicstrnew, nickname), message);
                    }
                    continue;
                }



                //길드레이드 완료
                if (message.Contains(sysguildraidfinish))
                {
                    message = message.Replace(sysguildraidfinish, "");
                    string[] data = message.Split(';'); //1은길드이름 2는 길드난이도

                    //string text = string.Format(Inventory.GetTranslate("Guild/길드레이드보상획득"), data[0], Inventory.GetTranslate(MapDB.Instance.Find_id(data[1]).mapname));
                    //무기 드롭
                   // ShowChatSystemNameSystem(nickname, text);
                    //ShowDropAlertChat(text);
                }
                else            //무기
             if (message.Contains(syslevelartifact))
                {
                    //Debug.Log("하이");
                    message = message.Replace(syslevelartifact, "");
                    //Debug.Log(message);
                    string text = string.Format(Inventory.GetTranslate("UI/5_유물강화 성공"), nickname, message);
                    //무기 드롭
                    ShowChatSystemNameSystem(nickname, text);
                    ShowDropAlertChat(text);
                }
                else            //무기 득템알람
                if (message.Contains(sysdropartifact))
                {
                    message = message.Replace(sysdropartifact, "");
                    //  Debug.Log(message);
                    string text = string.Format(Inventory.GetTranslate("UI/5_유물획득"), nickname, message);
                    ShowChatSystemNameSystem(nickname, text);
                    ShowDropAlertChat(text);
                }
                else            //진유물 제작
                if (message.Contains(sysjinartifact))
                {
                    message = message.Replace(sysjinartifact, "");
                    //  Debug.Log(message);
                    string text = string.Format(Inventory.GetTranslate("UI/5_진유물성공"), nickname, message);
                    ShowChatSystemNameSystem(nickname, text);
                    ShowDropAlertChat(text);
                }
                else if (message.Contains(sysdropweapon))
                {
                    message = message.Replace(sysdropweapon, "");
                    //Debug.Log(message);
                  //  string itemname = Inventory.GetTranslate(ItemdatabaseDB.Instance.Find_id(message).name);
                  //  itemname = Inventory.Instance.ChangeItemRareColor(itemname, ItemdatabaseDB.Instance.Find_id(message).rare);

                  //  string text = string.Format(Inventory.GetTranslate("UI/5_무기획득"), nickname, itemname);
                    //무기 드롭
                  //  ShowChatSystemNameSystem(nickname, text);
                //    ShowDropAlertChat(text);
                }
                else
                {
                    //일반채팅
                    //Shownotifi();
                    string[] chatdata = message.Split('|');
                    Debug.Log(message);
                    if (message.Contains("|"))
                    {
                        ShowChat(nickname, string.Format(publicstr, chatdata[0], nickname), chatdata[1]);
                    }
                    else
                    {
                        ShowChat(nickname, string.Format(publicstrnew, nickname), chatdata[1]);
                    }
                }

            }
        }



   
    }

    #endregion}
*/

public class ChatChannel
{
   public string inDate;// 채널 uuid
    public string alias;//: "projectName#1",  // 채널 별칭 ({프로젝트 이름}#{auto increment value})
    public string registrationDate; //: "2018.12.26 04:42:06.010Z", // 채널 생성일시 (utc)
    public string serverAddress; //: "ec2-54-180-93-125.ap-northeast-2.compute.amazonaws.com", // 채널 서버 host name
    public ushort serverPort;//: 50000;// 채널 서버 port
    public string joinedUserCount; // 채널에 입장한 현재 유저 수
    public string maxUserCount;// 채널에 입장할 수 있는 유저 수


    public ChatChannel(string inDate, string alias, string serverAddress, ushort serverPort, string joinedUserCount, string maxUserCount)
    {
        this.inDate = inDate;
        this.alias = alias;
        this.serverAddress = serverAddress;
        this.serverPort = serverPort;
        this.joinedUserCount = joinedUserCount;
        this.maxUserCount = maxUserCount;
    }

   public int getJoinUserCount()
    {
        return int.Parse(joinedUserCount);
    }
}
