using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Petmixmother : MonoBehaviour
{
    public int mothernum;
    public petmixslot[] PetMixslots; //9개



    public Text[] PercentEnd; //0은 유지 //1은 변경
    public GameObject QuestionMark;
    
    
    public GameObject MixResultPanel;
    public Image MixResultImage;
    public Image MixResultRare;
    public Text MixResultName;
    public ParticleSystem endparticle;

    
    //현재 처음 고르는 등급
    public string NowRare = "-1";


    public void SetData(int sonsnum,string petid)
    {
        PetMixslots[sonsnum].SetData(petid);
        if (petmanager.Instance.PetMixDic.ContainsKey(petid))
        {
            petmanager.Instance.PetMixDic[petid]++;
        }
        else
        {
            petmanager.Instance.PetMixDic.Add(petid, 1);
        }

        RefreshFinish();
    }
    
    public void Bt_selectItem(int sonnum)
    {
        petmanager.Instance.ShowSelectMixItem(mothernum,sonnum);
    }
    
    
    public void ResetAll()
    {
        foreach (var t in PetMixslots)
            t.RemoveData();
        NowRare = "-1";
        RefreshFinish();
    }
     void ResetAll2()
    {
        foreach (var t in PetMixslots)
            t.RemoveData();
        NowRare = "-1";
    }
    public void RemoveData(int num)
    {
        PetMixslots[num].RemoveData();
        RefreshFinish();
    }

    public string rewardid;
    public void StartMix()
    {
        if (!iscanmake)
        {
            Debug.Log("제작 불가");
            ResetAll2();
            return;
        }
        iscanmake = false;
        if (NowRare == "-1")
        {
            Debug.Log("레어가 안맞다");
            ResetAll2();
            return;
        }
        petmanager.Instance.ismix = true;

        Random.InitState(PlayerBackendData.Instance.GetRandomSeed() + (int)Time.deltaTime);
        int ran = Random.Range(0, 101);
        int r = 0;
        //계산
        switch (NowRare)
        {
            
            case "1":
                if (ran <= 50)
                {
                    //1급
                    r = Random.Range(0,petmanager.Instance.rare1.Length);
                    rewardid =  petmanager.Instance.rare1[r];
                }
                else
                {
                    r = Random.Range(0,petmanager.Instance.rare2.Length);
                    rewardid =  petmanager.Instance.rare2[r];
                }
                break;
            case "2":
                if (ran <= 55)
                {
                    //1급
                    r = Random.Range(0,petmanager.Instance.rare2.Length);
                    rewardid =  petmanager.Instance.rare2[r];
                }
                else
                {
                    r = Random.Range(0,petmanager.Instance.rare3.Length);
                    rewardid =  petmanager.Instance.rare3[r];
                }
                break;
            case "3":
                if (ran <= 60)
                {
                    //1급
                    r = Random.Range(0,petmanager.Instance.rare3.Length);
                    rewardid =  petmanager.Instance.rare3[r];
                }
                else
                {
                    r = Random.Range(0,petmanager.Instance.rare4.Length);
                    rewardid =  petmanager.Instance.rare4[r];
                }
                break;
            case "4":
                if (ran <= 70)
                {
                    //1급
                    r = Random.Range(0,petmanager.Instance.rare4.Length);
                    rewardid =  petmanager.Instance.rare4[r];
                }
                else
                {
                    r = Random.Range(0,petmanager.Instance.rare5.Length);
                    rewardid =  petmanager.Instance.rare5[r];
                }
              
                break;
            case "5":
                if (ran <= 70)
                {
                    //1급
                    r = Random.Range(0,petmanager.Instance.rare5.Length);
                    rewardid =  petmanager.Instance.rare5[r];
                }
                else
                {
                    r = Random.Range(0,petmanager.Instance.rare6.Length);
                    rewardid =  petmanager.Instance.rare6[r];
                }
                break;
            case "6":
                    //1급
                    r = Random.Range(0,petmanager.Instance.rare6.Length);
                    rewardid =  petmanager.Instance.rare6[r];
                break;

        }
        petmanager.Instance.issave = true;
        petmanager.Instance.Bt_GetPet(rewardid);
        petmanager.Instance.listslot[rewardid].Init(rewardid);
        petmanager.Instance.log_rewardpet.Add(rewardid);
        for (int i = 0; i < PetMixslots.Length; i++)
        {
            PlayerBackendData.Instance.PetData[PetMixslots[i].petid].Havecount--;
            petmanager.Instance.listslot[PetMixslots[i].petid].Init(PetMixslots[i].petid);
            petmanager.Instance.log_removepet.Add(PetMixslots[i].petid);
        }
       
        StartCoroutine(MixTime());
    }

    IEnumerator MixTime()
    {
        for (int i = 0; i < PetMixslots.Length; i++)
        {
            PetMixslots[i].startparticle.Play();
        }
        yield return new WaitForSeconds(0.8f);
        
        for (int i = 0; i < PetMixslots.Length; i++)
        {
            PetMixslots[i].RemoveData();
        }
       
        MixResultPanel.SetActive(true);
        MixResultImage.sprite = SpriteManager.Instance.GetSprite(PetDB.Instance.Find_id(rewardid).sprite);
        MixResultRare.color = Inventory.Instance.GetRareColor(PetDB.Instance.Find_id(rewardid).rare);
        MixResultName.text = Inventory.GetTranslate(PetDB.Instance.Find_id(rewardid).name);
        MixResultName.color = Inventory.Instance.GetRareColor(PetDB.Instance.Find_id(rewardid).rare);
        endparticle.Play();
        ResetAll2();
        yield return new WaitForSeconds(1f);
        
        petmanager.Instance.issave = false;
        petmanager.Instance.ismix = false;
    }
    
    private bool iscanmake = false;
    public void RefreshFinish()
    {
        MixResultPanel.SetActive(false);
        int count = 0;
        for (int i = 0; i < PetMixslots.Length; i++)
        {
            if (PetMixslots[i].petid != "")
            {
                count++;
            }
        }

        iscanmake = true;
        //3개가 있다.
        if (count == 3)
        {
            QuestionMark.SetActive(false);
            switch (NowRare)
            {
                case "1":
                    PercentEnd[0].text = "50%";
                    Inventory.Instance.ChangeItemRareColor(PercentEnd[0],"1");
                    PercentEnd[1].text = "50%";
                    Inventory.Instance.ChangeItemRareColor(PercentEnd[1],"2");
                    PercentEnd[0].gameObject.SetActive(true);
                    PercentEnd[1].gameObject.SetActive(true);
                    break;
                case "2":
                    PercentEnd[0].text = "55%";
                    Inventory.Instance.ChangeItemRareColor(PercentEnd[0],"2");
                    PercentEnd[1].text = "45%";
                    Inventory.Instance.ChangeItemRareColor(PercentEnd[1],"3");
                    PercentEnd[0].gameObject.SetActive(true);
                    PercentEnd[1].gameObject.SetActive(true);
                    break;
                case "3":
                    PercentEnd[0].text = "60%";
                    Inventory.Instance.ChangeItemRareColor(PercentEnd[0],"3");
                    PercentEnd[1].text = "40%";
                    Inventory.Instance.ChangeItemRareColor(PercentEnd[1],"4");
                    PercentEnd[0].gameObject.SetActive(true);
                    PercentEnd[1].gameObject.SetActive(true);
                    break;
                case "4":
                    PercentEnd[0].text = "70%";
                    Inventory.Instance.ChangeItemRareColor(PercentEnd[0],"4");
                    PercentEnd[1].text = "30%";
                    Inventory.Instance.ChangeItemRareColor(PercentEnd[1],"5");
                    PercentEnd[0].gameObject.SetActive(true);
                    PercentEnd[1].gameObject.SetActive(true);
                    break;
                case "5":
                    PercentEnd[0].text = "75%";
                    Inventory.Instance.ChangeItemRareColor(PercentEnd[0],"5");
                    PercentEnd[1].text = "25%";
                    Inventory.Instance.ChangeItemRareColor(PercentEnd[1],"6");
                    PercentEnd[0].gameObject.SetActive(true);
                    PercentEnd[1].gameObject.SetActive(true);
                    break;
                case "6":
                    PercentEnd[0].text = "100%";
                    Inventory.Instance.ChangeItemRareColor(PercentEnd[0],"6");
                    PercentEnd[0].gameObject.SetActive(true);
                    PercentEnd[1].gameObject.SetActive(false);
                    break;
                
            }
        }
        else
        {
            PercentEnd[0].gameObject.SetActive(false);
            PercentEnd[1].gameObject.SetActive(false);
            QuestionMark.SetActive(true);
            iscanmake = false;
        }
    }
}
