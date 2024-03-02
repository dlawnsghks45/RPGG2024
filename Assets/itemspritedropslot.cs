using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class itemspritedropslot : MonoBehaviour
{
  public SpriteRenderer sprite;
  public Vector3 m_Target;
  public float m_Speed = 10;
  public float m_HeightArc = 1;
  private Vector3 m_StartPosition;
  [SerializeField]
  private bool m_IsStart;

  public GameObject rareitem;
  public GameObject rareeffect1;

  public void Setitem(Sprite image,bool israre, Vector3 endpos,Vector3 startpos)
  {
    if (israre)
    {
      rareeffect1.SetActive(true);
      rareitem.SetActive(true);
      Soundmanager.Instance.PlayerSound("Attack/DropSound",1f);
      ItemDropManager.Instance.fade.Fade2();
    }
    else
    {
      rareeffect1.SetActive(false);
      rareitem.SetActive(false);
    }
    transform.position = startpos;
    sprite.sprite = image;
    m_Target = endpos;
    m_StartPosition = startpos;
    m_IsStart = true;
  }

  private void OnEnable()
  {
    m_IsStart = true;
    Invoke("falseobj",3f);
  }

  void Update()
  {
    if (m_IsStart)
    {
      float x0 = m_StartPosition.x;
      float x1 = m_Target.x;
      float distance = x1 - x0;
      float nextX = Mathf.MoveTowards(transform.position.x, x1, m_Speed * Time.deltaTime);
      float baseY = Mathf.Lerp(m_StartPosition.y, m_Target.y, (nextX - x0) / distance);
      float arc = m_HeightArc * (nextX - x0) * (nextX - x1) / (-0.25f * distance * distance);
      Vector3 nextPosition = new Vector3(nextX, baseY + arc, transform.position.z);

      transform.position = nextPosition;

      if (nextPosition == m_Target)
        Arrived();
    }
  }

  private void falseobj()
  {
    gameObject.SetActive(false);
  
  }

  void Arrived()
  {
    Debug.Log("µµÂø");
    m_IsStart = false;
  }


}
