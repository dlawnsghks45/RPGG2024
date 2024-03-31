using Doozy.Engine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdventureLvManager : MonoBehaviour
{
    //승급 성공 시 
    public void Succ()
    {
        //
        PlayerBackendData.Instance.AddAdLv(1);
        Savemanager.Instance.SaveExpData();
        Savemanager.Instance.Save();

    }

    //승급 실패
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
//        Debug.Log("레벨" + maxlv);
        if(PlayerBackendData.Instance.GetAdLv() == maxlv)
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/최고랭크"), alertmanager.alertenum.일반);
            return;
        }

        MapDB.Row mapdata_Now = MapDB.Instance.Find_id(PlayerBackendData.Instance.nowstage);
        if (mapdata_Now.maptype != "0")
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/사냥터만가능"),alertmanager.alertenum.주의);
            return;
        }
        
        adpanel.Show(false);
        //현재 맵에 맞는 몬스터 설정
        //몬스터 설정
        nowaddmapid = (2000 + (PlayerBackendData.Instance.GetAdLv() - 1)).ToString(); //2000번부터 시작
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
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI/사냥터만가능"),alertmanager.alertenum.주의);
            return;
        }
        if (mapmanager.Instance.islocating)
        {
            alertmanager.Instance.ShowAlert(Inventory.GetTranslate("UI2/맵이동중불가"),alertmanager.alertenum.주의);
            return;
        }
        mapmanager.Instance.LocateMap(nowaddmapid);
        
    }
}
