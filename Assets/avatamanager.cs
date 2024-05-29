 using System.Collections;
using System.Collections.Generic;
 using Doozy.Engine.UI;
 using UnityEngine;
using UnityEngine.UI;

public class avatamanager : MonoBehaviour
{


    private static avatamanager _instance = null;


    public static avatamanager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(avatamanager)) as avatamanager;

                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }

            return _instance;
        }
    }
    public UIView View;

    public Dictionary<string, avartaslot> listslot = new Dictionary<string, avartaslot>();

    public avartaslot avaobj;

    public Transform A_trans;
    public Transform W_trans;

    public Transform S_trans;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < AvartaDB.Instance.NumRows(); i++)
        {
            if (AvartaDB.Instance.GetAt(i).isshow.Equals("TRUE"))
            {
                avartaslot obj = Instantiate(avaobj);
                obj.Init(AvartaDB.Instance.GetAt(i).id);
                switch (AvartaDB.Instance.GetAt(i).type)
                {
                    case "A":
                        obj.transform.transform.SetParent(A_trans);
                        break;
                    case "W":
                        obj.transform.transform.SetParent(W_trans);
                        break;
                    case "S":
                        obj.transform.transform.SetParent(S_trans);
                        break;
                }

                obj.gameObject.SetActive(true);
                listslot.Add(AvartaDB.Instance.GetAt(i).id, obj);
                obj.transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }
    }


    public GameObject selectavata;

    public string selectavataid;
    public Image selectAvataimage;
    public Text selectavataname;
    public Text selectavatainfo;
    public Text selectavatagetinfo;

    public void Bt_ShowAvata(string id)
    {
        selectavataid = id;
        AvartaDB.Row data = AvartaDB.Instance.Find_id(id);
        selectAvataimage.sprite = SpriteManager.Instance.GetSprite(data.sprite);
        selectavataname.text = Inventory.GetTranslate(data.name);
        selectavataname.color = Inventory.Instance.GetRareColor(data.rare);
        selectavatainfo.text = Inventory.GetTranslate(data.statinfo);
        selectavatagetinfo.text = Inventory.GetTranslate(data.getinfo);


        bool isequip = false;
        switch (AvartaDB.Instance.Find_id(id).type)
        {
            case "A":
                if (PlayerBackendData.Instance.avata_avata.Equals(id))
                {
                    isequip = true;
                }

                break;
            case "W":
                if (PlayerBackendData.Instance.avata_weapon.Equals(id))
                {
                    isequip = true;
                }

                break;
            case "S":
                if (PlayerBackendData.Instance.avata_subweapon.Equals(id))
                {
                    isequip = true;
                }

                break;
        }

        AlreadyEquip.SetActive(false);
        Equipbutton.SetActive(false);
        CrystalBuy.SetActive(false);
        ItemBuy.SetActive(false);

        if (isequip)
        {
            AlreadyEquip.SetActive(true);
        }
        else
        {
            //ÀåÂø ¾ÈÇÔ
            if (!PlayerBackendData.Instance.playeravata[int.Parse(AvartaDB.Instance.Find_id(id).num)])
            {
                //¾øÀ½
                switch (AvartaDB.Instance.Find_id(id).costtype)
                {
                    case "crystal":
                        Avartacost.text = AvartaDB.Instance.Find_id(id).cost;
                        CrystalBuy.SetActive(true);
                        break;
                    case "item":
                        ItemBuy.SetActive(true);
                        break;
                }
            }
            else
            {
                Equipbutton.SetActive(true);
            }


        }

        selectavata.SetActive(true);

    }

    public Text Avartacost;
    public GameObject AlreadyEquip; //ÀÌ¹Ì ÀåÂø
    public GameObject Equipbutton; //ÀåÂøÇÏ±â
    public GameObject CrystalBuy; //Å©¸®½ºÅ»±¸¸Å
    public GameObject ItemBuy; //¾ÆÀÌÅÛ È¹µæ

    public GameObject Crystalbuypanel;
    public Text Crystalbuytext;

    public void Bt_Crystalbuy()
    {
        Crystalbuypanel.SetActive(true);
        Crystalbuytext.text = Avartacost.text;
    }


    public Image Earn_Avata;
    public GameObject Earn_Avata_WeaponBody;
    public Text Earn_AvataName;
    public Animator ani;
    private static readonly int Show = Animator.StringToHash("show");

    public void EarnShowAvata(string id)
    {
        Earn_Avata_WeaponBody.SetActive(false);
        switch (AvartaDB.Instance.Find_id(id).type)
        {
            case "W":
            case "S":
                Earn_Avata_WeaponBody.SetActive(true);
                break;
        }

        Earn_AvataName.text = Inventory.GetTranslate(AvartaDB.Instance.Find_id(id).name);
        Earn_Avata.sprite = SpriteManager.Instance.GetSprite(AvartaDB.Instance.Find_id(id).sprite);
        ani.SetTrigger(Show);
    }

    public void EarnPet(string id)
    {
        Earn_Avata_WeaponBody.SetActive(false);
        Earn_AvataName.text = Inventory.GetTranslate(PetDB.Instance.Find_id(id).name);
        Earn_Avata.sprite = SpriteManager.Instance.GetSprite(PetDB.Instance.Find_id(id).sprite);
        ani.SetTrigger(Show);
    }


    public void Bt_ButAvata()
    {
        if (PlayerBackendData.Instance.GetCash() >= int.Parse(Crystalbuytext.text))
        {
            int n = int.Parse(AvartaDB.Instance.Find_id(selectavataid).num);

            PlayerData.Instance.DownCash(int.Parse(Crystalbuytext.text));
            PlayerBackendData.Instance.playeravata[n] = true;
            //ÀúÀå
            EarnShowAvata(selectavataid);
            Bt_ShowAvata(selectavataid);
            listslot[selectavataid].Refresh();
            Crystalbuypanel.SetActive(false);
        }

        PlayerData.Instance.RefreshPlayerstat();
        Savemanager.Instance.SaveCash();
        Savemanager.Instance.SaveAvataData();
        Savemanager.Instance.Save();
    }

    public void Bt_EquipAvata()
    {
        switch (AvartaDB.Instance.Find_id(selectavataid).type)
        {
            case "A":
                PlayerBackendData.Instance.avata_avata = selectavataid;
                PlayerData.Instance.SetAvartaImage();
                break;
            case "W":
                PlayerBackendData.Instance.avata_weapon = selectavataid;
                PlayerData.Instance.SetWeaponImage();
                break;
            case "S":
                PlayerBackendData.Instance.avata_subweapon = selectavataid;
                PlayerData.Instance.SetSubWeaponImage();
                break;
        }

        Bt_ShowAvata(selectavataid);
        foreach (var VARIABLE in listslot)
        {
            VARIABLE.Value.Refresh();
        }
        PartyraidChatManager.Instance.Chat_ChangeVisual();
        Savemanager.Instance.SaveAvataData();
        Savemanager.Instance.Save();
    }

    public void RefreshBt_()
    {
        foreach (var VARIABLE in listslot)
        {
            VARIABLE.Value.Refresh();
        }
    }

    public void UnBodyEquipAvata()
    {
        if(PlayerBackendData.Instance.avata_avata == "")
            return;
        PlayerBackendData.Instance.avata_avata = "";
        PlayerData.Instance.SetAvartaImage(
            SpriteManager.Instance.GetSprite(ClassDB.Instance.Find_id(PlayerBackendData.Instance.ClassId).classsprite),
            ClassDB.Instance.Find_id(PlayerBackendData.Instance.ClassId).classsprite);
        
        foreach (var VARIABLE in listslot)
        {
            VARIABLE.Value.Refresh();
        }
        PartyraidChatManager.Instance.Chat_ChangeVisual();
        Savemanager.Instance.SaveAvataData();
        Savemanager.Instance.Save();

    }

    public void UnWeaponEquipAvata()
    {
        if(PlayerBackendData.Instance.avata_weapon == "")
            return;
        PlayerBackendData.Instance.avata_weapon = "";
        PlayerData.Instance.SetWeaponImage(SpriteManager.Instance.GetSprite(EquipItemDB.Instance.Find_id(Inventory.Instance.data.Itemid).EquipSprite), EquipItemDB.Instance.Find_id(Inventory.Instance.data.Itemid).EquipSprite);

        foreach (var VARIABLE in listslot)
        {
            VARIABLE.Value.Refresh();
        }
        PartyraidChatManager.Instance.Chat_ChangeVisual();
        Savemanager.Instance.SaveAvataData();
        Savemanager.Instance.Save();
    }
}