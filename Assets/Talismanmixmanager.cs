using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using UnityEngine;

public class Talismanmixmanager : MonoBehaviour
{
    //싱글톤만들기.
    private static Talismanmixmanager _instance = null;
    public static Talismanmixmanager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(Talismanmixmanager)) as Talismanmixmanager;

                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }
            return _instance;
        }
    }

    public talismanmixslot[] mixslots;
    public talismanslot resultslot;
    public UIView mixpanel;

    public bool ismix;

    public void Bt_OpenMixPanel()
    {
        ismix = true;
        mixpanel.Show(false);

        for (int i = 0; i < mixslots.Length; i++)
        {
            mixslots[i].RemoveItem();
        }
        
        RefreshResult();
    }

    public UIButton MixButton;

    public void RefreshResult()
    {
        int a = 0;
        for (int i = 0; i < mixslots.Length; i++)
        {
            if (mixslots[i].keyid != "")
            {
                a++;
            }
        }
        if (a == 3)
        {
            MixButton.Interactable = true;
        }
        else
        {
            MixButton.Interactable = false;
        }
    }
    public void Bt_ExitMixPanel()
    {
        ismix = false;
    }

    public int nowselectmixnum;
    public void Bt_SelectItem(int selectnum)
    {
        nowselectmixnum = selectnum;
        TalismanManager.Instance.InvenPanel.Show(false);
    }

    public string[] RandomTalismanID;

    private bool ismixbool = false;
    public GameObject Blind;
    Talismandatabase A;
    public void Bt_StartMix()
    {
        if (!ismixbool)
        {
            ismixbool = true;
            Blind.SetActive(true);
            for (int i = 0; i < mixslots.Length; i++)
            {
                PlayerBackendData.Instance.TalismanData.Remove(mixslots[i].keyid);
            }
            int r = Random.Range(0, RandomTalismanID.Length);
            Debug.Log(r);
            Debug.Log(RandomTalismanID[r]);
            A = PlayerBackendData.Instance.MakeTalismanDatabase(RandomTalismanID[r]);
            Savemanager.Instance.SaveTalisman();
            Savemanager.Instance.Save();
            StartCoroutine(StartMix());
        }
    }

    public ParticleSystem particle;
    public GameObject ResultFinishButton;
    IEnumerator StartMix()
    {
        for (int i = 0; i < mixslots.Length; i++)
        {
            mixslots[i].StartMix();
        }
        yield return new WaitForSeconds(1.2f);
        resultslot.Refresh(A);
        particle.Play();
        resultslot.gameObject.SetActive(true);
        MixButton.gameObject.SetActive(false);
        for (int i = 0; i < mixslots.Length; i++)
        {
            mixslots[i].RemoveItem();
        }
        RefreshResult();
        yield return new WaitForSeconds(0.45f);
        ResultFinishButton.SetActive(true);
        TalismanManager.Instance.RefreshInven();
    }
    //결과확인을누름
    public void Bt_FinishResults()
    {
        Blind.SetActive(false);
        ismixbool = false;
        resultslot.gameObject.SetActive(false);
        ResultFinishButton.SetActive(false);
        MixButton.gameObject.SetActive(true);
    }
}
