using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class talismanequipslot : MonoBehaviour
{
    public Image SetColor;
    private int num;
    public Image Image;
    public string keyid;
    public Image[] Eskill;
    public GameObject[] Stateobj; //0�� ���� //1�� ������ //2�� ���

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

        
        if (!PlayerBackendData.Instance.TalismanLock[num])
        {
            //���
            PlayerBackendData.Instance.EquipTalisman[num] = "";
            Stateobj[2].SetActive(true);
            return;
        }

        if (PlayerBackendData.Instance.EquipTalisman[num] != "")
        {
            //������
            Image.sprite = SpriteManager.Instance.GetSprite(TalismanDB.Instance.Find_id(
                PlayerBackendData.Instance.TalismanData[PlayerBackendData.Instance.EquipTalisman[num]].Itemid).sprite);
            Stateobj[1].SetActive(true);
            
            //Ư��ȿ��üũ
        }
        else
        {
            //��������
            Stateobj[0].SetActive(true);
        }
    }
    
    
}
