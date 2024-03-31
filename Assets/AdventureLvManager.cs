using Doozy.Engine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdventureLvManager : MonoBehaviour
{
    //�±� ���� �� 
    public void Succ()
    {
        //
        PlayerBackendData.Instance.AddAdLv(1);
        Savemanager.Instance.SaveExpData();
        Savemanager.Instance.Save();

    }

    //�±� ����
    public Image AdMonSprite;
    public Text AdMonHp;
    public Text AdLvPrev;
    public Text AdLvNext;
    public string nowaddmapid;
    public UIView adpanel;

    public itemiconslot[] Reward;

    const int maxlv = 32;
    
    
    public void Bt_OpenAdPanel()
    {
//        Debug.Log("����" + maxlv);
        if(PlayerBackendData.Instance.GetAdLv() == maxlv)
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/�ְ�ũ"), alertmanager.alertenum.�Ϲ�);
            return;
        }

        MapDB.Row mapdata_Now = MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage);
        if (mapdata_Now.maptype != "0")
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/����͸�����"),alertmanager.alertenum.����);
            return;
        }
        
        adpanel.Show(false);
        //���� �ʿ� �´� ���� ����
        //���� ����
        nowaddmapid = (2000 + (PlayerBackendData.Instance.GetAdLv() - 1)).ToString(); //2000������ ����
        monid = MapDB.Instance.Find_id(nowaddmapid).monsterid;
    
        AdMonHp.text =
            $"HP {(dpsmanager.convertNumber(decimal.Parse(monsterDB.Instance.Find_id(monid).hp)))}";
        AdMonSprite.sprite = SpriteManager.Instance.GetSprite(monsterDB.Instance.Find_id(MapDB.Instance.Find_id(nowaddmapid).monsterid).sprite);
        AdLvPrev.text =  PlayerData.Instance.gettierstar(PlayerBackendData.Instance.GetAdLv().ToString());
        AdLvNext.text = PlayerData.Instance.gettierstar((PlayerBackendData.Instance.GetAdLv()+1).ToString());

        foreach (var t in Reward)
            t.gameObject.SetActive(false);

        string dropid = monsterDB.Instance.Find_id(monid).dropid;
        bool isdrop = false;
        int num = 0;
        for (int i = 0; i < MonDropDB.Instance.NumRows() - 1; i++)
        {
            if (MonDropDB.Instance.Find_num(i.ToString()).id.Equals(dropid))
            {
                
                isdrop = true;
//                Debug.Log(MonDropDB.Instance.Find_num(i.ToString()).itemid);
                Reward[num].Refresh(MonDropDB.Instance.Find_num(i.ToString()).itemid, int.Parse(MonDropDB.Instance.Find_num(i.ToString()).minhowmany), false);
                Reward[num].gameObject.SetActive(true);
                num++;
            }
            if (isdrop && !MonDropDB.Instance.Find_num(i.ToString()).id.Equals(dropid)) 
                break;
        }
        
    }

    public string monid;
    
    public void Bt_GoAdMap()
    {
        MapDB.Row mapdata_Now = MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage);
        if (mapdata_Now.maptype != "0")
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/����͸�����"),alertmanager.alertenum.����);
            return;
        }
        if (mapmanager.Instance.islocating)
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI2/���̵��ߺҰ�"),alertmanager.alertenum.����);
            return;
        }
        mapmanager.Instance.LocateMap(nowaddmapid);
        
    }
}
