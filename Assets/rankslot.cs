using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rankslot : MonoBehaviour
{
    public Sprite[] ranksprite;
    public Color[] rankcolor;

    public string playernickname;
    public string indate;
    public Image RankPanel;
    public Image RankTop;
    public Text Rank;
    public Text Name;
    public Text Score;

    public Image avata;
    public Image weapon;
    public Image subweapon;
    
    public void NoRank()
    {
        RankTop.enabled = false;
        Rank.text = "-";
        Name.text = "-";
        Score.text = "-";
        RankPanel.color = Color.white;
    }

    public void SetRank(string rank, string name, string score, string indate,string avatadata = "")
    {
        RankTop.enabled = false;
        Rank.text = rank;
        Name.text = name;
        Score.text = score;
        RankPanel.color = Color.white;
        //Debug.Log("스ㅗ컹"+ score);
        playernickname = name;
        this.indate = indate;

        switch (rank)
        {
            case "1":
                RankPanel.color = rankcolor[0];
                RankTop.sprite = ranksprite[0];
                Rank.text = "";
                RankTop.enabled = true;
                break;
            case "2":
                RankPanel.color = rankcolor[1];
                RankTop.sprite = ranksprite[1];
                Rank.text = "";
                RankTop.enabled = true;
                break;
            case "3":
                RankPanel.color = rankcolor[2];
                RankTop.sprite = ranksprite[2];
                Rank.text = "";
                RankTop.enabled = true;
                break;
        }

        if (avatadata != "")
        {
            string[] datas = avatadata.Split(';');

            avata.gameObject.SetActive(true);
            weapon.gameObject.SetActive(true);
            subweapon.gameObject.SetActive(true);
            
            //위장
            if (datas[3] != "")
            {
                avata.sprite = SpriteManager.Instance.GetSprite(AvartaDB.Instance.Find_id(datas[3]).sprite);
            }
            else
            {
                avata.sprite = SpriteManager.Instance.GetSprite(ClassDB.Instance.Find_id(datas[0]).classsprite);
            }
            
            //위장
            if (datas[4] != "")
            {
                weapon.sprite = SpriteManager.Instance.GetSprite(AvartaDB.Instance.Find_id(datas[4]).sprite);
            }
            else
            {
                weapon.sprite = SpriteManager.Instance.GetSprite(EquipItemDB.Instance.Find_id(datas[1]).EquipSprite);
            }
            
            //위장
            if (datas[5] != "")
            {
                subweapon.sprite = SpriteManager.Instance.GetSprite(AvartaDB.Instance.Find_id(datas[5]).sprite);
            }
            else
            {
                subweapon.sprite = SpriteManager.Instance.GetSprite(EquipItemDB.Instance.Find_id(datas[2]).EquipSprite);
            }
            
        }
        else
        {
            avata.gameObject.SetActive(false);
            weapon.gameObject.SetActive(false);
            subweapon.gameObject.SetActive(false);
        }
    }

    public void Bt_ShowUserData()
    {
        switch (RankingManager.Instance.nowselectnum)
        {
            case 0:
            case 1:
            case 2:
            case 5:
                otherusermanager.Instance.ShowPlayerData(playernickname);
                uimanager.Instance.AddUiview(RankingManager.Instance.RankingPanel, true);
                break;
            case 3:
                otherusermanager.Instance.ShowPlayerData_Training(playernickname);
                uimanager.Instance.AddUiview(RankingManager.Instance.RankingPanel, true);
                break;
            case 4:
                GuildManager.Instance.Bt_ShowSelectGuild(indate);
                uimanager.Instance.AddUiview(RankingManager.Instance.RankingPanel, true);
                break;
        }

    }
}
