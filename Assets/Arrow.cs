using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Arrow : MonoBehaviour
{
    public bool islaunch;
    public Hpmanager target;
    public SpriteRenderer arrowsprite;
    public Animator arrowani;

    public int speed = 100; //스피드 
    public float size = 1; //스피드 

    private bool istargetNull;
    // Update is called once per frame


    // ReSharper disable Unity.PerformanceAnalysis
    private void Start()
    {
        istargetNull = target == null;
    }

    public void SetArrow(Transform startpos, Hpmanager target, float rotation = 0)
    {
//        Debug.Log("name" + name);
        this.gameObject.SetActive(true);
        transform.position = startpos.position;
        this.target = target;
        islaunch = true;
        Invoke(nameof(disable), 0.2f);
    }
    // ReSharper disable Unity.PerformanceAnalysis
    public void SetArrow(string trigger,Transform startpos, Hpmanager target,float rotation = 0)
    {
//        Debug.Log("name" + name);
        this.gameObject.SetActive(true);
        if (arrowani != null)
        {
            arrowani.SetTrigger(trigger);
        }
        transform.position = startpos.position;
        arrowsprite.transform.rotation = Quaternion.Euler(0, 0, rotation);
        this.target = target;
        islaunch = true;
        Invoke(nameof(disable), 0.2f);
    }
    public void SetSprite(Sprite arrowimage, int speed, float size, float degreeRotate)
    {
        arrowsprite.sprite = arrowimage;
        this.speed = 15;
        this.size = 1;
        var transform1 = arrowsprite.transform;
        transform1.localScale = new Vector3(size, size, size);
        //transform1.localEulerAngles = new Vector3(0, 0, degreeRotate);
    }

      public void SetAni(string trigger,int speed)
      {
          if (arrowani == null) return;
          arrowani.SetTrigger(trigger);
          this.speed = speed;
      }

    void Update()
    {
        if (islaunch)
        {
            if(istargetNull)
            {
                disable();
            }
            else
            {
                Vector3 dir = target.Effecttrans.position - transform.position;

                // transform.LookAt(target);
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.Translate(Vector3.right * (speed * Time.deltaTime));
                if (Vector2.Distance(transform.position, target.Effecttrans.position) <= 0.06f)
                {
                    gameObject.SetActive(false);
                }
            }
           
        }


        if (Vector2.Distance(transform.position, target.Effecttrans.position) <= 0.06f)
            gameObject.SetActive(false);
    }

    public void disable()
    {
        gameObject.SetActive(false);
    }

    private void OnDisablrffffve()
    {
        islaunch = false;
    }
}
