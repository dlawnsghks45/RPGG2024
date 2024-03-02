using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : MonoBehaviour
{
    public AttackType attacktype;
    public Transform MeleeSit;
    public Transform RangeSit;
    public Transform ArrowPos;

    Animator ani;
    readonly int poolcount = 20;

    public List<Arrow> effect = new List<Arrow>();

    private void InitArrow()
    {
        effect.Clear();
        if (effect.Count > 0) return;
        for (int i = 0; i < poolcount; i++)
        {
            //충전
            Arrow arrow = Instantiate(ObjectPoolManager.Instance.arrowobj, ObjectPoolManager.Instance.ArrowPoolTrans);
            //arrow.SetSprite(SpriteManager.Instance.GetSprite(itemdata.arrowsprite));
            arrow.gameObject.SetActive(false);
            effect.Add(arrow);
        }
    }


    private void Start()
    {
        ani = GetComponent<Animator>();
        StartCoroutine(battlestart());
        InitArrow();
    }

    public void StartAni(string trigger)
    {
        ani.SetTrigger(trigger);
    }


    [SerializeField] readonly WaitForSeconds wait = new WaitForSeconds(0.1f);

    private IEnumerator battlestart()
    {
        while (true)
        {
            yield return new WaitUntil(() => Battlemanager.Instance.isbattle);

            if (Battlemanager.Instance.isbattle && EnemySpawnManager.Instance.GetDistance() < 0.1f)
            {

                yield return wait;
            }
        }
    }

    public void Update()
    {
        //근거리
        transform.position = Battlemanager.Instance.isbattle ? Vector3.Lerp(transform.position, attacktype == AttackType.Melee ? MeleeSit.position : RangeSit.position, 10 * Time.deltaTime) : Vector3.Lerp(transform.position, RangeSit.position, 10 * Time.deltaTime);
    }
}
