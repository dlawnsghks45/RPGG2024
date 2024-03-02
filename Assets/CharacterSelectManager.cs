using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectManager : MonoBehaviour
{
    public string[] StandardClass = { "C1000","C1001","C1002"}; //1000 , 1001 , 1002
     string[] StandardWeapon; //1000 , 1001 , 1002
     string[] StandardSubWeapon; //1000 , 1001 , 1002

    public Image[] PlayerImage;
    public Image ClassSkillImage;


    int nowpage = 0;
    int maxpage = 3;

    public Button Prevbutton;
    public Button Nextbutton;


    public Text ClassName;
    public Text ClassInfo;
    public Text[] ClassLvStat;
    public Text ClassStatName;
    public Text ClassStatInfo;
    public Text ClassSkill;

     string[] statname = { "힘증가기본", "민첩증가기본", "지능증가기본", "지혜증가기본" };
     string[] statinfo = { "힘설명", "민첩설명", "지능설명", "지혜설명" };

    public void ShowStatInfo(int index)
    {
        ClassStatName.text = GetTranslate("Stat/" + statname[index]);
        ClassStatInfo.text = GetTranslate("Stat/" + statinfo[index]);
    }


    private void Start()
    {
        ShowJobData();
    }

    public void ShowJobData()
    {
        string nowclass = StandardClass[nowpage];
        //Debug.Log(StandardClass[nowpage]);
        //아바타 세팅
        ClassDB.Row data = ClassDB.Instance.Find_id(nowclass);
        //Debug.Log(data.classsprite);
        PlayerImage[0].sprite = GetSprite(data.classsprite);
        PlayerImage[1].sprite = GetSprite(EquipItemDB.Instance.Find_id(ClassDB.Instance.Find_id(nowclass).standweapon).EquipSprite);
        PlayerImage[2].sprite = GetSprite(EquipItemDB.Instance.Find_id(ClassDB.Instance.Find_id(nowclass).standsubweapon).EquipSprite);

        ClassName.text = GetTranslate(data.name);
        ClassInfo.text = GetTranslate(data.info);

        ClassLvStat[0].text = data.strperlv;
        ClassLvStat[1].text = data.dexperlv;
        ClassLvStat[2].text = data.intperlv;
        ClassLvStat[3].text = data.wisperlv;
        ClassLvStat[4].text = data.hpperlv;
        ClassLvStat[5].text = data.mpperlv;

        //  ClassPassive.text = GetTranslate(data.name);
        SkillDB.Row skilldata = SkillDB.Instance.Find_Id(data.giveskill);
        //Debug.Log(data.giveskill);
        //Debug.Log(skilldata.Sprite);
        ClassSkillImage.sprite = GetSprite(skilldata.Sprite);
        ClassSkill.text = $"<size=30>{GetTranslate(skilldata.Name)}</size>\n{GetTranslate(skilldata.Info)}";

    }

    public void UpPage()
    {
        //다음페이지
        if (nowpage != maxpage - 1)
        {
            nowpage++;
            if (nowpage == maxpage - 1)
                Nextbutton.interactable = false;
            else
                Nextbutton.interactable = true;

            Prevbutton.interactable = true;
            ShowJobData();
        }

    }
    public void DownPage()
    {
        //다음페이지
        if (nowpage != 0)
        {
            nowpage--;
            if (nowpage == 0)
            {
                Prevbutton.interactable = false;

            }
            else
            {
                Prevbutton.interactable = true;

            }
            Nextbutton.interactable = true;
            ShowJobData();
        }
    }

    public string GetTranslate(string path)
    {
        return I2.Loc.LocalizationManager.GetTermTranslation(path);
    }

    private Dictionary<string, Sprite> Sprites = new Dictionary<string, Sprite>();
    public Sprite GetSprite(string _key)
    {
        if (Sprites.ContainsKey(_key))
        {
            //  Debug.Log("Have");
            //   Debug.Log(_key);
            return Sprites[_key];
        }
        Sprite value = Resources.Load<Sprite>(_key);
        Sprites.Add(_key, value);
        return value;
    }


    public void Bt_SetClass()
    {
        PlayerBackendData.Instance.ClassData[StandardClass[nowpage]].Lv1 = 1;
        PlayerBackendData.Instance.ClassData[StandardClass[nowpage]].Isown = true;
        PlayerBackendData.Instance.ClassId = StandardClass[nowpage];
        ClassDB.Row data = ClassDB.Instance.Find_id(StandardClass[nowpage]);
        //아이템 지급
        PlayerBackendData.Instance.MakeEquipmentAndEquip(data.standweapon);
        PlayerBackendData.Instance.MakeEquipmentAndEquip(data.standsubweapon);

        string[] skills = ClassDB.Instance.Find_id(PlayerBackendData.Instance.ClassId).giveskill.Split(';');

        for(int i = 0; i < skills.Length;i++)
        {
            PlayerBackendData.Instance.AddSkill(skills[i]);
        }
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

    public void StartClassSetting()
    {
        StartCoroutine(LoadScene(LobbySelectScene ));
    }

    //접속
    [SerializeField]
    Image progressBar;
    public GameObject progressobj;

    public string LobbySelectScene;

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

}
