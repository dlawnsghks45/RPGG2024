using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settimetext : MonoBehaviour
{
    public Timemanager.ContentEnumDaily type;
    // Start is called before the first frame update

    public Text text;
    public Button buttons;
  
    private void Awake()
    {
        Timemanager.Instance.timedele += Refresh;
    }
    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        try
        {
            if (!this.gameObject.activeSelf) return;
            
            
            if (buttons != null)
            {
                buttons.interactable = Timemanager.Instance.GetNowCount_daily(type) != 0;
            }

            if (text == null) return;
            if (Timemanager.ContentEnumDaily.월드보스1공격횟수 == type ||
                Timemanager.ContentEnumDaily.월드보스2공격횟수 == type ||
                Timemanager.ContentEnumDaily.월드보스3공격횟수 == type ||
                Timemanager.ContentEnumDaily.월드보스4공격횟수 == type ||
                Timemanager.ContentEnumDaily.월드보스5공격횟수 == type ||
                Timemanager.ContentEnumDaily.월드보스6공격횟수 == type ||
                Timemanager.ContentEnumDaily.월드보스7공격횟수 == type)
            {
                text.text = string.Format(Inventory.GetTranslate("UI6/월드보스공격횟수"), Timemanager.Instance.GetNowCount_daily(type), Timemanager.Instance.GetMaxCount_daily(type));
            }
            else if (Timemanager.ContentEnumDaily.월드보스공격횟수 == type)
            {
                text.text = string.Format(Inventory.GetTranslate("UI6/월드보스총공격횟수"), Timemanager.Instance.GetNowCount_daily(type), Timemanager.Instance.GetMaxCount_daily(type));
            }
            else if (Timemanager.ContentEnumDaily.월드보스보상횟수 == type)
            {
                text.text = string.Format(Inventory.GetTranslate("UI6/월드보스보상횟수"), Timemanager.Instance.GetNowCount_daily(type), Timemanager.Instance.GetMaxCount_daily(type));
            }
            else
            {
                text.text = string.Format(Inventory.GetTranslate("UI/남은횟수"),
                    Timemanager.Instance.GetNowCount_daily(type), Timemanager.Instance.GetMaxCount_daily(type));
            }

            text.color = Timemanager.Instance.GetNowCount_daily(type) == 0 ? Color.red : Color.cyan;
        }
        catch
        {

        }
    }
}
