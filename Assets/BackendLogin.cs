using System;
using BackEnd;
using GooglePlayGames;
using System.Collections;
using Doozy.Engine.UI;
using LitJson;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackendLogin : MonoBehaviour
{
    //싱글톤만들기.
    private static BackendLogin _instance = null;
    public static BackendLogin Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(BackendLogin)) as BackendLogin;

                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }
            return _instance;
        }
    }

    public GameObject serverobj;
    //서비스 ㅣ용약관
    public bool ispolicyon = false;
    public GameObject PolicyPanel;
    public UIToggle[] policytoggles;
    
    public GameObject EditorLoginObj;
    private void SavePolicy()
    {
        PlayerPrefs.SetString("PolicyTrue",ispolicyon.ToString());
    }
   
    public void LoaddPolicy()
    {
        if (PlayerPrefs.HasKey("PolicyTrue"))
        {
            ispolicyon = bool.Parse(PlayerPrefs.GetString("PolicyTrue"));
        }

        if (ispolicyon)
        {
            //바로 시작한다.
            BackendFerderationAuth.Instance.StartGoogle();
        }
        else
        {
            PolicyPanel.SetActive(true);
        }
        
    }

    public UIButton startpolicytoggle;
    public void CheckPolicy()
    {
        startpolicytoggle.Interactable = false;
        if (!policytoggles[0].IsOn || !policytoggles[1].IsOn) return;
        ispolicyon = true;
        startpolicytoggle.Interactable = true;
       
    }

    public void Bt_StartPolicy()
    {
        SavePolicy();
        PolicyPanel.SetActive(false);
        BackendFerderationAuth.Instance.StartGoogle();
    }
    public void Bt_LinkSite(string Link)
    {
        Application.OpenURL(Link);
    }
    
    
    public void BtOnLogOut()
    {
        BackendReturnObject BRO = Backend.BMember.Logout();

        if (BRO.IsSuccess())
        {
            LogoutGoogle();
        }
    }
    public void LogoutGoogle()
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated() == true)
        {
            PlayGamesPlatform.Instance.SignOut();
            PlayGamesPlatform.Activate();
            Application.Quit();
        }
    }

    BackendReturnObject bro;

    [SerializeField] GameObject ServerStatePanel;
    [SerializeField] GameObject NeedUpdatePanel;
    [SerializeField] Text Updatetext;


    public GameObject[] LoginButtons;


    // Start is called before the first frame update
   // async Task Start()
    private void Start()
    {
        // 첫번째 방법 (동기)
        var bro = Backend.Initialize(true);

        if (bro.IsSuccess())
        {
            //Debug.Log(bro);

            Invoke("LoaddPolicy", 2f);

            //// 초기화 성공 시 로직
            //if (callback.GetReturnValuetoJSON()["serverStatus"].ToString() == "2")
            // {
            //    ServerStatePanel.SetActive(true);
            //    return;
            //}
#if !UNITY_EDITOR
                  bro = Backend.Utils.GetLatestVersion();
                  if (bro.GetReturnValuetoJSON()["version"].ToString() != Application.version)
                  {
/*          
                      if (GoolgeManager.Instance != null)
                          await GoolgeManager.Instance.UpdateApp();
                          */
                        EditorLoginObj.SetActive(false);
                      GetComponent<BackendFerderationAuth>().needupdate = true;
                      NeedUpdatePanel.SetActive(true);
                      Updatetext.text = $"Current Version {Application.version}\n" +
                                        $"Server Version {bro.GetReturnValuetoJSON()["version"].ToString()}";
                      return;
                  }
#endif
            EditorLoginObj.SetActive(false);

#if UNITY_EDITOR
            EditorLoginObj.SetActive(true);
#endif

        }

        else
        {
            // 초기화 실패 시 로직
            Application.Quit();
        }

        if (Application.platform == RuntimePlatform.Android)
        {
            
        }

        initdata();
    }

    void initdata()
    {
        PlayerBackendData.Instance.InitClassData();
        PlayerBackendData.Instance.InitPetData();
        PlayerBackendData.Instance.InitAchieveData();
        PlayerBackendData.Instance.InitCollection();
    }
    
    public TMP_InputField testinput;
    public void Bt_Check()
    {
        if (testinput.text.Equals("gksrnr13"))
        {
            testinput.gameObject.SetActive(false);
            initdata();
            GetComponent<BackendFerderationAuth>().needupdate = false;
            GetComponent<BackendFerderationAuth>().StartGoogle();
            PlayerBackendData.istest = true;
        }
    }
    
    public void bt_updatebutton()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.LKGames.com.unity.RPGG2");
    }


    public void StartLogin()
    {
        GetComponent<BackendFerderationAuth>().Bt_LoginGoogle();
    }

    public void WWWURL(string url)
    {
        Application.OpenURL(url);
    }



    public void StartLobby()
    {
      
        foreach (var t in LoginButtons)
            t.SetActive(false);

        StartCoroutine(LoadScene(LobbySelectScene));
    }
    public void StartLobby_Load()
    {
        StartCoroutine(LoadScene(LobbySelectScene));
    }
    public void StartClassSetting()
    {
        
        foreach (var t in LoginButtons)
            t.SetActive(false);

        StartCoroutine(LoadScene(LobbySelectScene));
    }

    //접속
    [SerializeField]
    Image progressBar;
    public GameObject progressobj;

    public string LobbySelectScene;
    public string CharacterSelectScene;

    public void Bt_SetClass()
    {
        PlayerBackendData.Instance.ClassData["C999"].Lv1 = 1;
        PlayerBackendData.Instance.ClassData["C999"].Isown = true;
        PlayerBackendData.Instance.ClassId = "C999";
        ClassDB.Row data = ClassDB.Instance.Find_id("C999");
        //아이템 지급
        PlayerBackendData.Instance.MakeEquipmentAndEquip(data.standweapon);
        PlayerBackendData.Instance.MakeEquipmentAndEquip(data.standsubweapon);

        PlayerBackendData.Instance.Additem("1700",1);
        PlayerBackendData.Instance.Additem("10",1);
        
        PlayerBackendData.Instance.nowstage = "1000";
        progressobj.SetActive(true);
        Savemanager.Instance.SaveStageData();
        Savemanager.Instance.SaveClassData();
        Savemanager.Instance.SaveEquip();
        Savemanager.Instance.SaveSkillData();
        Savemanager.Instance.GameDataInsert_ToServer();
        Savemanager.Instance.Init();
        Savemanager.Instance.Save();
        StartClassSetting();
    }
    
    IEnumerator LoadScene(string scenename)
    {
        yield return new WaitForSeconds(2f);

        progressobj.SetActive(true);
        //Debug.Log(CharacterSelectScene);
        var op = SceneManager.LoadSceneAsync(scenename);
        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;
            timer += Time.deltaTime;
            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer);
                if (progressBar.fillAmount >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);
                if (progressBar.fillAmount == 1.0f)

                {
                    op.allowSceneActivation = true;

                    yield break;
                }
            }
        }
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(2f);
        progressobj.SetActive(true);
        //Debug.Log(CharacterSelectScene);
        var op = SceneManager.LoadSceneAsync(CharacterSelectScene);
        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;
            timer += Time.deltaTime;
            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer);
                if (progressBar.fillAmount >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);
                if (progressBar.fillAmount == 1.0f)

                {
                    op.allowSceneActivation = true;

                    yield break;
                }
            }
        }
    }

    public void Bt_ExitGame()
    {
        Application.Quit();
    }
    
    
    public Text RecentServerDateTimeText;
    public Text RecentServerDataCanLoadText; //불러올수있는지 알 수 있는 테이블
    public GameObject PlayerServerDatapanel;
    private DateTime Savedtime;
    private bool iscanload = false;
    
    public void RefreshServerSavedData(JsonData data)
    {
        //로컬 데이터가 없는데 
        //서버 저장 키가 있다면
        if (data == null)
            return;
        
        PlayerServerDatapanel.SetActive(false);

        if (data.ContainsKey("PlayerSaveTime"))
        {
            RecentServerDateTimeText.text = TimeZoneInfo.ConvertTimeToUtc(DateTime.Parse(data["PlayerSaveTime"].ToString()))
                .ToString("yyyy-MM-dd HH:mm:ss");
            Savedtime = DateTime.Parse(data["PlayerSaveTime"].ToString());
        }
        else
        {
            RecentServerDateTimeText.text = "-";
        }


        bool iscan = data["PlayerCanLoadBool"].ToString() == "trues"  ||
                     data["PlayerCanLoadBool"].ToString() == "true";

        if (PlayerBackendData.Instance.ServerLv > PlayerBackendData.Instance.GetLv())
        {
            Debug.Log("레벨클라"+ PlayerBackendData.Instance.GetLv());
            Debug.Log("레벨서버"+ PlayerBackendData.Instance.ServerLv);
            RecentServerDataCanLoadText.text =
                string.Format(Inventory.GetTranslate("UI/설정_저장_불러오기유무"), "<Color=lime>On</color>");
            iscan = true;
        }
        else
        {
            RecentServerDataCanLoadText.text = iscan
                ? string.Format(Inventory.GetTranslate("UI/설정_저장_불러오기유무"),
                    iscan ? "<Color=lime>On</color>" : "<Color=red>Off</color>")
                : string.Format(Inventory.GetTranslate("UI/설정_저장_불러오기유무"), "<Color=red>Off</color>");
        }
   
        iscanload = iscan;
        ES3.DeleteFile(Savemanager.Instance.GetFileName());
        PlayerServerDatapanel.SetActive(true);
    }


    public void Bt_LoadPlayerServerData()
    {
        if (!iscanload)
        {
            return;
        }


        if (Savemanager.Instance.GameDataGet())
        {
            BackendFerderationAuth.Instance.GetRankingData();
            PlayerServerDatapanel.SetActive(false);
            foreach (var t in LoginButtons)
                t.SetActive(false);
            
            StartLobby_Load();
        }
    }
}
