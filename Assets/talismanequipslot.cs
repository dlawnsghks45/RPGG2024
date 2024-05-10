using UnityEngine;
using UnityEngine.UI;

public class talismanequipslot : MonoBehaviour
{
    public Image SetColor;
    private int num;
    public Image Image;
    public string keyid;
    public Image[] Eskill;
    public GameObject[] Stateobj; //0Àº ¾øÀ½ //1Àº ÀåÂøÇÔ //2´Â Àá±Ý

    public void refersh(int num)
    {
        this.num = num;

        foreach (var VARIABLE in Stateobj)
        {
            VARIABLE.SetActive(false);
        }
        foreach (var VARIABLE in Eskill)
        {
            VARIABLE.gameObject.SetActive(false);
        }
        SetColor.color =Color.white;
        /*
        if (!PlayerBackendData.Instance.TalismanLock[num])
        {
            //Àá±è
            PlayerBackendData.Instance.EquipTalisman[num] = "";
            Stateobj[2].SetActive(true);
            return;
        }
*/
//        Debug.Log(PlayerBackendData.Instance.GiveEquipTalismanData().Length);
        if (PlayerBackendData.Instance.GiveEquipTalismanData()[num] != null)
        {
            //ÀåÂøÇÔ

            Talismandatabase data = PlayerBackendData.Instance.GiveEquipTalismanData()[num];

            Image.sprite = SpriteManager.Instance.GetSprite(TalismanDB.Instance.Find_id(
                data.Itemid).sprite);
            Stateobj[1].SetActive(true);



            //Æ¯¼öÈ¿°úÃ¼Å©
            if (data.Eskill != null)
            {
                int colornum = 0;
                for (int i = 0; i < data.Eskill.Count; i++)
                {
                    Eskill[i].gameObject.SetActive(true);
                    Eskill[i].color = Inventory.Instance.GetRareColor(EquipSkillDB.Instance.Find_id(data.Eskill[i]).rare);
                    colornum += int.Parse(EquipSkillDB.Instance.Find_id(data.Eskill[i]).rare);
                }                    


                SetColor.color = TalismanManager.Instance.GetTalismanColor(colornum);
            }
        }
        else
        {
            //ÀåÂø¾ÈÇÔ
            Stateobj[0].SetActive(true);
        }
    }


    public void Bt_EquipTalisman(int num)
    {
        if (TalismanManager.Instance.isequiping)
        {
            TalismanManager.Instance.isequiping = false;
            TalismanManager.Instance.isequipingpanel.SetActive(false);
            //ÀåÂøÄ­ÀÌÄÑÁ³´Ù¸é
            if (PlayerBackendData.Instance.TalismanData.ContainsKey(TalismanManager.Instance.nowselectkey))
            {
                if (PlayerBackendData.Instance.TalismanPreset[PlayerBackendData.Instance.nowtalismanpreset]
                        .Talismanset[num] != null)
                {
                    //ÀåÂøÇÑ°É »­.
                    PlayerBackendData.Instance.TalismanData.Add(PlayerBackendData.Instance.TalismanPreset[PlayerBackendData.Instance.nowtalismanpreset]
                        .Talismanset[num].Keyid,PlayerBackendData.Instance.TalismanPreset[PlayerBackendData.Instance.nowtalismanpreset]
                        .Talismanset[num]);
                }
                
                
                
                PlayerBackendData.Instance.TalismanPreset[PlayerBackendData.Instance.nowtalismanpreset]
                    .Talismanset[num] = PlayerBackendData.Instance.TalismanData[TalismanManager.Instance.nowselectkey];
                PlayerBackendData.Instance.TalismanData.Remove(TalismanManager.Instance.nowselectkey);
            }
            TalismanManager.Instance.Refresh();
            Savemanager.Instance.SaveTalisman();
            Savemanager.Instance.Save();
        }
        else
        {
            if (PlayerBackendData.Instance.GiveEquipTalismanData()[num] != null)
            {
                TalismanManager.Instance.Bt_ShowTalisman(PlayerBackendData.Instance.GiveEquipTalismanData()[num], null,num);
            }
            else
            {
                TalismanManager.Instance.Bt_ShowInven();
            }
        }
    }


}
