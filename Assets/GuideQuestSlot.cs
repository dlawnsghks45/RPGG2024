using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideQuestSlot : MonoBehaviour
{
    public string id;
    public Text title;
    public GameObject FinishObj;
    public GameObject NextObj;
    public itemiconslot[] Reward;
    public Image QuestPanel;
    public void Refresh(growthguideDB.Row data)
    {
     if(data.id.Equals(""))
         return;
     
        id = data.id;
        title.text = Inventory.GetTranslate(data.name);

        for (int i = 0; i < Reward.Length; i++)
        {
            Reward[i].gameObject.SetActive(false);
        }
        

        Reward[0].Refresh(data.itemid,decimal.Parse(data.howmany),false);
        Reward[0].gameObject.SetActive(true);
        
        Reward[1].Refresh("1011",decimal.Parse(data.exp),false);
        Reward[1].gameObject.SetActive(true);

        if (PlayerBackendData.Instance.tutoguideid == int.Parse(data.id))
        {
            FinishObj.SetActive(false);   
            NextObj.SetActive(false);   
            QuestPanel.color = Color.cyan;
        }
        else if (PlayerBackendData.Instance.tutoguideid < int.Parse(data.id))
        {
            FinishObj.SetActive(false);   
            NextObj.SetActive(true);   
            QuestPanel.color = Color.white;
        }
        else if (PlayerBackendData.Instance.tutoguideid > int.Parse(id))
        {
            FinishObj.SetActive(true);   
            NextObj.SetActive(false);   
            QuestPanel.color = Color.gray;
        }
        
    }

    public void RefreshQuest()
    {
        if(id == "")
            return;
        if (PlayerBackendData.Instance.tutoguideid == int.Parse(id))
        {
            FinishObj.SetActive(false);   
            NextObj.SetActive(false);   
            QuestPanel.color = Color.cyan;
        }
        else if (PlayerBackendData.Instance.tutoguideid < int.Parse(id))
        {
            FinishObj.SetActive(false);   
            NextObj.SetActive(true);   
            QuestPanel.color = Color.white;
        }
        else if (PlayerBackendData.Instance.tutoguideid > int.Parse(id))
        {
         //   gameObject.SetActive(false);
            FinishObj.SetActive(true);   
            NextObj.SetActive(false);   
            QuestPanel.color = Color.gray;
        }
    }

    public void FinishQuest()
    {
        FinishObj.SetActive(true);
        NextObj.SetActive(false);
        QuestPanel.color = Color.gray;
    }


}
