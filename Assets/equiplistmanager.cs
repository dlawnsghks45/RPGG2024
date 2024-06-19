using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;

public class equiplistmanager : MonoBehaviour
{
    public equipshowslot obj;

    //풀링
    public const int init = 30;
    public List<equipshowslot> lists = new List<equipshowslot>();
    public Transform objtrans;
    public void Bt_ShowPanel(string TypeName)
    {
        List<int> nums = new List<int>();
        
        
        for (int i = 0; i < EquipListDB.Instance.NumRows(); i++)
        {
            if (EquipListDB.Instance.GetAt(i).type.Equals(TypeName))
            {
                nums.Add(i);
            }
        }

        if (lists.Count < nums.Count)
        {
            for (int i = 0; i < nums.Count; i++)
            {
                lists.Add(Instantiate(obj, objtrans));
            }
        }
        
        for(int i = 0 ; i < lists.Count;i++)
            lists[i].gameObject.SetActive(false);
        
        for (int i = 0; i < nums.Count; i++)
        {
            lists[i].Init(EquipListDB.Instance.GetAt(nums[i]).id);
            lists[i].gameObject.SetActive(true);
        }
    }
    
}
