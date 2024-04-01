using System;
using BackEnd;
using LitJson;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEditor;

public class BackendFerderationAuth : MonoBehaviour
{
    //싱글톤만들기.
    private static BackendFerderationAuth _instance = null;
    public static BackendFerderationAuth Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(BackendFerderationAuth)) as BackendFerderationAuth;

                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }
            return _instance;
        }
    }

    private void OnDestroy()
    {
        _instance = null;
    }

    PlayerBackendData player;
    public GameObject obj; 
   

    public static bool isgoole;     
    public Text VerText;
    private void Start()
    {
        obj.SetActive(true);
        player = PlayerBackendData.Instance;
        //debuginput.text = Backend.Utils.GetGoogleHash();
        idpass[0].text = PlayerPrefs.GetString("id");
        idpass[1].text = PlayerPrefs.GetString("pass");
      //  hash.text = Backend.Utils.GetGoogleHash();
        VerText.text = $"Ver.{Application.version}";
    }

    public GameObject LoginPanel; //업데이트가 완료되면 보임

    public void InitLogin()
    {
        LoginPanel.SetActive(true);
    }
   

    public void ChangeLoc(int num)
    {
        switch (num)
        {
            case 0:
                break;
            case 1:

                break;
        }
    }

    public void StartGoogle()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            Invoke(nameof(InitGoogle), 2f);
        }

    }
 
    string[] select =
    {
        "level",
        "levelExp",
        "PlayerSaveTime",
        "PlayerCanLoadBool",
        "PlayerLoadUID",
        "PlayerLoadModel",
        "PlayerLoadTime",
        "SpeedHackOn",
        "AntcaveLv",
        "AntTotalClear",
        "Playertime",
        "ContentLevel",
        "nowpresetnumber",
        "PresetData",
        "SeasonPassPremium",
        "SeasonPassNum",
        "SeasonPassExp",
        "SeasonPassBasicReward",
        "SeasonPassPremiumReward",
        "CollectionNew",
        "sotang_raid",
        "sotang_dungeon",
        "PlayerShopTimes",
        "PlayerShopTimesbuys",
        "SaveNum",
        "PetData",
        "PetCount",
        "nowPetid",
        "SettingData",
        "OfflineData"
    };
    

    public void GetAndUpdateUserScore()
    {
        BackendReturnObject callback = Backend.GameData.Get("Character", new Where());
        if (callback.IsSuccess() == false)
        {
            // Debug.LogError("게임 정보 조회 실패: " + callback);
            return;
        }


        var data = callback.FlattenRows();

        if (data.Count < 0)
        {
            Debug.LogError("게임정보가 비어있습니다.");
            return;
        }

        var indate = data[0]["inDate"].ToString();

    }


    public TMP_InputField[] idpass;
    public TMP_InputField hash;
    public const int initbagcount = 50;


    public void InitGoogle()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration
           .Builder()
           .RequestServerAuthCode(false)
           .RequestEmail()
           .RequestIdToken()
           .Build();
        //커스텀된 정보로 초기화
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = false;
        //시작
        PlayGamesPlatform.Activate();
        InvokeRepeating(nameof(Bt_LoginGoogle), 1f, 1f);
        //GetComponent<BackendLogin>().LoginButtons[1].SetActive(true);
    }

    public GameObject BlockPanel;
    public GameObject RemovingPanel;
    public Text Blockinfo;
    public Text Removinginfo;
    public void GoogleAuth()
    {
        if (PlayGamesPlatform.Instance.localUser.authenticated == false)
        {
            Social.localUser.Authenticate(success =>
            {
                if (success == false)
                {
                    Invoke(nameof(GoogleAuth),1f);
                    return;
                }
                Debug.Log("GetIdToken - " + PlayGamesPlatform.Instance.GetIdToken());
                Debug.Log("Email - " + ((PlayGamesLocalUser)Social.localUser).Email);
                Debug.Log("GoogleID - " + Social.localUser.id);
                Debug.Log("UserName - " + Social.localUser.userName);
                Debug.Log("UserName - " + PlayGamesPlatform.Instance.GetUserDisplayName());
                Bt_LoginGoogle();
            });
        }
    }

    public GameObject NotThisDeviceObj;
    public Text ModelText;
    public GameObject FileDifferentobj;
    private JsonData jdata;
    
    public void MakeID()
    {
        string testid = idpass[0].text;
        string testpw = idpass[1].text;
        Backend.BMember.CustomSignUp(testid, testpw);

        PlayerPrefs.SetString("id", idpass[0].text);
        PlayerPrefs.SetString("pass", idpass[1].text);

      //  Debug.Log("응애");
        
        BackendReturnObject bro = Backend.BMember.CustomLogin(testid, testpw, "비번:" + idpass[1].text);
 Debug.Log(bro);
        if (bro.GetStatusCode() == "403")
        {
            //차단유
            BlockPanel.SetActive(true);
            Blockinfo.text = bro.GetErrorCode();
            return;
        }
        if (bro.GetStatusCode() == "410")
        {
            //차단유
            RemovingPanel.SetActive(true);
            DateTime now = DateTime.Now;
            int time = 60 - now.Minute;
            Removinginfo.text = string.Format(Inventory.GetTranslate("UI4/현재 삭제중"), time.ToString());
            return;
        }
        if (bro.IsSuccess())
        {
//            Debug.Log("응애");

            bro = Backend.BMember.GetUserInfo();
           // Debug.Log("응애");

            //Debug.Log(bro);
            if (bro.IsSuccess())
            {
              //  Debug.Log("응애");

                JsonData data = bro.GetReturnValuetoJSON()["row"]["nickname"];
//                Debug.Log("ㅇ");

                PlayerBackendData.Instance.playerindate = bro.GetReturnValuetoJSON()["row"]["inDate"].ToString();
                //Debug.Log("응애");
                Savemanager.Instance.Init();
                Timemanager.Instance.TimeInit();
         //       Debug.Log("ㅇ");

                if (data == null)
                {
                    PlayerBackendData.Instance.Id = bro.GetInDate();
                    GetComponent<BackendNickname>().Nickname_logoPanel.SetActive(true);
                }
                else
                {
                    PlayerBackendData.Instance.Id = bro.GetInDate();
                    player.nickname = bro.GetReturnValuetoJSON()["row"]["nickname"].ToString();
                    Savemanager.Instance.Init();

                    LoadPlayerData();
                    if (!Savemanager.Instance.LoadCheckSaveTime())
                    {
                        //다르게 불러온 데이터이다.
                        //데이터가 다른다.
                        Debug.Log("시간 데이터가 다르다");
                        FileDifferentobj.SetActive(true);
                        return;
                    }
                    
//                    Debug.Log("ㅇ");

                    //데이터를 받아온다.
                    Where where1 = new Where();
                    where1.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);

                    var broj = Backend.GameData.GetMyData("PlayerData", where1,select, 1);
                    jdata = null;
                    if (broj.IsSuccess())
                    {
                        Debug.Log("플레이어 데이터" + broj);

                        if (broj.GetReturnValuetoJSON()["rows"].Count == 0)
                        {
                            Debug.Log("개수는 0개 캐릭터를 만들자");
                            GetComponent<BackendLogin>().StartClassSetting();
                            return;
                        }
                        
                        jdata = broj.FlattenRows()[0];
                    }
                    if (jdata != null)
                    {
                        Debug.Log("레벨 데이터" + jdata["level"].ToString());
                        if (jdata.ContainsKey("SaveNum"))
                        {
                            PlayerBackendData.Instance.ServerLv = int.Parse(jdata["level"].ToString());
                            PlayerBackendData.Instance.ServerEXP = decimal.Parse(jdata["levelExp"].ToString());
                            PlayerBackendData.Instance.ServerSaveNum = int.Parse(jdata["SaveNum"].ToString());

                            
                            Debug.Log("서버LV " + PlayerBackendData.Instance.ServerLv);
                            Debug.Log("서버EXP " + PlayerBackendData.Instance.ServerEXP);
                            Debug.Log("서버SaveNum " + PlayerBackendData.Instance.ServerSaveNum);
                            Debug.Log("클라lV " + PlayerBackendData.Instance.GetLv());
                            Debug.Log("클라EXP " + PlayerBackendData.Instance.GetExp());
                            Debug.Log("클라SaveNum " + PlayerBackendData.Instance.ClientSaveNum);
                            //레벨이 서버가 더 높으면 삭제
                            if (PlayerBackendData.Instance.ServerSaveNum > PlayerBackendData.Instance.ClientSaveNum)
                            {
                                //레벨이 서버가 더 낮으면 말이안된다 서버는 가끔이고 로컬은 실시간이기에
                                Debug.Log("서버데이터가 더 높아 데이터 삭제해야함");
                                ES3.DeleteFile(Savemanager.Instance.GetFileName());
                                GetComponent<BackendLogin>().RefreshServerSavedData(jdata);
                                return;
                            }
                        }
                    }
                    if (jdata != null)
                    {
//                        Debug.Log(jdata.ContainsKey("SpeedHackOn"));
                        if(jdata.ContainsKey("SpeedHackOn"))
                        {
                            Debug.Log("차단" +jdata["SpeedHackOn"].ToString() );
                            if (jdata["SpeedHackOn"].ToString() == "True")
                            {
                                BlockPanel.SetActive(true);
                                Blockinfo.text = "Speed Hack";
                                return;
                            }
                            //스피드핵감지
                        }
                    }


                    if (jdata != null && !jdata.ContainsKey("PlayerCanLoadBool"))
                    {
                        GetComponent<BackendLogin>().StartClassSetting();
                        return;
                    }
                    
                    //개발자가 불러오기를 뜨게함
                    if (jdata["PlayerCanLoadBool"].ToString() == "trues")
                    {
                        //ES3.DeleteFile(Savemanager.Instance.GetFileName());
                        GetComponent<BackendLogin>().RefreshServerSavedData(jdata);
                        return;
                    }
                    
                    if (jdata.ContainsKey("PlayerLoadUID"))
                    {
                        if (jdata["PlayerLoadUID"].ToString() == SystemInfo.deviceUniqueIdentifier)
                        {
                            Debug.Log("같은 계정이다.");
                        }
                        else
                        {
                            NotThisDeviceObj.SetActive(true);
                            ModelText.text = string.Format(TranslateManager.Instance.GetTranslate("UI2/모델명"),
                                jdata["PlayerLoadModel"].ToString());
                            Debug.Log("다른 계정이다.");
                            return;
                        }
                    }
                    //접속 시 현재불러온시간과 서버 불러온 시간을 비교
                    //비교해서 다른데 서버불러오기가 true라면 
                    if (jdata.ContainsKey("PlayerLoadTime"))
                    {
                        if (PlayerBackendData.Instance.playersaveindate != "")
                        {
                            if (PlayerBackendData.Instance.playersaveindate != jdata["PlayerLoadTime"].ToString()
                                && jdata["PlayerCanLoadBool"].ToString() == "true")
                            {
                                Debug.Log("시간이 다르다");
                                GetComponent<BackendLogin>().RefreshServerSavedData(jdata);
                                return;
                            }
                        }
                    }
                    
                    //개미굴횟수가져옴
                    if (jdata.ContainsKey("AntTotalClear"))
                    {
                        Debug.Log("개미굴 총 횟수 가져옴" + jdata["AntTotalClear"].ToString());
                        PlayerBackendData.Instance.AntTotalClear = int.Parse(jdata["AntTotalClear"].ToString());
                                
                    }
                    
//                    Debug.Log(PlayerBackendData.Instance.ClassId);
                    if (PlayerBackendData.Instance.ClassId == "0" && jdata != null)
                    { 
                        //서버데이터는 있는데 //저장이없다 
                        GetComponent<BackendLogin>().RefreshServerSavedData(jdata);
                    }
                    else if (PlayerBackendData.Instance.ClassId == "0")
                    {
                        GetComponent<BackendLogin>().StartClassSetting();
                    }
                    else
                    {
                        GetRankingData();
                        GetComponent<BackendLogin>().StartLobby();
                    }
                }
            }

        }
    }

    public void GetRankingData()
    {
        //데이터를 받아온다.
        Where where1 = new Where();
        where1.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);

        string[] selects =
        {
            "AntcaveLv"
        };

        var broj = Backend.GameData.GetMyData("RankData", where1,selects, 1);
        JsonData jdatas = null;
        if (broj.IsSuccess())
        {
//            Debug.Log("랭킹데이터 불렁모" + broj);

            if (broj.GetReturnValuetoJSON()["rows"].Count == 0)
            {
               return;
            }
                        
            jdatas = broj.FlattenRows()[0];
            
            if (jdatas.ContainsKey("AntcaveLv"))
            {
//                Debug.Log("개미굴랭킹가져옴"+jdatas["AntcaveLv"].ToString());
                
                PlayerBackendData.Instance.AntCaveLv = int.Parse(jdatas["AntcaveLv"].ToString());
            }
        }
        
        if (jdata.ContainsKey("Playertime"))
        {
            for (int i = 0; i < jdata["Playertime"].Count; i++)
            {
                PlayerBackendData.Instance.PlayerTimes[i] = DateTime.Parse(jdata["Playertime"][i].ToString());
            }
        }
        
        
        if (jdata.ContainsKey("ContentLevel"))
        {
            for (int i = 0; i < jdata["ContentLevel"].Count; i++)
            {
                PlayerBackendData.Instance.ContentLevel[i] = int.Parse(jdata["ContentLevel"][i].ToString());
            }
        }
        if (jdata.ContainsKey("nowpresetnumber"))
        {
                PlayerBackendData.Instance.nowpresetnumber = int.Parse(jdata["nowpresetnumber"].ToString());
        }
        
        if (jdata.ContainsKey("CollectionNew"))
        {
            //클래스 데이터
            for(int i = 0 ; i < jdata["CollectionNew"].Count;i++)
            {
                //  Debug.Log("직업입력" + key);
                PlayerBackendData.Instance.RenewalCollectData[i] 
                    = bool.Parse(jdata["CollectionNew"][i].ToString());
            }
        }
        
        if (jdata.ContainsKey("PetData"))
        {
            //클래스 데이터
            foreach (string key in jdata["PetData"].Keys)
            {
//                Debug.Log("펫 불러옴" + jdata["PetData"][key].Count);
                PlayerBackendData.Instance.PetData[key] = new petdatabase(jdata["PetData"][key]);
            }

            for (int i = 0; i < jdata["PetCount"].Count; i++)
            {
                PlayerBackendData.Instance.PetCount[i] = int.Parse(jdata["PetCount"][i].ToString());
            }
        }
        if (jdata.ContainsKey("nowPetid"))
        {
            PlayerBackendData.Instance.nowPetid = jdata["nowPetid"].ToString();
        }

        if (jdata.ContainsKey("OfflineData"))
        {
            PlayerBackendData.Instance.Offlinedata = new OfflineData();
            PlayerBackendData.Instance.Offlinedata.mapid = jdata["OfflineData"]["mapid"].ToString();
            PlayerBackendData.Instance.Offlinedata.level = int.Parse(jdata["OfflineData"]["level"].ToString());
            PlayerBackendData.Instance.Offlinedata.time = int.Parse(jdata["OfflineData"]["time"].ToString());
        }

        if (jdata.ContainsKey("SettingData"))
        {
            if (jdata["SettingData"].ContainsKey("Backsound"))
            {
                //사운드
                PlayerBackendData.Instance.settingdata.Backsound = float.Parse(jdata["SettingData"]["Backsound"].ToString());
                PlayerBackendData.Instance.settingdata.Effectsound = float.Parse(jdata["SettingData"]["Effectsound"].ToString());
                
                
                PlayerBackendData.Instance.settingdata.ButtonSound = int.Parse(jdata["SettingData"]["ButtonSound"].ToString());
//                Debug.Log("버튼사운드는" + PlayerBackendData.Instance.settingdata.ButtonSound );
                //피해량 스킬 이펙트
                PlayerBackendData.Instance.settingdata.DmgCountNum = int.Parse(jdata["SettingData"]["DmgCountNum"].ToString());
                PlayerBackendData.Instance.settingdata.DmgShowNum = int.Parse(jdata["SettingData"]["DmgShowNum"].ToString());
                PlayerBackendData.Instance.settingdata.EffectColor = int.Parse(jdata["SettingData"]["EffectColor"].ToString());
                PlayerBackendData.Instance.settingdata.EffectShow = int.Parse(jdata["SettingData"]["EffectShow"].ToString());
                //아이템 표기
                PlayerBackendData.Instance.settingdata.ItemPanel = int.Parse(jdata["SettingData"]["ItemPanel"].ToString());
                PlayerBackendData.Instance.settingdata.ItemDrop = int.Parse(jdata["SettingData"]["ItemDrop"].ToString());
                PlayerBackendData.Instance.settingdata.EskillPanel = int.Parse(jdata["SettingData"]["EskillPanel"].ToString());
                PlayerBackendData.Instance.settingdata.SystemChat = int.Parse(jdata["SettingData"]["SystemChat"].ToString());
                //물약
                PlayerBackendData.Instance.settingdata.Hp = float.Parse(jdata["SettingData"]["Hp"].ToString());
                PlayerBackendData.Instance.settingdata.Mp = float.Parse(jdata["SettingData"]["Mp"].ToString());
            }
        }
        
        //소탕 정보
        if (jdata.ContainsKey("sotang_raid"))
        {
            //클래스 데이터
            PlayerBackendData.Instance.sotang_raid.Clear();
            for (int i = 0; i < jdata["sotang_raid"].Count; i++)
            {
                //  Debug.Log("직업입력" + key);
                if (!PlayerBackendData.Instance.sotang_raid.Contains(jdata["sotang_raid"][i].ToString()))
                    PlayerBackendData.Instance.sotang_raid.Add(jdata["sotang_raid"][i].ToString());
            }
        }

        //소탕 정보
        if (jdata.ContainsKey("sotang_dungeon"))
        {
            //클래스 데이터
            PlayerBackendData.Instance.sotang_dungeon.Clear();

            for (int i = 0; i < jdata["sotang_dungeon"].Count; i++)
            {
                if (!PlayerBackendData.Instance.sotang_dungeon.Contains(jdata["sotang_dungeon"][i].ToString()))
                    PlayerBackendData.Instance.sotang_dungeon.Add(jdata["sotang_dungeon"][i].ToString());
            }
        }

        //소탕 정보
        if (jdata.ContainsKey("PlayerShopTimesbuys"))
        {
            //클래스 데이터
            for(int i = 0 ; i < jdata["PlayerShopTimes"].Count;i++)
            {
                //  Debug.Log("직업입력" + key);
                PlayerBackendData.Instance.PlayerShopTimes[i] = DateTime.Parse(jdata["PlayerShopTimes"][i].ToString());
                PlayerBackendData.Instance.PlayerShopTimesbuys[i] = bool.Parse(jdata["PlayerShopTimesbuys"][i].ToString());
                
            }
        }
        
        if (jdata.ContainsKey("PresetData"))
        {
//            Debug.Log("프리셋 불러오자");
            for (int i = 0; i < jdata["PresetData"].Count; i++)
            {
                PresetItem temp = new PresetItem();

                temp.presetname = jdata["PresetData"][i]["presetname"].ToString();

                //스킬
                for (int o = 0; o < jdata["PresetData"][i]["Skills"].Count; o++)
                {
                    temp.Skills[o] = jdata["PresetData"][i]["Skills"][o].ToString();
                }
                //장비
                for (int o = 0; o < jdata["PresetData"][i]["EquipDatas"].Count; o++)
                {
                    temp.EquipDatas[o] = jdata["PresetData"][i]["EquipDatas"][o].ToString();
                }
                //패시브
                for (int o = 0; o < jdata["PresetData"][i]["Passive"].Count; o++)
                {
                    temp.Passive[o] = jdata["PresetData"][i]["Passive"][o].ToString();
                }

                if (jdata["PresetData"][i].ContainsKey("Ability"))
                {
                    
                    //패시브
                    for (int o = 0; o < jdata["PresetData"][i]["Ability"].Count; o++)
                    {
                        temp.Ability[o] = jdata["PresetData"][i]["Ability"][o].ToString();
                    }
                }
                
                if (jdata["PresetData"][i].ContainsKey("PetID"))
                {
                    temp.PetID = jdata["PresetData"][i]["PetID"].ToString();
                }
                
                temp.Classid = jdata["PresetData"][i]["Classid"].ToString();
                PlayerBackendData.Instance.Presets[i] = temp;
            }
            PlayerBackendData.Instance.isloadpreset = true;
        }

        if (jdata.ContainsKey("SeasonPassPremium"))
        {
//            Debug.Log("패스 불러옴");
            for (int i = 0; i < jdata["SeasonPassBasicReward"].Count; i++)
            {
                PlayerBackendData.Instance.SeasonPassBasicReward[i] =
                    bool.Parse(jdata["SeasonPassBasicReward"][i].ToString());
            }

            for (int i = 0; i < jdata["SeasonPassPremiumReward"].Count; i++)
            {
                PlayerBackendData.Instance.SeasonPassPremiumReward[i] =
                    bool.Parse(jdata["SeasonPassPremiumReward"][i].ToString());
            }

            PlayerBackendData.Instance.SeasonPassPremium = bool.Parse(jdata["SeasonPassPremium"].ToString());
            PlayerBackendData.Instance.SeasonPassExp = decimal.Parse(jdata["SeasonPassExp"].ToString());
        }
        
        if (jdata.ContainsKey("SeasonPassNum"))
        {
//            Debug.Log("패스 불러옴2");

            PlayerBackendData.Instance.SeasonPassNum = int.Parse(jdata["SeasonPassNum"].ToString());
        }
        
        /*
        if (jdata.ContainsKey("Roulette"))
        {
            Debug.Log("룰렛가져옴");
            for (int i = 0; i < jdata["Roulette"].Count; i++)
            {
                PlayerBackendData.Instance.RouletteCount[i] =
                    int.Parse(jdata["Roulette"][i].ToString());
            }
        }
        */
    }

    public void GPGSLogin()
    {
        // 이미 로그인 된 경우
        if (Social.localUser.authenticated == true)
        {
            BackendReturnObject BRO = Backend.BMember.AuthorizeFederation(GetToken(), FederationType.Google, "gpgs");
        }
        else
        {
            Social.localUser.Authenticate((bool success) => {
                if (success)
                {
                    // 로그인 성공 -> 뒤끝 서버에 획득한 구글 토큰으로 가입요청
                    BackendReturnObject BRO = Backend.BMember.AuthorizeFederation(GetToken(), FederationType.Google, "gpgs");
                }
                else
                {
                    // 로그인 실패
                    Debug.Log("Login failed for some reason");
                }
            });
        }
    }
    // 구글 토큰 받아옴
    public string GetToken()
    {
        if (PlayGamesPlatform.Instance.localUser.authenticated)
        {
            // 유저 토큰 받기 첫번째 방법
            string _IDtoken = PlayGamesPlatform.Instance.GetIdToken();
            // 두번째 방법

            if (_IDtoken == "")
                _IDtoken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();
            return _IDtoken;
        }
        else
        {
            Debug.Log("접속되어있지 않습니다. PlayGamesPlatform.Instance.localUser.authenticated :  fail");
            Invoke(nameof(GoogleAuth),2f);
            return null;
        }
    }

    public bool needupdate = false;
    //구글 로그인
    public static BackendReturnObject GoogleBRO;
    public void Bt_LoginGoogle()
    {
        Debug.Log("고유 번호는 " + SystemInfo.deviceUniqueIdentifier);
        if(needupdate) return;
        // 이미 로그인 된 경우
        if (Social.localUser.authenticated == true)
        {
            Debug.Log("로그인");
            GoogleBRO = Backend.BMember.AuthorizeFederation(GetToken(), FederationType.Google, "gpgs");
            Debug.Log(GoogleBRO);
            
            
            if (GoogleBRO.GetStatusCode() == "403")
            {
                //차단유
                BlockPanel.SetActive(true);
                Blockinfo.text = GoogleBRO.GetErrorCode();
                return;
            }
            if (GoogleBRO.GetStatusCode() == "410")
            {
                //차단유
                RemovingPanel.SetActive(true);
                DateTime now = DateTime.Now;
                int time = 60 - now.Minute;
                Removinginfo.text = string.Format(Inventory.GetTranslate("UI4/현재 삭제중"), time.ToString());
                return;
            }
            if (GoogleBRO.IsSuccess())
            {
                Debug.Log("시작");
                GetComponent<BackendLogin>().serverobj.SetActive(false);
                CancelInvoke(nameof(Bt_LoginGoogle));
                obj.SetActive(false);
                isgoole = true;
                //닉네임 체크
                GoogleBRO = Backend.BMember.GetUserInfo();
                Debug.Log("1");

                if (GoogleBRO.IsSuccess())
                {
                    Debug.Log(GoogleBRO.GetReturnValuetoJSON()["row"]);
                    JsonData data = GoogleBRO.GetReturnValuetoJSON()["row"]["nickname"];
                    PlayerBackendData.Instance.playerindate = GoogleBRO.GetReturnValuetoJSON()["row"]["inDate"].ToString();
                    Savemanager.Instance.Init();
                    Timemanager.Instance.TimeInit();
                    if (data == null)
                    {
                        PlayerBackendData.Instance.Id = GoogleBRO.GetInDate();
                        GetComponent<BackendNickname>().Nickname_logoPanel.SetActive(true);
                    }
                    else
                    {
                        PlayerBackendData.Instance.Id = GoogleBRO.GetInDate();
                        player.nickname = GoogleBRO.GetReturnValuetoJSON()["row"]["nickname"].ToString();
                        Savemanager.Instance.Init();
                        //데이터 불러오기
                        LoadPlayerData();
                        //데이터를 받아온다.
                        Where where1 = new Where();
                        where1.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);
                       
                        var broj = Backend.GameData.GetMyData("PlayerData", where1,select, 1);
                        jdata = null;
                        if (broj.IsSuccess())
                        {
                            
                            if (broj.GetReturnValuetoJSON()["rows"].Count == 0)
                            {
                                Debug.Log("개수는 0개 캐릭터를 만들자");
                                GetComponent<BackendLogin>().StartClassSetting();
                                return;
                            }
                            
                            
                            jdata = broj.FlattenRows()[0];
                        }

                        if (jdata != null)
                        {
                            Debug.Log("레벨 데이터" + jdata["level"].ToString());
                            Debug.Log("경험치 데이터" + jdata["levelExp"].ToString());
                            Debug.Log("서버LV " + PlayerBackendData.Instance.ServerLv);
                            Debug.Log("클라lV " + PlayerBackendData.Instance.GetLv());
                            if (jdata.ContainsKey("SaveNum"))
                            {
                                PlayerBackendData.Instance.ServerLv = int.Parse(jdata["level"].ToString());
                                PlayerBackendData.Instance.ServerEXP = decimal.Parse(jdata["levelExp"].ToString());
                                PlayerBackendData.Instance.ServerSaveNum = int.Parse(jdata["SaveNum"].ToString());
                                //레벨이 서버가 더 높으면 삭제
                                if (PlayerBackendData.Instance.ServerSaveNum > PlayerBackendData.Instance.ClientSaveNum
                                    || PlayerBackendData.Instance.ServerLv > PlayerBackendData.Instance.GetLv())
                                {
                                        //레벨이 서버가 더 낮으면 말이안된다 서버는 가끔이고 로컬은 실시간이기에
                                        //더 낮으면 
                                        Debug.Log("서버데이터가 더 높아 데이터 삭제해야함");
                                        ES3.DeleteFile(Savemanager.Instance.GetFileName());
                                        GetComponent<BackendLogin>().RefreshServerSavedData(jdata);
                                        return;
                                }
                                /*
                                //같은데 경험치가 더 적으면 삭제
                                if (PlayerBackendData.Instance.ServerLv == PlayerBackendData.Instance.GetLv()
                                    && PlayerBackendData.Instance.ServerEXP > PlayerBackendData.Instance.GetExp())
                                {
                                    //레벨이 서버가 더 낮으면 말이안된다 서버는 가끔이고 로컬은 실시간이기에
                                    //더 낮으면 
                                    Debug.Log("서버LV " + PlayerBackendData.Instance.ServerLv);
                                    Debug.Log("클라lV " + PlayerBackendData.Instance.GetLv());
                                    Debug.Log("서버EXP " + PlayerBackendData.Instance.ServerEXP);
                                    Debug.Log("클라EXP " + PlayerBackendData.Instance.GetExp());
                                    Debug.Log("서버데이터가 더 높아 데이터 삭제해야함");
                                    ES3.DeleteFile(Savemanager.Instance.GetFileName());
                                    GetComponent<BackendLogin>().RefreshServerSavedData(jdata);
                                    return;
                                }
                                
                                */
                            }
                        }


                        if (jdata != null)
                        {
                            Debug.Log(jdata.ContainsKey("SpeedHackOn"));
                            if(jdata.ContainsKey("SpeedHackOn"))
                            {
                                Debug.Log("차단" +jdata["SpeedHackOn"].ToString() );
                                if (jdata["SpeedHackOn"].ToString() == "true")
                                {
                                    BlockPanel.SetActive(true);
                                    return;
                                }
                                //스피드핵감지
                            }
                        }
                        
                        
                        if (jdata != null && !jdata.ContainsKey("PlayerCanLoadBool"))
                        {
                            GetComponent<BackendLogin>().StartClassSetting();
                            return;
                        }
                        
                        //개발자가 불러오기를 뜨게함
                        if (jdata["PlayerCanLoadBool"].ToString() == "trues")
                        {
                           // ES3.DeleteFile(Savemanager.Instance.GetFileName());
                            GetComponent<BackendLogin>().RefreshServerSavedData(jdata);
                            return;
                        }
                        
                        
                        
                        if (jdata != null && jdata.ContainsKey("PlayerLoadUID"))
                        {
                            if (jdata["PlayerLoadUID"].ToString() == SystemInfo.deviceUniqueIdentifier)
                            {
                                Debug.Log("같은 계정이다.");
                                //접속가능
                            }
                            else
                            {
                                Debug.Log("데이터 삭제");
                                ES3.DeleteFile(Savemanager.Instance.GetFileName());
                                if (jdata["PlayerCanLoadBool"].ToString() == "true")
                                {
                                    //불러오기 가능
                                }
                                else
                                {
                                    //다른계정이니 현재자리삭제
                                    NotThisDeviceObj.SetActive(true);
                                    ModelText.text = string.Format(TranslateManager.Instance.GetTranslate("UI2/모델명"),
                                        jdata["PlayerLoadModel"].ToString());
                                    Debug.Log("다른 계정이다.");
                                    return;
                                }
                             
                            }
                        }
                        //접속 시 현재불러온시간과 서버 불러온 시간을 비교
                        //비교해서 다른데 서버불러오기가 true라면 
                        if (jdata != null && jdata.ContainsKey("PlayerLoadTime"))
                        {
                            if (PlayerBackendData.Instance.playersaveindate != "")
                            {
                                if (PlayerBackendData.Instance.playersaveindate != jdata["PlayerLoadTime"].ToString()
                                    && jdata["PlayerCanLoadBool"].ToString() == "true")
                                {
                                    Debug.Log("시간이 다르다");
                                    Debug.Log("데이터 삭제");
                                    //ES3.DeleteFile(Savemanager.Instance.GetFileName());
                                    GetComponent<BackendLogin>().RefreshServerSavedData(jdata);
                                    return;
                                }
                            }
                        }
                        
                        //개미굴횟수가져옴
                        if (jdata != null && jdata.ContainsKey("AntTotalClear"))
                        {
                            Debug.Log("개미굴 총 횟수 가져옴" + jdata["AntTotalClear"].ToString());
                            PlayerBackendData.Instance.AntTotalClear = int.Parse(jdata["AntTotalClear"].ToString());
                                
                        }
                        
                        if (PlayerBackendData.Instance.ClassId == "0" && jdata != null)
                        { 
                            //서버데이터는 있는데 //저장이없다 
                            GetComponent<BackendLogin>().RefreshServerSavedData(jdata);
                        }
                        else if (PlayerBackendData.Instance.ClassId == "0")
                        {
                            GetComponent<BackendLogin>().StartClassSetting();
                        }
                        else
                        {
                            GetRankingData();
                            GetComponent<BackendLogin>().StartLobby();
                        }
                    }
                }
            }
            else
            {
                CheckPlayerType(GoogleBRO.GetStatusCode());
            }
        }
        else
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                   // debuginput.text += "밑에꺼\n";
                    // 로그인 성공 -> 뒤끝 서버에 획득한 구글 토큰으로 가입요청
                    BackendReturnObject BRO = Backend.BMember.AuthorizeFederation(GetToken(), FederationType.Google, "gpgs");
                   // debuginput.text += BRO.ToString() + "\n";

                    // Debug.Log(BRO);
                    if (BRO.IsSuccess())
                    {
                        Backend.BMember.UpdateFederationEmail(GetToken(), FederationType.Google);
                        BRO = Backend.BMember.GetUserInfo();
                        player.playerindate = GoogleBRO.GetReturnValuetoJSON()["row"]["inDate"].ToString();
                        GoogleBRO = BRO;
                       if (!BRO.IsSuccess()) return;
                        JsonData data = BRO.GetReturnValuetoJSON()["row"]["nickname"];
                        if (data == null)
                        {
                            isgoole = true;
                            PlayerBackendData.Instance.Id = BRO.GetInDate();
                            GetComponent<BackendNickname>().Nickname_logoPanel.SetActive(true);
                        }
                        else
                        {
                            PlayerBackendData.Instance.Id = BRO.GetInDate();
                            player.nickname = BRO.GetReturnValuetoJSON()["row"]["nickname"].ToString();
                            LoadPlayerData();
                            
                            
                            //데이터를 받아온다.
                            Where where1 = new Where();
                            where1.Equal("owner_inDate", PlayerBackendData.Instance.playerindate);

                        
                            var broj = Backend.GameData.GetMyData("PlayerData", where1,select, 1);
                            jdata = null;
                            if (broj.IsSuccess())
                            {
                                jdata = broj.FlattenRows()[0];
                            }
                            
                            
                            if (jdata != null && jdata.ContainsKey("PlayerLoadUID"))
                            {
                                if (jdata["PlayerLoadUID"].ToString() == SystemInfo.deviceUniqueIdentifier)
                                {
                                    Debug.Log("같은 계정이다.");
                                }
                                else
                                {
                                    NotThisDeviceObj.SetActive(true);
                                    ModelText.text = string.Format(TranslateManager.Instance.GetTranslate("UI2/모델명"),
                                        jdata["PlayerLoadModel"].ToString());
                                    Debug.Log("다른 계정이다.");
                                    return;
                                }
                            }
                            //접속 시 현재불러온시간과 서버 불러온 시간을 비교
                            //비교해서 다른데 서버불러오기가 true라면 
                            if (jdata != null && jdata.ContainsKey("PlayerLoadTime"))
                            {
                                if (PlayerBackendData.Instance.playersaveindate != "")
                                {
                                    if (PlayerBackendData.Instance.playersaveindate != jdata["PlayerLoadTime"].ToString()
                                        && jdata["PlayerCanLoadBool"].ToString() == "true")
                                    {
                                        Debug.Log("시간이 다르다");
                                        GetComponent<BackendLogin>().RefreshServerSavedData(jdata);
                                        return;
                                    }
                                }
                            }
                            
                            //개미굴횟수가져옴
                            if (jdata != null && jdata.ContainsKey("AntTotalClear"))
                            {
                                Debug.Log("개미굴 총 횟수 가져옴" + jdata["AntTotalClear"].ToString());
                                PlayerBackendData.Instance.AntTotalClear = int.Parse(jdata["AntTotalClear"].ToString());
                                
                            }
                            
                            if (PlayerBackendData.Instance.ClassId == "0" && jdata != null)
                            { 
                                //서버데이터는 있는데 //저장이없다 
                                GetComponent<BackendLogin>().RefreshServerSavedData(jdata);
                            }
                            else if (PlayerBackendData.Instance.ClassId == "0")
                            {
                                GetComponent<BackendLogin>().StartClassSetting();
                            }
                            else
                            {
                                GetRankingData();
                                GetComponent<BackendLogin>().StartLobby();
                            }
                        }
                    }
                    else
                    {
                        CheckPlayerType(GoogleBRO.GetStatusCode());
                    }
                }
                else
                {
                    // 로그인 실패
                    Debug.Log("Login failed for some reason");
                }
            });
        }
    }
    void CheckPlayerType(string status)
    {
        switch (status)
        {
            case "403": //정지된 플레이어
                //text.text = TranslateManager.Instance.GetTranslate("POPUP/정지된플레이어");
                break;
        }
    }
    public void LoadPlayerData()
    {
        ES3Settings.defaultSettings.path = $"{player.Id}.es3";
        Savemanager.Instance.LoadData();
    }
}