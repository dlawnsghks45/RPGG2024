using Doozy.Engine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class craftslot : MonoBehaviour
{
    public string id;
    public Image succimage;
    public Text succname;
    public Text iscanmake; //제작가능여부
    public GameObject craftlock;
    public Text AdventuredLv;

    CraftTableDB.Row item;
    public void setcraft(string craftid)
    {
        id = craftid;
        item = CraftTableDB.Instance.Find_id(id);
        if (item.isequip == "TRUE")
        {
            //Debug.Log("아이디" + item.Successid);
            succimage.sprite = SpriteManager.Instance.GetSprite(EquipItemDB.Instance.Find_id(item.Successid).Sprite);
            if (item.MinSuccesshowmany != "1")
                succname.text =
                    $"{Inventory.GetTranslate(EquipItemDB.Instance.Find_id(item.Successid).Name)} x {item.MinSuccesshowmany}";
            else
                succname.text = Inventory.GetTranslate(EquipItemDB.Instance.Find_id(item.Successid).Name);

            succname.color = Inventory.Instance.GetRareColor(EquipItemDB.Instance.Find_id(item.Successid).Rare);
        }
        else
        {
            succimage.sprite = SpriteManager.Instance.GetSprite(ItemdatabasecsvDB.Instance.Find_id(item.Successid).sprite);

            if (item.MinSuccesshowmany != "1")
                succname.text =
                    $"{Inventory.GetTranslate(ItemdatabasecsvDB.Instance.Find_id(item.Successid).name)} x {item.MinSuccesshowmany}";
            else
                succname.text = Inventory.GetTranslate(ItemdatabasecsvDB.Instance.Find_id(item.Successid).name);

            succname.color = Inventory.Instance.GetRareColor(ItemdatabasecsvDB.Instance.Find_id(item.Successid).rare);
        }
        Refresh();
    }

    void Refresh()
    {
        if(int.Parse(item.maprank) > PlayerBackendData.Instance.GetAdLv())
        {
            #if UNITY_EDITOR 
            craftlock.SetActive(false);
            return;
            #endif
            //잠금걸림
            craftlock.SetActive(true);
            AdventuredLv.text = string.Format(Inventory.GetTranslate("ButtonUI/제작잠금"),
                PlayerData.Instance.gettierstar(item.maprank));
        }
        else
        {
            //잠금풀림
            craftlock.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if (!craftlock.activeSelf) //잠금이안나와야한다.
        {
            Refresh();
        }

    }

    public void Bt_ShowInfo()
    {
        if (!craftlock.activeSelf) //잠금이안나와야한다.

            if (id.Equals("1021") || id.Equals("10") &&
                TutorialDB.Instance.Find_id(PlayerBackendData.Instance.tutoid).type.Equals("craft"))
            {
                Tutorialmanager.Instance.NewTuto1[3].SetActive(false);
                Tutorialmanager.Instance.NewTuto1[4].SetActive(true);
            }

        CraftManager.Instance.Bt_ShowCraftResourceInfo(id);
    }
}
