using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class PetSlot : MonoBehaviour
{
    public string Petid;
    public string Rare;
    public GameObject[] StarBack;
    public GameObject[] StarCount;

    public GameObject isequip;
    
    public Text havecounttext;
    public Text petname;

    public Image petimage;
    public Image petrare;
    public GameObject haveobj;

    public GameObject SmeltPanel;
    public Text SmeltCount;
    
    public void Init(string petid)
    {
        SmeltPanel.SetActive(false);
        Petid = petid;
        foreach (var t in StarBack)
        {
            t.SetActive(false);
        }
        foreach (var t in StarCount)
        {
            t.SetActive(false);
        }
        if (PlayerBackendData.Instance.PetData.Keys.Contains(petid))
        {
            petdatabase data = PlayerBackendData.Instance.PetData[petid];
            //별최대개수
            for (int i = 0; i < int.Parse(PetDB.Instance.Find_id(data.Petid).starcount); i++)
            {
                StarBack[i].SetActive(true);
            }
            //별개수
            for (int i = 0; i < data.Petstar; i++)
            {
                StarCount[i].SetActive(true);
            }

            Rare = PetDB.Instance.Find_id(data.Petid).rare;
            petimage.sprite = SpriteManager.Instance.GetSprite(PetDB.Instance.Find_id(data.Petid).sprite);
            petrare.color = Inventory.Instance.GetRareColor(Rare);
            petname.text = Inventory.GetTranslate(PetDB.Instance.Find_id(petid).name);
            petname.color = petrare.color;
            if (data.Ishave)
            {
                if (data.Isequip)
                {
                    //장착중이면
                    isequip.SetActive(true);
                }
                else
                {
                    isequip.SetActive(false);
                }

                haveobj.SetActive(false);
                //가진개수
                if (data.Havecount > 0)
                    havecounttext.text = $"X{data.Havecount}";
                else
                {
                    havecounttext.text = "";
                }
                petimage.color = Color.white;
            }
            else
            {
                petimage.color = Color.gray;
                havecounttext.text = "";
                haveobj.SetActive(true);
            }
        }
    }

    public void RefreshSmelt(int count)
    {
        if (count > 0)
        {
            SmeltPanel.SetActive(true);
            SmeltCount.text = $"x{count.ToString()}";
        }
        else
            SmeltPanel.SetActive(false);
    }

    public void Bt_SmeltAll()
    {
        if (PlayerBackendData.Instance.PetData[Petid].Havecount > 0)
        {
            petmanager.Instance.Bt_SellectSmeltingData(Petid, true);
            petmanager.Instance.Bt_SellectSmeltingData(Petid, true);
        }
    }
    public void Bt_SmeltPlus()
    {
        petmanager.Instance.Bt_SellectSmeltingData(Petid,false);
    } 
    public void Bt_SmeltMinus()
    {
        petmanager.Instance.Bt_SellectSmeltingData(Petid,false,true);
    }
    
    public void Bt_ShowPet()
    {
        switch (petmanager.Instance.panelstate)
        {
            case 0://기본
                petmanager.Instance.Bt_ShowPet(Petid);
                break;
            case 1://합성
                if (PlayerBackendData.Instance.PetData[Petid].Havecount > 0)
                    petmanager.Instance.EndSelectMixPet(Petid);
                break;
            case 2://분해
                if (PlayerBackendData.Instance.PetData[Petid].Havecount > 0)
                {
                    petmanager.Instance.Bt_SellectSmeltingData(Petid);
                }
                break;
        }
    }
}
