using Doozy.Engine.UI;
using Doozy.Engine.UI;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{
    private WaitForSeconds save = new WaitForSeconds(600f);
    public IEnumerator SaveAuto()
    {
        yield return save;
        Savemanager.Instance.Save();
    }
    
    
    //싱글톤만들기.
    private static PlayerData _instance = null;
    public static PlayerData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(PlayerData)) as PlayerData;

                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }
            return _instance;
        }
    }

    
    public Animator NewContentObj;
    public Text NewContentTitle;
    public Text NewContentInfo;
    public Text NewContentHow;
    public GameObject Elliobj;
    public GameObject ElliNoobj;
    public Text ElliFinishText;
    public GameObject ElliBuyButton;

    public GameObject adfreeobj;

    
    //왼쪽 상단 플레이어 정보
    public Text PlayerName;
    public Text PlayerJob;
 //   public Text PlayerLv;

    public Text PlayerName2;
    public Text PlayerJob2;


    public Image PlayerExp;
//    public Text PlayerExpText;
 ///  public Image PlayerAvarta;
//    public Image PlayerMainWeapon;
//    public Image PlayerSubWeapon;
    public Text PlayerGold;
    public Text PlayerCash;
    public itemshowcountslot[] PlayerCashs;
    

    
    //플레이어 몸체
    [SerializeField]
    List<Material> MainWeaponMaterial = new List<Material>();
    [SerializeField]
    List<Material> SubWeaponMaterial = new List<Material>();
    [SerializeField]
    List<Material> PetMaterial = new List<Material>();
    
    //새로운 컨텐츠 보여주기
    private string newcontentstring = "";
    public void ShowNewContentReal(string name)
    {
        newcontentstring = name;
        Invoke(nameof(ShowNewContent),3f);
    }
    void ShowNewContent()
    {
        if(newcontentstring.Equals(""))
            return;
        
        switch (newcontentstring)
        {
            case "감염된농장":
                NewContentTitle.text = Inventory.GetTranslate("Content/3차던전제목");
                NewContentInfo.text = Inventory.GetTranslate("Content/3차던전설명");
                NewContentHow.text = Inventory.GetTranslate("Content/던전메뉴");
                break;
            case "늪지대":
                NewContentTitle.text = Inventory.GetTranslate("Content/4차던전제목");
                NewContentInfo.text = Inventory.GetTranslate("Content/4차던전설명");
                NewContentHow.text = Inventory.GetTranslate("Content/던전메뉴");
                PlayerData.Instance.ShowNewContentReal("레이드");
                break;
            case "직업":
                NewContentTitle.text = Inventory.GetTranslate("Content/직업제목");
                NewContentInfo.text = Inventory.GetTranslate("Content/직업설명");
                NewContentHow.text = Inventory.GetTranslate("Content/메뉴직업");
                PlayerData.Instance.ShowNewContentReal("프리미엄상점");
                break;
            case"레이드":
                NewContentTitle.text = Inventory.GetTranslate("Content/레이드제목");
                NewContentInfo.text = Inventory.GetTranslate("Content/레이드설명");
                NewContentHow.text = Inventory.GetTranslate("Content/메뉴전투입장");
                break;
            case "5던전":
                NewContentTitle.text = Inventory.GetTranslate("Content/5차던전제목");
                NewContentInfo.text = Inventory.GetTranslate("Content/5차던전설명");
                NewContentHow.text = Inventory.GetTranslate("Content/던전메뉴");
                PlayerData.Instance.ShowNewContentReal("자동보상");
                break;
            case "6던전":
                NewContentTitle.text = Inventory.GetTranslate("Content/6차던전제목");
                NewContentInfo.text = Inventory.GetTranslate("Content/6차던전설명");
                NewContentHow.text = Inventory.GetTranslate("Content/던전메뉴");
                break;
            case "7던전":
                NewContentTitle.text = Inventory.GetTranslate("Content/7차던전제목");
                NewContentInfo.text = Inventory.GetTranslate("Content/7차던전설명");
                NewContentHow.text = Inventory.GetTranslate("Content/던전메뉴");
                PlayerData.Instance.ShowNewContentReal("성물전쟁");
                break;
            case "성물전쟁":
                NewContentTitle.text = Inventory.GetTranslate("Content/성물전쟁제목");
                NewContentInfo.text = Inventory.GetTranslate("Content/성물전쟁설명");
                NewContentHow.text = Inventory.GetTranslate("Content/컨텐츠성물전쟁");
                PlayerData.Instance.ShowNewContentReal("제단");

                break;
            case "프리미엄상점":
                NewContentTitle.text = Inventory.GetTranslate("Content/상점제목");
                NewContentInfo.text = Inventory.GetTranslate("Content/상점설명");
                NewContentHow.text = Inventory.GetTranslate("Content/상점위치");
                PlayerData.Instance.ShowNewContentReal("교환소");

                break;
            case "교환소":
                NewContentTitle.text = Inventory.GetTranslate("Content/교환소제목");
                NewContentInfo.text = Inventory.GetTranslate("Content/교환소내용");
                NewContentHow.text = Inventory.GetTranslate("Content/교환소아이템");
                PlayerData.Instance.ShowNewContentReal("감염된농장");
                break;
            case "제단":
                NewContentTitle.text = Inventory.GetTranslate("Content/제단제목");
                NewContentInfo.text = Inventory.GetTranslate("Content/제단내용");
                NewContentHow.text = Inventory.GetTranslate("Content/제단위치");
                break;
            case "자동보상":
                NewContentTitle.text = Inventory.GetTranslate("Content/자동사냥제목");
                NewContentInfo.text = Inventory.GetTranslate("Content/자동사냥설명");
                NewContentHow.text = Inventory.GetTranslate("Content/자동사냥위치");
                break;
            case "길드":
                NewContentTitle.text = Inventory.GetTranslate("Content/길드제목");
                NewContentInfo.text = Inventory.GetTranslate("Content/길드설명");
                NewContentHow.text = Inventory.GetTranslate("Content/길드위치");
                break;
            case "허수아비":
                NewContentTitle.text = Inventory.GetTranslate("Content/허수아비제목");
                NewContentInfo.text = Inventory.GetTranslate("Content/허수아비설명");
                NewContentHow.text = Inventory.GetTranslate("Content/허수아비위치");
                break;
            case "11던전":
                NewContentTitle.text = Inventory.GetTranslate("Content/11차던전제목");
                NewContentInfo.text = Inventory.GetTranslate("Content/11차던전설명");
                NewContentHow.text = Inventory.GetTranslate("Content/던전메뉴");
                PlayerData.Instance.ShowNewContentReal("개미굴");
                break;
            case "12던전":
                NewContentTitle.text = Inventory.GetTranslate("Content/12차던전제목");
                NewContentInfo.text = Inventory.GetTranslate("Content/12차던전설명");
                NewContentHow.text = Inventory.GetTranslate("Content/던전메뉴");
                break;
            case "13던전":
                NewContentTitle.text = Inventory.GetTranslate("Content/13차던전제목");
                NewContentInfo.text = Inventory.GetTranslate("Content/13차던전설명");
                NewContentHow.text = Inventory.GetTranslate("Content/던전메뉴");
                break;
            case "14던전":
                NewContentTitle.text = Inventory.GetTranslate("Content/14차던전제목");
                NewContentInfo.text = Inventory.GetTranslate("Content/14차던전설명");
                NewContentHow.text = Inventory.GetTranslate("Content/던전메뉴");
                break;
            case "15던전":
                NewContentTitle.text = Inventory.GetTranslate("Content/15차던전제목");
                NewContentInfo.text = Inventory.GetTranslate("Content/15차던전설명");
                NewContentHow.text = Inventory.GetTranslate("Content/던전메뉴");
                PlayerData.Instance.ShowNewContentReal("펫");

                break;
            case "개미굴":
                NewContentTitle.text = Inventory.GetTranslate("Content/개미굴");
                NewContentInfo.text = Inventory.GetTranslate("Content/개미굴설명");
                NewContentHow.text = Inventory.GetTranslate("Content/개미굴위치");
                break;
            case "펫":
                NewContentTitle.text = Inventory.GetTranslate("Content/펫");
                NewContentInfo.text = Inventory.GetTranslate("Content/펫설명");
                NewContentHow.text = Inventory.GetTranslate("Content/펫위치");
                break;
            case "20던전":
                NewContentTitle.text = Inventory.GetTranslate("Content/20차던전제목");
                NewContentInfo.text = Inventory.GetTranslate("Content/20차던전설명");
                NewContentHow.text = Inventory.GetTranslate("Content/던전메뉴");
                break;
            case "21던전":
                NewContentTitle.text = Inventory.GetTranslate("Content/21차던전제목");
                NewContentInfo.text = Inventory.GetTranslate("Content/21차던전설명");
                NewContentHow.text = Inventory.GetTranslate("Content/던전메뉴");
                break;
            case "22던전":
                NewContentTitle.text = Inventory.GetTranslate("Content/22차던전제목");
                NewContentInfo.text = Inventory.GetTranslate("Content/22차던전설명");
                NewContentHow.text = Inventory.GetTranslate("Content/던전메뉴");
                break;
            case "23던전":
                NewContentTitle.text = Inventory.GetTranslate("Content/23차던전제목");
                NewContentInfo.text = Inventory.GetTranslate("Content/23차던전설명");
                NewContentHow.text = Inventory.GetTranslate("Content/던전메뉴");
                break;
            case "24던전":
                NewContentTitle.text = Inventory.GetTranslate("Content/24차던전제목");
                NewContentInfo.text = Inventory.GetTranslate("Content/24차던전설명");
                NewContentHow.text = Inventory.GetTranslate("Content/던전메뉴");
                break;
            case "25던전":
                NewContentTitle.text = Inventory.GetTranslate("Content/25차던전제목");
                NewContentInfo.text = Inventory.GetTranslate("Content/25차던전설명");
                NewContentHow.text = Inventory.GetTranslate("Content/던전메뉴");
                break;
            case "26던전":
                NewContentTitle.text = Inventory.GetTranslate("Content/26차던전제목");
                NewContentInfo.text = Inventory.GetTranslate("Content/26차던전설명");
                NewContentHow.text = Inventory.GetTranslate("Content/던전메뉴");
                break;
            case "27던전":
                NewContentTitle.text = Inventory.GetTranslate("Content/27차던전제목");
                NewContentInfo.text = Inventory.GetTranslate("Content/27차던전설명");
                NewContentHow.text = Inventory.GetTranslate("Content/던전메뉴");
                break;
            case "28던전":
                NewContentTitle.text = Inventory.GetTranslate("Content/28차던전제목");
                NewContentInfo.text = Inventory.GetTranslate("Content/28차던전설명");
                NewContentHow.text = Inventory.GetTranslate("Content/던전메뉴");
                break;
            case "29던전":
                NewContentTitle.text = Inventory.GetTranslate("Content/29차던전제목");
                NewContentInfo.text = Inventory.GetTranslate("Content/29차던전설명");
                NewContentHow.text = Inventory.GetTranslate("Content/던전메뉴");
                break;
            case "30던전":
                NewContentTitle.text = Inventory.GetTranslate("Content/30차던전제목");
                NewContentInfo.text = Inventory.GetTranslate("Content/30차던전설명");
                NewContentHow.text = Inventory.GetTranslate("Content/던전메뉴");
                break;
        }
        
        
        NewContentObj.SetTrigger(Show);
    }
    
    
    public void SetWeaponRareOther(Material materials,int rare)
    {
        if (rare < 2)
        {
                materials.SetFloat(Shader.PropertyToID("_OuterOutlineFade"), 0f);
        }
        else
        {
                materials.SetFloat(Shader.PropertyToID("_OuterOutlineFade"), 1f);
                materials.SetColor(Shader.PropertyToID("_OuterOutlineColor"),
                    Inventory.Instance.GetRareColor((rare+1).ToString()));
        }
    }

    public void SetPetStarOther(Material materials, int star)
    {
        switch (star)
        {
            case 0:
                materials.SetFloat(Shader.PropertyToID("_OuterOutlineFade"), 0f);
                break;
            case 1:
            case 2:
            case 3:
            case 4:
                materials.SetFloat(Shader.PropertyToID("_OuterOutlineFade"), 1f);
                materials.SetColor(Shader.PropertyToID("_OuterOutlineColor"),
                    Inventory.Instance.GetRareColor((star+3).ToString()));
                break;
        }
    }
    
    public void SetPetRare(int star)
    {
        switch (star)
        {
            case 0:
                foreach (var t in PetMaterial)
                    t.SetFloat(Shader.PropertyToID("_OuterOutlineFade"), 0f);
                break;
            case 1:
            case 2:
            case 3:
            case 4:
                foreach (var t in PetMaterial)
                {
                    t.SetFloat(Shader.PropertyToID("_OuterOutlineFade"), 1f);
                    t.SetColor(Shader.PropertyToID("_OuterOutlineColor"),
                        Inventory.Instance.GetRareColor((star+3).ToString()));
                }
                break;
        }
    }

    public void SetMainWeaponRare(int rare)
    {
        if (rare < 2)
        {
            foreach (var t in MainWeaponMaterial)
                t.SetFloat(Shader.PropertyToID("_OuterOutlineFade"), 0f);
        }
        else
        {
            foreach (var t in MainWeaponMaterial)
            {
//                Debug.Log("무기 레어" + ( rare+ 1));
                t.SetFloat(Shader.PropertyToID("_OuterOutlineFade"), 1f);
                t.SetColor(Shader.PropertyToID("_OuterOutlineColor"),
                    Inventory.Instance.GetRareColor((rare+1).ToString()));
            }
        }
    }

    public void SetSubWeaponRare(int rare)
    {
        if (rare < 2)
        {
            foreach (var t in SubWeaponMaterial)
                t.SetFloat(Shader.PropertyToID("_OuterOutlineFade"), 0f);
        }
        else
        {
            foreach (var t in SubWeaponMaterial)
            {
                //Debug.Log("보조무기");
                t.SetFloat(Shader.PropertyToID("_OuterOutlineFade"), 1f);
                t.SetColor(Shader.PropertyToID("_OuterOutlineColor"),
                    Inventory.Instance.GetRareColor((rare+1).ToString()));
            }
        }
    }

    [BoxGroup("PlayerImage")] public Image PlayerAvarta1; //장비창
    [BoxGroup("PlayerImage")] public Image PlayerAvarta2; //상단창
    [BoxGroup("PlayerImage")] public SpriteRenderer PlayerAvarta_Sprite;
    [BoxGroup("PlayerImage")] public string PlayerAvarta_Path;

    [BoxGroup("PlayerImage")] public Image PlayerAvartaWeapon1; //주무기
    [BoxGroup("PlayerImage")] public Image PlayerAvartaWeapon2; //주무기
    [BoxGroup("PlayerImage")] public SpriteRenderer PlayerAvartaWeapon_Sprite; //주무기
    [BoxGroup("PlayerImage")] public string PlayerAvartaWeapon_Path;



    [BoxGroup("PlayerImage")] public Image PlayerAvartaSubWeapon1; //보조무기
    [BoxGroup("PlayerImage")] public Image PlayerAvartaSubWeapon2; //보조무기
    [BoxGroup("PlayerImage")] public SpriteRenderer PlayerAvartaSubWeapon_Sprite;
    [BoxGroup("PlayerImage")] public string PlayerAvartaSubWeapon_Path;
    
    //펫
    [BoxGroup("PlayerImage")] public Image PlayerPet1;
    [BoxGroup("PlayerImage")] public Image PlayerPet2;
    [BoxGroup("PlayerImage")] public SpriteRenderer PlayerPet_sprite;
    [BoxGroup("PlayerImage")] public string PlayerPet_Path;

    #region 플레이어 이미지 변경
    //무기

    public string GetAvartaData()
    {
        return $"{PlayerAvarta_Path};{PlayerAvartaWeapon_Path};{PlayerAvartaSubWeapon_Path}";
    }

    public void SetWeaponImage(Image images,Sprite image)
    {
        images.sprite = image;
        images.enabled = true;
    }
    public void SetWeaponImage_remove_Other(Image images)
    {
        images.enabled = false;
    }
    
    
    public void SetWeaponImage(Sprite image=null, string path=null)
    {
        if (PlayerBackendData.Instance.avata_weapon != "")
        {
            AvartaDB.Row data = AvartaDB.Instance.Find_id(PlayerBackendData.Instance.avata_weapon);
            PlayerAvartaWeapon_Path = data.sprite;
            PlayerAvartaWeapon1.sprite = SpriteManager.Instance.GetSprite(data.sprite);
            PlayerAvartaWeapon2.sprite =  SpriteManager.Instance.GetSprite(data.sprite);
            PlayerAvartaWeapon_Sprite.sprite =  SpriteManager.Instance.GetSprite(data.sprite);
        }
        else
        {
            PlayerAvartaWeapon_Path = path;
            PlayerAvartaWeapon1.sprite = image;
            PlayerAvartaWeapon2.sprite = image;
            PlayerAvartaWeapon_Sprite.sprite = image;
        }
        
      
        PlayerAvartaWeapon1.enabled = true;
        PlayerAvartaWeapon2.enabled = true;
        PlayerAvartaWeapon_Sprite.enabled = true;
    }


    public void SetPetImage_Remove()
    {
        PlayerPet_Path = null;
        PlayerPet1.enabled = false;
        PlayerPet2.enabled = false;
        PlayerPet_sprite.enabled = false;
        
    }
    
    //보조무기
    public void SetpetImage(Sprite image=null, string path=null)
    {
        if (PlayerBackendData.Instance.nowPetid != "")
        {
            PetDB.Row data = PetDB.Instance.Find_id(PlayerBackendData.Instance.nowPetid);
            PlayerPet_Path = data.sprite;
            PlayerPet1.sprite = SpriteManager.Instance.GetSprite(data.sprite);
            PlayerPet2.sprite =  SpriteManager.Instance.GetSprite(data.sprite);
            PlayerPet_sprite.sprite =  SpriteManager.Instance.GetSprite(data.sprite);
        }
        else
        {
            PlayerPet_Path = path;
            PlayerPet1.sprite = image;
            PlayerPet2.sprite = image;
            PlayerPet_sprite.sprite = image;
        }
        
      
        PlayerPet1.enabled = true;
        PlayerPet2.enabled = true;
        PlayerPet_sprite.enabled = true;
        PartyraidChatManager.Instance.Chat_ChangeVisual();

    }
    
    public void SetWeaponImage_remove()
    {
        PlayerAvartaWeapon_Path = null;
        PlayerAvartaWeapon1.enabled = false;
        PlayerAvartaWeapon2.enabled = false;
        PlayerAvartaWeapon_Sprite.enabled = false;
    }
    //보조무기
    public void SetSubWeaponImage(Sprite image=null, string path=null)
    {
        if (PlayerBackendData.Instance.avata_subweapon != "")
        {
            AvartaDB.Row data = AvartaDB.Instance.Find_id(PlayerBackendData.Instance.avata_subweapon);
            PlayerAvartaSubWeapon_Path = data.sprite;
            PlayerAvartaSubWeapon1.sprite = SpriteManager.Instance.GetSprite(data.sprite);
            PlayerAvartaSubWeapon2.sprite =  SpriteManager.Instance.GetSprite(data.sprite);
            PlayerAvartaSubWeapon_Sprite.sprite =  SpriteManager.Instance.GetSprite(data.sprite);
        }
        else
        {
            PlayerAvartaSubWeapon_Path = path;
            PlayerAvartaSubWeapon1.sprite = image;
            PlayerAvartaSubWeapon2.sprite = image;
            PlayerAvartaSubWeapon_Sprite.sprite = image;
        }
        
      
        PlayerAvartaSubWeapon1.enabled = true;
        PlayerAvartaSubWeapon2.enabled = true;
        PlayerAvartaSubWeapon_Sprite.enabled = true;
        PartyraidChatManager.Instance.Chat_ChangeVisual();

    }

    public void SetSubWeaponImage_remove()
    {
        PlayerAvartaSubWeapon_Path = null;
        PlayerAvartaSubWeapon1.enabled = false;
        PlayerAvartaSubWeapon2.enabled = false;
        PlayerAvartaSubWeapon_Sprite.enabled = false;
    }
    public void SetSubWeaponImage(Image images,Sprite image)
    {
        images.sprite = image;
        images.enabled = true;
    }

    public void SetSubWeaponImage_remove(Image images)
    {
        images.enabled = false;
    }
    //위장

    public void SetRefreshAllAvarta()
    {
        SetAvartaImage();
        SetWeaponImage();
        SetSubWeaponImage();
    }
    public void SetAvartaImage(Sprite image=null, string path=null)
    {
        if (PlayerBackendData.Instance.avata_avata != "")
        {
            AvartaDB.Row data = AvartaDB.Instance.Find_id(PlayerBackendData.Instance.avata_avata);
            PlayerAvarta_Path = data.sprite;
            PlayerAvarta1.sprite = SpriteManager.Instance.GetSprite(data.sprite);
            PlayerAvarta2.sprite =  SpriteManager.Instance.GetSprite(data.sprite);
            PlayerAvarta_Sprite.sprite =  SpriteManager.Instance.GetSprite(data.sprite);
        }
        else
        {
            PlayerAvarta_Path = path;
            PlayerAvarta1.sprite = image;
            PlayerAvarta2.sprite = image;
            PlayerAvarta_Sprite.sprite = image;
        }

        PlayerAvarta1.enabled = true;
        PlayerAvarta2.enabled = true;
        PlayerAvarta_Sprite.enabled = true;
        PlayerAvartaSubWeapon1.enabled = true;
        PlayerAvartaSubWeapon2.enabled = true;
        PlayerAvartaSubWeapon_Sprite.enabled = true;
        PlayerAvartaWeapon1.enabled = true;
        PlayerAvartaWeapon2.enabled = true;
        PlayerAvartaWeapon_Sprite.enabled = true;
        PartyraidChatManager.Instance.Chat_ChangeVisual();
    }

    public void SetAvartaImage(Image images ,Sprite image)
    {
        images.sprite = image;
        images.enabled = true;
    }

    public void SetAvartaImage_remove()
    {
        PlayerAvarta_Path = null;
        PlayerAvarta1.enabled = false;
        PlayerAvarta2.enabled = false;
        PlayerAvarta_Sprite.enabled = false;
    }

    #endregion

    public Text PlayerExpText;
    public void RefreshExp()
    {
        PlayerExp.fillAmount = (0.14f + (float)(PlayerBackendData.Instance.GetExp() / PlayerBackendData.Instance.GetMaxExp()));
        PlayerExpText.text = $"{PlayerBackendData.Instance.GetExp():N0} / {PlayerBackendData.Instance.GetMaxExp():N0}";
        Level_exp.text = $"{((PlayerBackendData.Instance.GetExp() / PlayerBackendData.Instance.GetMaxExp()) * 100):N2}%";
        Level_slider.value = PlayerExp.fillAmount;
        CheckLvup();
    }

    public void RefreshAchExp()
    {
        Level_Achexp.text =
            $"{PlayerBackendData.Instance.GetAchExp().ToString("N0")}/{PlayerBackendData.Instance.GetMaxAchExp().ToString("N0")}";
        Level_Achslider.fillAmount = (float)PlayerBackendData.Instance.GetAchExp() / (float)PlayerBackendData.Instance.GetMaxAchExp();
        //CheckLvup();
    }

    //레벨및 경험치등
    public void RefreshInitData()
    {
        PlayerName.text = PlayerBackendData.Instance.nickname;
        PlayerName2.text = PlayerBackendData.Instance.nickname;
      //  Debug.Log(PlayerBackendData.Instance.ClassId);
//        Debug.Log(ClassDB.Instance.Find_id(PlayerBackendData.Instance.ClassId).name);
        PlayerJob.text =
            $"{Inventory.GetTranslate(ClassDB.Instance.Find_id(PlayerBackendData.Instance.ClassId).name)}";
        PlayerJob2.text =
            $"{Inventory.GetTranslate(ClassDB.Instance.Find_id(PlayerBackendData.Instance.ClassId).name)}";
        
        Inventory.Instance.ChangeItemRareColor(PlayerJob,ClassDB.Instance.Find_id(PlayerBackendData.Instance.ClassId).tier);
        Inventory.Instance.ChangeItemRareColor(PlayerJob2,ClassDB.Instance.Find_id(PlayerBackendData.Instance.ClassId).tier);

        
        //PlayerLv.text = $"Lv.{PlayerBackendData.Instance.GetLv().ToString()}";
        Level_lv.text = $"Lv.{PlayerBackendData.Instance.GetLv().ToString()}";
        Level_Achlv.text = $"Lv.{PlayerBackendData.Instance.GetAchLv().ToString()}";
        string stat = AchievementStatDB.Instance.Find_id(PlayerBackendData.Instance.GetAchLv().ToString()).str;
        AchStat.text = string.Format(Inventory.GetTranslate("UI/업적 능력치 상세"), stat);

//////        Debug.Log(LevelDB.Instance.Find_Lv(PlayerBackendData.Instance.GetLv().ToString()).RequiredExp);
        //맥스경험치
        PlayerBackendData.Instance.SetMaxExp(decimal.Parse(LevelDB.Instance.Find_Lv(PlayerBackendData.Instance.GetLv().ToString()).RequiredExp));

        //맥스경험치 ㅇ업적
      //  PlayerBackendData.Instance.SetMaxAchExp(decimal.Parse(AchievementStatDB.Instance.Find_level(PlayerBackendData.Instance.GetAchLv().ToString()).EXP));
    }

    public void RefreshClassName()
    {
        PlayerJob.text =
            $"{Inventory.GetTranslate(ClassDB.Instance.Find_id(PlayerBackendData.Instance.ClassId).name)}";
        PlayerJob2.text =
            $"{Inventory.GetTranslate(ClassDB.Instance.Find_id(PlayerBackendData.Instance.ClassId).name)}";
        Inventory.Instance.ChangeItemRareColor(PlayerJob,ClassDB.Instance.Find_id(PlayerBackendData.Instance.ClassId).tier);
        Inventory.Instance.ChangeItemRareColor(PlayerJob2,ClassDB.Instance.Find_id(PlayerBackendData.Instance.ClassId).tier);

    }

    public string gettierstarText(string transkey,string tier)
    {
        return string.Format(Inventory.GetTranslate(transkey),tier);
    }
    
    public string gettierstar(string tier)
    {
        return tier;
    }

    public string getRank(string Rank)
    {
        switch (Rank)
        {
            case "1":
                return Inventory.GetTranslate("UI/1차");
            case "2":
                return Inventory.GetTranslate("UI/2차");
            case "3":
                return Inventory.GetTranslate("UI/3차");
            case "4":
                return Inventory.GetTranslate("UI/4차");
            case "5":
                return Inventory.GetTranslate("UI/5차");
            case "6":
                return Inventory.GetTranslate("UI/6차");
            case "7":
                return Inventory.GetTranslate("UI/7차");
            case "8":
                return Inventory.GetTranslate("UI/8차");
            case "9":
                return Inventory.GetTranslate("UI/9차");
            case "10":
                return Inventory.GetTranslate("UI/10차");
            case "11":
                return Inventory.GetTranslate("UI/11차");
            case "12":
                return Inventory.GetTranslate("UI/12차");
            case "13":
                return Inventory.GetTranslate("UI/13차");
            case "14":
                return Inventory.GetTranslate("UI/14차");
            case "15":
                return Inventory.GetTranslate("UI/15차");
        }
        return "0";
    }

    private void Awake()
    {
        RefreshInitData();
        RefreshExp();
        RefreshAchExp();
        PlayerGold.text = dpsmanager.convertNumber(PlayerBackendData.Instance.GetMoney());
        PlayerCash.text = PlayerBackendData.Instance.GetCash().ToString("N0");
        RefreshCashes();
        RefreshMyBattlePoint();
    }

    void RefreshCashes()
    {
        foreach (var t in PlayerCashs)
        {
            t.SetData();
        }
    }

    IEnumerator Count(moneyenum moneytype, Text text, decimal target, decimal current)
    {
        decimal duration = 0.5m; // 카운팅에 걸리는 시간 설정. 
        decimal offset = (target - current) / duration;
        switch (moneytype)
        {
            case moneyenum.골드:
                PlayerBackendData.Instance.SetMoney(target);
                break;
            case moneyenum.불꽃:
                PlayerBackendData.Instance.SetCash(target);
                break;
        }

        while (current < target)
        {
            current += (decimal)offset * (decimal)Time.deltaTime;
            switch (moneytype)
            {
                case moneyenum.골드:
                    PlayerGold.text =  dpsmanager.convertNumber((current));
                    break;
                case moneyenum.불꽃:
                    PlayerCash.text = (current).ToString("N0");
                    break;
            }

            yield return null;
        }



        current = target;
        RefreshCashes();
        switch (moneytype)
        {
            case moneyenum.골드:
                PlayerGold.text = dpsmanager.convertNumber(current);
                break;
            case moneyenum.불꽃:
                PlayerCash.text = (current).ToString("N0");
                break;
        }
    }

    public void UpGold(decimal money)
    {
        StartCoroutine(Count(moneyenum.골드,PlayerGold, PlayerBackendData.Instance.GetMoney()+money, PlayerBackendData.Instance.GetMoney()));
        Savemanager.Instance.SaveMoneyData();
    }
    public void UpGoldMon(decimal money)
    {
        batterysaver.Instance.AdditemGoldExp("1000", GetGold(money));
        StartCoroutine(Count(moneyenum.골드,PlayerGold, PlayerBackendData.Instance.GetMoney()+GetGold(money), PlayerBackendData.Instance.GetMoney()));
        //Savemanager.Instance.SaveMoneyData();
    }
    public void UpCash(decimal cash)
    {
        LogManager.EarnCrystal += (int)cash;
        StartCoroutine(Count(moneyenum.불꽃, PlayerCash, PlayerBackendData.Instance.GetCash() + cash, PlayerBackendData.Instance.GetCash()));
        Savemanager.Instance.SaveCash();

    }

    public void DownCash(decimal cash)
    {
        StartCoroutine(Count(moneyenum.불꽃, PlayerCash, PlayerBackendData.Instance.GetCash() - cash, PlayerBackendData.Instance.GetCash()));
    }
    public void DownGold(decimal gold)
    {
        StartCoroutine(Count(moneyenum. 골드, PlayerGold, PlayerBackendData.Instance.GetMoney() - gold, PlayerBackendData.Instance.GetMoney()));
    }
  
    public decimal GetGold(decimal Gold)
    {
        decimal count =Gold;
        //Debug.Log("기본 골드" +  count);
        count += (decimal)(count * (decimal)mainplayer.Stat_ExtraGold);
        //Debug.Log("추가 후 골드" +  count);

        return count;
    }

    public bool isexpeventon; 
    public decimal GetExp(decimal Exp)
    {
        decimal count = Exp;
    //    Debug.Log("기본 경험치" +  count);
        count += (decimal)(count * (decimal)mainplayer.Stat_ExtraExp);
      //  Debug.Log("추가 경험치 계산" +  count);
        if (PlayerBackendData.Instance.ispremium)
        {
            if (PlayerBackendData.Instance.GetLv() < 1200)
            {
                count *= 2.5m;
            }
            else
            {
                count *= 1.5m;
            }
       //     Debug.Log("엘리 적용 경험치 계산" +  count);
        }

        if (isexpeventon)
        {
            count *= 2m;
        }
        return count;
    }

    public void EarnExp(decimal exp, bool isave = true)
    {
        decimal exps = GetExp(exp);
        PlayerBackendData.Instance.AddExp(exps);
        batterysaver.Instance.AdditemGoldExp("1002", exps);

        if (AutoLvUpToggle.isOn)
            bt_LvupbuttonAuto();
        RefreshExp();
        if (isave)
            Savemanager.Instance.SaveOnlyExp();
    }
    public void EarnExpNoPre(decimal exp, bool isave = true)
    {
        PlayerBackendData.Instance.AddExp(exp);
        if (AutoLvUpToggle.isOn)
            bt_LvupbuttonAuto();
        RefreshExp();
        if (isave)
            Savemanager.Instance.SaveOnlyExp();
    }
    public void EarnExpNoPreGUIDE(decimal exp, bool isave = true)
    {
        PlayerBackendData.Instance.AddExp(exp);
        if (AutoLvUpToggle.isOn)
            bt_LvupbuttonAuto2();
    }
    public void EarnAchExp(decimal exp)
    {
        if (PlayerBackendData.Instance.GetAchLv() >= 1200)
        {
            
            return;
        }
        
        PlayerBackendData.Instance.AddAchExp(exp);
        bt_LvupbuttonAch();
        RefreshAchExp();
        Savemanager.Instance.SaveExpData();
    }
    public void EarnSeaSonPassExp(decimal exp)
    {
        PlayerBackendData.Instance.AddSeasonPassExp(exp);
        SeasonPass.Instance.Refresh();
        Savemanager.Instance.SaveExpData();
    }

    public enum moneyenum
    {
        골드,
        불꽃,
        Length
    }

    //하단정보
    public UIButton lvupbutton;
    public Text Level_exp;
    public Text Level_Achexp;
    public Text Level_lv;
    public Text Level_ADlv;
    public Text Level_Achlv;
    public Text AchStat;
    public Slider Level_slider;
    public Image Level_Achslider;

    public Text Battlepoint;
    public Text BattlepointEquipBag;


    public void RefreshMyBattlePoint()
    {
        Battlepoint.text = GetEquipPoint(PlayerBackendData.Instance.EquipEquiptment0).ToString("N0");
        PlayerStat_info[(int)statenum.장비점수].text = Battlepoint.text;
        BattlepointEquipBag.text = Battlepoint.text;
        
        //장비 제련 점수
        
        
    }
    public int GetEquipPoint(EquipDatabase[] datas)
    {
        int num = 0;
        for (int i = 0; i < datas.Length; i++)
        {
            if (datas[i] != null)
            {
                num += datas[i].GetBattlePoint();
            }
        }

        return num;
    }
    
    
    public void  CheckLvup()
    {
        if (PlayerBackendData.Instance.GetExp() >= PlayerBackendData.Instance.GetMaxExp())
        {
            lvupbutton.Interactable = true;
        }
        else
        {
            lvupbutton.Interactable = false;
        }
    }
    public Toggle AutoLvUpToggle;

    private void bt_LvupbuttonAuto()
    {
        bool islvup = false;
        while (true)
        {
            if (PlayerBackendData.Instance.GetExp() >= PlayerBackendData.Instance.GetMaxExp())
            {
                islvup = true;
                //레벨업가능
                PlayerBackendData.Instance.AddExp(-PlayerBackendData.Instance.GetMaxExp());
                PlayerBackendData.Instance.AddLv(1);
                DamageManager.Instance.ShowEffect_LvUp(mainplayer.hpmanager.Effecttrans);
                RefreshInitData();
                RefreshExp();
                RefreshAchExp();
                RefreshPlayerstat();
                if (PlayerBackendData.Instance.GetLv() == 900)
                {
                    Tutorialmanager.Instance.review.SetActive(true);
                }
            }
            else
            {
                lvupbutton.Interactable = false;
                break;
            }
        }

        if (islvup)
            Savemanager.Instance.SaveOnlyLv();
    }

    private void bt_LvupbuttonAuto2()
    {
        bool islvup = false;
        while (true)
        {
            if (PlayerBackendData.Instance.GetExp() >= PlayerBackendData.Instance.GetMaxExp())
            {
                islvup = true;
                //레벨업가능
                PlayerBackendData.Instance.AddExp(-PlayerBackendData.Instance.GetMaxExp());
                PlayerBackendData.Instance.AddLv(1);
                PlayerBackendData.Instance.SetMaxExp(decimal.Parse(LevelDB.Instance.Find_Lv(PlayerBackendData.Instance.GetLv().ToString()).RequiredExp));
                RefreshExp();
            }
            else
            {
                lvupbutton.Interactable = false;
                break;
            }
        }
    }
    
    public void bt_LvupbuttonAch()
    {
        while (true)
        {
            if (PlayerBackendData.Instance.GetAchExp() >= PlayerBackendData.Instance.GetMaxAchExp())
            {
                //레벨업가능
                PlayerBackendData.Instance.AddAchExp(-PlayerBackendData.Instance.GetMaxAchExp());
                PlayerBackendData.Instance.AddAchLv(1);
                //DamageManager.Instance.ShowEffect_LvUp(mainplayer.hpmanager.Effecttrans);
                RefreshInitData();
                RefreshAchExp();
                RefreshPlayerstat();
                Savemanager.Instance.SaveAchieve();
                Savemanager.Instance.SaveExpData();
            }
            else
            {
                lvupbutton.Interactable = false;
                break;
            }
        }
    }
    
   
    
    public void bt_Lvupbutton()
    {
        if (PlayerBackendData.Instance.GetExp() >= PlayerBackendData.Instance.GetMaxExp())
        {
            //레벨업가능
            PlayerBackendData.Instance.AddExp(-PlayerBackendData.Instance.GetMaxExp());
            PlayerBackendData.Instance.AddLv(1);
            DamageManager.Instance.ShowEffect_LvUp(mainplayer.hpmanager.Effecttrans);
            RefreshInitData();
            RefreshExp();
            RefreshPlayerstat();
            Savemanager.Instance.SaveExpData();
        }
        else
        {
            lvupbutton.Interactable = false;
        }
    }
    public Player mainplayer;
    private void Start()
    {
        mainplayer = Battlemanager.Instance.mainplayer;
        RefreshPlayerstat();
        mainplayer.hpmanager.CurHp = mainplayer.hpmanager.MaxHp;
        mainplayer.hpmanager.CurMp = mainplayer.hpmanager.MaxMp;
        mainplayer.hpmanager.RefreshHp();
        StartCoroutine(SaveAuto());
        LogManager.Instance.CheckBug();
    }

    //플레이어 스탯
    public Text[] PlayerStat;
    private static readonly int Show = Animator.StringToHash("show");

    public Text[] PlayerStat_info;

    
    public float[] GetPlayerStatForSave()
    {
        float[] stat = new float[20];
        stat[0] = mainplayer.stat_atk;
        stat[1] = mainplayer.stat_matk;
        stat[2] = (mainplayer.Stat_DotDmgUp) * 100f;
        stat[3] = mainplayer.stat_hp;
        stat[4] =  mainplayer.stat_mp;
        stat[5] = mainplayer.stat_str;
        stat[6] = mainplayer.stat_dex;
        stat[7] = mainplayer.stat_int;
        stat[8] = mainplayer.stat_wis;
        stat[9] = mainplayer.AttackCount;
        stat[10] = (mainplayer.stat_atkspeed) * 100f;
        stat[11] = mainplayer.stat_crit;
        stat[12] = (mainplayer.stat_critdmg) * 100f;
        stat[13] = (mainplayer.stat_castspeed ) * 100f;
        stat[14] = (mainplayer.Stat_ReduceCoolDown) * 100f;
        stat[15] = (mainplayer.stat_Bossdmg) * 100f;
        stat[16] = (mainplayer.AlldmgUp + Battlemanager.Instance.mainplayer.Stat_SmeltDmg) * 100f;

        return stat;
    }

    public GameObject[] DamagerTypeObj; //물리냐 마법이냐
    public void RefreshPlayerstat()
    {
        mainplayer.RefreshStat();
        AdventureTier();

        PlayerStat_info[(int)statenum.레벨].text = PlayerBackendData.Instance.GetLv().ToString();
        PlayerStat_info[(int)statenum.모험랭크].text =  PlayerBackendData.Instance.GetAdLv().ToString();
        PlayerStat_info[(int)statenum.능력치총합].text = (mainplayer.stat_str + mainplayer.stat_dex + mainplayer.stat_int + mainplayer.stat_wis).ToString("N0"); 

        PlayerStat_info[(int)statenum.피해증가].text = $"{((mainplayer.AlldmgUp+mainplayer.Stat_SmeltDmg) * 100f):N0}%";
        PlayerStat_info[(int)statenum.버프효율증가].text = $"{(mainplayer.Stat_totalbuff * 100f):N0}%";
        PlayerStat_info[(int)statenum.버프효율증가].color = Color.white;
        
        
        PlayerStat[(int)playerstatenum.힘].text = mainplayer.stat_str.ToString("N0");
        PlayerStat_info[(int)statenum.힘].text = PlayerStat[(int)playerstatenum.힘].text;
        if (mainplayer.buff_str > 0)
        {
            PlayerStat[(int)playerstatenum.힘].color = Color.cyan;
            PlayerStat_info[(int)statenum.힘].color = Color.cyan;
            
        }
        else
        {
            PlayerStat[(int)playerstatenum.힘].color = Color.white;
            PlayerStat_info[(int)statenum.힘].color = Color.white;
        }
        
        PlayerStat[(int)playerstatenum.민첩].text = mainplayer.stat_dex.ToString("N0");
        PlayerStat_info[(int)statenum.민첩].text = PlayerStat[(int)playerstatenum.민첩].text;
        if (mainplayer.buff_dex > 0)
        {
            PlayerStat[(int)playerstatenum.민첩].color = Color.cyan;
            PlayerStat_info[(int)statenum.민첩].color = Color.cyan;
            
        }
        else
        {
            PlayerStat[(int)playerstatenum.민첩].color = Color.white;
            PlayerStat_info[(int)statenum.민첩].color = Color.white;
        }
        
        PlayerStat[(int)playerstatenum.지능].text = mainplayer.stat_int.ToString("N0");
        PlayerStat_info[(int)statenum.지능].text = PlayerStat[(int)playerstatenum.지능].text;
        if (mainplayer.buff_int > 0)
        {
            PlayerStat[(int)playerstatenum.지능].color = Color.cyan;
            PlayerStat_info[(int)statenum.지능].color = Color.cyan;
            
        }
        else
        {
            PlayerStat[(int)playerstatenum.지능].color = Color.white;
            PlayerStat_info[(int)statenum.지능].color = Color.white;
        }
        
        PlayerStat[(int)playerstatenum.지혜].text = mainplayer.stat_wis.ToString("N0");
        PlayerStat_info[(int)statenum.지혜].text = PlayerStat[(int)playerstatenum.지혜].text;
        if (mainplayer.buff_wis > 0)
        {
            PlayerStat[(int)playerstatenum.지혜].color = Color.cyan;
            PlayerStat_info[(int)statenum.지혜].color = Color.cyan;
            
        }
        else
        {
            PlayerStat[(int)playerstatenum.지혜].color = Color.white;
            PlayerStat_info[(int)statenum.지혜].color = Color.white;
        }
        
        PlayerStat[(int)playerstatenum.물리공격력].text = dpsmanager.convertNumber((decimal)mainplayer.stat_atk);
        PlayerStat_info[(int)statenum.물공].text = PlayerStat[(int)playerstatenum.물리공격력].text;

        if (mainplayer.buff_atk > 0 || mainplayer.buff_atkPercent > 0)
        {
            PlayerStat[(int)playerstatenum.물리공격력].color = Color.cyan;
            PlayerStat_info[(int)statenum.물공].color = Color.cyan;
        }
        else
        {
            PlayerStat[(int)playerstatenum.물리공격력].color = Color.white;
            PlayerStat_info[(int)statenum.물공].color = Color.white;
        }

        PlayerStat[(int)playerstatenum.마법공격력].text = dpsmanager.convertNumber((decimal)mainplayer.stat_matk);
        PlayerStat_info[(int)statenum.마공].text = PlayerStat[(int)playerstatenum.마법공격력].text;
        if (mainplayer.buff_matk > 0 || mainplayer.buff_matkPercent > 0)
        {
            PlayerStat[(int)playerstatenum.마법공격력].color = Color.cyan;
            PlayerStat_info[(int)statenum.마공].color = Color.cyan;
        }
        else
        {
            PlayerStat[(int)playerstatenum.마법공격력].color = Color.white;
            PlayerStat_info[(int)statenum.마공].color = Color.white;
        }

        
        PlayerStat[(int)playerstatenum.상태이상피해].text = $"{(mainplayer.Stat_DotDmgUp * 100f):N0}%";
        PlayerStat_info[(int)statenum.상태이상].text = PlayerStat[(int)playerstatenum.상태이상피해].text;
        if (mainplayer.buff_dotdmgup > 0)
        {
            PlayerStat[(int)playerstatenum.상태이상피해].color = Color.cyan;
            PlayerStat_info[(int)statenum.상태이상].color = Color.cyan;
        }
        else
        {
            PlayerStat[(int)playerstatenum.상태이상피해].color = Color.white;
            PlayerStat_info[(int)statenum.상태이상].color = Color.white;
        }
            
        PlayerStat[(int)playerstatenum.생명력].text = mainplayer.stat_hp.ToString("N0");
        PlayerStat_info[(int)statenum.생명력].text = mainplayer.stat_hp.ToString("N0");
        if (mainplayer.buff_hp > 0)
        {
            PlayerStat[(int)playerstatenum.생명력].color = Color.cyan;
            PlayerStat_info[(int)statenum.생명력].color = Color.cyan;

        }
        else
        {
            PlayerStat[(int)playerstatenum.생명력].color = Color.white;
            PlayerStat_info[(int)statenum.생명력].color = Color.white;
        }
        
        
        PlayerStat[(int)playerstatenum.정신력].text = mainplayer.stat_mp.ToString("N0");
        PlayerStat_info[(int)statenum.정신력].text = PlayerStat[(int)playerstatenum.정신력].text;
        if (mainplayer.buff_mp > 0)
        {
            PlayerStat[(int)playerstatenum.정신력].color = Color.cyan;
            PlayerStat_info[(int)statenum.정신력].color = Color.cyan;

        }
        else
        {
            PlayerStat[(int)playerstatenum.정신력].color = Color.white;
            PlayerStat_info[(int)statenum.정신력].color = Color.white;
        }

     

        
        PlayerStat[(int)playerstatenum.보스추가피해].text = $"{(mainplayer.stat_Bossdmg * 100f):N0}%";
        PlayerStat_info[(int)statenum.보스피해].text = PlayerStat[(int)playerstatenum.보스추가피해].text;
        if (mainplayer.buff_bossdmg > 0)
        {
            PlayerStat[(int)playerstatenum.보스추가피해].color = Color.cyan;
            PlayerStat_info[(int)statenum.보스피해].color = Color.cyan;
        }
        else
        {
            PlayerStat[(int)playerstatenum.보스추가피해].color = Color.white;
            PlayerStat_info[(int)statenum.보스피해].color = Color.white;
        }
          
        PlayerStat_info[(int)statenum.일반피해].text = $"{(mainplayer.stat_Monsterdmg * 100f):N0}%";
        if (mainplayer.buff_bossdmg > 0)
        {
            PlayerStat_info[(int)statenum.일반피해].color = Color.cyan;
        }
        else
        {
            PlayerStat_info[(int)statenum.일반피해].color = Color.white;
        }
        
        
        PlayerStat[(int)playerstatenum.치명타확률].text = $"{mainplayer.stat_crit:N0}%";
        PlayerStat_info[(int)statenum.치명타].text = PlayerStat[(int)playerstatenum.치명타확률].text;
        if (mainplayer.buff_crit > 0)
        {
            PlayerStat[(int)playerstatenum.치명타확률].color = Color.cyan;
            PlayerStat_info[(int)statenum.치명타].color = Color.cyan;
        }
        else
        {
            PlayerStat[(int)playerstatenum.치명타확률].color = Color.white;
            PlayerStat_info[(int)statenum.치명타].color = Color.white;
        }
        
        if (mapmanager.Instance.BossPenalty[1].activeSelf)
        {
            Debug.Log("치명타 감소");
            PlayerStat[(int)playerstatenum.치명타확률].color = Color.red;
            PlayerStat_info[(int)statenum.치명타].color = Color.red;
        }

        PlayerStat[(int)playerstatenum.치명타피해].text = $"{(mainplayer.stat_critdmg * 100f):N0}%";
        PlayerStat_info[(int)statenum.치명타피해].text = PlayerStat[(int)playerstatenum.치명타피해].text;
        if (mainplayer.buff_critdmg > 0)
        {
            PlayerStat[(int)playerstatenum.치명타피해].color = Color.cyan;
            PlayerStat_info[(int)statenum.치명타피해].color = Color.cyan;
        }
        else
        {
            PlayerStat[(int)playerstatenum.치명타피해].color = Color.white;
            PlayerStat_info[(int)statenum.치명타피해].color = Color.white;
        }

        
        
        PlayerStat[(int)playerstatenum.공격속도].text = $"{(mainplayer.stat_atkspeed * 100f):N0}%";
        PlayerStat_info[(int)statenum.공속].text = PlayerStat[(int)playerstatenum.공격속도].text;
        if (mainplayer.buff_atkspeed > 0)
        {
            PlayerStat[(int)playerstatenum.공격속도].color = Color.cyan;
            PlayerStat_info[(int)statenum.공속].color = Color.cyan;
        }
        else
        {
            PlayerStat[(int)playerstatenum.공격속도].color = Color.white;
            PlayerStat_info[(int)statenum.공속].color = Color.white;
        }
        
        PlayerStat[(int)playerstatenum.기본공격횟수].text = mainplayer.AttackCount.ToString("N0");
        PlayerStat_info[(int)statenum.기본공격횟수].text = PlayerStat[(int)playerstatenum.기본공격횟수].text;
        if (mainplayer.buff_def > 0)
        {
            PlayerStat[(int)playerstatenum.기본공격횟수].color = Color.cyan;
            PlayerStat_info[(int)statenum.기본공격횟수].color = Color.cyan;
        }
        else
        {
            PlayerStat[(int)playerstatenum.기본공격횟수].color = Color.white;
            PlayerStat_info[(int)statenum.기본공격횟수].color = Color.white;
        }
        
        PlayerStat_info[(int)statenum.기본공격피해].text = $"{(mainplayer.stat_basicatkup * 100f):N0}%";
        if (mainplayer.buff_basicatkup > 0)
        {
            PlayerStat_info[(int)statenum.기본공격피해].color = Color.cyan;
        }
        else
        {
            PlayerStat_info[(int)statenum.기본공격피해].color = Color.white;
        }
        
        
        PlayerStat_info[(int)statenum.재사용시간].text = $"{(mainplayer.Stat_ReduceCoolDown * 100f):N0}%";
        if (mainplayer.buff_reducecooldown > 0)
        {
            PlayerStat_info[(int)statenum.재사용시간].color = Color.cyan;
        }
        else
        {
            PlayerStat_info[(int)statenum.재사용시간].color = Color.white;
        }

        PlayerStat_info[(int)statenum.시속].text = $"{(mainplayer.stat_castspeed * 100f):N0}%";
        if (mainplayer.buff_castspeed > 0)
        {
            PlayerStat_info[(int)statenum.시속].color = Color.cyan;
        }
        else
        {
            PlayerStat_info[(int)statenum.시속].color = Color.white;
        }
        
        PlayerStat_info[(int)statenum.경험치].text = $"{(mainplayer.Stat_ExtraExp * 100f):N0}%";
        PlayerStat_info[(int)statenum.골드].text = $"{(mainplayer.Stat_ExtraGold * 100f):N0}%";
        PlayerStat_info[(int)statenum.드랍율].text = $"{(mainplayer.Stat_ExtraDrop * 100f):N0}%";
        
        if ((decimal)mainplayer.stat_atk < (decimal)mainplayer.stat_matk)
        {
            
            DamagerTypeObj[1].SetActive(true);
            DamagerTypeObj[0].SetActive(false);
        }
        else
        {
            DamagerTypeObj[0].SetActive(true);
            DamagerTypeObj[1].SetActive(false);
        }
       

        float temps = mainplayer.stat_str;
        int types = 0;
        if(temps < mainplayer.stat_dex)
        {
            temps = mainplayer.stat_dex;
            types = 1;
        }

        if(temps < mainplayer.stat_int)
        {
            temps = mainplayer.stat_int;
            types = 2;
        }
        if(temps < mainplayer.stat_wis)
        {
            types = 3;
        }
        switch (types)
        {
            case 0:
                PlayerStat[(int)playerstatenum.힘].color = Color.cyan;
                PlayerStat[(int)playerstatenum.민첩].color = Color.white;
                PlayerStat[(int)playerstatenum.지능].color = Color.white;
                PlayerStat[(int)playerstatenum.지혜].color = Color.white;
                break;
            case 1:
                PlayerStat[(int)playerstatenum.힘].color = Color.white;
                PlayerStat[(int)playerstatenum.민첩].color = Color.cyan;
                PlayerStat[(int)playerstatenum.지능].color = Color.white;
                PlayerStat[(int)playerstatenum.지혜].color = Color.white;
                break;
            case 2:
                PlayerStat[(int)playerstatenum.지능].color = Color.white;
                PlayerStat[(int)playerstatenum.민첩].color = Color.white ;
                PlayerStat[(int)playerstatenum.지능].color = Color.cyan;
                PlayerStat[(int)playerstatenum.지혜].color = Color.white;

                break;
            case 3:
                PlayerStat[(int)playerstatenum.지능].color = Color.white;
                PlayerStat[(int)playerstatenum.민첩].color = Color.white ;
                PlayerStat[(int)playerstatenum.지능].color = Color.white;
                PlayerStat[(int)playerstatenum.지혜].color = Color.cyan;
                break;
        }

        RefreshMyBattlePoint();
        RefreshSmelt();
    }
    public void RefreshPlayerstat_Equip()
    {
        RefreshPlayerstat();
        
        return;
        AdventureTier();
        PlayerStat[(int)playerstatenum.생명력].text = mainplayer.stat_hp.ToString("N0");
        PlayerStat[(int)playerstatenum.정신력].text = mainplayer.stat_mp.ToString("N0");
        PlayerStat[(int)playerstatenum.힘].text = mainplayer.stat_str.ToString("N0");
        PlayerStat[(int)playerstatenum.민첩].text = mainplayer.stat_dex.ToString("N0");
        PlayerStat[(int)playerstatenum.지능].text = mainplayer.stat_int.ToString("N0");
        PlayerStat[(int)playerstatenum.지혜].text = mainplayer.stat_wis.ToString("N0");
        PlayerStat[(int)playerstatenum.물리공격력].text = mainplayer.stat_atk.ToString("N0");
        PlayerStat[(int)playerstatenum.마법공격력].text = mainplayer.stat_matk.ToString("N0");
        PlayerStat[(int)playerstatenum.기본공격횟수].text = mainplayer.AttackCount.ToString("N0");
        PlayerStat[(int)playerstatenum.재사용시간].text = mainplayer.Stat_ReduceCoolDown.ToString("N0");
        PlayerStat[(int)playerstatenum.치명타확률].text = $"{mainplayer.stat_crit:N0}%";
        PlayerStat[(int)playerstatenum.치명타피해].text = $"{(mainplayer.stat_critdmg * 100f):N0}%";
        PlayerStat[(int)playerstatenum.공격속도].text = $"{(mainplayer.stat_atkspeed * 100f):N0}%";
        PlayerStat[(int)playerstatenum.시전속도].text = $"{(mainplayer.stat_castspeed * 100f):N0}%";
        PlayerStat[(int)playerstatenum.상태이상피해].text = $"{(mainplayer.Stat_DotDmgUp * 100f):N0}%";
        PlayerStat[(int)playerstatenum.보스추가피해].text = $"{(mainplayer.stat_Bossdmg * 100f):N0}%";
        RefreshMyBattlePoint();
        RefreshSmelt();
    }

 
    public void AdventureTier()
    {
        Level_ADlv.text = string.Format(Inventory.GetTranslate("UI/모험가티어"),gettierstar(PlayerBackendData.Instance.GetAdLv().ToString()));
    }

    public enum playerstatenum
    {
        생명력,
        정신력,
        힘,
        민첩,
        지능,
        지혜,
        물리공격력,
        마법공격력,
        기본공격횟수,
        재사용시간,
        치명타확률,
        치명타피해,
        공격속도,
        시전속도,
        상태이상피해,
        보스추가피해,
        length
    }
    
    public enum statenum
    {
        레벨,
        장비점수,
        모험랭크,
        피해증가 ,
        버프효율증가,
        능력치총합,
        힘,
        민첩,
        지능,
        지혜 ,
        물공 ,
        마공 ,
        상태이상,
        생명력,
        정신력,
        보스피해,
        일반피해,
        치명타,
        치명타피해,
        공속,
        기본공격횟수,
        기본공격피해,
        시속,
        재사용시간,
        경험치,
        골드,
        드랍율,
        length
    }
    
    //제련
    public Text SmeltPoint;
    public Text SmeltText;


    public void RefreshSmelt()
    {
        SmeltPoint.text = mainplayer.Stat_SmeltPoint.ToString("N0");
        SmeltText.text = string.Format(Inventory.GetTranslate("UI6/피해 증가"), (mainplayer.Stat_SmeltDmg*100f).ToString("N0"));
    }

}