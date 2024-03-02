using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TMProHyperLink : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    public TextMeshProUGUI m_TextMeshPro;
    Camera m_Camera;
    Canvas m_Canvas;
    public string id;
    bool ismine;
    public ChatType Types;
    EquipDatabase Equipdata;
    public enum ChatType
    {
        EquipSkill,
        SetInfo,
        EquipItem,
        Length
    }
    public void SetEquipSkill(string id)
    {
        if(id == "")
            return;
        this.id = id;
        m_TextMeshPro.text =
            $"[{Inventory.GetTranslate(EquipSkillDB.Instance.Find_id(id).name)} Lv.{EquipSkillDB.Instance.Find_id(id).lv}]";
        Inventory.Instance.ChangeItemRareColorTMPro(m_TextMeshPro, EquipSkillDB.Instance.Find_id(id).rare);
    }

    public void SetEquipItem_Mine(EquipDatabase data)
    {
        ismine = true;
        Equipdata = data;
        m_TextMeshPro.text = Inventory.GetTranslate(EquipItemDB.Instance.Find_id(data.Itemid).Name);
    }

    public void SetEquipItem_NotMine(string id)
    {
        ismine = false;
        this.id = id;
        Equipdata = new EquipDatabase(id);
        //EquipSetmanager.Instance.infopanel.Hide(true);
        m_TextMeshPro.text = Inventory.GetTranslate(EquipItemDB.Instance.Find_id(id).Name);
    }

    public void SetSetName(string Equip)
    {
        this.id = Equip;
        if (EquipSetmanager.Instance.CheckSetItem(Equip))
        {
            m_TextMeshPro.color = Color.cyan;
        }
        else
        {
            m_TextMeshPro.color = Color.gray;
        }
        m_TextMeshPro.text = string.Format(Inventory.GetTranslate("UI/세트아이템"), Inventory.GetTranslate(SetDBDB.Instance.Find_id(EquipItemDB.Instance.Find_id(Equip).SetID).name));
    }
    void Start()
    {
        m_Camera = Camera.main;

        m_Canvas = gameObject.GetComponentInParent<Canvas>();
        if (m_Canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            m_Camera = null;
        else
            m_Camera = m_Canvas.worldCamera;

        //m_TextMeshPro.ForceMeshUpdate();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        switch(Types)
        {
            case ChatType.EquipSkill:
                Inventory.Instance.ShowEquipskill(id);
                break;
            case ChatType.SetInfo:
                EquipSetmanager.Instance.Bt_ShowSetInfo();
                    break;
            case ChatType.EquipItem:
                if(ismine)
                {
                    Inventory.Instance.ShowInventoryItem(Equipdata);
                    EquipSetmanager.Instance.infopanel.Hide(true);

                }
                else
                {
                    Inventory.Instance.ShowInventoryItem_NotMine(Equipdata);
                    EquipSetmanager.Instance.infopanel.Hide(true);
                }
                break;
        }
    }
}