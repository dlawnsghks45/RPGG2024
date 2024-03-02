using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteManager : MonoBehaviour
{
    private void OnDestroy()
    {
        _instance = null;
    }

    //싱글톤만들기.
    private static SpriteManager _instance = null;
    public static SpriteManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(SpriteManager)) as SpriteManager;

                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }
            return _instance;
        }
    }

    private readonly Dictionary<string, Sprite> Sprites = new Dictionary<string, Sprite>();
    public Sprite GetSprite(string _key)
    {
        if (Sprites.TryGetValue(_key, out var sprite))
        {
            //  Debug.Log("Have");
            //   Debug.Log(_key);
            return sprite;
        }
        Sprite value = Resources.Load<Sprite>(_key);
        Sprites.Add(_key, value);
        return value;
    }
    /*
    public void GetMonsterSprite(Image monimage,string _key)
    {
        if (Sprites.ContainsKey(_key))
        {
            //  Debug.Log("Have");
            //   Debug.Log(_key);
            monimage.sprite = Sprites[_key];
        }
        else
        {
            Sprite value = Resources.Load<Sprite>(_key);
            Sprites.Add(_key, value);
            monimage.sprite = value;
        }

        switch(monsterDB.Instance.Find_sprite(_key).sizetype)
        {

            case "0": //레이어 랩 사이즈
                monimage.transform.localScale = BattleManager.Instance.vec1;
                break;
            case "1": //던전크롤러 사이즈
                monimage.transform.localScale = BattleManager.Instance.vec2;
                break;
        }
    }*/

    private readonly Dictionary<string, AudioClip> Sounds = new Dictionary<string, AudioClip>();
    public AudioClip GetSound(string _key)
    {
        if (Sounds.TryGetValue(_key, out var sound))
        {
            //  Debug.Log("Have");
            //   Debug.Log(_key);
            return sound;

        }
        AudioClip value = Resources.Load<AudioClip>(_key);
        Sounds.Add(_key, value);
        return value;
    }


    private readonly Dictionary<float, WaitForSeconds> Waitfor = new Dictionary<float, WaitForSeconds>();
    public WaitForSeconds GetWaitforSecond(float _key)
    {
        if (Waitfor.TryGetValue(_key, out var second))
        {
            //  Debug.Log("Have");
            //   Debug.Log(_key);
            return second;

        }
        WaitForSeconds value = new WaitForSeconds(_key);
        Waitfor.Add(_key, value);
        return value;
    }


}
